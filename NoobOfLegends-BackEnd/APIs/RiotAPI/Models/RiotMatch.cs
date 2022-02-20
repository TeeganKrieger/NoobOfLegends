namespace NoobOfLegends.APIs.RiotApi
{
    /// <summary>
    /// Data class representing a MatchDto object within the Riot Games Api.
    /// </summary>
    public class RiotMatch
    {
        public Metadata metadata;
        public Info info;

        public RiotMatch()
        {

        }

        #region NESTED CLASSES

        /// <summary>
        /// Data class representing a MetadataDto object within the Riot Games Api.
        /// </summary>
        public class Metadata
        {
            public string dataVersion;
            public string matchId;
            public string[] participants;
        }

        /// <summary>
        /// Data class representing an InfoDto object within the Riot Games Api.
        /// </summary>
        public class Info
        {
            public long gameCreation;
            public long gameDuration;
            public long gameEndTimestamp;
            public long gameId;
            public string gameMode;
            public string gameName;
            public long gameStartTimestamp;
            public string gameType;
            public string gameVersion;
            public int mapId;
            public string platformId;
            public int queueId;
            public string tournamentCode;

            public Participant[] participants;
            public Team[] teams;
        }

        /// <summary>
        /// Data class representing a ParticipantDto object within the Riot Games Api.
        /// </summary>
        public class Participant
        {
            public int assists;
            public int baronKills;
            public int bountyLevel;
            public int champExperience;
            public int champLevel;
            public int championId;
            public string championName;
            public int championTransform;
            public int consumablesPurchases;
            public int damageDealtToBuildings;
            public int damageDealtToObjectives;
            public int damageDealtToTurrets;
            public int damageSelfMitigated;
            public int deaths;
            public int detectorWardsPlaced;
            public int doubleKills;
            public int dragonKills;
            public bool firstBloodAssist;
            public bool firstBloodKill;
            public bool firstTowerAssist;
            public bool firstTowerKill;
            public bool gameEndedInEarlySurrender;
            public bool gameEndedInSurrender;
            public int goldEarned;
            public int goldSpent;
            public string individualPosition;
            public int inhibitorKills;
            public int inhibitorTakedowns;
            public int inhibitorsLost;
            public int item0;
            public int item1;
            public int item2;
            public int item3;
            public int item4;
            public int item5;
            public int item6;
            public int itemsPurchased;
            public int killingSprees;
            public int kills;
            public string lane;
            public int largestCriticalStrike;
            public int largestKillingSpree;
            public int largestMultiKill;
            public int longestTimeSpentLiving;
            public int magicDamageDealt;
            public int magicDamageDealtToChampions;
            public int magicDamageTaken;
            public int neutralMinionsKilled;
            public int nexusKills;
            public int nexusLost;
            public int nexusTakedowns;
            public int objectivesStolen;
            public int objectivesStolenAssists;
            public int participantId;
            public int pentaKills;
            //Perks
            public int physicalDamageDealt;
            public int physicalDamageDealtToChampions;
            public int physicalDamageTaken;
            public int profileIcon;
            public string puuid;
            public int quadraKills;
            public string riotIdName;
            public string riotIdTagline;
            public string role;
            public int sightWardsBoughtInGame;
            public int spell1Casts;
            public int spell2Casts;
            public int spell3Casts;
            public int spell4Casts;
            public int summoner1Casts;
            public int summoner1Id;
            public int summoner2Casts;
            public int summoner2Id;
            public string summonerId;
            public int summonerLevel;
            public string summonerName;
            public bool teamEarlySurrendered;
            public int teamId;
            public string teamPosition;
            public int timeCCingOthers;
            public int timePlayed;
            public int totalDamageDealt;
            public int totalDamageDealtToChampions;
            public int totalDamageShieldedOnTeammates;
            public int totalDamageTaken;
            public int totalHeal;
            public int totalHealsOnTeammates;
            public int totalMinionsKilled;
            public int totalTimeCCDealt;
            public int totalTimeSpentDead;
            public int totalUnitsHealed;
            public int tripleKills;
            public int trueDamageDealt;
            public int trueDamageDealtToChampions;
            public int trueDamageTaken;
            public int turretKills;
            public int turretTakedowns;
            public int turretsLost;
            public int unrealKills;
            public int visionScore;
            public int visionWardsBoughtInGame;
            public int wardsKilled;
            public int wardsPlaced;
            public bool win;

        }

        /// <summary>
        /// Data class representing a TeamDto object within the Riot Games Api.
        /// </summary>
        public class Team
        {
            public int teamId;
            public bool win;
            public Objectives objectives;
        }

        /// <summary>
        /// Data class representing an ObjectivesDto object within the Riot Games Api.
        /// </summary>
        public class Objectives
        {
            public Objective baron;
            public Objective champion;
            public Objective dragon;
            public Objective inhibitor;
            public Objective riftHerald;
            public Objective tower;
        }

        /// <summary>
        /// Data class representing an ObjectiveDto object within the Riot Games Api.
        /// </summary>
        public class Objective
        {
            public bool first;
            public int kills;
        }

        #endregion
    }
}
