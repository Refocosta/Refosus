using Microsoft.EntityFrameworkCore.Migrations;

namespace Refosus.Web.Migrations
{
    public partial class departments1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CityEntity_DepartmentEntity_DepartmentId",
                table: "CityEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_DepartmentEntity_Countries_CountryId",
                table: "DepartmentEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DepartmentEntity",
                table: "DepartmentEntity");

            migrationBuilder.RenameTable(
                name: "DepartmentEntity",
                newName: "Deparments");

            migrationBuilder.RenameIndex(
                name: "IX_DepartmentEntity_CountryId",
                table: "Deparments",
                newName: "IX_Deparments_CountryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Deparments",
                table: "Deparments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CityEntity_Deparments_DepartmentId",
                table: "CityEntity",
                column: "DepartmentId",
                principalTable: "Deparments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Deparments_Countries_CountryId",
                table: "Deparments",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CityEntity_Deparments_DepartmentId",
                table: "CityEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_Deparments_Countries_CountryId",
                table: "Deparments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Deparments",
                table: "Deparments");

            migrationBuilder.RenameTable(
                name: "Deparments",
                newName: "DepartmentEntity");

            migrationBuilder.RenameIndex(
                name: "IX_Deparments_CountryId",
                table: "DepartmentEntity",
                newName: "IX_DepartmentEntity_CountryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DepartmentEntity",
                table: "DepartmentEntity",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CityEntity_DepartmentEntity_DepartmentId",
                table: "CityEntity",
                column: "DepartmentId",
                principalTable: "DepartmentEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DepartmentEntity_Countries_CountryId",
                table: "DepartmentEntity",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
