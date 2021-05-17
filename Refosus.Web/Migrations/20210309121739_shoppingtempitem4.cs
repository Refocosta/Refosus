using Microsoft.EntityFrameworkCore.Migrations;

namespace Refosus.Web.Migrations
{
    public partial class shoppingtempitem4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingItems_TP_Shoping_Item_State_StateId",
                table: "ShoppingItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TP_Shoping_Item_State",
                table: "TP_Shoping_Item_State");

            migrationBuilder.RenameTable(
                name: "TP_Shoping_Item_State",
                newName: "TP_Shopping_Item_State");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TP_Shopping_Item_State",
                table: "TP_Shopping_Item_State",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingItems_TP_Shopping_Item_State_StateId",
                table: "ShoppingItems",
                column: "StateId",
                principalTable: "TP_Shopping_Item_State",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingItems_TP_Shopping_Item_State_StateId",
                table: "ShoppingItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TP_Shopping_Item_State",
                table: "TP_Shopping_Item_State");

            migrationBuilder.RenameTable(
                name: "TP_Shopping_Item_State",
                newName: "TP_Shoping_Item_State");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TP_Shoping_Item_State",
                table: "TP_Shoping_Item_State",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingItems_TP_Shoping_Item_State_StateId",
                table: "ShoppingItems",
                column: "StateId",
                principalTable: "TP_Shoping_Item_State",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
