using Microsoft.EntityFrameworkCore.Migrations;

namespace Refosus.Web.Migrations
{
    public partial class ChangeMessage2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Companies_CompanyId",
                table: "Messages");

            migrationBuilder.AlterColumn<int>(
                name: "CompanyId",
                table: "Messages",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

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
                name: "FK_Messages_Companies_CompanyId",
                table: "Messages");

            migrationBuilder.AlterColumn<int>(
                name: "CompanyId",
                table: "Messages",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Companies_CompanyId",
                table: "Messages",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
