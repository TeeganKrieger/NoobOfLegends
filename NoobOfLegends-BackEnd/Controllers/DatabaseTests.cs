using Microsoft.AspNetCore.Mvc;
using NoobOfLegends.APIs.RiotApi;
using NoobOfLegends.Models.Database;
using NoobOfLegends.Models.Services;

namespace NoobOfLegends_BackEnd.Controllers
{
    public class DatabaseTests : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly RiotGamesApiTranslator _translator;


        public DatabaseTests(AppDbContext _dbContext, RiotGamesApiTranslator _translator)
        {
            this._dbContext = _dbContext;
            this._translator = _translator;
        }

        //[HttpGet("/api/tests/database/lazy")]
        public async Task<IActionResult> Index()
        {
            RiotPUUID puuid = await _translator.GetPUUID("Saltymate8", "NA1");

            LolUser user = new LolUser() { UsernameAndTagline = "Saltymate8#NA1", SummonerName = "Saltymate8" };
            _dbContext.LolUsers.Add(user);
            _dbContext.SaveChanges();

            await Task.Delay(1200);

            string[] matchHistory = await _translator.GetMatchHistory(puuid, count: 5);

            await Task.Delay(1200);

            foreach (string match in matchHistory)
            {
                RiotMatch rm = await _translator.GetMatch(match);
                await Task.Delay(1200);
                if (rm == null)
                {
                    System.Diagnostics.Debug.WriteLine($"Skipping Match {match} due to likely rate limit exceeding.");
                    continue;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"Processing match {match}.");
                }

                Match.MatchAndParticipants m = await Match.FromRiotMatch(_dbContext, rm);

                _dbContext.Matches.Add(m.Match);
                foreach (MatchParticipant p in m.MatchParticipants)
                    _dbContext.MatchParticipants.Add(p);
            }
            _dbContext.SaveChanges();

            user = _dbContext.LolUsers.Where(x => x.UsernameAndTagline == "Saltymate8#NA1").FirstOrDefault();

            foreach (Match m in user.Matches)
            {
                System.Diagnostics.Debug.WriteLine($"Match: {m.MatchID}");

                foreach (MatchParticipant p in m.Participants)
                {
                    System.Diagnostics.Debug.WriteLine($"Participant: {p.PlayerName}");
                }
            }

            return Ok();
        }
    }
}
