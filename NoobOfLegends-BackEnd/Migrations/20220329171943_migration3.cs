using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoobOfLegends_BackEnd.Migrations
{
    public partial class migration3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "GameStartTime",
                table: "Matches",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "Integer");

            migrationBuilder.AlterColumn<long>(
                name: "GameEndTime",
                table: "Matches",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "Integer");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "GameStartTime",
                table: "Matches",
                type: "Integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "GameEndTime",
                table: "Matches",
                type: "Integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");
        }
    }
}
