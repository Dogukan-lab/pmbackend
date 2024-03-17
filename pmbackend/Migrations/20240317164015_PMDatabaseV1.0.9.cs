using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pmbackend.Migrations
{
    /// <inheritdoc />
    public partial class PMDatabaseV109 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_PmUserId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_PmUserId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PmUserId",
                table: "AspNetUsers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PmUserId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_PmUserId",
                table: "AspNetUsers",
                column: "PmUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_PmUserId",
                table: "AspNetUsers",
                column: "PmUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
