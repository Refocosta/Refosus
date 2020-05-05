using Microsoft.EntityFrameworkCore.Migrations;

namespace Refosus.Web.Migrations
{
    public partial class cities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CityEntity_Conuntries_ConuntryId",
                table: "CityEntity");

            migrationBuilder.DropTable(
                name: "Conuntries");

            migrationBuilder.DropIndex(
                name: "IX_CityEntity_ConuntryId",
                table: "CityEntity");

            migrationBuilder.DropColumn(
                name: "ConuntryId",
                table: "CityEntity");

            migrationBuilder.AddColumn<int>(
                name: "CountryId",
                table: "CityEntity",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CityEntity_CountryId",
                table: "CityEntity",
                column: "CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_CityEntity_Countries_CountryId",
                table: "CityEntity",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CityEntity_Countries_CountryId",
                table: "CityEntity");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropIndex(
                name: "IX_CityEntity_CountryId",
                table: "CityEntity");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "CityEntity");

            migrationBuilder.AddColumn<int>(
                name: "ConuntryId",
                table: "CityEntity",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Conuntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conuntries", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CityEntity_ConuntryId",
                table: "CityEntity",
                column: "ConuntryId");

            migrationBuilder.AddForeignKey(
                name: "FK_CityEntity_Conuntries_ConuntryId",
                table: "CityEntity",
                column: "ConuntryId",
                principalTable: "Conuntries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
