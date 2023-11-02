using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraphQLApi.Migrations
{
    public partial class UserPreferencesAndLeagueAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Leagues",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Leagues", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserPreferences",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Mode = table.Column<string>(type: "text", nullable: false),
                    BottomMenu = table.Column<bool>(type: "boolean", nullable: false),
                    NavbarColor = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPreferences", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Scores",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Season = table.Column<int>(type: "integer", nullable: false),
                    Points = table.Column<int>(type: "integer", nullable: false),
                    Wins = table.Column<int>(type: "integer", nullable: false),
                    Draws = table.Column<int>(type: "integer", nullable: false),
                    Lost = table.Column<int>(type: "integer", nullable: false),
                    Form = table.Column<string>(type: "text", nullable: false),
                    TeamId = table.Column<Guid>(type: "uuid", nullable: false),
                    LeagueModelId = table.Column<Guid>(type: "uuid", nullable: true),
                    TeamModelId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Scores_Leagues_LeagueModelId",
                        column: x => x.LeagueModelId,
                        principalTable: "Leagues",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Scores_Teams_TeamModelId",
                        column: x => x.TeamModelId,
                        principalTable: "Teams",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Scores_LeagueModelId",
                table: "Scores",
                column: "LeagueModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Scores_TeamModelId",
                table: "Scores",
                column: "TeamModelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Scores");

            migrationBuilder.DropTable(
                name: "UserPreferences");

            migrationBuilder.DropTable(
                name: "Leagues");
        }
    }
}
