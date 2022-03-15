using Microsoft.EntityFrameworkCore;
using NoobOfLegends.Models.Database;

namespace NoobOfLegends.Models.Services
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions options) : base(options) { }

        //DbSets will go here
        public DbSet<LolUser> LolUsers;

        public DbSet<LolUserAverage> LolUserAverages;

        public DbSet<Match> Matches;

        public DbSet<MatchTeam> MatchTeams;

        public DbSet<MatchParticipant> MatchParticipants;

        public DbSet<LolGlobalAverage> LolGlobalAverages;
    }
}
