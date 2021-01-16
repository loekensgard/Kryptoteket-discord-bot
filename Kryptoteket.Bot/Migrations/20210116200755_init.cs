using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Kryptoteket.Bot.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bets",
                columns: table => new
                {
                    BetId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShortName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bets", x => x.BetId);
                });

            migrationBuilder.CreateTable(
                name: "BetUsers",
                columns: table => new
                {
                    BetUserId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Points = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BetUsers", x => x.BetUserId);
                });

            migrationBuilder.CreateTable(
                name: "RefExchanges",
                columns: table => new
                {
                    RefExchangeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefExchanges", x => x.RefExchangeId);
                });

            migrationBuilder.CreateTable(
                name: "RefUsers",
                columns: table => new
                {
                    RefUserId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Approved = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefUsers", x => x.RefUserId);
                });

            migrationBuilder.CreateTable(
                name: "FinishedBetPlacements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BetId = table.Column<int>(type: "int", nullable: false),
                    Place = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BetUserId = table.Column<decimal>(type: "decimal(20,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinishedBetPlacements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FinishedBetPlacements_BetUsers_BetUserId",
                        column: x => x.BetUserId,
                        principalTable: "BetUsers",
                        principalColumn: "BetUserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlacedBets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BetId = table.Column<int>(type: "int", nullable: false),
                    BetPlaced = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    BetUserId = table.Column<decimal>(type: "decimal(20,0)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlacedBets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlacedBets_Bets_BetId",
                        column: x => x.BetId,
                        principalTable: "Bets",
                        principalColumn: "BetId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlacedBets_BetUsers_BetUserId",
                        column: x => x.BetUserId,
                        principalTable: "BetUsers",
                        principalColumn: "BetUserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RefExchangeRefUsers",
                columns: table => new
                {
                    RefExchangesRefExchangeId = table.Column<int>(type: "int", nullable: false),
                    RefUsersRefUserId = table.Column<decimal>(type: "decimal(20,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefExchangeRefUsers", x => new { x.RefExchangesRefExchangeId, x.RefUsersRefUserId });
                    table.ForeignKey(
                        name: "FK_RefExchangeRefUsers_RefExchanges_RefExchangesRefExchangeId",
                        column: x => x.RefExchangesRefExchangeId,
                        principalTable: "RefExchanges",
                        principalColumn: "RefExchangeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RefExchangeRefUsers_RefUsers_RefUsersRefUserId",
                        column: x => x.RefUsersRefUserId,
                        principalTable: "RefUsers",
                        principalColumn: "RefUserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RefLinks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RefExchangeId = table.Column<int>(type: "int", nullable: false),
                    RefUserId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefLinks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefLinks_RefExchanges_RefExchangeId",
                        column: x => x.RefExchangeId,
                        principalTable: "RefExchanges",
                        principalColumn: "RefExchangeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RefLinks_RefUsers_RefUserId",
                        column: x => x.RefUserId,
                        principalTable: "RefUsers",
                        principalColumn: "RefUserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FinishedBetPlacements_BetUserId",
                table: "FinishedBetPlacements",
                column: "BetUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PlacedBets_BetId",
                table: "PlacedBets",
                column: "BetId");

            migrationBuilder.CreateIndex(
                name: "IX_PlacedBets_BetUserId",
                table: "PlacedBets",
                column: "BetUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RefExchangeRefUsers_RefUsersRefUserId",
                table: "RefExchangeRefUsers",
                column: "RefUsersRefUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RefLinks_RefExchangeId",
                table: "RefLinks",
                column: "RefExchangeId");

            migrationBuilder.CreateIndex(
                name: "IX_RefLinks_RefUserId",
                table: "RefLinks",
                column: "RefUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FinishedBetPlacements");

            migrationBuilder.DropTable(
                name: "PlacedBets");

            migrationBuilder.DropTable(
                name: "RefExchangeRefUsers");

            migrationBuilder.DropTable(
                name: "RefLinks");

            migrationBuilder.DropTable(
                name: "Bets");

            migrationBuilder.DropTable(
                name: "BetUsers");

            migrationBuilder.DropTable(
                name: "RefExchanges");

            migrationBuilder.DropTable(
                name: "RefUsers");
        }
    }
}
