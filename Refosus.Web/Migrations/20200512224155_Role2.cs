using Microsoft.EntityFrameworkCore.Migrations;

namespace Refosus.Web.Migrations
{
    public partial class Role2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoleMenuEntity_Menus_MenuId",
                table: "RoleMenuEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_RoleMenuEntity_AspNetRoles_RoleId",
                table: "RoleMenuEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoleMenuEntity",
                table: "RoleMenuEntity");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetRoles");

            migrationBuilder.RenameTable(
                name: "RoleMenuEntity",
                newName: "RoleMenus");

            migrationBuilder.RenameIndex(
                name: "IX_RoleMenuEntity_RoleId",
                table: "RoleMenus",
                newName: "IX_RoleMenus_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_RoleMenuEntity_MenuId",
                table: "RoleMenus",
                newName: "IX_RoleMenus_MenuId");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "AspNetRoles",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoleMenus",
                table: "RoleMenus",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RoleMenus_Menus_MenuId",
                table: "RoleMenus",
                column: "MenuId",
                principalTable: "Menus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RoleMenus_AspNetRoles_RoleId",
                table: "RoleMenus",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoleMenus_Menus_MenuId",
                table: "RoleMenus");

            migrationBuilder.DropForeignKey(
                name: "FK_RoleMenus_AspNetRoles_RoleId",
                table: "RoleMenus");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoleMenus",
                table: "RoleMenus");

            migrationBuilder.RenameTable(
                name: "RoleMenus",
                newName: "RoleMenuEntity");

            migrationBuilder.RenameIndex(
                name: "IX_RoleMenus_RoleId",
                table: "RoleMenuEntity",
                newName: "IX_RoleMenuEntity_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_RoleMenus_MenuId",
                table: "RoleMenuEntity",
                newName: "IX_RoleMenuEntity_MenuId");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "AspNetRoles",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool));

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetRoles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoleMenuEntity",
                table: "RoleMenuEntity",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RoleMenuEntity_Menus_MenuId",
                table: "RoleMenuEntity",
                column: "MenuId",
                principalTable: "Menus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RoleMenuEntity_AspNetRoles_RoleId",
                table: "RoleMenuEntity",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
