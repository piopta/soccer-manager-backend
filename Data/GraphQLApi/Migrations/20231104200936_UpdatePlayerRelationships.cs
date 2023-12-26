using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraphQLApi.Migrations
{
    public partial class UpdatePlayerRelationships : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeamHistories_Players_PlayerModelId",
                table: "TeamHistories");

            migrationBuilder.DropIndex(
                name: "IX_TeamHistories_PlayerModelId",
                table: "TeamHistories");

            migrationBuilder.DropColumn(
                name: "PlayerModelId",
                table: "TeamHistories");

            migrationBuilder.AddColumn<double>(
                name: "Budget",
                table: "Teams",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<Guid>(
                name: "PlayerId",
                table: "TeamHistories",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<double>(
                name: "MarketValue",
                table: "Players",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateIndex(
                name: "IX_TeamHistories_PlayerId",
                table: "TeamHistories",
                column: "PlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_TeamHistories_Players_PlayerId",
                table: "TeamHistories",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeamHistories_Players_PlayerId",
                table: "TeamHistories");

            migrationBuilder.DropIndex(
                name: "IX_TeamHistories_PlayerId",
                table: "TeamHistories");

            migrationBuilder.DropColumn(
                name: "Budget",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "PlayerId",
                table: "TeamHistories");

            migrationBuilder.DropColumn(
                name: "MarketValue",
                table: "Players");

            migrationBuilder.AddColumn<Guid>(
                name: "PlayerModelId",
                table: "TeamHistories",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TeamHistories_PlayerModelId",
                table: "TeamHistories",
                column: "PlayerModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_TeamHistories_Players_PlayerModelId",
                table: "TeamHistories",
                column: "PlayerModelId",
                principalTable: "Players",
                principalColumn: "Id");
        }
    }
}
