namespace NoobOfLegends.APIs.RiotApi
{

    /// <summary>
    /// Data struct representing a Puuid within the Riot Games Api.
    /// </summary>
    public struct RiotPUUID
    {
        public string puuid;

        public RiotPUUID(string puuid)
        {
            this.puuid = puuid;
        }

        public override string ToString()
        {
            return puuid;
        }
    }
}
