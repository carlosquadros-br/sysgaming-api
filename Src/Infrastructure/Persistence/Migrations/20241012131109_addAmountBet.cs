using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SysgamingApi.Src.Infrastructure.Persistence.Migrations
{
    public partial class addAmountBet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "Bets",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

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

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Bets",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "Bets");

            migrationBuilder.DropColumn(
                name: "GameId",
                table: "Bets");

            migrationBuilder.DropColumn(
                name: "TeamId",
                table: "Bets");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Bets");
        }
    }
}
