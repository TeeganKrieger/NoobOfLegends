using NoobOfLegends.APIs.RiotApi;
using NoobOfLegends.Models.Database;
using NoobOfLegends.Models.Services;
using System.Collections.Concurrent;

namespace NoobOfLegends_BackEnd.Models
{
    public class UserResultsTask
    {
        private readonly IServiceProvider _serviceProvider;
        private AppDbContext _dbContext;
        private RiotGamesApiTranslator _translator;

        private string username;
        private string tagline;

        private ConcurrentDictionary<int, MatchDataResult> matchesList = new ConcurrentDictionary<int, MatchDataResult>();
        private UserInfoResult userInfoResult;
        private float completionEstimate;
        private int lastError;

        public UserResultsTask(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
        }

        public void Setup(string username, string tagline)
        {
            this.username = username;
            this.tagline = tagline;
        }

        public async Task StartAsync()
        {
            using (IServiceScope scope = _serviceProvider.CreateScope())
            {
                _dbContext =
                    scope.ServiceProvider.GetRequiredService<AppDbContext>();

                _translator =
                    scope.ServiceProvider.GetRequiredService<RiotGamesApiTranslator>();

                await DoWork();
            }
        }

        private async Task DoWork()
        {

            ////////////////////////////////////////////////////////////////////////////////////////////
            System.Diagnostics.Debug.Write($"Fetching User {username}#{tagline} from database: ");

            LolUser user = _dbContext.LolUsers.Where(x => x.UsernameAndTagline == $"{username}#{tagline}").FirstOrDefault();
            RiotPUUID puuid = default;
            RiotSummoner summoner = null;


            System.Diagnostics.Debug.Write($"Failed! Fetching from API instead: ");

            puuid = await _translator.GetPUUID(username, tagline);

            if (puuid.puuid != null)
            {

                summoner = await _translator.GetSummoner(puuid);

                if (summoner != null)
                {
                    System.Diagnostics.Debug.WriteLine($"Success!");
                    if (user == null)
                    {
                        user = new LolUser()
                        {
                            UsernameAndTagline = $"{username}#{tagline}",
                            Matches = new List<Match>(),
                            SummonerName = summoner.name,
                        };

                        _dbContext.LolUsers.Add(user);
                        _dbContext.SaveChanges();
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"Failed! Summoner not found!");
                    lastError = 400;
                    return;
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"Failed! PUUID Not Found!");
                lastError = 400;
                return;
            }


            RiotRankedResult[] rankedResults = await _translator.GetSummonerRankedData(summoner);

            userInfoResult = new UserInfoResult(username, tagline, rankedResults);


            ////////////////////////////////////////////////////////////////////////////////////////////////

            int matchIndex = 0;
            List<Match> matches = new List<Match>(user.Matches);
            HashSet<string> matchesHashset = new HashSet<string>(matches.Select(x => x.MatchID));

            System.Diagnostics.Debug.Write($"Fetching match history from API: ");

            string[] matchHistory = await _translator.GetMatchHistory(puuid);

            if (matchHistory == null)
            {
                System.Diagnostics.Debug.WriteLine($"Failed! Match History not found!");
                lastError = 400;
                return;
            }

            //Discover the number of matches we need to collect to be up to date
            int matchDiffCount = 0;
            foreach (string matchId in matchHistory)
            {
                if (matchesHashset.Contains(matchId))
                    break;
                else
                    matchDiffCount++;
            }
            int takeCount = Math.Min(100 - matchDiffCount, matches.Count);

            matches = new List<Match>(matches.Take(takeCount));
            matchesHashset = new HashSet<string>(matches.Select(x => x.MatchID));

            foreach (Match match in matches)
            {
                matchesList.TryAdd(matchIndex++, new MatchDataResult(match, summoner));
                completionEstimate += 0.01f;
            }

            //Fetch Matches
            foreach (string matchId in matchHistory)
            {
                System.Diagnostics.Debug.Write($"Fetching data for match {matchId} from API: ");
                //Fetch match from api
                if (!matchesHashset.Contains(matchId))
                {
                    Match existingMatchOfSameId = _dbContext.Matches.Where(x => x.MatchID == matchId).FirstOrDefault();
                    if (existingMatchOfSameId == null)
                    {
                        RiotMatch riotMatch = await _translator.GetMatch(matchId);

                        if (riotMatch != null)
                        {
                            System.Diagnostics.Debug.WriteLine($"Success! Translating Match...");
                            Match match = Match.FromRiotMatch(riotMatch);

                            match.Users.Add(user);
                            user.Matches.Add(match);

                            _dbContext.Update(user);
                            _dbContext.Matches.Add(match);
                            _dbContext.SaveChanges();

                            matches.Add(match);
                            matchesList.TryAdd(matchIndex++, new MatchDataResult(match, summoner));
                            completionEstimate += 0.01f;
                            matchesHashset.Add(matchId);
                        }
                        else
                            System.Diagnostics.Debug.WriteLine($"Failed! Match Not Found: ");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"Success! Match Already In DB. Associating with user {user.SummonerName}");
                        user.Matches.Add(existingMatchOfSameId);
                        existingMatchOfSameId.Users.Add(user);
                        _dbContext.Update(user);
                        _dbContext.Update(existingMatchOfSameId);
                        _dbContext.SaveChanges();
                    }
                }
                else
                    System.Diagnostics.Debug.WriteLine($"Match Already In Database and Associated");
            }
            /////////////////////////////////////////////////////////////////////////////////////////////////

