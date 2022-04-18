using NoobOfLegends.APIs.RiotApi;
using NoobOfLegends.Models.Database;
using NoobOfLegends.Models.Services;
using System.Threading.Tasks;

namespace NoobOfLegends_BackEnd.Models.SkillAnalysis 
{
    public class SkillAnalysis
    {
        private readonly AppDbContext _dbContext;

        #region CONSTRUCTOR

        public SkillAnalysis(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #endregion

        public class SkillAnalysisInput
        {
            public string rank { get; set; }
            public string division { get; set; }
            public string username { get; set; }
            public string[] matchIDs { get; set; }
        }

        public class Skill
        {
            public Func<MatchParticipant, LolGlobalAverage, bool> checkExpression;
            public bool PositiveSkill;
            public string ID;

            public Skill(string id, bool positiveSkill, Func<MatchParticipant, LolGlobalAverage, bool> checkExpression)
            {
                this.checkExpression = checkExpression;
                PositiveSkill = positiveSkill;
                ID = id;
            }
        }


        public async Task AnalyzeSkills(SkillAnalysisInput input)
        {
            // Margin of allowed error for skill checking
            // i.e. Good performance is higher than average + 10% and bad performance is lower than average - 10%
            double moe = 0.10;

            // Create skill list
            Skill highKillPart = new Skill("High Kill Participation", true, (m, lga) => { return m.KillParticipation < (lga.KillParticipation - (lga.KillParticipation * moe)); });
            Skill lowKillParticipation = new Skill("Low Kill Particiaption", false, (m, lga) => { return m.KillParticipation >= (lga.KillParticipation + (lga.KillParticipation * moe)); });
            Skill goodCS = new Skill("Good CS", true, (m, lga) => { return (m.MinionKills + m.JungleMinionKills) < (lga.MinionKills + lga.JungleMinionKills) - ((lga.MinionKills + lga.JungleMinionKills) * moe); });
            Skill poorCS = new Skill("Poor CS", false, (m, lga) => { return (m.MinionKills + m.JungleMinionKills) >= (lga.MinionKills + lga.JungleMinionKills) + ((lga.MinionKills + lga.JungleMinionKills) * moe); });
            //Skill diesTooMuch = new Skill("Dying Too Much", false, (m, lga) => { return m < lga; });
            //Skill killStealer = new Skill("Kill Stealer", false, (m, lga) => { return m < lga; });
            Skill goodVision = new Skill("Good Vision", true, (m, lga) => { return m.VisionScore < (lga.VisionScore - (lga.VisionScore * moe)); });
            Skill poorVision = new Skill("Poor Vision", false, (m, lga) => { return m.VisionScore >= (lga.VisionScore + (lga.VisionScore * moe)); });
            Skill goodXP = new Skill("Good XP", true, (m, lga) => { return m.XP < (lga.XP - (lga.XP * moe)); });
            Skill poorXP = new Skill("Poor XP", false, (m, lga) => { return m.XP >= (lga.XP + (lga.XP * moe)); });
            Skill goodGoldIncome= new Skill("Good Gold Income", true, (m, lga) => { return m.Gold < (lga.Gold - (lga.Gold * moe)); });
            Skill poorGoldIncome = new Skill("Poor Gold Income", false, (m, lga) => { return m.Gold >= (lga.Gold + (lga.Gold * moe)); });
            Skill getsJGObjective = new Skill("Gets Jungle Objectives", true, (m, lga) => { return (m.DragonKills + m.BaronKills) < (lga.DragonKills + lga.BaronKills) - ((lga.DragonKills + lga.BaronKills) * moe); });
            Skill forgetsJGObjective = new Skill("Forgets Jungle Objectives", false, (m, lga) => { return (m.DragonKills + m.BaronKills) >= (lga.DragonKills + lga.BaronKills) - ((lga.DragonKills + lga.BaronKills) * moe); });
            Skill goodHealing = new Skill("Good Healing", true, (m, lga) => { return m.HealingToChampions < (lga.HealingToChampions - (lga.HealingToChampions * moe)); });
            Skill poorHealing = new Skill("Poor Healing", false, (m, lga) => { return m.HealingToChampions >= (lga.HealingToChampions + (lga.HealingToChampions * moe)); });

            MatchParticipant averageVals = new MatchParticipant();
            
            // Get global average that matches player's rank
            LolGlobalAverage globalAverage = new LolGlobalAverage();


            foreach (string matchId in input.matchIDs)
            {
                Match match = _dbContext?.Matches.Where(x => x.MatchID == matchId).FirstOrDefault();

                if (match != null)
                {
                    MatchParticipant participant = match.Participants.Where(x => x.PlayerName == input.username).FirstOrDefault();

                    if (participant != null)
                    {
                        // Add values to the user's average across all matches in list
                        averageVals.Gold += participant.Gold;
                        averageVals.XP += participant.XP;
                        averageVals.Kills += participant.XP;
                        averageVals.Deaths += participant.Deaths;
                        averageVals.TimeSpentDead += participant.TimeSpentDead;
                        averageVals.Assists += participant.Assists;
                        averageVals.BaronKills += participant.BaronKills;
                        averageVals.DragonKills += participant.DragonKills;
                        averageVals.JungleMinionKills += participant.JungleMinionKills;
                        averageVals.VisionScore += participant.VisionScore;
                        averageVals.HealingToChampions += participant.HealingToChampions;

                        // Average Role is determined by most frequent role
                        // Have separate counters to track this??
                    }
                }
            }

            // Get the average of the user's selected matches
            averageVals.Gold /= input.matchIDs.Length;
            averageVals.XP /= input.matchIDs.Length;
            averageVals.Kills /= input.matchIDs.Length;
            averageVals.Deaths /= input.matchIDs.Length;
            averageVals.TimeSpentDead /= input.matchIDs.Length;
            averageVals.Assists /= input.matchIDs.Length;
            averageVals.BaronKills /= input.matchIDs.Length;
            averageVals.DragonKills /= input.matchIDs.Length;
            averageVals.JungleMinionKills /= input.matchIDs.Length;
            averageVals.VisionScore /= input.matchIDs.Length;
            averageVals.HealingToChampions /= input.matchIDs.Length;




            // Generate list of skills to check

            List<string> skillsToReturn = new List<string>()

                // Change to return name of skill and boolean
            foreach (Skill skill in SkillsList)
            {
                if (skill.checkExpression(averageVals, globalAverage){
                    skillsToReturn.Add(skill.ID);
                }
            }
        }
    }
}
