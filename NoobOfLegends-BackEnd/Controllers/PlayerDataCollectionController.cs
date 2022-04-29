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
    /// <summary>
    /// Class that contains logic for Player Data Collection API endpoints.
    /// </summary>
    public class PlayerDataCollectionController : ControllerBase
    {
        private static Dictionary<string, UserResultsTask> progressiveLoadingTasks = new Dictionary<string, UserResultsTask>();

        //private readonly AppDbContext _dbContext;
        //private readonly RiotGamesApiTranslator _translator;
        private readonly IServiceProvider _serviceProvider;

        public PlayerDataCollectionController(IServiceProvider provider)
        {
            this._serviceProvider = provider;
            //_dbContext = dbContext;
            //_translator = translator;
        }

        /// <summary>
        /// Gets the current progress for a given checkpointID. Also returns the differences from the last to the current checkpoint check.
        /// </summary>
        /// <param name="checkpointId">The checkpointID to check against.</param>
        /// <returns>A checkpoint result object.</returns>
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

        /// <summary>
        /// Starts a user data collection task.
        /// </summary>
        /// <param name="username">The username of the user to collect data on.</param>
        /// <param name="tagline">The tagline of the user to collect data on.</param>
        /// <returns>Returns a checkpointID that can be used to periodically check progress of this task.</returns>
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
    }
}