using Microsoft.EntityFrameworkCore.Migrations;

namespace Kryptoteket.Bot.Migrations
{
    public partial class bets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlacedBets_BetUsers_BetUserId",
                table: "PlacedBets");

            migrationBuilder.AlterColumn<int>(
                name: "Price",
                table: "PlacedBets",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "BetUserId",
                table: "PlacedBets",
                type: "decimal(20,0)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(20,0)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Place",
                table: "FinishedBetPlacements",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PlacedBets_BetUsers_BetUserId",
                table: "PlacedBets",
                column: "BetUserId",
                principalTable: "BetUsers",
                principalColumn: "BetUserId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlacedBets_BetUsers_BetUserId",
                table: "PlacedBets");

            migrationBuilder.AlterColumn<string>(
                name: "Price",
                table: "PlacedBets",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "BetUserId",
                table: "PlacedBets",
                type: "decimal(20,0)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(20,0)");

            migrationBuilder.AlterColumn<string>(
                name: "Place",
                table: "FinishedBetPlacements",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_PlacedBets_BetUsers_BetUserId",
                table: "PlacedBets",
                column: "BetUserId",
                principalTable: "BetUsers",
                principalColumn: "BetUserId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
