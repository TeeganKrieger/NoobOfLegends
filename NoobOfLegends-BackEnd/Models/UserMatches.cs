using System.Text;
using NoobOfLegends.Models.Database;
using System.Collections.Generic;
using NoobOfLegends.APIS.RiotGamesApiTranslator;

namespace NoobOfLegends_BackEnd.Models.HTTP
{
    public class UserMatches
    {
        private readonly AppContext _dbContext;
        private readonly RiotGamesApiTranslator _translator;

        public UserMatches(RiotGamesApiTranslator translator, AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _translator = translator;
        }

        [HttpGet("api/matches/{username}/{tagline}")]
        public async match[] GetUserMatchData(string username, string tagline)
        {
            LolUser user = _dbContext.LolUser.Where(x => x.username == username && x.tagline == tagline).FirstOrDefault;
            if (user == null) {
                return new match[];
            }

            HashSet<Match> matches = new HashSet<Match>();

            string[] DatabaseMatchIds = user.MatchIdList;

            for (int i = 0; DatabaseMatchIds.Length; i++) {
                Match match = _dbContext.Match.Where(async x => x.MatchId == DatabaseMatchIds[i]).FirstOrDefault;
                if (match != null) {
                    matches.Add(match);
                }   
            }

            // get user id
            RiotPUUID puuid = await _translator.GetPUUID(username, tagline);
            // get most recent 100 riot matches
            string[] RiotMatchIds = _translator.GetMatchHistory(puuid);
            // if match is not found in our database then add it
            foreach (string riotMatchId in RiotMatchIds) {
                if (!matches.Contains(riotMatchId)) {
                    RiotMatch rm = _translator.GetMatch(matchId);
                    Match newMatch = _dbContext.Matches.FromRiotMatch(rm, out MatchParticipant[] participants);
                    _dbContext.Matches.Add( newMatch );
                    foreach (MatchParticipant p in participants) {
                        _dbContext.MatchParticipants.Add(p);
                    }
                    matches.Add( newMatch );
                }
            }
            _dbContext.SaveChanges();

            return matches.ToArray();
        }
    }
}
