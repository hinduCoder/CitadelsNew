using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Citadels.Client.Telegram.Migrations
{
    /// <inheritdoc />
    public partial class UserChatId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "PrivateChatId",
                table: "User",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrivateChatId",
                table: "User");
        }
    }
}
