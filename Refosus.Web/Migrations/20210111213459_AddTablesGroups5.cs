using Microsoft.EntityFrameworkCore.Migrations;

namespace Refosus.Web.Migrations
{
    public partial class AddTablesGroups5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "TP_Groups",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TP_Groups_CompanyId",
                table: "TP_Groups",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_TP_Groups_Companies_CompanyId",
                table: "TP_Groups",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TP_Groups_Companies_CompanyId",
                table: "TP_Groups");

            migrationBuilder.DropIndex(
                name: "IX_TP_Groups_CompanyId",
                table: "TP_Groups");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "TP_Groups");
        }
    }
}
