using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Citadels.Client.Telegram.Migrations
{
    /// <inheritdoc />
    public partial class UserMessageId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UpdatingTelegramMessageId",
                table: "User",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdatingTelegramMessageId",
                table: "User");
        }
    }
}
