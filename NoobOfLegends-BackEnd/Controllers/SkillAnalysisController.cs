using Microsoft.AspNetCore.Mvc;
using NoobOfLegends.Models.Services;
using NoobOfLegends_BackEnd.Models.SkillAnalysis;
using NoobOfLegends.APIs.RiotApi;
using NoobOfLegends.Models.Database;

namespace NoobOfLegends_BackEnd.Controllers
{
    public class SkillAnalysisController : Controller
    {
        private readonly AppDbContext _dbContext;

        public SkillAnalysisController(AppDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        [HttpGet("/api/skills/get/{username}/{rank}/{division}/{matchIDs}")]
        public async Task<IActionResult> GetSkills(string username, string rank, string division, string[] matchIDs)
        {
            try
            {
                SkillAnalysis analyzer = new SkillAnalysis(_dbContext);
                SkillAnalysis.SkillAnalysisInput input = new SkillAnalysis.SkillAnalysisInput(username, rank, division, matchIDs);
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
