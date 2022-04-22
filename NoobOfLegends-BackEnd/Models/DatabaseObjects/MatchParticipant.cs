using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoobOfLegends.Models.Database
{
    public class MatchParticipant
    {
        [Key]
        [Column(TypeName = "Integer")]
        public int ID { get; set; }

        [Required]
        [JsonIgnore]
        public virtual Match Match { get; set; }

        [Column(TypeName = "NVARCHAR(128)")]
        public string PlayerName { get; set; }

        [Column(TypeName = "Integer")]
        public int TeamID { get; set; }

        [Column(TypeName = "Integer")]
        public int Gold { get; set; }

        [Column(TypeName = "Integer")]
        public int XP { get; set; }

        [Column(TypeName = "Integer")]
        public int Kills { get; set; }

        [Column(TypeName = "Integer")]
        public int Deaths { get; set; }

        [Column(TypeName = "Integer")]
        public int TimeSpentDead { get; set; }

        [Column(TypeName = "Integer")]
        public int Assists { get; set; }

        [Column(TypeName = "Integer")]
        public int TotalDamageDealtToChampions { get; set; }

        [Column(TypeName = "Integer")]
        public int BaronKills { get; set; }

        [Column(TypeName = "Integer")]
        public int DragonKills { get; set; }

        [Column(TypeName = "Integer")]
        public int MinionKills { get; set; }

        [Column(TypeName = "Integer")]
        public int JungleMinionKills { get; set; }

        [Column(TypeName = "Integer")]
        public int VisionScore { get; set; }


        [Column(TypeName = "Float")]
        public float KillParticipation { get; set; }

        [Column(TypeName = "Integer")]
        public int HealingToChampions { get; set; }

        [Column(TypeName = "Integer")]
        public int ChampionLevel { get; set; }

        [Column(TypeName = "NVARCHAR(64)")]
        public string Champion { get; set; }

        [Column(TypeName = "NVARCHAR(64)")]
        public string SelectedRole { get; set; }

        [Column(TypeName = "NVARCHAR(64)")]
        public string ActualRole { get; set; }

        [Column(TypeName = "Integer")]
        public int Rank { get; set; }
    }
}
