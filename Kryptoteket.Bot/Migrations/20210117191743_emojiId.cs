using Microsoft.EntityFrameworkCore.Migrations;

namespace Kryptoteket.Bot.Migrations
{
    public partial class emojiId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Emoji",
                table: "RefExchanges");

            migrationBuilder.AddColumn<decimal>(
                name: "EmojiId",
                table: "RefExchanges",
                type: "decimal(20,0)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmojiId",
                table: "RefExchanges");

            migrationBuilder.AddColumn<string>(
                name: "Emoji",
                table: "RefExchanges",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
