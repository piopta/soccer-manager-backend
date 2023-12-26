using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraphQLApi.Migrations
{
    public partial class UpdatePlayerSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlayerRating_Id",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "PotentialRating_Id",
                table: "Players");

            migrationBuilder.RenameColumn(
                name: "PotentialRating_Rating",
                table: "Players",
                newName: "PotentialRating");

            migrationBuilder.RenameColumn(
                name: "PlayerRating_Rating",
                table: "Players",
                newName: "PlayerRating");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PotentialRating",
                table: "Players",
                newName: "PotentialRating_Rating");

            migrationBuilder.RenameColumn(
                name: "PlayerRating",
                table: "Players",
                newName: "PlayerRating_Rating");

            migrationBuilder.AddColumn<Guid>(
                name: "PlayerRating_Id",
                table: "Players",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "PotentialRating_Id",
                table: "Players",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
