using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pmbackend.Migrations
{
    /// <inheritdoc />
    public partial class PMDatabaseV110 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserFriend",
                columns: table => new
                {
                    FriendsId = table.Column<int>(type: "int", nullable: false),
                    PmUserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFriend", x => new { x.FriendsId, x.PmUserId });
                    table.ForeignKey(
                        name: "FK_UserFriend_AspNetUsers_FriendsId",
                        column: x => x.FriendsId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserFriend_AspNetUsers_PmUserId",
                        column: x => x.PmUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_UserFriend_PmUserId",
                table: "UserFriend",
                column: "PmUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserFriend");
        }
    }
}
