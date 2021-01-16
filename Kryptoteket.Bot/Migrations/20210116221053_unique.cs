using Microsoft.EntityFrameworkCore.Migrations;

namespace Kryptoteket.Bot.Migrations
{
    public partial class unique : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ShortName",
                table: "Bets",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bets_ShortName",
                table: "Bets",
                column: "ShortName",
                unique: true,
                filter: "[ShortName] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Bets_ShortName",
                table: "Bets");

            migrationBuilder.AlterColumn<string>(
                name: "ShortName",
                table: "Bets",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
