using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NoobOfLegends.APIs.RiotApi;
using NoobOfLegends.Models.Database;
using NoobOfLegends.Models.Services;
using NoobOfLegends_BackEnd.Models;
using System.Linq;

namespace NoobOfLegends_BackEnd.Controllers
{
    public class StatController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly RiotGamesApiTranslator _translator;

        public StatController(AppDbContext dbContext, RiotGamesApiTranslator translator)
        {
            _dbContext = dbContext;
            _translator = translator;
        }

        [HttpGet("api/info/{username}/{tagline}")]
        public async Task<IActionResult> GetUserInfo(string username, string tagline)
        {
            RiotPUUID puuid = await _translator.GetPUUID(username, tagline);

            if (puuid.puuid == null)
                return BadRequest();

            RiotSummoner summoner = await _translator.GetSummoner(puuid);

            if (summoner == null)
                return BadRequest();

            RiotRankedResult[] rankedResults = await _translator.GetSummonerRankedData(summoner);

            return Ok(new UserInfoResult(username, tagline, rankedResults));
        }

        [HttpGet("api/matches/{username}/{tagline}")]
        public async Task<IActionResult> GetUserMatchData(string username, string tagline)
        {
            RiotPUUID puuid = await _translator.GetPUUID(username, tagline);

            if (puuid.puuid == null)
                return BadRequest();

            RiotSummoner summoner = await _translator.GetSummoner(puuid);

            if (summoner == null)
                return BadRequest();

            UserMatches matchesModel = new UserMatches(_translator, _dbContext);
            Match[] matches = await matchesModel.GetUserMatchData(username, tagline);

            List<MatchDataResult> results = new List<MatchDataResult>();

            foreach (Match match in matches)
                results.Add(new MatchDataResult(match, summoner));

            return Ok(JsonConvert.SerializeObject(results.OrderByDescending(x => x.playedOn)));
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
                    this.won = false;
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
}