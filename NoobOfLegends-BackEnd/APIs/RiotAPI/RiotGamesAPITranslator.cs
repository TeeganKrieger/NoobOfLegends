using Newtonsoft.Json;
using NoobOfLegends_BackEnd.Models.HTTP;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;

namespace NoobOfLegends.APIs.RiotApi
{
    
    /// <summary>
    /// An Api Translator for the Riot Games Api.
    /// Implements select endpoints that are useful to this project.
    /// </summary>
    public class RiotGamesApiTranslator : IDisposable
    {
        private enum RiotApiRegion
        {
            Americas,
            Asia,
            Europe
        }

        private const string ARG_NULL_EXC_MSG = "The arguement '{0}' cannot be null!";
        private const string ARG_DFT_EXC_MSG = "The arguement '{0}' cannot be the default value!";

        private const string API_KEY = "RGAPI-133db675-abd3-4126-9d27-0f2f59190fcf";

        private const string URL_BASE_AMERICAS = "https://americas.api.riotgames.com/";
        private const string URL_BASE_ASIA = "https://asia.api.riotgames.com/";
        private const string URL_BASE_EUROPE = "https://europe.api.riotgames.com/";

        private const string URL_BASE_BR1 = "https://br1.api.riotgames.com/";
        private const string URL_BASE_EUN1 = "https://eun1.api.riotgames.com/";
        private const string URL_BASE_EUW1 = "https://euw1.api.riotgames.com/";
        private const string URL_BASE_JP1 = "https://jp1.api.riotgames.com/";
        private const string URL_BASE_KR = "https://kr.api.riotgames.com/";
        private const string URL_BASE_LA1 = "https://la1.api.riotgames.com/";
        private const string URL_BASE_LA2 = "https://la2.api.riotgames.com/";
        private const string URL_BASE_NA1 = "https://na1.api.riotgames.com/";
        private const string URL_BASE_OC1 = "https://oc1.api.riotgames.com/";
        private const string URL_BASE_TR1 = "https://tr1.api.riotgames.com/";
        private const string URL_BASE_RU = "https://ru.api.riotgames.com/";

        private const string URL_ACCOUNT_BY_ID = "/riot/account/v1/accounts/by-riot-id/{0}/{1}";

        private const string URL_SUMMONER_BY_PUUID = "/lol/summoner/v4/summoners/by-puuid/{0}";

        private const string URL_MATCH_HISTORY = "/lol/match/v5/matches/by-puuid/{0}/ids";
        private const string URL_MATCH = "/lol/match/v5/matches/{0}";
        private const string URL_MATCH_TIMELINE = "/lol/match/v5/matches/{0}/timeline";

        private const string URL_RANKED_BY_SUMMONER = "/lol/league/v4/entries/by-summoner/{0}";
        private const string URL_RANKED_LEAGUES = "/lol/league-exp/v4/entries/{0}/{1}/{2}";

        private RiotApiPlatform platform;
        private RiotApiRegion region;

        private Dictionary<string, HttpClient> httpClients;


        #region CONSTRUCTOR/DISPOSE

        public RiotGamesApiTranslator()
        {
            httpClients = new Dictionary<string, HttpClient>();

            platform = RiotApiPlatform.NA1;
            region = RiotApiRegion.Americas;
        }

        public void Dispose()
        {
            foreach (HttpClient client in httpClients.Values)
                client.Dispose();
        }

        #endregion


        #region MISC

        /// <summary>
        /// Set the platform used to make api calls.
        /// </summary>
        /// <param name="platform">The platform to use.</param>
        public void SetPlatform(RiotApiPlatform platform)
        {
            this.platform = platform;

            switch (platform)
            {
                case RiotApiPlatform.BR1:
                case RiotApiPlatform.LA1:
                case RiotApiPlatform.LA2:
                case RiotApiPlatform.NA1:
                    this.region = RiotApiRegion.Americas;
                    break;

                case RiotApiPlatform.EUN1:
                case RiotApiPlatform.EUW1:
                case RiotApiPlatform.TR1:
                    this.region = RiotApiRegion.Europe;
                    break;

                case RiotApiPlatform.JP1:
                case RiotApiPlatform.KR:
                case RiotApiPlatform.RU:
                case RiotApiPlatform.OC1:
                    this.region = RiotApiRegion.Asia;
                    break;
            }
        }

        #endregion


        #region ENDPOINTS


        #region ACCOUNTSV1

