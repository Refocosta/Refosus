using Microsoft.EntityFrameworkCore.Migrations;

namespace Refosus.Web.Migrations
{
    public partial class Menu4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Dependence",
                table: "Menus");

            migrationBuilder.AddColumn<int>(
                name: "MenusId",
                table: "Menus",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Menus_MenusId",
                table: "Menus",
                column: "MenusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Menus_Menus_MenusId",
                table: "Menus",
                column: "MenusId",
                principalTable: "Menus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Menus_Menus_MenusId",
                table: "Menus");

            migrationBuilder.DropIndex(
                name: "IX_Menus_MenusId",
                table: "Menus");

            migrationBuilder.DropColumn(
                name: "MenusId",
                table: "Menus");

            migrationBuilder.AddColumn<int>(
                name: "Dependence",
                table: "Menus",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
