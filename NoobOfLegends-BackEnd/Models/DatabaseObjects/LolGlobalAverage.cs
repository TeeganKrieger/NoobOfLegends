namespace NoobOfLegends.Whatever 
{
  public class LolGlobalAverage 
  {
    [Key]
    [Column(TypeName = "NVARCHAR(64)")]
    public string RoleAndRankAndDivision { get; set; }

    public string Role => RoleAndRankAndDivision?.Split('#')[0];
    public string Rank => RoleAndRankAndDivision?.Split('#')[1];
    public string Division => RoleAndRankAndDivision?.Split('#')[2];

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
    public int TotalDamageDealt { get; set; }

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

    [Column(TypeName = "Integer")]
    public int KillParticipation { get; set; }

    [Column(TypeName = "Integer")]
    public int HealingToChampions { get; set; }

    [Column(TypeName = "Integer")]
    public int NumberOfMatches { get; set; }
  }
}
