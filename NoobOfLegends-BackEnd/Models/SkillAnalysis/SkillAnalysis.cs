using NoobOfLegends.APIs.RiotApi;
using NoobOfLegends.Models.Database;
using NoobOfLegends.Models.Services;
using System.Threading.Tasks;
using System.Collections.Generic;
using NoobOfLegends_BackEnd.Controllers;

namespace NoobOfLegends_BackEnd.Models.SkillAnalysis 
{
    /// <summary>
    /// Logic model that computes skills players could improve upon.
    /// </summary>
    public class SkillAnalysis
    {
        private readonly AppDbContext _dbContext;

        #region CONSTRUCTOR

        public SkillAnalysis(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #endregion

        
        /// <summary>
        /// Data model that defines a skill's ID, the expression to check this skill against, and a guide url.
        /// </summary>
        private class Skill
        {
            public Func<MatchParticipant, LolGlobalAverage, bool> checkExpression;
            public bool PositiveSkill;
            public string ID;
            public string url;

            public Skill(string id, bool positiveSkill, string url, Func<MatchParticipant, LolGlobalAverage, bool> checkExpression)
            {
                this.checkExpression = checkExpression;
                PositiveSkill = positiveSkill;
                ID = id;
                this.url = url;
            }
        }

        /// <summary>
        /// Analyzes matches to determine what skills apply to these matches.
        /// </summary>
        /// <param name="input">The input model to use for skill analysis.</param>
        /// <returns>A list of skills a player could improve upon.</returns>
        public async Task<List<Tuple<string, bool, string>>> AnalyzeSkills(SkillAnalysisController.SkillAnalysisInput input) 
        {
            // Margin of allowed error for skill checking
            // i.e. Good performance is higher than average + 10% and bad performance is lower than average - 10%

            // Create skill list
            List<Skill> skillsList = new List<Skill>()
            {
                new Skill("High Kill Participation", true, "", (m, lga) => { return m.KillParticipation >= (lga.AverageKillParticipation + (lga.AverageKillParticipation * 0.10)); }),
                new Skill("Low Kill Particiaption", false, "https://www.youtube.com/watch?v=YrxoA8eFug4", (m, lga) => { return m.KillParticipation < (lga.AverageKillParticipation - (lga.AverageKillParticipation * 0.10)); }),
                new Skill("Good CS", true, "", (m, lga) => { return (m.MinionKills + m.JungleMinionKills) >= (lga.AverageMinionKills + lga.AverageJungleMinionKills) + ((lga.AverageMinionKills + lga.AverageJungleMinionKills) * 0.10); }),
                new Skill("Poor CS", false, "https://www.youtube.com/watch?v=jOSyf1NQspo", (m, lga) => { return (m.MinionKills + m.JungleMinionKills) < (lga.AverageMinionKills + lga.AverageJungleMinionKills) - ((lga.AverageMinionKills + lga.AverageJungleMinionKills) * 0.10); }),
                new Skill("Good Vision", true, "", (m, lga) => { return m.VisionScore >= (lga.AverageVisionScore + (lga.AverageVisionScore * 0.10)); }),
                new Skill("Poor Vision", false, "https://youtu.be/6cXqzH2vMH8", (m, lga) => { return m.VisionScore < (lga.AverageVisionScore - (lga.AverageVisionScore * 0.10)); }),
                new Skill("Good XP", true, "", (m, lga) => { return m.XP >= (lga.AverageXP + (lga.AverageXP * 0.10)); }),
                new Skill("Poor XP", false, "https://www.youtube.com/watch?v=cL6cWGQtocw", (m, lga) => { return m.XP < (lga.AverageXP - (lga.AverageXP * 0.10)); }),
                new Skill("Good Gold Income", true, "", (m, lga) => { return m.Gold >= (lga.AverageGold + (lga.AverageGold * 0.10)); }),
                new Skill("Poor Gold Income", false, "https://www.youtube.com/watch?v=Gd8DirIRazI", (m, lga) => { return m.Gold < (lga.AverageGold - (lga.AverageGold * 0.10)); }),
                new Skill("Gets Jungle Objectives", true, "", (m, lga) => { return (m.DragonKills + m.BaronKills) >= (lga.AverageDragonKills + lga.AverageBaronKills) + ((lga.AverageDragonKills + lga.AverageBaronKills) * 0.10); }),
                new Skill("Forgets Jungle Objectives", false, "https://boosteria.org/guides/league-legends-objectives-guide", (m, lga) => { return (m.DragonKills + m.BaronKills) < (lga.AverageDragonKills + lga.AverageBaronKills) - ((lga.AverageDragonKills + lga.AverageBaronKills) * 0.10); }),
                new Skill("Good Healing", true, "", (m, lga) => { return m.HealingToChampions >= (lga.AverageHealingToChampions + (lga.AverageHealingToChampions * 0.10)); }),
                new Skill("Poor Healing", false, "https://www.metabomb.net/leagueoflegends/gameplay-guides/league-of-legends-support-guide-how-to-play-support", (m, lga) => { return m.HealingToChampions < (lga.AverageHealingToChampions - (lga.AverageHealingToChampions * 0.10)); }),
            };

            // Create dictionary to track 'average' role
            var countRoles = new Dictionary<string, int>()
            {
                {"TOP", 0},
                {"JUNGLE", 0},
                {"MIDDLE", 0},
                {"BOTTOM", 0},
                {"SUPPORT", 0}
            };

            // Create average data from select matches
            MatchParticipant averageVals = new MatchParticipant();

            foreach (string matchId in input.MatchIDs)
            {
                Match match = _dbContext?.Matches.Where(x => x.MatchID == matchId).FirstOrDefault();

                if (match != null)
                {
                    MatchParticipant participant = match.Participants.Where(x => x.PlayerName == input.Username).FirstOrDefault();

                    if (participant != null)
                    {
                        // Add values to the user's average across all matches in list
                        averageVals.Gold += participant.Gold;
                        averageVals.XP += participant.XP;
                        averageVals.Kills += participant.Kills;
                        averageVals.Deaths += participant.Deaths;
                        averageVals.TimeSpentDead += participant.TimeSpentDead;
                        averageVals.Assists += participant.Assists;
                        averageVals.BaronKills += participant.BaronKills;
                        averageVals.DragonKills += participant.DragonKills;
                        averageVals.MinionKills += participant.MinionKills;
                        averageVals.JungleMinionKills += participant.JungleMinionKills;
                        averageVals.VisionScore += participant.VisionScore;
                        averageVals.HealingToChampions += participant.HealingToChampions;

                        // Average Role is determined by most frequent role. Increase values in dictionary by 1
                        if (averageVals.ActualRole != null)
                            countRoles[averageVals.ActualRole] += 1;
                        else
                            countRoles["MIDDLE"] += 1;
                    }
                }
            }

            // Get the average of the user's selected matches
            averageVals.Gold /= input.MatchIDs.Length;
            averageVals.XP /= input.MatchIDs.Length;
            averageVals.Kills /= input.MatchIDs.Length;
            averageVals.Deaths /= input.MatchIDs.Length;
            averageVals.TimeSpentDead /= input.MatchIDs.Length;
            averageVals.Assists /= input.MatchIDs.Length;
            averageVals.BaronKills /= input.MatchIDs.Length;
            averageVals.DragonKills /= input.MatchIDs.Length;
            averageVals.MinionKills /= input.MatchIDs.Length;
            averageVals.JungleMinionKills /= input.MatchIDs.Length;
            averageVals.VisionScore /= input.MatchIDs.Length;
            averageVals.HealingToChampions /= input.MatchIDs.Length;

            // Get the user's most played role from match selection
            var averageRole = countRoles.OrderByDescending(x => x.Value).First().Key;

            //Compare unranked players to average ranked players
            if (input.Rank == null || input.Rank == "" || input.Rank == "Unranked")
            {
                input.Rank = "Gold";
                input.Division = "IV";
            }

            // Get global average that matches player's rank/division/role
            LolGlobalAverage globalAverage= _dbContext?.LolGlobalAverages.Where(x => x.RoleAndRankAndDivision == $"{averageRole}#{input.Rank}#{input.Division}").FirstOrDefault();

            List<Tuple<string, bool, string>> skillsToReturn = new List<Tuple<string, bool, string>>();

            // Change to return name of skill and boolean
            foreach (Skill skill in skillsList)
            {
                if (skill.checkExpression(averageVals, globalAverage)){
                    skillsToReturn.Add(Tuple.Create(skill.ID, skill.PositiveSkill, skill.url));
                }
            }

            return skillsToReturn;
        }
    }
}