using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoobOfLegends.Models.Database
{
  public class MatchTeam
  {
    [Key]
    [Column(TypeName = "NVARCHAR(64)")]
    public string MatchID { get; set; }

    [Column(TypeName = "Integer")]
    public int TeamID { get; set; }

    [Column(TypeName = "Boolean")]
    public bool Won { get; set; }
  }
}
