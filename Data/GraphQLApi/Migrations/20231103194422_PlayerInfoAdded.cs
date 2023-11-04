using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraphQLApi.Migrations
{
    public partial class PlayerInfoAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TeamModelId",
                table: "Calendars",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PlayerName = table.Column<string>(type: "text", nullable: false),
                    PlayerRating_Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PlayerRating_Rating = table.Column<int>(type: "integer", nullable: false),
                    PotentialRating_Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PotentialRating_Rating = table.Column<int>(type: "integer", nullable: false),
                    PositionType = table.Column<int>(type: "integer", nullable: false),
                    PlayerNumber = table.Column<int>(type: "integer", nullable: false),
                    Image = table.Column<string>(type: "text", nullable: false),
                    Condition = table.Column<int>(type: "integer", nullable: false),
                    CountryCode = table.Column<string>(type: "text", nullable: false),
                    Foot = table.Column<string>(type: "text", nullable: false),
                    TeamId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsBenched = table.Column<bool>(type: "boolean", nullable: false),
                    InjuredTill = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Suspended = table.Column<bool>(type: "boolean", nullable: false),
                    YellowCard = table.Column<bool>(type: "boolean", nullable: false),
                    Age = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Players_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TeamHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TeamId = table.Column<Guid>(type: "uuid", nullable: false),
                    From = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    To = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    PlayerModelId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeamHistories_Players_PlayerModelId",
                        column: x => x.PlayerModelId,
                        principalTable: "Players",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TeamHistories_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Calendars_TeamModelId",
                table: "Calendars",
                column: "TeamModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_TeamId",
                table: "Players",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamHistories_PlayerModelId",
                table: "TeamHistories",
                column: "PlayerModelId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamHistories_TeamId",
                table: "TeamHistories",
                column: "TeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_Calendars_Teams_TeamModelId",
                table: "Calendars",
                column: "TeamModelId",
                principalTable: "Teams",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Calendars_Teams_TeamModelId",
                table: "Calendars");

            migrationBuilder.DropTable(
                name: "TeamHistories");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropIndex(
                name: "IX_Calendars_TeamModelId",
                table: "Calendars");

            migrationBuilder.DropColumn(
                name: "TeamModelId",
                table: "Calendars");
        }
    }
}
