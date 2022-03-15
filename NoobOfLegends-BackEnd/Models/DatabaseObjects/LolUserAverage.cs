namespace NoobOfLegends.Whatever 
{
  public class LolUserAverage 
  {
    [Key]
    [Column(TypeName = "NVARCHAR(64)")]
    public string UsernameAndTaglineAndRole { get; set; }

    public string Username => UsernameAndTaglineAndRole?.Split('#')[0];
    public string Tagline => UsernameAndTaglineAndRole?.Split('#')[1];
    public string Role => UsernameAndTaglineAndRole?.Split('#')[2];
    
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
