using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraphQLApi.Migrations
{
    public partial class AddAcademyPropertyForPlayer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsInAcademy",
                table: "Players",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsInAcademy",
                table: "Players");
        }
    }
}
