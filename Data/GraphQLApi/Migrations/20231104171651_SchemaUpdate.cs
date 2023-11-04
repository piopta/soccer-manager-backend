using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraphQLApi.Migrations
{
    public partial class SchemaUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Logos_Teams_TeamId",
                table: "Logos");

            migrationBuilder.DropIndex(
                name: "IX_Logos_TeamId",
                table: "Logos");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_LogoId",
                table: "Teams",
                column: "LogoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_Logos_LogoId",
                table: "Teams",
                column: "LogoId",
                principalTable: "Logos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teams_Logos_LogoId",
                table: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_Teams_LogoId",
                table: "Teams");

            migrationBuilder.CreateIndex(
                name: "IX_Logos_TeamId",
                table: "Logos",
                column: "TeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_Logos_Teams_TeamId",
                table: "Logos",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
