using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Migrations
{
    public partial class ConnectEntitiesOfUserAndInvalidToken : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "InvalidTokens",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_InvalidTokens_UserId",
                table: "InvalidTokens",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_InvalidTokens_AspNetUsers_UserId",
                table: "InvalidTokens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvalidTokens_AspNetUsers_UserId",
                table: "InvalidTokens");

            migrationBuilder.DropIndex(
                name: "IX_InvalidTokens_UserId",
                table: "InvalidTokens");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "InvalidTokens");
        }
    }
}