        /// <summary>
        /// Get the PUUID of a Riot Games Accounts using a username and tagline.
        /// </summary>
        /// <remarks>
        /// Input string should be in the format "username#tagline".
        /// </remarks>
        /// <param name="usernameAndTagline">The username and tagline used to lookup the riot account.</param>
        /// <returns>A RiotPUUID instance.</returns>
        /// <exception cref="ArgumentException">Thrown if the input string is not valid.</exception>
        /// <exception cref="ArgumentNullException">Thrown if the usernameAndTagline parameter is null.</exception>
        public async Task<RiotPUUID> GetPUUID(string usernameAndTagline)
        {
            if (usernameAndTagline == null)
                throw new ArgumentNullException(nameof(usernameAndTagline), string.Format(ARG_NULL_EXC_MSG, nameof(usernameAndTagline)));

            string[] split = usernameAndTagline.Split('#');
            if (split.Length != 2)
                throw new ArgumentException("The provided username and tagline could not be split. Ensure your input is in the form \"Username#Tagline\"!", nameof(usernameAndTagline));

            return await GetPUUID(split[0], split[1]);
        }

        /// <summary>
        /// Get the PUUID of a Riot Games Accounts using a username and tagline.
        /// </summary>
        /// <param name="username">The username used to lookup the riot account.</param>
        /// <param name="tagline">The tagline used to lookup the riot account.</param>
        /// <returns>A RiotPUUID instance.</returns>
        /// <exception cref="ArgumentNullException">Thrown if either the username or tagline parameters are null.</exception>
        public async Task<RiotPUUID> GetPUUID(string username, string tagline)
        {
            if (username == null)
                throw new ArgumentNullException(nameof(username), string.Format(ARG_NULL_EXC_MSG, nameof(username)));
            if (tagline == null)
                throw new ArgumentNullException(nameof(tagline), string.Format(ARG_NULL_EXC_MSG, nameof(tagline)));

            //Fetch Appropriate HTTP Client
            HttpClient client = GetRegionClient();

            //Build parameter string
            string parameters = string.Format(URL_ACCOUNT_BY_ID, username, tagline);

            //Make http request
            HttpResponseMessage response = await client.GetAsync(parameters);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();

                //Parse the JSON into the appropriate data object
                RiotPUUID puuid = JsonConvert.DeserializeObject<RiotPUUID>(json);

                //Return the PUUID from the data object
                return puuid;
            }
            else
            {
                return default;
            }
        }

        #endregion


        #region SUMMONERV4