            completionEstimate = 1.0f;
        }

        private MatchDataResult[] GetMatches()
        {
            MatchDataResult[] results = matchesList.Values.OrderByDescending(x => x.playedOn).ToArray();
            matchesList.Clear();
            return results;
        }

        private UserInfoResult GetUserInfo()
        {
            UserInfoResult uir = userInfoResult;
            userInfoResult = null;
            return uir;
        }

        public CheckpointResult GetCheckpointResult()
        {
            if (lastError != 0)
                return new CheckpointResult()
                {
                    Completed = true,
                    Completion = 1.0f,
                    Error = lastError,
                };

            return new CheckpointResult()
            {
                Completed = completionEstimate >= 1.0f,
                Completion = completionEstimate,
                UserInfo = GetUserInfo(),
                Matches = GetMatches(),
            };
        }
    }

    public class CheckpointResult
    {
        public UserInfoResult UserInfo { get; set; }
        public MatchDataResult[] Matches { get; set; }
        public float Completion { get; set; }
        public bool Completed { get; set; }
        public int Error { get; set; }
    }

    public class MatchDataResult
    {
        public string id { get; set; }
        public long playedOn { get; set; }
        public string champion { get; set; }
        public bool won { get; set; }

        public int gold { get; set; }
        public int kills { get; set; }
        public int deaths { get; set; }
        public int assists { get; set; }
        public int timeSpentDead { get; set; }
        public int totalDamageDealt { get; set; }
        public int baronKills { get; set; }
        public int dragonKills { get; set; }
        public int minionKills { get; set; }
        public int jungleMinionKills { get; set; }
        public int visionScore { get; set; }
        public float killParticipation { get; set; }
        public int healing { get; set; }

        public MatchDataResult(Match match, RiotSummoner me)
        {
            this.id = match.MatchID;
            this.playedOn = match.GameStartTime;
            MatchParticipant p = match.Participants.Where(x => x.PlayerName == me.name).FirstOrDefault();
            MatchTeam team = match.Teams.Where(x => x.TeamID == p.TeamID).FirstOrDefault();

            if (team != null)
            {
                this.won = team.Won;
            }
            else
            {
                throw new InvalidOperationException();
            }

            if (p != null)
            {
                this.champion = p.Champion;
                this.gold = p.Gold;
                this.kills = p.Kills;
                this.deaths = p.Deaths;
                this.assists = p.Assists;
                this.timeSpentDead = p.TimeSpentDead;
                this.totalDamageDealt = p.TotalDamageDealtToChampions;
                this.baronKills = p.BaronKills;
                this.dragonKills = p.DragonKills;
                this.minionKills = p.MinionKills;
                this.jungleMinionKills = p.JungleMinionKills;
                this.visionScore = p.VisionScore;
                this.killParticipation = p.KillParticipation;
                this.healing = p.HealingToChampions;
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
    }

    public class UserInfoResult
    {
        private static Dictionary<string, int> rankLookup = new Dictionary<string, int>()
            {
                {"IRON", 1 },
                {"BRONZE", 2 },
                {"SILVER", 3 },
                {"GOLD", 4 },
                {"PLATINUM", 5 },
                {"DIAMOND", 6 },
                {"MASTER", 7 },
                {"GRANDMASTER", 8 },
                {"CHALLENGER", 9 }
            };

        private static Dictionary<string, int> tierLookup = new Dictionary<string, int>()
            {
                {"I", 0 },
                {"II", 1 },
                {"III", 2 },
                {"IV", 3 },
            };


        public string username { get; set; }
        public string tagline { get; set; }

        public RankedInfo rankFlex { get; set; }
        public RankedInfo rankSoloDuo { get; set; }



        public UserInfoResult(string username, string tagline, RiotRankedResult[] rankedResults)
        {
            this.username = username;
            this.tagline = tagline;

            RiotRankedResult solo = rankedResults.Where(x => x.queueType == "RANKED_SOLO_5x5").FirstOrDefault();
            RiotRankedResult flex = rankedResults.Where(x => x.queueType == "RANKED_FLEX_SR").FirstOrDefault();

            if (solo == null)
            {
                rankSoloDuo = new RankedInfo()
                {
                    rank = -1,
                    tier = -1,
                    lp = 0
                };
            }
            else
            {
                rankSoloDuo = new RankedInfo()
                {
                    rank = rankLookup[solo.tier],
                    tier = tierLookup[solo.rank],
                    lp = solo.leaguePoints
                };
            }

            if (flex == null)
            {
                rankFlex = new RankedInfo()
                {
                    rank = -1,
                    tier = -1,
                    lp = 0
                };
            }
            else
            {
                rankFlex = new RankedInfo()
                {
                    rank = rankLookup[flex.tier],
                    tier = tierLookup[flex.rank],
                    lp = flex.leaguePoints
                };
            }
        }

        public class RankedInfo
        {
            public int rank { get; set; }
            public int tier { get; set; }
            public int lp { get; set; }
        }
    }

}
