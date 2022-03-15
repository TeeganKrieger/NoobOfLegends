namespace NoobOfLegends.Whatever 
{
  public class Match 
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
