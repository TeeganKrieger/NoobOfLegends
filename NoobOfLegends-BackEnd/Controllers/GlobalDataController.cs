using Microsoft.AspNetCore.Mvc;
using NoobOfLegends.Models.Services;
using NoobOfLegends_BackEnd.Models.GlobalAggregation;
using NoobOfLegends.APIs.RiotApi;
using NoobOfLegends.Models.Database;

namespace NoobOfLegends_BackEnd.Controllers
{
    public class GlobalDataController : Controller
    {
        private readonly AppDbContext _dbContext;

        public GlobalDataController(AppDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        [HttpPost("/api/globaldata/execute/{numberOfTopPlayers}/{numberOfMatches}")]
        public async Task<IActionResult> ExecuteAggregation(int numberOfTopPlayers, int numberOfMatches)
        {
            try
            {
                //Trigger execution of the GlobalDataAggregation System
                GlobalAggregation aggregator = new GlobalAggregation(_dbContext); //TODO: Once Database Corrections Branch is merged into main, replace null with _dbContext
                await aggregator.AggregateGlobalData(RiotRankedQueue.RankedSolo5v5, numberOfTopPlayers, numberOfMatches);
                return Ok(); //If it works
            } catch (Exception ex)
            {
                return StatusCode(500); //If it fails
            }
        }

        [HttpGet("/api/globaldata/get/{role}/{rank}/{division}")]
        public async Task<IActionResult> GetGlobalData(string role, string rank, string division)
        {
            try
            {
                // Query the database for the specific LolGlobalAverage object
                LolGlobalAverage avg = _dbContext?.LolGlobalAverages.Where(x => x.RoleAndRankAndDivision == $"{role}#{rank}#{division}").FirstOrDefault();

                // Calculate the averages and put them into a seperate return object.
                // This can be an anonymous object or a type.
                var avgData = new
                {
                    gold = avg.Gold / avg.NumberOfMatches,
                    minionKills = avg.MinionKills / avg.NumberOfMatches,
                    xp = avg.XP / avg.NumberOfMatches,
                    kills = avg.Kills / avg.NumberOfMatches,
                    deaths = avg.Deaths / avg.NumberOfMatches,
                    timeSpentDead = avg.TimeSpentDead / avg.NumberOfMatches,
                    assists = avg.Assists / avg.NumberOfMatches,
                    totalDamageDealt = avg.TotalDamageDealt / avg.NumberOfMatches,
                    baronKills = avg.BaronKills / avg.NumberOfMatches,
                    dragonKills = avg.DragonKills / avg.NumberOfMatches,
                    jungleMinionKills = avg.JungleMinionKills / avg.NumberOfMatches,
                    visionScore = avg.VisionScore / avg.NumberOfMatches,
                    healingToChampions = avg.HealingToChampions / avg.NumberOfMatches
                };
                return Ok(avgData); //If it works
            } catch (Exception ex)
            {
                return BadRequest(); //If it fails
            }
        }
    }
}
