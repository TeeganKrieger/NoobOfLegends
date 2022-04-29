using Microsoft.AspNetCore.Mvc;
using NoobOfLegends.Models.Services;
using NoobOfLegends_BackEnd.Models.SkillAnalysis;
using NoobOfLegends.APIs.RiotApi;
using NoobOfLegends.Models.Database;

namespace NoobOfLegends_BackEnd.Controllers
{
    /// <summary>
    /// Class that contains logic for Skill Analysis API endpoints.
    /// </summary>
    public class SkillAnalysisController : Controller
    {
        private readonly AppDbContext _dbContext;

        public SkillAnalysisController(AppDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        /// <summary>
        /// When provided a SkillAnalysisInput object as the body of the request, returns a list of skill objects.
        /// </summary>
        /// <param name="input">The input used to calculate skill analysis.</param>
        /// <returns>Returns a list of skill objects.</returns>
        [HttpPost("/api/skills/get/")]
        public async Task<IActionResult> GetSkills([FromBody] SkillAnalysisInput input)
        {
            try
            {
                SkillAnalysis analyzer = new SkillAnalysis(_dbContext);
                // = new SkillAnalysisInput(username, rank, division, matchIDs);
                List<Tuple<string, bool, string>> skills = await analyzer.AnalyzeSkills(input);

                List<SkillResult> results = new List<SkillResult>();

                foreach (Tuple<string, bool, string> skill in skills)
                {
                    results.Add(new SkillResult(skill));
                }
                return Ok(results); //If it works
            } catch (Exception ex)
            {
                return BadRequest(); //If it fails
            }
        }

        /// <summary>
        /// Data model used for the input to the GetSkills api call.
        /// </summary>
        public class SkillAnalysisInput
        {
            public SkillAnalysisInput()
            {

            }

            public SkillAnalysisInput(string username, string rank, string division, string[] matchIDs)
            {
                this.Username = username;
                this.Rank = rank;
                this.Division = division;
                this.MatchIDs = matchIDs;
            }

            public string Username { get; set; }
            public string Rank { get; set; }
            public string Division { get; set; }
            public string[] MatchIDs { get; set; }
        }

        /// <summary>
        /// Data model used as the return result from the GetSkills api call.
        /// </summary>
        public class SkillResult
        {
            public string SkillName { get; set; }
            public bool Good { get; set; }
            public string Url { get; set; }

            public SkillResult(Tuple<string, bool, string> tuple)
            {
                SkillName = tuple.Item1;
                Good = tuple.Item2;
                Url = tuple.Item3;
            }
        }
    }
}
