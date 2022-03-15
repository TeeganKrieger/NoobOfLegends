namespace NoobOfLegends.Whatever 
{
  public class LolUser 
  {
    [Key]
    [Column(TypeName = "NVARCHAR(64)")]
    public string UsernameAndTagline { get; set; }

    public string Username => UsernameAndTagline?.Split('#')[0];

    public string Tagline => UsernameAndTagline?.Split('#')[1];
  }
}
