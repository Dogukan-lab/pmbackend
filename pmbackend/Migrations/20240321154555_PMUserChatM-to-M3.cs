using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pmbackend.Migrations
{
    /// <inheritdoc />
    public partial class PMUserChatMtoM3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PmUserChat_Chats_ChatsMessageId",
                table: "PmUserChat");

            migrationBuilder.RenameColumn(
                name: "ChatsMessageId",
                table: "PmUserChat",
                newName: "ChatsChatId");

            migrationBuilder.RenameColumn(
                name: "MessageId",
                table: "Chats",
                newName: "ChatId");

            migrationBuilder.AddForeignKey(
                name: "FK_PmUserChat_Chats_ChatsChatId",
                table: "PmUserChat",
                column: "ChatsChatId",
                principalTable: "Chats",
                principalColumn: "ChatId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PmUserChat_Chats_ChatsChatId",
                table: "PmUserChat");

            migrationBuilder.RenameColumn(
                name: "ChatsChatId",
                table: "PmUserChat",
                newName: "ChatsMessageId");

            migrationBuilder.RenameColumn(
                name: "ChatId",
                table: "Chats",
                newName: "MessageId");

            migrationBuilder.AddForeignKey(
                name: "FK_PmUserChat_Chats_ChatsMessageId",
                table: "PmUserChat",
                column: "ChatsMessageId",
                principalTable: "Chats",
                principalColumn: "MessageId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
