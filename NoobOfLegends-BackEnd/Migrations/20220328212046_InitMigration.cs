using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoobOfLegends_BackEnd.Migrations
{
    public partial class InitMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LolGlobalAverages",
                columns: table => new
                {
                    RoleAndRankAndDivision = table.Column<string>(type: "NVARCHAR(64)", nullable: false),
                    Gold = table.Column<int>(type: "Integer", nullable: false),
                    XP = table.Column<int>(type: "Integer", nullable: false),
                    Kills = table.Column<int>(type: "Integer", nullable: false),
                    Deaths = table.Column<int>(type: "Integer", nullable: false),
                    TimeSpentDead = table.Column<int>(type: "Integer", nullable: false),
                    Assists = table.Column<int>(type: "Integer", nullable: false),
                    TotalDamageDealt = table.Column<int>(type: "Integer", nullable: false),
                    BaronKills = table.Column<int>(type: "Integer", nullable: false),
                    DragonKills = table.Column<int>(type: "Integer", nullable: false),
                    MinionKills = table.Column<int>(type: "Integer", nullable: false),
                    JungleMinionKills = table.Column<int>(type: "Integer", nullable: false),
                    VisionScore = table.Column<int>(type: "Integer", nullable: false),
                    KillParticipation = table.Column<int>(type: "Integer", nullable: false),
                    HealingToChampions = table.Column<int>(type: "Integer", nullable: false),
                    NumberOfMatches = table.Column<int>(type: "Integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LolGlobalAverages", x => x.RoleAndRankAndDivision);
                });

            migrationBuilder.CreateTable(
                name: "LolUserAverages",
                columns: table => new
                {
                    UsernameAndTaglineAndRole = table.Column<string>(type: "NVARCHAR(64)", nullable: false),
                    Gold = table.Column<int>(type: "Integer", nullable: false),
                    XP = table.Column<int>(type: "Integer", nullable: false),
                    Kills = table.Column<int>(type: "Integer", nullable: false),
                    Deaths = table.Column<int>(type: "Integer", nullable: false),
                    TimeSpentDead = table.Column<int>(type: "Integer", nullable: false),
                    BaronKills = table.Column<int>(type: "Integer", nullable: false),
                    DragonKills = table.Column<int>(type: "Integer", nullable: false),
                    MinionKills = table.Column<int>(type: "Integer", nullable: false),
                    JungleMinionKills = table.Column<int>(type: "Integer", nullable: false),
                    VisionScore = table.Column<int>(type: "Integer", nullable: false),
                    KillParticipation = table.Column<int>(type: "Integer", nullable: false),
                    HealingToChampions = table.Column<int>(type: "Integer", nullable: false),
                    NumberOfMatches = table.Column<int>(type: "Integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LolUserAverages", x => x.UsernameAndTaglineAndRole);
                });

            migrationBuilder.CreateTable(
                name: "LolUsers",
                columns: table => new
                {
                    UsernameAndTagline = table.Column<string>(type: "NVARCHAR(64)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LolUsers", x => x.UsernameAndTagline);
                });

            migrationBuilder.CreateTable(
                name: "Matches",
                columns: table => new
                {
                    MatchID = table.Column<string>(type: "NVARCHAR(64)", nullable: false),
                    GameStartTime = table.Column<int>(type: "Integer", nullable: false),
                    GameEndTime = table.Column<int>(type: "Integer", nullable: false),
                    GameMode = table.Column<string>(type: "NVARCHAR(64)", nullable: false),
                    QueueId = table.Column<int>(type: "Integer", nullable: false),
                    AverageRank = table.Column<int>(type: "Integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matches", x => x.MatchID);
                });

            migrationBuilder.CreateTable(
                name: "LolUserMatch",
                columns: table => new
                {
                    MatchesMatchID = table.Column<string>(type: "NVARCHAR(64)", nullable: false),
                    UsersUsernameAndTagline = table.Column<string>(type: "NVARCHAR(64)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LolUserMatch", x => new { x.MatchesMatchID, x.UsersUsernameAndTagline });
                    table.ForeignKey(
                        name: "FK_LolUserMatch_LolUsers_UsersUsernameAndTagline",
                        column: x => x.UsersUsernameAndTagline,
                        principalTable: "LolUsers",
                        principalColumn: "UsernameAndTagline",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LolUserMatch_Matches_MatchesMatchID",
                        column: x => x.MatchesMatchID,
                        principalTable: "Matches",
                        principalColumn: "MatchID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MatchParticipants",
                columns: table => new
                {
                    ID = table.Column<int>(type: "Integer", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MatchID = table.Column<string>(type: "NVARCHAR(64)", nullable: false),
                    PlayerName = table.Column<string>(type: "NVARCHAR(128)", nullable: false),
                    TeamID = table.Column<int>(type: "Integer", nullable: false),
                    Gold = table.Column<int>(type: "Integer", nullable: false),
                    XP = table.Column<int>(type: "Integer", nullable: false),
                    Kills = table.Column<int>(type: "Integer", nullable: false),
                    Deaths = table.Column<int>(type: "Integer", nullable: false),
                    TimeSpentDead = table.Column<int>(type: "Integer", nullable: false),
                    Assists = table.Column<int>(type: "Integer", nullable: false),
                    TotalDamageDealtToChampions = table.Column<int>(type: "Integer", nullable: false),
                    BaronKills = table.Column<int>(type: "Integer", nullable: false),
                    DragonKills = table.Column<int>(type: "Integer", nullable: false),
                    MinionKills = table.Column<int>(type: "Integer", nullable: false),
                    JungleMinionKills = table.Column<int>(type: "Integer", nullable: false),
                    VisionScore = table.Column<int>(type: "Integer", nullable: false),
                    KillParticipation = table.Column<double>(type: "Float", nullable: false),
                    HealingToChampions = table.Column<int>(type: "Integer", nullable: false),
                    ChampionLevel = table.Column<int>(type: "Integer", nullable: false),
                    Champion = table.Column<string>(type: "NVARCHAR(64)", nullable: false),
                    SelectedRole = table.Column<string>(type: "NVARCHAR(64)", nullable: false),
                    ActualRole = table.Column<string>(type: "NVARCHAR(64)", nullable: false),
                    Rank = table.Column<int>(type: "Integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchParticipants", x => x.ID);
                    table.ForeignKey(
                        name: "FK_MatchParticipants_Matches_MatchID",
                        column: x => x.MatchID,
                        principalTable: "Matches",
                        principalColumn: "MatchID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MatchTeams",
                columns: table => new
                {
                    ID = table.Column<int>(type: "Integer", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MatchID = table.Column<string>(type: "NVARCHAR(64)", nullable: false),
                    TeamID = table.Column<int>(type: "Integer", nullable: false),
                    Won = table.Column<bool>(type: "Bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchTeams", x => x.ID);
                    table.ForeignKey(
                        name: "FK_MatchTeams_Matches_MatchID",
                        column: x => x.MatchID,
                        principalTable: "Matches",
                        principalColumn: "MatchID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LolUserMatch_UsersUsernameAndTagline",
                table: "LolUserMatch",
                column: "UsersUsernameAndTagline");

            migrationBuilder.CreateIndex(
                name: "IX_MatchParticipants_MatchID",
                table: "MatchParticipants",
                column: "MatchID");

            migrationBuilder.CreateIndex(
                name: "IX_MatchTeams_MatchID",
                table: "MatchTeams",
                column: "MatchID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LolGlobalAverages");

            migrationBuilder.DropTable(
                name: "LolUserAverages");

            migrationBuilder.DropTable(
                name: "LolUserMatch");

            migrationBuilder.DropTable(
                name: "MatchParticipants");

            migrationBuilder.DropTable(
                name: "MatchTeams");

            migrationBuilder.DropTable(
                name: "LolUsers");

            migrationBuilder.DropTable(
                name: "Matches");
        }
    }
}
