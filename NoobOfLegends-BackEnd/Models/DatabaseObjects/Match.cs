using Newtonsoft.Json;
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

        [Column]
        public long GameStartTime { get; set; }

        [Column]
        public long GameEndTime { get; set; }

        [Column(TypeName = "NVARCHAR(64)")]
        public string GameMode { get; set; }

        [Column(TypeName = "Integer")]
        public int QueueId { get; set; }

        [Column(TypeName = "Integer")]
        public int AverageRank { get; set; }

        [JsonIgnore]
        public virtual List<LolUser> Users { get; set; }

        public virtual List<MatchParticipant> Participants { get; set; }
        public virtual List<MatchTeam> Teams { get; set; }

        public static Match FromRiotMatch(RiotMatch riotMatch)
        {
            Match match = new Match()
            {
                MatchID = riotMatch.metadata.matchId,
                GameStartTime = riotMatch.info.gameStartTimestamp,
                GameEndTime = riotMatch.info.gameEndTimestamp,
                GameMode = riotMatch.info.gameMode,
                QueueId = riotMatch.info.queueId,
                Users = new List<LolUser>(),
                Participants = new List<MatchParticipant>(),
                Teams = new List<MatchTeam>()
            };

            int[] killsPerTeam = new int[2] { 0, 0 };

            foreach (RiotMatch.Participant p in riotMatch.info.participants)
            {
                MatchParticipant part = new MatchParticipant();

                part.Match = match;
                part.PlayerName = p.summonerName;
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

                match.Participants.Add(part);
            }

            foreach (MatchParticipant p in match.Participants)
            {
                p.KillParticipation = (p.Kills + p.Assists) / (float)killsPerTeam[p.TeamID / 100 - 1];
                if (float.IsNaN(p.KillParticipation) || float.IsInfinity(p.KillParticipation))
                    p.KillParticipation = 0f;
            }

            return match;
        }

        public override int GetHashCode()
        {
            return MatchID.GetHashCode();
        }

    }
}
