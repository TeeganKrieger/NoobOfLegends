using Microsoft.AspNetCore.Mvc;
using NoobOfLegends.Models.Services;

namespace NoobOfLegends_BackEnd.Controllers
{
    public class GlobalDataController : Controller
    {
        private readonly AppDbContext _dbContext;

        public GlobalDataController(AppDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        [HttpPost("/api/globaldata/execute")]
        public async Task<IActionResult> ExecuteAggregation()
        {
            //Trigger execution of the GlobalDataAggregation System

            return Ok(); //If it works
            return StatusCode(500); //If it fails
        }

        [HttpGet("/api/globaldata/get/{role}/{rank}/{division}")]
        public async Task<IActionResult> GetGlobalData(string role, string rank, string division)
        {
            //Query the database for the specific LolGlobalAverage object
            //Then calculate the averages and put them into a seperate return object.
            //This can be an anonymous object or a type.

            return Ok(/*Some Data Object*/); //If it works
            return BadRequest(); //If it fails
        }
    }
}
