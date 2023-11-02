using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraphQLApi.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    LogoId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Logos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MainColor = table.Column<string>(type: "text", nullable: false),
                    SecondaryColor = table.Column<string>(type: "text", nullable: false),
                    IconId = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    TeamId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Logos_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Shirts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MainColor = table.Column<string>(type: "text", nullable: false),
                    SecondaryColor = table.Column<string>(type: "text", nullable: false),
                    IsSecond = table.Column<bool>(type: "boolean", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    TeamId = table.Column<Guid>(type: "uuid", nullable: false),
                    TeamModelId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shirts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Shirts_Teams_TeamModelId",
                        column: x => x.TeamModelId,
                        principalTable: "Teams",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Logos_TeamId",
                table: "Logos",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Shirts_TeamModelId",
                table: "Shirts",
                column: "TeamModelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Logos");

            migrationBuilder.DropTable(
                name: "Shirts");

            migrationBuilder.DropTable(
                name: "Teams");
        }
    }
}
