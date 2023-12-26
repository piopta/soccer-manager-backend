using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraphQLApi.Migrations
{
    public partial class UpdatedMultipleSchemas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Scores_Teams_TeamModelId",
                table: "Scores");

            migrationBuilder.DropIndex(
                name: "IX_Scores_TeamModelId",
                table: "Scores");

            migrationBuilder.DropColumn(
                name: "TeamModelId",
                table: "Scores");

            migrationBuilder.AddColumn<int>(
                name: "DayOfCreation",
                table: "Teams",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "LeagueId",
                table: "Scores",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "ContractTo",
                table: "Players",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsOnSale",
                table: "Players",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "Wage",
                table: "Players",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateIndex(
                name: "IX_Scores_TeamId",
                table: "Scores",
                column: "TeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_Scores_Teams_TeamId",
                table: "Scores",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Scores_Teams_TeamId",
                table: "Scores");

            migrationBuilder.DropIndex(
                name: "IX_Scores_TeamId",
                table: "Scores");

            migrationBuilder.DropColumn(
                name: "DayOfCreation",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "LeagueId",
                table: "Scores");

            migrationBuilder.DropColumn(
                name: "ContractTo",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "IsOnSale",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "Wage",
                table: "Players");

            migrationBuilder.AddColumn<Guid>(
                name: "TeamModelId",
                table: "Scores",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Scores_TeamModelId",
                table: "Scores",
                column: "TeamModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Scores_Teams_TeamModelId",
                table: "Scores",
                column: "TeamModelId",
                principalTable: "Teams",
                principalColumn: "Id");
        }
    }
}
