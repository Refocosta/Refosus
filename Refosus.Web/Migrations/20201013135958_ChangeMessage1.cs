using Microsoft.EntityFrameworkCore.Migrations;

namespace Refosus.Web.Migrations
{
    public partial class ChangeMessage1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Companies_CompanyId",
                table: "Messages");

            migrationBuilder.AlterColumn<int>(
                name: "CompanyId",
                table: "Messages",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Companies_CompanyId",
                table: "Messages",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Companies_CompanyId",
                table: "Messages");

            migrationBuilder.AlterColumn<int>(
                name: "CompanyId",
                table: "Messages",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Companies_CompanyId",
                table: "Messages",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
