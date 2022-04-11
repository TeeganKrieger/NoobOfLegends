using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NoobOfLegends.APIs.RiotApi;
using NoobOfLegends.Models.Database;
using NoobOfLegends.Models.Services;
using NoobOfLegends_BackEnd.Models;
using System.Collections.Concurrent;
using System.Linq;

namespace NoobOfLegends_BackEnd.Controllers
{
    public class StatController : ControllerBase
    {
        private static Dictionary<string, UserResultsTask> progressiveLoadingTasks = new Dictionary<string, UserResultsTask>();

        //private readonly AppDbContext _dbContext;
        //private readonly RiotGamesApiTranslator _translator;
        private readonly IServiceProvider _serviceProvider;

        public StatController(IServiceProvider provider)
        {
            this._serviceProvider = provider;
            //_dbContext = dbContext;
            //_translator = translator;
        }

        [HttpGet("/api/user/check/{checkpointId}")]
        public async Task<IActionResult> GetCheckpoint(string checkpointId)
        {
            if (progressiveLoadingTasks.ContainsKey(checkpointId))
            {
                UserResultsTask task = progressiveLoadingTasks[checkpointId];
                return Ok(task.GetCheckpointResult());
            }
            else
                return BadRequest();
        }

        [HttpGet("/api/user/start/{username}/{tagline}")]
        public async Task<IActionResult> GetUserResults(string username, string tagline)
        {
            UserResultsTask task = new UserResultsTask(_serviceProvider);
            task.Setup(username, tagline);

            Guid id = Guid.NewGuid();
            progressiveLoadingTasks.Add(id.ToString(), task);

            Task.Run(task.StartAsync);

            return Ok(id.ToString());
        }

        //[HttpGet("api/info/{username}/{tagline}")]
        //public async Task<IActionResult> GetUserInfo(string username, string tagline)
        //{
        //    RiotPUUID puuid = await _translator.GetPUUID(username, tagline);

        //    if (puuid.puuid == null)
        //        return BadRequest();

        //    RiotSummoner summoner = await _translator.GetSummoner(puuid);

        //    if (summoner == null)
        //        return BadRequest();

        //    RiotRankedResult[] rankedResults = await _translator.GetSummonerRankedData(summoner);

        //    return Ok(new UserInfoResult(username, tagline, rankedResults));
        //}

        //[HttpGet("api/matches/{username}/{tagline}")]
        //public async Task<IActionResult> GetUserMatchData(string username, string tagline)
        //{
        //    RiotPUUID puuid = await _translator.GetPUUID(username, tagline);

        //    if (puuid.puuid == null)
        //        return BadRequest();

        //    RiotSummoner summoner = await _translator.GetSummoner(puuid);

        //    if (summoner == null)
        //        return BadRequest();

        //    UserMatches matchesModel = new UserMatches(_translator, _dbContext);
        //    Match[] matches = await matchesModel.GetUserMatchData(username, tagline);

        //    List<MatchDataResult> results = new List<MatchDataResult>();

        //    foreach (Match match in matches)
        //        results.Add(new MatchDataResult(match, summoner));

        //    return Ok(JsonConvert.SerializeObject(results.OrderByDescending(x => x.playedOn)));
        //}
    }
}