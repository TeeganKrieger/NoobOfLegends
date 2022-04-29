using Microsoft.EntityFrameworkCore;
using NoobOfLegends.Models.Database;

namespace NoobOfLegends.Models.Services
{
    /// <summary>
    /// The database context object.
    /// This object contains references to all the explicit tables within the database.
    /// </summary>
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<LolUser> LolUsers { get; set; }

        public DbSet<LolUserAverage> LolUserAverages { get; set; }

        public DbSet<Match> Matches { get; set; }

        public DbSet<MatchTeam> MatchTeams { get; set; }

        public DbSet<MatchParticipant> MatchParticipants { get; set; }

        public DbSet<LolGlobalAverage> LolGlobalAverages { get; set; }
    }
}
