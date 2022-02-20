namespace NoobOfLegends.APIs.RiotApi
{

    /// <summary>
    /// Data class representing a SummonerDto object within the Riot Games Api.
    /// </summary>
    public class RiotSummoner
    {
        public string accountId;
        public int profileIconId;
        public long revisionDate;
        public string name;
        public string id;
        public string puuid;
        public long summonerLevel;

        public RiotSummoner(string summonerId)
        {
            this.id = summonerId;
        }

    }
}
