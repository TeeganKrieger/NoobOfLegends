namespace NoobOfLegends.APIs.RiotApi
{

    /// <summary>
    /// Data class representing a MatchTimelineDto object within the Riot Games Api.
    /// </summary>
    public class RiotMatchTimeline
    {
        public Metadata metadata;
        public Info info;

        public RiotMatchTimeline()
        {

        }

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
            public int frameInterval;
            public long gameId;
            public Frame[] frames;
            public Participant[] participants;
        }

        /// <summary>
        /// Data class representing a ParticipantDto object within the Riot Games Api.
        /// </summary>
        public class Participant
        {
            public int participantId;
            public string puuid;
        }

        /// <summary>
        /// Data class representing a FrameDto object within the Riot Games Api.
        /// </summary>
        public class Frame
        {
            public Event[] events;
            public Dictionary<string, ParticipantFrame> participantFrames;
            public int timestamp;
        }

        /// <summary>
        /// Data class representing an EventDto object within the Riot Games Api.
        /// Unimplemented currently due to complexity and lack of documentation.
        /// </summary>
        public class Event
        {

        }

        /// <summary>
        /// Data class representing a ParticipantFrameDto object within the Riot Games Api.
        /// </summary>
        public class ParticipantFrame
        {
            public ChampionStats championStats;
            public int currentGold;
            public DamageStats damageStats;
            public int goldPerSecond;
            public int jungleMinionsKilled;
            public int level;
            public int minionsKilled;
            public int participantId;
            public Position position;
            public int timeEnemySpentControlled;
            public int totalGold;
            public int xp;
        }

        /// <summary>
        /// Data class representing a ChampionStatsDto object within the Riot Games Api.
        /// </summary>
        public class ChampionStats
        {
            public int abilityHaste;
            public int abilityPower;
            public int armor;
            public int armorPen;
            public int armorPenPercent;
            public int attackDamage;
            public int attackSpeed;
            public int bonusArmorPenPercent;
            public int bonusMagicPenPercent;
            public int ccReduction;
            public int cooldownReduction;
            public int health;
            public int healthMax;
            public int healthRegen;
            public int lifesteal;
            public int magicPen;
            public int magicPenPercent;
            public int magicResist;
            public int movementSpeed;
            public int omnivamp;
            public int physicalVamp;
            public int power;
            public int powerMax;
            public int powerRegen;
            public int spellVamp;
        }

        /// <summary>
        /// Data class representing a DamageStatsDto object within the Riot Games Api.
        /// </summary>
        public class DamageStats
        {
            public int magicDamageDone;
            public int magicDamageDoneToChampions;
            public int magicDamageTaken;
            public int physicalDamageDone;
            public int physicalDamageDoneToChampions;
            public int physicalDamageTaken;
            public int totalDamageDone;
            public int totalDamageDoneToChampions;
            public int totalDamageTaken;
            public int trueDamageDone;
            public int trueDamageDoneToChampions;
            public int trueDamageTaken;
        }

        /// <summary>
        /// Data class representing a PositionDto object within the Riot Games Api.
        /// </summary>
        public class Position
        {
            public int x;
            public int y;
        }

    }
}
