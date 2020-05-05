using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Refosus.Web.Migrations
{
    public partial class Conuntries : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CampusEntityId",
                table: "Companies",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Companies",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "CampusEntity",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CityEntityId",
                table: "CampusEntity",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "CampusEntity",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "CampusEntity",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Conuntries",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conuntries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CityEntity",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Conuntry = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    ConuntryEntityId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CityEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CityEntity_Conuntries_ConuntryEntityId",
                        column: x => x.ConuntryEntityId,
                        principalTable: "Conuntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Companies_CampusEntityId",
                table: "Companies",
                column: "CampusEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_CampusEntity_CityEntityId",
                table: "CampusEntity",
                column: "CityEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_CityEntity_ConuntryEntityId",
                table: "CityEntity",
                column: "ConuntryEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_CampusEntity_CityEntity_CityEntityId",
                table: "CampusEntity",
                column: "CityEntityId",
                principalTable: "CityEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_CampusEntity_CampusEntityId",
                table: "Companies",
                column: "CampusEntityId",
                principalTable: "CampusEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CampusEntity_CityEntity_CityEntityId",
                table: "CampusEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_Companies_CampusEntity_CampusEntityId",
                table: "Companies");

            migrationBuilder.DropTable(
                name: "CityEntity");

            migrationBuilder.DropTable(
                name: "Conuntries");

            migrationBuilder.DropIndex(
                name: "IX_Companies_CampusEntityId",
                table: "Companies");

            migrationBuilder.DropIndex(
                name: "IX_CampusEntity_CityEntityId",
                table: "CampusEntity");

            migrationBuilder.DropColumn(
                name: "CampusEntityId",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "City",
                table: "CampusEntity");

            migrationBuilder.DropColumn(
                name: "CityEntityId",
                table: "CampusEntity");

            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "CampusEntity");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "CampusEntity");
        }
    }
}
