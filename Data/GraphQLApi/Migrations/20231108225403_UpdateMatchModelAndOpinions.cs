using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraphQLApi.Migrations
{
    public partial class UpdateMatchModelAndOpinions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Scores_Leagues_LeagueModelId",
                table: "Scores");

            migrationBuilder.DropIndex(
                name: "IX_Scores_LeagueModelId",
                table: "Scores");

            migrationBuilder.DropColumn(
                name: "LeagueModelId",
                table: "Scores");

            migrationBuilder.CreateIndex(
                name: "IX_Scores_LeagueId",
                table: "Scores",
                column: "LeagueId");

            migrationBuilder.AddForeignKey(
                name: "FK_Scores_Leagues_LeagueId",
                table: "Scores",
                column: "LeagueId",
                principalTable: "Leagues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Scores_Leagues_LeagueId",
                table: "Scores");

            migrationBuilder.DropIndex(
                name: "IX_Scores_LeagueId",
                table: "Scores");

            migrationBuilder.AddColumn<Guid>(
                name: "LeagueModelId",
                table: "Scores",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Scores_LeagueModelId",
                table: "Scores",
                column: "LeagueModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Scores_Leagues_LeagueModelId",
                table: "Scores",
                column: "LeagueModelId",
                principalTable: "Leagues",
                principalColumn: "Id");
        }
    }
}
