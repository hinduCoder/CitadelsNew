using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Citadels.Client.Telegram.Migrations
{
    /// <inheritdoc />
    public partial class UserCurrentGameOnDeleteSetNull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Game_CurrentGameId",
                table: "User");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Game_CurrentGameId",
                table: "User",
                column: "CurrentGameId",
                principalTable: "Game",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Game_CurrentGameId",
                table: "User");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Game_CurrentGameId",
                table: "User",
                column: "CurrentGameId",
                principalTable: "Game",
                principalColumn: "Id");
        }
    }
}
