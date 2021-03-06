using NoobOfLegends.APIs.RiotApi;
using NoobOfLegends.Models.Database;
using NoobOfLegends.Models.Services;
using System.Threading.Tasks;

namespace NoobOfLegends_BackEnd.Models.GlobalAggregation
{
    /// <summary>
    /// Logic model that supports aggregating user data and storing it in the database.
    /// </summary>
    public class GlobalAggregation
    {
        private readonly AppDbContext _dbContext;


        #region CONSTRUCTOR

        public GlobalAggregation(AppDbContext dbContext)
        {
            if (dbContext == null)
                System.Diagnostics.Debug.WriteLine("WARNING: No DBCONTEXT!");
            _dbContext = dbContext;
        }

        #endregion

        /// <summary>
        /// Runs a global aggregation task.
        /// </summary>
        /// <param name="queue">The queue to aggregate data from.</param>
        /// <param name="numTopPlayers">The number of top players in a given role, rank, and division to collect matches from.</param>
        /// <param name="numMatches">The number of matches to collect from each top player.</param>
        /// <returns>Returns nothing.</returns>
        public async Task AggregateGlobalData(RiotRankedQueue queue = RiotRankedQueue.RankedSolo5v5, int numTopPlayers = 1, int numMatches = 1)
        {

            // Get the correct RiotQueue based on the RiotRankedQueue parameter
            RiotQueue matchQueue = RiotQueue.None;
            if (queue == RiotRankedQueue.RankedSolo5v5)
            {
                matchQueue = RiotQueue.SummonersRift5v5RankedSolo;
            } else
            {
                matchQueue = RiotQueue.SummonersRift5v5RankedFlex;
            }

            RiotGamesApiTranslator translator = new RiotGamesApiTranslator();
            translator.SetPlatform(RiotApiPlatform.NA1);

            foreach (RiotRankedTier tier in Enum.GetValues(typeof(RiotRankedTier)))
            {
                foreach (RiotRankedDivision division in Enum.GetValues(typeof(RiotRankedDivision)))
                {
                    // Skip unnecessary iterations of Master or higher ranks
                    if (tier == RiotRankedTier.Master || tier == RiotRankedTier.Grandmaster || tier == RiotRankedTier.Challenger)
                    {
                        if (division != RiotRankedDivision.I)
                        {
                            continue;
                        }
                    }

                    // Get list of top summoners in a rank/division
                    RiotRankedResult[] rankedResults = await translator.GetListOfSummonersInRank(queue, tier, division);

                    System.Diagnostics.Debug.Write(tier);
                    System.Diagnostics.Debug.WriteLine(division);
                    //System.Diagnostics.Debug.Write(rankedResults[0].leagueId);
                    // Verify null/empty results
                    if (rankedResults != null)
                    {
                        if (rankedResults.Length > 0)
                        {
                            //System.Diagnostics.Debug.WriteLine(rankedResults[0].summonerName);
                            // Get match history of the top players
                            for (int i = 0; i < numTopPlayers; i++)
                            {
                                //TODO: Swap this api call for the GetPuuidBySummonerName call 
                                RiotSummoner summoner = await translator.GetSummoner(rankedResults[i].summonerName);
                                System.Diagnostics.Debug.WriteLine(rankedResults[i].summonerName);
                                
                                if (summoner == null)
                                {
                                    System.Diagnostics.Debug.WriteLine("Summoner does not exist");
                                    continue;
                                }

                                System.Diagnostics.Debug.WriteLine(summoner.puuid);
                                RiotPUUID puuid = new RiotPUUID(summoner.puuid);

                                //System.Diagnostics.Debug.WriteLine(summoner.puuid.ToString());
                                
                                // Call getMatchHistory with id and queue parameters
                                string[] matches = await translator.GetMatchHistory(puuid, -1, -1, matchQueue, RiotGameType.Ranked, 0, numMatches);

                                // Verify null/empty results
                                if (matches != null)
                                {
                                    for (int j = 0; j < matches.Length; j++)
                                    {
                                        System.Diagnostics.Debug.Write("First Match ID: ");
                                        System.Diagnostics.Debug.WriteLine(matches[0]);

                                        // Get the match data from the list
                                        RiotMatch matchData = await translator.GetMatch(matches[j]);

                                        if (matchData == null)
                                            continue;

                                        // Gather participant objects (Top = 0/5 : Jungle = 1/6 : Middle = 2/7: Bottom = 3/8: Utility = 4/9)
                                        RiotMatch.Participant[] participants = matchData.info.participants;

                                        string tierString = tier.ToString();
                                        string divisionString = division.ToString();

                                        // TODO: Calculate total kills for team 1 and team 2. Pass these into participant data function for KP

                                        // Top Laners
                                        await GatherParticipantData(participants[0], participants[5], "TOP", tierString, divisionString);

                                        // Junglers
                                        await GatherParticipantData(participants[1], participants[6], "JUNGLE", tierString, divisionString);

                                        // Mid Laners
                                        await GatherParticipantData(participants[2], participants[7], "MIDDLE", tierString, divisionString);

                                        // Bottom Laners
                                        await GatherParticipantData(participants[3], participants[8], "BOTTOM", tierString, divisionString);

                                        // Support Laners 
                                        await GatherParticipantData(participants[4], participants[9], "SUPPORT", tierString, divisionString);

                                        //TODO: When merged, we should take advantage of the FromRiotMatch method in the Match Table class to convert
                                        //This match into a match to store in our database

                                        

                                    }
                                }
                                _dbContext?.SaveChanges();
                            }
                        }
                    }
                    System.Diagnostics.Debug.WriteLine("-----------------------------------------");
                
                }
            }
            _dbContext?.SaveChanges();
            System.Diagnostics.Debug.WriteLine("Wrote to DB!");
        }

