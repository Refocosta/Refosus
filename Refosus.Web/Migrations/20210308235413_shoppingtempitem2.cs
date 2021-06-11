using Microsoft.EntityFrameworkCore.Migrations;

namespace Refosus.Web.Migrations
{
    public partial class shoppingtempitem2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FullCost",
                table: "ShoppingItems");

            migrationBuilder.DropColumn(
                name: "UnitCost",
                table: "ShoppingItems");

            migrationBuilder.AddColumn<string>(
                name: "Observation",
                table: "ShoppingTempItems",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Shoppings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Observation",
                table: "ShoppingItems",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserAssignedId",
                table: "ShoppingItems",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TP_Shopping_ItemProvedorEntity",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    PrecioUnidad = table.Column<string>(nullable: true),
                    PrecioTotal = table.Column<string>(nullable: true),
                    FileName = table.Column<string>(nullable: true),
                    FilePath = table.Column<string>(nullable: true),
                    Ext = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TP_Shopping_ItemProvedorEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TP_Shopping_ItemProvedorEntity_ShoppingItems_ItemId",
                        column: x => x.ItemId,
                        principalTable: "ShoppingItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Shoppings_CompanyId",
                table: "Shoppings",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingItems_UserAssignedId",
                table: "ShoppingItems",
                column: "UserAssignedId");

            migrationBuilder.CreateIndex(
                name: "IX_TP_Shopping_ItemProvedorEntity_ItemId",
                table: "TP_Shopping_ItemProvedorEntity",
                column: "ItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingItems_AspNetUsers_UserAssignedId",
                table: "ShoppingItems",
                column: "UserAssignedId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Shoppings_Companies_CompanyId",
                table: "Shoppings",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingItems_AspNetUsers_UserAssignedId",
                table: "ShoppingItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Shoppings_Companies_CompanyId",
                table: "Shoppings");

            migrationBuilder.DropTable(
                name: "TP_Shopping_ItemProvedorEntity");

            migrationBuilder.DropIndex(
                name: "IX_Shoppings_CompanyId",
                table: "Shoppings");

            migrationBuilder.DropIndex(
                name: "IX_ShoppingItems_UserAssignedId",
                table: "ShoppingItems");

            migrationBuilder.DropColumn(
                name: "Observation",
                table: "ShoppingTempItems");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Shoppings");

            migrationBuilder.DropColumn(
                name: "Observation",
                table: "ShoppingItems");

            migrationBuilder.DropColumn(
                name: "UserAssignedId",
                table: "ShoppingItems");

            migrationBuilder.AddColumn<double>(
                name: "FullCost",
                table: "ShoppingItems",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "UnitCost",
                table: "ShoppingItems",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
