using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Citadels.Client.Telegram.Migrations
{
    /// <inheritdoc />
    public partial class UserGame : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Game",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    HostUserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Game", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    TelegramUserId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    LanguageCode = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    CurrentGameId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.TelegramUserId);
                    table.ForeignKey(
                        name: "FK_User_Game_CurrentGameId",
                        column: x => x.CurrentGameId,
                        principalTable: "Game",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Game_HostUserId",
                table: "Game",
                column: "HostUserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_CurrentGameId",
                table: "User",
                column: "CurrentGameId");

            migrationBuilder.AddForeignKey(
                name: "FK_Game_User_HostUserId",
                table: "Game",
                column: "HostUserId",
                principalTable: "User",
                principalColumn: "TelegramUserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Game_User_HostUserId",
                table: "Game");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Game");
        }
    }
}
