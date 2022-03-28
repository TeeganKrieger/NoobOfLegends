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

    // returns string array of matchIds
    public string[] MatchIdList => MatchIdListRaw?.Split('|');

        [Column(TypeName = "NVARCHAR(1024)")]
        public string MatchIdListRaw { get; set; }

        public void AddMatch(string matchId)
        {
            string matchIdList = $"{MatchIdListRaw}|{matchId}";
            MatchIdListRaw = matchIdList;
        }

        public void RemoveMatch(string matchId)
        {
            string matchIdList = MatchIdListRaw;
            matchIdList.Replace(matchId, " ");
            matchIdList.Replace("| |", "|");
            matchIdList.Replace("| ", "");
            matchIdList.Replace(" |", "");
        }
  }
}
