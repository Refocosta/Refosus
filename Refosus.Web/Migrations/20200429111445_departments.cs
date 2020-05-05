using Microsoft.EntityFrameworkCore.Migrations;

namespace Refosus.Web.Migrations
{
    public partial class departments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CityEntity_Countries_CountryId",
                table: "CityEntity");

            migrationBuilder.DropIndex(
                name: "IX_CityEntity_CountryId",
                table: "CityEntity");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "CityEntity");

            migrationBuilder.AddColumn<int>(
                name: "DepartmentId",
                table: "CityEntity",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DepartmentEntity",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    CountryId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepartmentEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DepartmentEntity_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CityEntity_DepartmentId",
                table: "CityEntity",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentEntity_CountryId",
                table: "DepartmentEntity",
                column: "CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_CityEntity_DepartmentEntity_DepartmentId",
                table: "CityEntity",
                column: "DepartmentId",
                principalTable: "DepartmentEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CityEntity_DepartmentEntity_DepartmentId",
                table: "CityEntity");

            migrationBuilder.DropTable(
                name: "DepartmentEntity");

            migrationBuilder.DropIndex(
                name: "IX_CityEntity_DepartmentId",
                table: "CityEntity");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "CityEntity");

            migrationBuilder.AddColumn<int>(
                name: "CountryId",
                table: "CityEntity",
                type: "int",
                nullable: true);

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
    }
}
