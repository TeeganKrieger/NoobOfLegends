using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoobOfLegends.Models.Database
{
    public class MatchTeam
    {
        [Key]
        [Column(TypeName = "Integer")]
        public int ID { get; set; }

        [Required]
        public virtual Match Match { get; set; }

        [Column(TypeName = "Integer")]
        public int TeamID { get; set; }

        [Column(TypeName = "Bit")]
        public bool Won { get; set; }
    }
}
