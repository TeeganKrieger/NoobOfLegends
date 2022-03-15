using Microsoft.EntityFrameworkCore;

namespace NoobOfLegends.Models.Services
{
    public class AppDbContext : DbContext
    {
        //DbSets will go here
        public DbSet<LolUser> LolUsers;

        public DbSet<LolUserAverage> LolUserAverages;

        public DbSet<Match> Matches;

        public DbSet<MatchTeam> MatchTeams;

        public DbSet<MatchParticipant> MatchParticipants;

        public DbSet<LolGlobalAverage> LolGlobalAverages;
    }
}
