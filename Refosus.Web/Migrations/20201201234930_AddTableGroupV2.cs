using Microsoft.EntityFrameworkCore.Migrations;

namespace Refosus.Web.Migrations
{
    public partial class AddTableGroupV2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shoppings_TM_GroupEntity_GroupId",
                table: "Shoppings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TM_GroupEntity",
                table: "TM_GroupEntity");

            migrationBuilder.RenameTable(
                name: "TM_GroupEntity",
                newName: "TM_Groups");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TM_Groups",
                table: "TM_Groups",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Shoppings_TM_Groups_GroupId",
                table: "Shoppings",
                column: "GroupId",
                principalTable: "TM_Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shoppings_TM_Groups_GroupId",
                table: "Shoppings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TM_Groups",
                table: "TM_Groups");

            migrationBuilder.RenameTable(
                name: "TM_Groups",
                newName: "TM_GroupEntity");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TM_GroupEntity",
                table: "TM_GroupEntity",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Shoppings_TM_GroupEntity_GroupId",
                table: "Shoppings",
                column: "GroupId",
                principalTable: "TM_GroupEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
