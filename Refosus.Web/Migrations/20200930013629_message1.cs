using Microsoft.EntityFrameworkCore.Migrations;

namespace Refosus.Web.Migrations
{
    public partial class message1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Messages",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "CeCos",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "CeCos",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Messages_CompanyId",
                table: "Messages",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_CeCos_CompanyId",
                table: "CeCos",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_CeCos_Companies_CompanyId",
                table: "CeCos",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Companies_CompanyId",
                table: "Messages",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CeCos_Companies_CompanyId",
                table: "CeCos");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Companies_CompanyId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_CompanyId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_CeCos_CompanyId",
                table: "CeCos");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "CeCos");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "CeCos");
        }
    }
}
