using Microsoft.EntityFrameworkCore.Migrations;

namespace Refosus.Web.Migrations
{
    public partial class conuntries10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CampusEntity_CityEntity_CityEntityId",
                table: "CampusEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_CityEntity_Conuntries_ConuntryEntityId",
                table: "CityEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_Companies_CampusEntity_CampusEntityId",
                table: "Companies");

            migrationBuilder.DropIndex(
                name: "IX_Companies_CampusEntityId",
                table: "Companies");

            migrationBuilder.DropIndex(
                name: "IX_CityEntity_ConuntryEntityId",
                table: "CityEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CampusEntity",
                table: "CampusEntity");

            migrationBuilder.DropIndex(
                name: "IX_CampusEntity_CityEntityId",
                table: "CampusEntity");

            migrationBuilder.DropColumn(
                name: "CampusEntityId",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "Conuntry",
                table: "CityEntity");

            migrationBuilder.DropColumn(
                name: "ConuntryEntityId",
                table: "CityEntity");

            migrationBuilder.DropColumn(
                name: "City",
                table: "CampusEntity");

            migrationBuilder.DropColumn(
                name: "CityEntityId",
                table: "CampusEntity");

            migrationBuilder.RenameTable(
                name: "CampusEntity",
                newName: "Campus");

            migrationBuilder.AddColumn<int>(
                name: "ConuntryId",
                table: "CityEntity",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CityId",
                table: "Campus",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Campus",
                table: "Campus",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_CityEntity_ConuntryId",
                table: "CityEntity",
                column: "ConuntryId");

            migrationBuilder.CreateIndex(
                name: "IX_Campus_CityId",
                table: "Campus",
                column: "CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Campus_CityEntity_CityId",
                table: "Campus",
                column: "CityId",
                principalTable: "CityEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CityEntity_Conuntries_ConuntryId",
                table: "CityEntity",
                column: "ConuntryId",
                principalTable: "Conuntries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Campus_CityEntity_CityId",
                table: "Campus");

            migrationBuilder.DropForeignKey(
                name: "FK_CityEntity_Conuntries_ConuntryId",
                table: "CityEntity");

            migrationBuilder.DropIndex(
                name: "IX_CityEntity_ConuntryId",
                table: "CityEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Campus",
                table: "Campus");

            migrationBuilder.DropIndex(
                name: "IX_Campus_CityId",
                table: "Campus");

            migrationBuilder.DropColumn(
                name: "ConuntryId",
                table: "CityEntity");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "Campus");

            migrationBuilder.RenameTable(
                name: "Campus",
                newName: "CampusEntity");

            migrationBuilder.AddColumn<int>(
                name: "CampusEntityId",
                table: "Companies",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Conuntry",
                table: "CityEntity",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ConuntryEntityId",
                table: "CityEntity",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "CampusEntity",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CityEntityId",
                table: "CampusEntity",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CampusEntity",
                table: "CampusEntity",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_CampusEntityId",
                table: "Companies",
                column: "CampusEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_CityEntity_ConuntryEntityId",
                table: "CityEntity",
                column: "ConuntryEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_CampusEntity_CityEntityId",
                table: "CampusEntity",
                column: "CityEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_CampusEntity_CityEntity_CityEntityId",
                table: "CampusEntity",
                column: "CityEntityId",
                principalTable: "CityEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CityEntity_Conuntries_ConuntryEntityId",
                table: "CityEntity",
                column: "ConuntryEntityId",
                principalTable: "Conuntries",
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
    }
}
