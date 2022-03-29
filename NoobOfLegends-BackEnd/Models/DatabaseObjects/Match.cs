using NoobOfLegends.APIs.RiotApi;
using NoobOfLegends.Models.Services;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoobOfLegends.Models.Database
{
    public class Match
    {
        [Key]
        [Column(TypeName = "NVARCHAR(64)")]
        public string MatchID { get; set; }

        [Required]
        public virtual List<LolUser> Users { get; set; }

        [Column(TypeName = "Integer")]
        public int GameStartTime { get; set; }

        [Column(TypeName = "Integer")]
        public int GameEndTime { get; set; }

        [Column(TypeName = "NVARCHAR(64)")]
        public string GameMode { get; set; }

        [Column(TypeName = "Integer")]
        public int QueueId { get; set; }

        [Column(TypeName = "Integer")]
        public int AverageRank { get; set; }

        public virtual List<MatchParticipant> Participants { get; set; }
        public virtual List<MatchTeam> Teams { get; set; }

        public static async Task<MatchAndParticipants> FromRiotMatch(AppDbContext dbContext, RiotMatch match)
        {
            RiotGamesApiTranslator translator = new RiotGamesApiTranslator();
            Match m = new Match();

            m.MatchID = match.metadata.matchId;
            m.Users = new List<LolUser>();

            m.GameStartTime = (int)match.info.gameStartTimestamp;
            m.GameEndTime = (int)match.info.gameEndTimestamp;

            m.GameMode = match.info.gameMode;

            m.QueueId = match.info.queueId;

            List<MatchParticipant> participantList = new List<MatchParticipant>();

            int[] killsPerTeam = new int[2] { 0, 0 };

            foreach (RiotMatch.Participant p in match.info.participants)
            {
                LolUser user = dbContext.LolUsers.Where(x => x.SummonerName == p.summonerName).FirstOrDefault();

                if (user == null)
                {
                    RiotSummoner summoner = await translator.GetSummoner(p.summonerName);
                    if (summoner != null)
                    {
                        RiotAccount account = await translator.GetAccount(new RiotPUUID(summoner.puuid));
                        if (account != null)
                        {
                            user = new LolUser()
                            {
                                UsernameAndTagline = $"{account.gameName}#{account.tagLine}",
                                SummonerName = p.summonerName,
                            };
                            dbContext.LolUsers.Add(user);
                            dbContext.SaveChanges();
                        }
                    }
                }

                m.Users.Add(user);
                MatchParticipant part = new MatchParticipant();

                part.Match = m;
                part.PlayerName = user?.UsernameAndTagline;
                part.SelectedRole = p.role;
                part.ActualRole = p.teamPosition;
                part.Kills = p.kills;
                part.Deaths = p.deaths;
                part.Assists = p.assists;
                part.Champion = p.championName;
                part.ChampionLevel = p.champLevel;
                part.VisionScore = p.visionScore;
                part.XP = p.champExperience;
                part.BaronKills = p.baronKills;
                part.MinionKills = p.totalMinionsKilled;
                part.JungleMinionKills = p.neutralMinionsKilled;
                part.Gold = p.goldEarned;
                part.TeamID = p.teamId;
                part.TotalDamageDealtToChampions = p.totalDamageDealtToChampions;
                part.TimeSpentDead = p.totalTimeSpentDead;
                part.HealingToChampions = p.totalHealsOnTeammates;

                killsPerTeam[part.TeamID / 100 - 1] += part.Kills;

                participantList.Add(part);
            }

            foreach (MatchParticipant p in participantList)
            {
                p.KillParticipation = (p.Kills + p.Assists) / (float)killsPerTeam[p.TeamID / 100 - 1];
            }

            return new MatchAndParticipants(m, participantList.ToArray());
        }

        public class MatchAndParticipants
        {
            public Match Match { get; private set; }
            public MatchParticipant[] MatchParticipants { get; private set; }

            public MatchAndParticipants(Match match, MatchParticipant[] participants)
            {
                Match = match;
                MatchParticipants = participants;
            }
        }

    }
}
