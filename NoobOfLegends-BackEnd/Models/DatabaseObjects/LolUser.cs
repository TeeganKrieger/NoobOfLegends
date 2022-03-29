using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoobOfLegends.Models.Database
{
  public class LolUser 
  {
    [Key]
    [Column(TypeName = "NVARCHAR(64)")]
    public string UsernameAndTagline { get; set; }

    public string Username => UsernameAndTagline?.Split('#')[0];

    public string Tagline => UsernameAndTagline?.Split('#')[1];

    public string SummonerName { get; set; }

    public virtual List<Match> Matches { get; set; }
  }
}
