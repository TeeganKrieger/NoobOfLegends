using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoobOfLegends_BackEnd.Migrations
{
    public partial class Migration2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SummonerName",
                table: "LolUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SummonerName",
                table: "LolUsers");
        }
    }
}
