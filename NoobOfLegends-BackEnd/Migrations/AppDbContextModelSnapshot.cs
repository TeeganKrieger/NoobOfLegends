﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NoobOfLegends.Models.Services;

#nullable disable

namespace NoobOfLegends_BackEnd.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("LolUserMatch", b =>
                {
                    b.Property<string>("MatchesMatchID")
                        .HasColumnType("NVARCHAR(64)");

                    b.Property<string>("UsersUsernameAndTagline")
                        .HasColumnType("NVARCHAR(64)");

                    b.HasKey("MatchesMatchID", "UsersUsernameAndTagline");

                    b.HasIndex("UsersUsernameAndTagline");

                    b.ToTable("LolUserMatch");
                });

            modelBuilder.Entity("NoobOfLegends.Models.Database.LolGlobalAverage", b =>
                {
                    b.Property<string>("RoleAndRankAndDivision")
                        .HasColumnType("NVARCHAR(64)");

                    b.Property<int>("Assists")
                        .HasColumnType("Integer");

                    b.Property<int>("BaronKills")
                        .HasColumnType("Integer");

                    b.Property<int>("Deaths")
                        .HasColumnType("Integer");

                    b.Property<int>("DragonKills")
                        .HasColumnType("Integer");

                    b.Property<int>("Gold")
                        .HasColumnType("Integer");

                    b.Property<int>("HealingToChampions")
                        .HasColumnType("Integer");

                    b.Property<int>("JungleMinionKills")
                        .HasColumnType("Integer");

                    b.Property<int>("KillParticipation")
                        .HasColumnType("Integer");

                    b.Property<int>("Kills")
                        .HasColumnType("Integer");

                    b.Property<int>("MinionKills")
                        .HasColumnType("Integer");

                    b.Property<int>("NumberOfMatches")
                        .HasColumnType("Integer");

                    b.Property<int>("TimeSpentDead")
                        .HasColumnType("Integer");

                    b.Property<int>("TotalDamageDealt")
                        .HasColumnType("Integer");

                    b.Property<int>("VisionScore")
                        .HasColumnType("Integer");

                    b.Property<int>("XP")
                        .HasColumnType("Integer");

                    b.HasKey("RoleAndRankAndDivision");

                    b.ToTable("LolGlobalAverages");
                });

            modelBuilder.Entity("NoobOfLegends.Models.Database.LolUser", b =>
                {
                    b.Property<string>("UsernameAndTagline")
                        .HasColumnType("NVARCHAR(64)");

                    b.HasKey("UsernameAndTagline");

                    b.ToTable("LolUsers");
                });

            modelBuilder.Entity("NoobOfLegends.Models.Database.LolUserAverage", b =>
                {
                    b.Property<string>("UsernameAndTaglineAndRole")
                        .HasColumnType("NVARCHAR(64)");

                    b.Property<int>("BaronKills")
                        .HasColumnType("Integer");

                    b.Property<int>("Deaths")
                        .HasColumnType("Integer");

                    b.Property<int>("DragonKills")
                        .HasColumnType("Integer");

                    b.Property<int>("Gold")
                        .HasColumnType("Integer");

                    b.Property<int>("HealingToChampions")
                        .HasColumnType("Integer");

                    b.Property<int>("JungleMinionKills")
                        .HasColumnType("Integer");

                    b.Property<int>("KillParticipation")
                        .HasColumnType("Integer");

                    b.Property<int>("Kills")
                        .HasColumnType("Integer");

                    b.Property<int>("MinionKills")
                        .HasColumnType("Integer");

                    b.Property<int>("NumberOfMatches")
                        .HasColumnType("Integer");

                    b.Property<int>("TimeSpentDead")
                        .HasColumnType("Integer");

                    b.Property<int>("VisionScore")
                        .HasColumnType("Integer");

                    b.Property<int>("XP")
                        .HasColumnType("Integer");

                    b.HasKey("UsernameAndTaglineAndRole");

                    b.ToTable("LolUserAverages");
                });

            modelBuilder.Entity("NoobOfLegends.Models.Database.Match", b =>
                {
                    b.Property<string>("MatchID")
                        .HasColumnType("NVARCHAR(64)");

                    b.Property<int>("AverageRank")
                        .HasColumnType("Integer");

                    b.Property<int>("GameEndTime")
                        .HasColumnType("Integer");

                    b.Property<string>("GameMode")
                        .IsRequired()
                        .HasColumnType("NVARCHAR(64)");

                    b.Property<int>("GameStartTime")
                        .HasColumnType("Integer");

                    b.Property<int>("QueueId")
                        .HasColumnType("Integer");

                    b.HasKey("MatchID");

                    b.ToTable("Matches");
                });

            modelBuilder.Entity("NoobOfLegends.Models.Database.MatchParticipant", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("Integer");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<string>("ActualRole")
                        .IsRequired()
                        .HasColumnType("NVARCHAR(64)");

                    b.Property<int>("Assists")
                        .HasColumnType("Integer");

                    b.Property<int>("BaronKills")
                        .HasColumnType("Integer");

                    b.Property<string>("Champion")
                        .IsRequired()
                        .HasColumnType("NVARCHAR(64)");

                    b.Property<int>("ChampionLevel")
                        .HasColumnType("Integer");

                    b.Property<int>("Deaths")
                        .HasColumnType("Integer");

                    b.Property<int>("DragonKills")
                        .HasColumnType("Integer");

                    b.Property<int>("Gold")
                        .HasColumnType("Integer");

                    b.Property<int>("HealingToChampions")
                        .HasColumnType("Integer");

                    b.Property<int>("JungleMinionKills")
                        .HasColumnType("Integer");

                    b.Property<double>("KillParticipation")
                        .HasColumnType("Float");

                    b.Property<int>("Kills")
                        .HasColumnType("Integer");

                    b.Property<string>("MatchID")
                        .IsRequired()
                        .HasColumnType("NVARCHAR(64)");

                    b.Property<int>("MinionKills")
                        .HasColumnType("Integer");

                    b.Property<string>("PlayerName")
                        .IsRequired()
                        .HasColumnType("NVARCHAR(128)");

                    b.Property<int>("Rank")
                        .HasColumnType("Integer");

                    b.Property<string>("SelectedRole")
                        .IsRequired()
                        .HasColumnType("NVARCHAR(64)");

                    b.Property<int>("TeamID")
                        .HasColumnType("Integer");

                    b.Property<int>("TimeSpentDead")
                        .HasColumnType("Integer");

                    b.Property<int>("TotalDamageDealtToChampions")
                        .HasColumnType("Integer");

                    b.Property<int>("VisionScore")
                        .HasColumnType("Integer");

                    b.Property<int>("XP")
                        .HasColumnType("Integer");

                    b.HasKey("ID");

                    b.HasIndex("MatchID");

                    b.ToTable("MatchParticipants");
                });

            modelBuilder.Entity("NoobOfLegends.Models.Database.MatchTeam", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("Integer");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<string>("MatchID")
                        .IsRequired()
                        .HasColumnType("NVARCHAR(64)");

                    b.Property<int>("TeamID")
                        .HasColumnType("Integer");

                    b.Property<bool>("Won")
                        .HasColumnType("Bit");

                    b.HasKey("ID");

                    b.HasIndex("MatchID");

                    b.ToTable("MatchTeams");
                });

            modelBuilder.Entity("LolUserMatch", b =>
                {
                    b.HasOne("NoobOfLegends.Models.Database.Match", null)
                        .WithMany()
                        .HasForeignKey("MatchesMatchID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NoobOfLegends.Models.Database.LolUser", null)
                        .WithMany()
                        .HasForeignKey("UsersUsernameAndTagline")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("NoobOfLegends.Models.Database.MatchParticipant", b =>
                {
                    b.HasOne("NoobOfLegends.Models.Database.Match", "Match")
                        .WithMany("Participants")
                        .HasForeignKey("MatchID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Match");
                });

            modelBuilder.Entity("NoobOfLegends.Models.Database.MatchTeam", b =>
                {
                    b.HasOne("NoobOfLegends.Models.Database.Match", "Match")
                        .WithMany("Teams")
                        .HasForeignKey("MatchID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Match");
                });

            modelBuilder.Entity("NoobOfLegends.Models.Database.Match", b =>
                {
                    b.Navigation("Participants");

                    b.Navigation("Teams");
                });
#pragma warning restore 612, 618
        }
    }
}
