using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoobOfLegends.Models.Database
{
    public class Match
    {
        [Key]
        [Column(TypeName = "NVARCHAR(64)")]
        public string MatchID { get; set; }

        [Required]
        public virtual List<LolUser> Users { get; set; }

        [Column(TypeName = "Integer")]
        public int GameStartTime { get; set; }

        [Column(TypeName = "Integer")]
        public int GameEndTime { get; set; }

        [Column(TypeName = "NVARCHAR(64)")]
        public string GameMode { get; set; }

        [Column(TypeName = "Integer")]
        public int QueueId { get; set; }

        [Column(TypeName = "Integer")]
        public int AverageRank { get; set; }

        public virtual List<MatchParticipant> Participants { get; set; }
        public virtual List<MatchTeam> Teams { get; set; }
    }
}
