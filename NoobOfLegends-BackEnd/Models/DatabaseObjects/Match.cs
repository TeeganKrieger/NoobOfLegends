namespace NoobOfLegends.Whatever 
{
  public class Match 
  {
    [Key]
    [Column(TypeName = "NVARCHAR(64)")]
    public string MatchID { get; set; }

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
  }
}
