using NoobOfLegends.APIs.RiotApi;
using NoobOfLegends.Models.Database;
using NoobOfLegends.Models.Services;
using System.Threading.Tasks;
using System.Collections.Generic;

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
            public SkillAnalysisInput(string username, string rank, string division, string[] matchIDs)
            {
                this.username = username;
                this.rank = rank;
                this.division = division;
                this.matchIDs = matchIDs;
            }

            public string username { get; set; }
            public string rank { get; set; }
            public string division { get; set; }
            public string[] matchIDs { get; set; }
        }

        public class Skill
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

        // TODO: Finish skills list, get lolGlobalAverage for role/rank/division
        public async Task<List<Tuple<string, bool, string>>> AnalyzeSkills(SkillAnalysisInput input) 
        {
            // Margin of allowed error for skill checking
            // i.e. Good performance is higher than average + 10% and bad performance is lower than average - 10%

            // Create skill list
            List<Skill> skillsList = new List<Skill>()
            {
                new Skill("High Kill Participation", true, "", (m, lga) => { return m.KillParticipation < (lga.KillParticipation - (lga.KillParticipation * 0.10)); }),
                new Skill("Low Kill Particiaption", false, "https://www.youtube.com/watch?v=YrxoA8eFug4", (m, lga) => { return m.KillParticipation >= (lga.KillParticipation + (lga.KillParticipation * 0.10)); }),
                new Skill("Good CS", true, "", (m, lga) => { return (m.MinionKills + m.JungleMinionKills) < (lga.MinionKills + lga.JungleMinionKills) - ((lga.MinionKills + lga.JungleMinionKills) * 0.10); }),
                new Skill("Poor CS", false, "https://www.youtube.com/watch?v=jOSyf1NQspo", (m, lga) => { return (m.MinionKills + m.JungleMinionKills) >= (lga.MinionKills + lga.JungleMinionKills) + ((lga.MinionKills + lga.JungleMinionKills) * 0.10); }),
                //Skill diesTooMuch = new Skill("Dying Too Much", false, (m, lga) => { return m < lga; });
                //Skill killStealer = new Skill("Kill Stealer", false, (m, lga) => { return m < lga; });
                new Skill("Good Vision", true, "", (m, lga) => { return m.VisionScore < (lga.VisionScore - (lga.VisionScore * 0.10)); }),
                new Skill("Poor Vision", false, "https://youtu.be/6cXqzH2vMH8", (m, lga) => { return m.VisionScore >= (lga.VisionScore + (lga.VisionScore * 0.10)); }),
                new Skill("Good XP", true, "", (m, lga) => { return m.XP < (lga.XP - (lga.XP * 0.10)); }),
                new Skill("Poor XP", false, "https://www.youtube.com/watch?v=cL6cWGQtocw", (m, lga) => { return m.XP >= (lga.XP + (lga.XP * 0.10)); }),
                new Skill("Good Gold Income", true, "", (m, lga) => { return m.Gold < (lga.Gold - (lga.Gold * 0.10)); }),
                new Skill("Poor Gold Income", false, "https://www.youtube.com/watch?v=Gd8DirIRazI", (m, lga) => { return m.Gold >= (lga.Gold + (lga.Gold * 0.10)); }),
                new Skill("Gets Jungle Objectives", true, "", (m, lga) => { return (m.DragonKills + m.BaronKills) < (lga.DragonKills + lga.BaronKills) - ((lga.DragonKills + lga.BaronKills) * 0.10); }),
                new Skill("Forgets Jungle Objectives", false, "https://boosteria.org/guides/league-legends-objectives-guide", (m, lga) => { return (m.DragonKills + m.BaronKills) >= (lga.DragonKills + lga.BaronKills) - ((lga.DragonKills + lga.BaronKills) * 0.10); }),
                new Skill("Good Healing", true, "", (m, lga) => { return m.HealingToChampions < (lga.HealingToChampions - (lga.HealingToChampions * 0.10)); }),
                new Skill("Poor Healing", false, "https://www.metabomb.net/leagueoflegends/gameplay-guides/league-of-legends-support-guide-how-to-play-support", (m, lga) => { return m.HealingToChampions >= (lga.HealingToChampions + (lga.HealingToChampions * 0.10)); }),
            };

            // Create dictionary to track 'average' role
            var countRoles = new Dictionary<string, int>()
            {
                {"TOP", 0},
                {"JUNGLE", 0},
                {"MID", 0},
                {"BOTTOM", 0},
                {"SUPPORT", 0}
            };

            // Create average data from select matches
            MatchParticipant averageVals = new MatchParticipant();

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

                        // Average Role is determined by most frequent role. Increase values in dictionary by 1
                        countRoles[averageVals.ActualRole] += 1;
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

            // Get the user's most played role from match selection
            var averageRole = countRoles.OrderByDescending(x => x.Value).First();

            // Get global average that matches player's rank/division/role
            LolGlobalAverage globalAverage= _dbContext?.LolGlobalAverages.Where(x => x.RoleAndRankAndDivision == $"{averageRole}#{input.rank}#{input.division}").FirstOrDefault();

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