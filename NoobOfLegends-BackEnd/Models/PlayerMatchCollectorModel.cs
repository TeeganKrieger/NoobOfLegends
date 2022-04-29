using System.Text;
using NoobOfLegends.Models.Database;
using System.Collections.Generic;
using NoobOfLegends.APIs.RiotApi;
using Microsoft.AspNetCore.Mvc;
using NoobOfLegends.Models.Services;

namespace NoobOfLegends_BackEnd.Models
{
    /// <summary>
    /// Logic model that collects user data and stores it in the database.
    /// </summary>
    public class PlayerMatchCollectorModel
    {
        private readonly AppDbContext _dbContext;
        private readonly RiotGamesApiTranslator _translator;

        public PlayerMatchCollectorModel(RiotGamesApiTranslator translator, AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _translator = translator;
        }

        /// <summary>
        /// Collect a players's match history and store it in the database.
        /// </summary>
        /// <param name="username">The username to use when collecting data.</param>
        /// <param name="tagline">The username to use when collecting data.</param>
        /// <returns>A list of the 100 most recent matches a player has played.</returns>
        public async Task<Match[]> GetUserMatchData(string username, string tagline)
        {
            System.Diagnostics.Debug.Write($"Fetching User {username}#{tagline} from database: ");

            LolUser user = _dbContext.LolUsers.Where(x => x.UsernameAndTagline == $"{username}#{tagline}").FirstOrDefault();
            RiotPUUID puuid;
            if (user == null)
            {
                System.Diagnostics.Debug.Write($"Failed! Fetching from API instead: ");

                puuid = await _translator.GetPUUID(username, tagline);

                if (puuid.puuid != null)
                {

                    RiotSummoner sum = await _translator.GetSummoner(puuid);

                    if (sum != null)
                    {
                        System.Diagnostics.Debug.WriteLine($"Success!");
                        user = new LolUser()
                        {
                            UsernameAndTagline = $"{username}#{tagline}",
                            Matches = new List<Match>(),
                            SummonerName = sum.name,
                        };

                        _dbContext.LolUsers.Add(user);
                        _dbContext.SaveChanges();
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"Failed! Summoner not found!");
                        return null;
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"Failed! PUUID Not Found!");
                    return null;
                }
            }

            List<Match> matches = new List<Match>(user.Matches);
            HashSet<string> matchesHashset = new HashSet<string>(matches.Select(x => x.MatchID));

            System.Diagnostics.Debug.Write($"Fetching match history from API: ");
            puuid = await _translator.GetPUUID(username, tagline);

            if (puuid.puuid == null)
            {
                System.Diagnostics.Debug.WriteLine($"Failed! PUUID not found!");
                return null;
            }

            string[] matchHistory = await _translator.GetMatchHistory(puuid);

            if (matchHistory == null)
            {
                System.Diagnostics.Debug.WriteLine($"Failed! Match History not found!");
                return null;
            }

            foreach (string matchId in matchHistory)
            {
                System.Diagnostics.Debug.Write($"Fetching data for match {matchId} from API: ");
                //Fetch match from api
                if (!matchesHashset.Contains(matchId))
                {
                    Match existingMatchOfSameId = _dbContext.Matches.Where(x => x.MatchID == matchId).FirstOrDefault();
                    if (existingMatchOfSameId == null)
                    {
                        RiotMatch riotMatch = await _translator.GetMatch(matchId);

                        if (riotMatch != null)
                        {
                            System.Diagnostics.Debug.WriteLine($"Success! Translating Match...");
                            Match match = Match.FromRiotMatch(riotMatch);
                            
                            match.Users.Add(user);
                            user.Matches.Add(match);

                            _dbContext.Update(user);
                            _dbContext.Matches.Add(match);
                            _dbContext.SaveChanges();

                            matches.Add(match);
                            matchesHashset.Add(matchId);
                        }
                        else
                            System.Diagnostics.Debug.WriteLine($"Failed! Match Not Found: ");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"Success! Match Already In DB. Associating with user {user.SummonerName}");
                        user.Matches.Add(existingMatchOfSameId);
                        existingMatchOfSameId.Users.Add(user);
                        _dbContext.Update(user);
                        _dbContext.Update(existingMatchOfSameId);
                        _dbContext.SaveChanges();
                    }
                }
                else
                    System.Diagnostics.Debug.WriteLine($"Match Already In Database and Associated");
            }

            return matches.Skip(Math.Max(matches.Count - 100, 0)).ToArray();
        }
    }
}
