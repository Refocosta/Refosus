using Microsoft.EntityFrameworkCore.Migrations;

namespace Refosus.Web.Migrations
{
    public partial class AddTableShoppingItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingItemsEntity_ShoppingCategories_CategoryId",
                table: "ShoppingItemsEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingItemsEntity_ShoppingMeasures_MeasureId",
                table: "ShoppingItemsEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingItemsEntity_Shoppings_ShopingId",
                table: "ShoppingItemsEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingItemsEntity_ShoppingCategories_SubCategoryId",
                table: "ShoppingItemsEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingItemsEntity_ShoppingUnits_UnitId",
                table: "ShoppingItemsEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShoppingItemsEntity",
                table: "ShoppingItemsEntity");

            migrationBuilder.RenameTable(
                name: "ShoppingItemsEntity",
                newName: "ShoppingItems");

            migrationBuilder.RenameIndex(
                name: "IX_ShoppingItemsEntity_UnitId",
                table: "ShoppingItems",
                newName: "IX_ShoppingItems_UnitId");

            migrationBuilder.RenameIndex(
                name: "IX_ShoppingItemsEntity_SubCategoryId",
                table: "ShoppingItems",
                newName: "IX_ShoppingItems_SubCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_ShoppingItemsEntity_ShopingId",
                table: "ShoppingItems",
                newName: "IX_ShoppingItems_ShopingId");

            migrationBuilder.RenameIndex(
                name: "IX_ShoppingItemsEntity_MeasureId",
                table: "ShoppingItems",
                newName: "IX_ShoppingItems_MeasureId");

            migrationBuilder.RenameIndex(
                name: "IX_ShoppingItemsEntity_CategoryId",
                table: "ShoppingItems",
                newName: "IX_ShoppingItems_CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShoppingItems",
                table: "ShoppingItems",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingItems_ShoppingCategories_CategoryId",
                table: "ShoppingItems",
                column: "CategoryId",
                principalTable: "ShoppingCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingItems_ShoppingMeasures_MeasureId",
                table: "ShoppingItems",
                column: "MeasureId",
                principalTable: "ShoppingMeasures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingItems_Shoppings_ShopingId",
                table: "ShoppingItems",
                column: "ShopingId",
                principalTable: "Shoppings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingItems_ShoppingCategories_SubCategoryId",
                table: "ShoppingItems",
                column: "SubCategoryId",
                principalTable: "ShoppingCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingItems_ShoppingUnits_UnitId",
                table: "ShoppingItems",
                column: "UnitId",
                principalTable: "ShoppingUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingItems_ShoppingCategories_CategoryId",
                table: "ShoppingItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingItems_ShoppingMeasures_MeasureId",
                table: "ShoppingItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingItems_Shoppings_ShopingId",
                table: "ShoppingItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingItems_ShoppingCategories_SubCategoryId",
                table: "ShoppingItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingItems_ShoppingUnits_UnitId",
                table: "ShoppingItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShoppingItems",
                table: "ShoppingItems");

            migrationBuilder.RenameTable(
                name: "ShoppingItems",
                newName: "ShoppingItemsEntity");

            migrationBuilder.RenameIndex(
                name: "IX_ShoppingItems_UnitId",
                table: "ShoppingItemsEntity",
                newName: "IX_ShoppingItemsEntity_UnitId");

            migrationBuilder.RenameIndex(
                name: "IX_ShoppingItems_SubCategoryId",
                table: "ShoppingItemsEntity",
                newName: "IX_ShoppingItemsEntity_SubCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_ShoppingItems_ShopingId",
                table: "ShoppingItemsEntity",
                newName: "IX_ShoppingItemsEntity_ShopingId");

            migrationBuilder.RenameIndex(
                name: "IX_ShoppingItems_MeasureId",
                table: "ShoppingItemsEntity",
                newName: "IX_ShoppingItemsEntity_MeasureId");

            migrationBuilder.RenameIndex(
                name: "IX_ShoppingItems_CategoryId",
                table: "ShoppingItemsEntity",
                newName: "IX_ShoppingItemsEntity_CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShoppingItemsEntity",
                table: "ShoppingItemsEntity",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingItemsEntity_ShoppingCategories_CategoryId",
                table: "ShoppingItemsEntity",
                column: "CategoryId",
                principalTable: "ShoppingCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingItemsEntity_ShoppingMeasures_MeasureId",
                table: "ShoppingItemsEntity",
                column: "MeasureId",
                principalTable: "ShoppingMeasures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingItemsEntity_Shoppings_ShopingId",
                table: "ShoppingItemsEntity",
                column: "ShopingId",
                principalTable: "Shoppings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingItemsEntity_ShoppingCategories_SubCategoryId",
                table: "ShoppingItemsEntity",
                column: "SubCategoryId",
                principalTable: "ShoppingCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingItemsEntity_ShoppingUnits_UnitId",
                table: "ShoppingItemsEntity",
                column: "UnitId",
                principalTable: "ShoppingUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
