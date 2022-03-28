using Microsoft.AspNetCore.Mvc;

namespace NoobOfLegends_BackEnd.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StatController : ControllerBase
    {
        public StatController(ILogger<HelloWorldController> logger)
        {
            
        }

        [HttpGet("api/matches/{username}/{tagline}")]
        public string GetUserMatchData(string username, string tagline)
        {
            // make api call
            // call getusermatchdata on model
            // return json
        }

    }
}