        /// <summary>
        /// Breaks apart two RiotMatch Participants of the same role into individual stats and adds these stats to the global average.
        /// </summary>
        /// <param name="participant1">The first participant.</param>
        /// <param name="participant2">The second participant.</param>
        /// <param name="role">The role of these participants.</param>
        /// <param name="rank">The rank of the global average being aggregated.</param>
        /// <param name="division">The division of the global average being aggregated.</param>
        /// <returns>Returns nothing.</returns>
        public async Task GatherParticipantData(RiotMatch.Participant participant1, RiotMatch.Participant participant2, string role, string rank, string division)
        {
            int gold = participant1.goldEarned + participant2.goldEarned;
            int xp = participant1.champExperience + participant2.champExperience;
            int kills = participant1.kills + participant2.kills;
            int deaths = participant1.deaths + participant2.deaths;
            int timeSpentDead = participant1.totalTimeSpentDead + participant2.totalTimeSpentDead;
            int assists = participant1.assists + participant2.assists;
            int totalDamageDealt = participant1.totalDamageDealt + participant2.totalDamageDealt;
            int baronKills = participant1.baronKills + participant2.baronKills;
            int dragonKills = participant1.dragonKills + participant2.dragonKills;
            int minionKills = participant1.totalMinionsKilled + participant2.totalMinionsKilled;
            int jungleMinionKills = participant1.neutralMinionsKilled + participant2.neutralMinionsKilled;
            int visionScore = participant1.visionScore + participant2.visionScore;
            //int killParticipation = ((participant1.kills + participant1.assists)/team1kills) + ((participant2.kills + participant2.assists)/team2kills) 
            int healingToChampions = participant1.totalHealsOnTeammates + participant2.totalHealsOnTeammates;

            //System.Diagnostics.Debug.WriteLine(participant1.championName);
            //System.Diagnostics.Debug.WriteLine(participant2.championName);

            // Connect to DB here and update rows
            LolGlobalAverage avg = _dbContext?.LolGlobalAverages.Where(x => x.RoleAndRankAndDivision == $"{role}#{rank}#{division}").FirstOrDefault();

            if (avg == null)
            {
                System.Diagnostics.Debug.WriteLine("Creating new lolglobalaverage object");
                avg = new LolGlobalAverage();
                avg.RoleAndRankAndDivision = $"{role}#{rank}#{division}";
                _dbContext?.LolGlobalAverages.Add(avg);
            }
            _dbContext?.SaveChanges();
            System.Diagnostics.Debug.WriteLine("Wrote to DB!");

            avg.Gold += gold;
            avg.MinionKills += minionKills;
            avg.XP += xp;
            avg.Kills += kills;
            avg.Deaths += deaths;
            avg.TimeSpentDead += timeSpentDead;
            avg.Assists += assists;
            avg.TotalDamageDealt += totalDamageDealt;
            avg.BaronKills += baronKills;
            avg.DragonKills += dragonKills;
            avg.JungleMinionKills += jungleMinionKills;
            avg.VisionScore += visionScore;
            avg.HealingToChampions += healingToChampions;
            //avg.KillParticipation += killParticipation;
            avg.NumberOfMatches += 2;
        }

    }
}
