using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoobOfLegends.Models.Database
{
  public class Match 
  {
    [Key]
    [Column(TypeName = "NVARCHAR(64)")]
    public string MatchID { get; set; }

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


    public static Match FromRiotMatch(RiotMatch match, out MatchParticipant[] participants)
        {
            Match m = new Match();

            m.MatchID = match.metadata.matchId;

            m.GameStartTime = (int)match.info.gameStartTimestamp;
            m.GameEndTime = (int)match.info.gameEndTimestamp;

            m.GameMode = match.info.gameMode;

            m.QueueId = match.info.queueId;

            List<MatchParticipant> participantList = new List<MatchParticipant>();

            int[] killsPerTeam = new int[2] { 0, 0 };

            foreach (RiotMatch.Participant p in match.info.participants)
            {
                MatchParticipant part = new MatchParticipant();
                
                part.MatchID = match.metadata.matchId;
                part.SelectedRole = p.role;
                part.ActualRole = p.teamPosition;
                part.ParticipantID = p.participantId;
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

                killsPerTeam[part.TeamID] += part.Kills;
            }

            foreach (MatchParticipant p in participantList)
            {
                p.KillParticipation = (p.Kills + p.Assists) / killsPerTeam[p.TeamID];
            }

            participants = participantList.ToArray();

            return m;
        }
  }
}
