using Microsoft.EntityFrameworkCore.Migrations;

namespace Refosus.Web.Migrations
{
    public partial class ChangeMessage3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "CeCos",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "UserResponsibleId",
                table: "CeCos",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CeCos_UserResponsibleId",
                table: "CeCos",
                column: "UserResponsibleId");

            migrationBuilder.AddForeignKey(
                name: "FK_CeCos_AspNetUsers_UserResponsibleId",
                table: "CeCos",
                column: "UserResponsibleId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CeCos_AspNetUsers_UserResponsibleId",
                table: "CeCos");

            migrationBuilder.DropIndex(
                name: "IX_CeCos_UserResponsibleId",
                table: "CeCos");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "CeCos");

            migrationBuilder.DropColumn(
                name: "UserResponsibleId",
                table: "CeCos");
        }
    }
}