        /// <summary>
        /// Get the Summoner information for a riot account.
        /// </summary>
        /// <param name="puuid">The RiotPUUID used to get the Summoner information.</param>
        /// <returns>A RiotSummoner object.</returns>
        /// <exception cref="ArgumentException">Thrown if the provided puuid is default.</exception>
        public async Task<RiotSummoner> GetSummoner(RiotPUUID puuid)
        {
            if (puuid.Equals(default(RiotPUUID)))
                throw new ArgumentException(string.Format(ARG_DFT_EXC_MSG, nameof(puuid)),nameof(puuid));

            //Fetch Appropriate HTTP Client
            HttpClient client = GetPlatformClient();

            //Build parameter string
            string parameters = string.Format(URL_SUMMONER_BY_PUUID, puuid);

            //Make http request
            HttpResponseMessage response = await client.GetAsync(parameters);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();

                //Parse the JSON into the appropriate data object
                RiotSummoner summoner = JsonConvert.DeserializeObject<RiotSummoner>(json);

                //Return the PUUID from the data object
                return summoner;
            }
            else
            {
                return null;
            }
        }

        #endregion


        #region MATCHV5

        /// <summary>
        /// Get a user's match history using their username and tagline.
        /// </summary>
        /// <param name="puuid">A RiotPUUID respresenting the user.</param>
        /// <param name="startTime">The time used to begin the search for matches. This is an Epoch Timestamp in seconds.</param>
        /// <param name="endTime">The time used to end the search for matches. This is an Epoch Timestamp in seconds.</param>
        /// <param name="queue">Filter matches by a queue id.</param>
        /// <param name="type">Filter matches by a match type.</param>
        /// <param name="start">The index within the given timestamp to start fetching results at.</param>
        /// <param name="count">The number of results to fetch.</param>
        /// <returns>A list of match IDs</returns>
        /// <exception cref="ArgumentException">Thrown if the provided puuid is default.</exception>
        public async Task<string[]> GetMatchHistory(RiotPUUID puuid, long startTime = -1, long endTime = -1, RiotQueue queue = RiotQueue.None, RiotGameType type = RiotGameType.None, int start = 0, int count = 100)
        {
            if (puuid.Equals(default(RiotPUUID)))
                throw new ArgumentException(string.Format(ARG_DFT_EXC_MSG, nameof(puuid)), nameof(puuid));

            //Fetch Appropriate HTTP Client
            HttpClient client = GetRegionClient();

            //Build parameter string
            string parameters = string.Format(URL_MATCH_HISTORY, puuid);

            //Build Query String
            QueryBuilder builder = new QueryBuilder();
            
            if (startTime > -1)
                builder.AddQuery(nameof(startTime), startTime);
            if (endTime > -1)
                builder.AddQuery(nameof(endTime), endTime);
            if (queue != RiotQueue.None)
                builder.AddQuery(nameof(queue), (int)queue);
            if (type != RiotGameType.None)
                builder.AddQuery(nameof(type), type.ToString().ToLower());
            if (start > -1)
                builder.AddQuery(nameof(start), start);
            if (count > -1)
                builder.AddQuery(nameof(count), count);

            //Make http request
            string endpoint = parameters + builder.ToString();

            HttpResponseMessage response = await client.GetAsync(endpoint);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();

                //Parse the JSON into the appropriate data object
                string[] matchIds = JsonConvert.DeserializeObject<string[]>(json);

                //Return the PUUID from the data object
                return matchIds;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Get a specific match using its Match ID.
        /// </summary>
        /// <param name="matchId">The ID of the match to lookup.</param>
        /// <returns>A RiotMatch object.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the matchId parameter is null.</exception>
        public async Task<RiotMatch> GetMatch(string matchId)
        {
            if (matchId == null)
                throw new ArgumentNullException(nameof(matchId), string.Format(ARG_NULL_EXC_MSG, nameof(matchId)));

            //Fetch Appropriate HTTP Client
            HttpClient client = GetRegionClient();

            //Build parameter string
            string parameters = string.Format(URL_MATCH, matchId);

            //Make http request
            HttpResponseMessage response = await client.GetAsync(parameters);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();

                //Parse the JSON into the appropriate data object
                RiotMatch match = JsonConvert.DeserializeObject<RiotMatch>(json);

                //Return the PUUID from the data object
                return match;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Get the timeline of a specific match using its Match ID.
        /// </summary>
        /// <param name="matchId">The ID of the match to lookup.</param>
        /// <returns>A RiotMatchTimeline object.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the matchId parameter is null.</exception>
        public async Task<RiotMatchTimeline> GetMatchTimeline(string matchId)
        {
            if (matchId == null)
                throw new ArgumentNullException(nameof(matchId), string.Format(ARG_NULL_EXC_MSG, nameof(matchId)));

            //Fetch Appropriate HTTP Client
            HttpClient client = GetRegionClient();

            //Build parameter string
            string parameters = string.Format(URL_MATCH_TIMELINE, matchId);

            //Make http request
            HttpResponseMessage response = await client.GetAsync(parameters);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();

                //Parse the JSON into the appropriate data object
                RiotMatchTimeline match = JsonConvert.DeserializeObject<RiotMatchTimeline>(json);

                //Return the PUUID from the data object
                return match;
            }
            else
            {
                return null;
            }
        }

        #endregion


        #region LEAGUEV4

        /// <summary>
        /// Get Ranked Results of a specific summoner.
        /// </summary>
        /// <param name="summoner">The summoner to gather ranked results for.</param>
        /// <returns>An array of RiotRankedResult objects.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the summoner parameter is null.</exception>
        public async Task<RiotRankedResult[]> GetSummonerRankedData(RiotSummoner summoner)
        {
            if (summoner == null)
                throw new ArgumentNullException(nameof(summoner), string.Format(ARG_NULL_EXC_MSG, nameof(summoner)));

            //Fetch Appropriate HTTP Client
            HttpClient client = GetPlatformClient();

            //Build parameter string
            string parameters = string.Format(URL_RANKED_BY_SUMMONER, summoner.id);

            //Make http request
            HttpResponseMessage response = await client.GetAsync(parameters);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();

                //Parse the JSON into the appropriate data object
                RiotRankedResult[] results = JsonConvert.DeserializeObject<RiotRankedResult[]>(json);

                //Return the PUUID from the data object
                return results;
            }
            else
            {
                return null;
            }
        }
        
        /// <summary>
        /// Get an array of summoner ranked results for a given queue, rank and division.
        /// </summary>
        /// <param name="queue">The queue to gather results from.</param>
        /// <param name="tier">The tier to gather results from.</param>
        /// <param name="division">The division to gather results from.</param>
        /// <returns>An array of RiotRankedResult objects.</returns>
        public async Task<RiotRankedResult[]> GetListOfSummonersInRank(RiotRankedQueue queue, RiotRankedTier tier, RiotRankedDivision division)
        {
            //Fix Division Issue
            if (tier == RiotRankedTier.Master || tier == RiotRankedTier.Grandmaster || tier == RiotRankedTier.Challenger)
                division = RiotRankedDivision.I;

            //Fetch Appropriate HTTP Client
            HttpClient client = GetPlatformClient();

            //Create Parameters
            string parameters = string.Format(URL_RANKED_LEAGUES, GetRankedQueueString(queue), tier.ToString().ToUpper(), division.ToString());

            //Make http request
            HttpResponseMessage response = await client.GetAsync(parameters);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();

                //Parse the JSON into the appropriate data object
                RiotRankedResult[] results = JsonConvert.DeserializeObject<RiotRankedResult[]>(json);

                //Return the PUUID from the data object
                return results;
            }
            else
            {
                return null;
            }
        }

        #endregion


        #endregion


        #region CLIENT HELPERS

        /// <summary>
        /// Get the HttpClient object associated with the current platform.
        /// </summary>
        /// <returns>An HttpClient object.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private HttpClient GetPlatformClient()
        {
            string platformString;
            switch (platform)
            {
                case RiotApiPlatform.BR1:
                    platformString = URL_BASE_BR1;
                    break;
                case RiotApiPlatform.EUN1:
                    platformString = URL_BASE_EUN1;
                    break;
                case RiotApiPlatform.EUW1:
                    platformString = URL_BASE_EUW1;
                    break;
                case RiotApiPlatform.JP1:
                    platformString = URL_BASE_JP1;
                    break;
                case RiotApiPlatform.KR:
                    platformString = URL_BASE_KR;
                    break;
                case RiotApiPlatform.LA1:
                    platformString = URL_BASE_LA1;
                    break;
                case RiotApiPlatform.LA2:
                    platformString = URL_BASE_LA2;
                    break;
                case RiotApiPlatform.NA1:
                    platformString = URL_BASE_NA1;
                    break;
                case RiotApiPlatform.OC1:
                    platformString = URL_BASE_OC1;
                    break;
                case RiotApiPlatform.TR1:
                    platformString = URL_BASE_TR1;
                    break;
                case RiotApiPlatform.RU:
                    platformString = URL_BASE_RU;
                    break;
                default:
                    platformString = URL_BASE_NA1;
                    break;
            }

            if (!httpClients.ContainsKey(platformString))
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(platformString);

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                client.DefaultRequestHeaders.Add("X-Riot-Token", API_KEY);
                httpClients.Add(platformString, client);
            }

            return httpClients[platformString];
        }

        /// <summary>
        /// Get the HttpClient object associated with the current region.
        /// </summary>
        /// <returns>An HttpClient object.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private HttpClient GetRegionClient()
        {
            string regionString;

            switch (region)
            {
                case RiotApiRegion.Americas:
                    regionString = URL_BASE_AMERICAS;
                    break;
                case RiotApiRegion.Asia:
                    regionString = URL_BASE_ASIA;
                    break;
                case RiotApiRegion.Europe:
                    regionString = URL_BASE_EUROPE;
                    break;
                default:
                    regionString = URL_BASE_AMERICAS;
                    break;
            }

            if (!httpClients.ContainsKey(regionString))
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(regionString);

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                client.DefaultRequestHeaders.Add("X-Riot-Token", API_KEY);
                httpClients.Add(regionString, client);
            }

            return httpClients[regionString];
        }

        #endregion


        #region URL HELPERS

        /// <summary>
        /// Get the string representation of a RiotRankedQueue enum value.
        /// </summary>
        /// <param name="queue">The queue to get the string representation of.</param>
        /// <returns>The string representation of the queue parameter.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private string GetRankedQueueString(RiotRankedQueue queue)
        {
            switch (queue)
            {
                case RiotRankedQueue.RankedSolo5v5:
                    return "RANKED_SOLO_5x5";
                case RiotRankedQueue.RankedFlex:
                    return "RANKED_FLEX_SR";
                default:
                    return "RANKED_FLEX_SR";
            }
        }

        #endregion

    }
}
