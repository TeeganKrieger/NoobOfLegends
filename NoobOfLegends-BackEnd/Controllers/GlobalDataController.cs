using Microsoft.AspNetCore.Mvc;
using NoobOfLegends.Models.Services;
using NoobOfLegends_BackEnd.Models.GlobalAggregation;
using NoobOfLegends.APIs.RiotApi;
using NoobOfLegends.Models.Database;

namespace NoobOfLegends_BackEnd.Controllers
{

    /// <summary>
    /// Class that contains logic for Global Data Aggregation API endpoints.
    /// </summary>
    public class GlobalDataController : Controller
    {
        private readonly AppDbContext _dbContext;

        public GlobalDataController(AppDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        /// <summary>
        /// Begins data aggregation when a request is made to this endpoint.
        /// </summary>
        /// <param name="numberOfTopPlayers">The number of top players to aggregate in a rank and division.</param>
        /// <param name="numberOfMatches">The number of matches per top player to add to the aggregation model.</param>
        /// <returns>200 if successful, otherwise returns 500.</returns>
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

        /// <summary>
        /// Fetches the global averages for a given role, rank, and division from the database.
        /// </summary>
        /// <param name="role">The role to use when querying the database.</param>
        /// <param name="rank">The rank to use when querying the database.</param>
        /// <param name="division">The division to use when querying the database.</param>
        /// <returns>The global averages for a given role, rank, and division.</returns>
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
