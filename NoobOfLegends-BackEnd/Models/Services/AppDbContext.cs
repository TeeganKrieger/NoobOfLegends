using Microsoft.EntityFrameworkCore;
using NoobOfLegends.Models.Database;

namespace NoobOfLegends.Models.Services
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions options) : base(options) { }

        //DbSets will go here
        public DbSet<LolUser> LolUsers { get; set; }

        public DbSet<LolUserAverage> LolUserAverages { get; set; }

        public DbSet<Match> Matches { get; set; }

        public DbSet<MatchTeam> MatchTeams { get; set; }

        public DbSet<MatchParticipant> MatchParticipants { get; set; }

        public DbSet<LolGlobalAverage> LolGlobalAverages { get; set; }
    }
}
