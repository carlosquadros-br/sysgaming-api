using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SysgamingApi.Src.Infrastructure.Persistence.Migrations
{
    public partial class AddAccountBalance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GameId",
                table: "Bets");

            migrationBuilder.DropColumn(
                name: "TeamId",
                table: "Bets");

            migrationBuilder.AddColumn<int>(
                name: "Result",
                table: "Bets",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Bets",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Result",
                table: "Bets");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Bets");

            migrationBuilder.AddColumn<string>(
                name: "GameId",
                table: "Bets",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TeamId",
                table: "Bets",
                type: "text",
                nullable: true);
        }
    }
}
