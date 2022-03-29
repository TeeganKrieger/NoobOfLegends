using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NoobOfLegends.APIs.RiotApi;
using NoobOfLegends.Models.Database;
using NoobOfLegends.Models.Services;
using NoobOfLegends_BackEnd.Models;
using System.Linq;

namespace NoobOfLegends_BackEnd.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StatController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly RiotGamesApiTranslator _translator;

        public StatController(AppDbContext dbContext, RiotGamesApiTranslator translator)
        {
            _dbContext = dbContext;
            _translator = translator;
        }

        [HttpGet("api/matches/{username}/{tagline}")]
        public async Task<IActionResult> GetUserMatchData(string username, string tagline)
        {
            RiotPUUID puuid = await _translator.GetPUUID(username, tagline);

            if (puuid.puuid == null)
                return BadRequest();

            RiotSummoner summoner = await _translator.GetSummoner(puuid);

            if (summoner == null)
                return BadRequest();

            UserMatches matchesModel = new UserMatches(_translator, _dbContext);
            Match[] matches = await matchesModel.GetUserMatchData(username, tagline);
            MatchParticipant[] parts = matches.Select(x => x.Participants.Where(y => y.PlayerName == summoner.name).FirstOrDefault()).ToArray();
            return Ok(JsonConvert.SerializeObject(parts));
        }

    }
}