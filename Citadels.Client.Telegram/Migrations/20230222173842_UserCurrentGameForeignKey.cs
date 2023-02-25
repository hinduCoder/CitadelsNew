using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Citadels.Client.Telegram.Migrations
{
    /// <inheritdoc />
    public partial class UserCurrentGameForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Game_CurrentGameId",
                table: "User");

            migrationBuilder.AlterColumn<Guid>(
                name: "CurrentGameId",
                table: "User",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Game_CurrentGameId",
                table: "User",
                column: "CurrentGameId",
                principalTable: "Game",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Game_CurrentGameId",
                table: "User");

            migrationBuilder.AlterColumn<Guid>(
                name: "CurrentGameId",
                table: "User",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_User_Game_CurrentGameId",
                table: "User",
                column: "CurrentGameId",
                principalTable: "Game",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
