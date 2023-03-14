using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chat.Migrations
{
    /// <inheritdoc />
    public partial class reworkrelationshipsforclans : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Clans_UserClanId1",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_UserClanId1",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UserClanId1",
                table: "AspNetUsers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserClanId1",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_UserClanId1",
                table: "AspNetUsers",
                column: "UserClanId1");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Clans_UserClanId1",
                table: "AspNetUsers",
                column: "UserClanId1",
                principalTable: "Clans",
                principalColumn: "Id");
        }
    }
}
