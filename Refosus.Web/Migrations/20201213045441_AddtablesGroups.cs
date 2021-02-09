using Microsoft.EntityFrameworkCore.Migrations;

namespace Refosus.Web.Migrations
{
    public partial class AddtablesGroups : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shoppings_TP_Groups_GroupId",
                table: "Shoppings");

            migrationBuilder.DropIndex(
                name: "IX_Shoppings_GroupId",
                table: "Shoppings");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "Shoppings");

            migrationBuilder.AddColumn<int>(
                name: "AssignedGroupId",
                table: "Shoppings",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreateGroupId",
                table: "Shoppings",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Total",
                table: "Shoppings",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "observations",
                table: "Shoppings",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "FullCost",
                table: "ShoppingItems",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "QuantityDelivered",
                table: "ShoppingItems",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StateId",
                table: "ShoppingItems",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "UnitCost",
                table: "ShoppingItems",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateTable(
                name: "TP_Shoping_Item_State",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(nullable: true),
                    State = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TP_Shoping_Item_State", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TP_Shopping_Article",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodSAP = table.Column<string>(nullable: true),
                    CategoryId = table.Column<int>(nullable: true),
                    SubCategoryId = table.Column<int>(nullable: true),
                    UnitId = table.Column<int>(nullable: true),
                    MeasureId = table.Column<int>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Refence = table.Column<string>(nullable: true),
                    Mark = table.Column<string>(nullable: true),
                    InternalOrder = table.Column<string>(nullable: true),
                    NumInternalOrder = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TP_Shopping_Article", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TP_Shopping_Article_ShoppingCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "ShoppingCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TP_Shopping_Article_ShoppingMeasures_MeasureId",
                        column: x => x.MeasureId,
                        principalTable: "ShoppingMeasures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TP_Shopping_Article_ShoppingCategories_SubCategoryId",
                        column: x => x.SubCategoryId,
                        principalTable: "ShoppingCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TP_Shopping_Article_ShoppingUnits_UnitId",
                        column: x => x.UnitId,
                        principalTable: "ShoppingUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TP_Shopping_Usu_Apr_Gro",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    User = table.Column<string>(nullable: true),
                    Group = table.Column<int>(nullable: false),
                    Amount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TP_Shopping_Usu_Apr_Gro", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Shoppings_AssignedGroupId",
                table: "Shoppings",
                column: "AssignedGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Shoppings_CreateGroupId",
                table: "Shoppings",
                column: "CreateGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingItems_StateId",
                table: "ShoppingItems",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_TP_Shopping_Article_CategoryId",
                table: "TP_Shopping_Article",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_TP_Shopping_Article_MeasureId",
                table: "TP_Shopping_Article",
                column: "MeasureId");

            migrationBuilder.CreateIndex(
                name: "IX_TP_Shopping_Article_SubCategoryId",
                table: "TP_Shopping_Article",
                column: "SubCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_TP_Shopping_Article_UnitId",
                table: "TP_Shopping_Article",
                column: "UnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingItems_TP_Shoping_Item_State_StateId",
                table: "ShoppingItems",
                column: "StateId",
                principalTable: "TP_Shoping_Item_State",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Shoppings_TP_Groups_AssignedGroupId",
                table: "Shoppings",
                column: "AssignedGroupId",
                principalTable: "TP_Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Shoppings_TP_Groups_CreateGroupId",
                table: "Shoppings",
                column: "CreateGroupId",
                principalTable: "TP_Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingItems_TP_Shoping_Item_State_StateId",
                table: "ShoppingItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Shoppings_TP_Groups_AssignedGroupId",
                table: "Shoppings");

            migrationBuilder.DropForeignKey(
                name: "FK_Shoppings_TP_Groups_CreateGroupId",
                table: "Shoppings");

            migrationBuilder.DropTable(
                name: "TP_Shoping_Item_State");

            migrationBuilder.DropTable(
                name: "TP_Shopping_Article");

            migrationBuilder.DropTable(
                name: "TP_Shopping_Usu_Apr_Gro");

            migrationBuilder.DropIndex(
                name: "IX_Shoppings_AssignedGroupId",
                table: "Shoppings");

            migrationBuilder.DropIndex(
                name: "IX_Shoppings_CreateGroupId",
                table: "Shoppings");

            migrationBuilder.DropIndex(
                name: "IX_ShoppingItems_StateId",
                table: "ShoppingItems");

            migrationBuilder.DropColumn(
                name: "AssignedGroupId",
                table: "Shoppings");

            migrationBuilder.DropColumn(
                name: "CreateGroupId",
                table: "Shoppings");

            migrationBuilder.DropColumn(
                name: "Total",
                table: "Shoppings");

            migrationBuilder.DropColumn(
                name: "observations",
                table: "Shoppings");

            migrationBuilder.DropColumn(
                name: "FullCost",
                table: "ShoppingItems");

            migrationBuilder.DropColumn(
                name: "QuantityDelivered",
                table: "ShoppingItems");

            migrationBuilder.DropColumn(
                name: "StateId",
                table: "ShoppingItems");

            migrationBuilder.DropColumn(
                name: "UnitCost",
                table: "ShoppingItems");

            migrationBuilder.AddColumn<int>(
                name: "GroupId",
                table: "Shoppings",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Shoppings_GroupId",
                table: "Shoppings",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Shoppings_TP_Groups_GroupId",
                table: "Shoppings",
                column: "GroupId",
                principalTable: "TP_Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
