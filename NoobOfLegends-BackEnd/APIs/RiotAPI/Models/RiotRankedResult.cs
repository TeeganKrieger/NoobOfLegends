namespace NoobOfLegends.APIs.RiotApi
{

    /// <summary>
    /// Data class representing a LeagueEntryDto object within the Riot Games Api.
    /// </summary>
    public class RiotRankedResult
    {
        public string leagueId;
        public string queueType;
        public string tier;
        public string rank;
        public string summonerId;
        public string summonerName;
        public int leaguePoints;
        public int wins;
        public int losses;
        public bool veteran;
        public bool inactive;
        public bool freshBlood;
        public bool hotStreak;
        public RankedMiniSeries miniSeries;

        /// <summary>
        /// Data class representing a MiniSeriesDto object within the Riot Games Api.
        /// </summary>
        public class RankedMiniSeries
        {
            public int losses;
            public string progress;
            public int target;
            public int wins;
        }
    }
}
