using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraphQLApi.Migrations
{
    public partial class MoneyRelatedModelsAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AcademyFacilities",
                columns: table => new
                {
                    AcademyId = table.Column<Guid>(type: "uuid", nullable: false),
                    SecondTeamName = table.Column<string>(type: "text", nullable: false),
                    ManagerQuality = table.Column<int>(type: "integer", nullable: false),
                    FacilitiesQuality = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcademyFacilities", x => x.AcademyId);
                });

            migrationBuilder.CreateTable(
                name: "Profits",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Season = table.Column<int>(type: "integer", nullable: false),
                    Transfers = table.Column<double>(type: "double precision", nullable: true),
                    Stadium = table.Column<double>(type: "double precision", nullable: true),
                    TeamId = table.Column<Guid>(type: "uuid", nullable: false),
                    TeamModelId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Profits_Teams_TeamModelId",
                        column: x => x.TeamModelId,
                        principalTable: "Teams",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Spendings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Season = table.Column<int>(type: "integer", nullable: false),
                    Transfers = table.Column<double>(type: "double precision", nullable: true),
                    Salaries = table.Column<double>(type: "double precision", nullable: true),
                    TeamId = table.Column<Guid>(type: "uuid", nullable: false),
                    TeamModelId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Spendings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Spendings_Teams_TeamModelId",
                        column: x => x.TeamModelId,
                        principalTable: "Teams",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Stadiums",
                columns: table => new
                {
                    StadiumId = table.Column<Guid>(type: "uuid", nullable: false),
                    StadiumName = table.Column<string>(type: "text", nullable: false),
                    Capacity = table.Column<int>(type: "integer", nullable: false),
                    SeatQuality = table.Column<int>(type: "integer", nullable: false),
                    FansExtrasQuality = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stadiums", x => x.StadiumId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Profits_TeamModelId",
                table: "Profits",
                column: "TeamModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Spendings_TeamModelId",
                table: "Spendings",
                column: "TeamModelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AcademyFacilities");

            migrationBuilder.DropTable(
                name: "Profits");

            migrationBuilder.DropTable(
                name: "Spendings");

            migrationBuilder.DropTable(
                name: "Stadiums");
        }
    }
}
