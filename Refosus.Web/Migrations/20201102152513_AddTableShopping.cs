using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Refosus.Web.Migrations
{
    public partial class AddTableShopping : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ShoppingEntityId",
                table: "ShoppingTempItems",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Shoppings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserCreateId = table.Column<string>(nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    UpdateDate = table.Column<DateTime>(nullable: false),
                    UserAssignedId = table.Column<string>(nullable: true),
                    StateId = table.Column<int>(nullable: true),
                    ProjectId = table.Column<int>(nullable: true),
                    UserProjectBossId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shoppings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Shoppings_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Shoppings_ShoppingStates_StateId",
                        column: x => x.StateId,
                        principalTable: "ShoppingStates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Shoppings_AspNetUsers_UserAssignedId",
                        column: x => x.UserAssignedId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Shoppings_AspNetUsers_UserCreateId",
                        column: x => x.UserCreateId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Shoppings_AspNetUsers_UserProjectBossId",
                        column: x => x.UserProjectBossId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ShoppingItemsEntity",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShopingId = table.Column<int>(nullable: true),
                    CodSAP = table.Column<string>(nullable: true),
                    CategoryId = table.Column<int>(nullable: true),
                    SubCategoryId = table.Column<int>(nullable: true),
                    UnitId = table.Column<int>(nullable: true),
                    MeasureId = table.Column<int>(nullable: true),
                    Quantity = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Refence = table.Column<string>(nullable: true),
                    Mark = table.Column<string>(nullable: true),
                    InternalOrder = table.Column<string>(nullable: true),
                    NumInternalOrder = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingItemsEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShoppingItemsEntity_ShoppingCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "ShoppingCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ShoppingItemsEntity_ShoppingMeasures_MeasureId",
                        column: x => x.MeasureId,
                        principalTable: "ShoppingMeasures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ShoppingItemsEntity_Shoppings_ShopingId",
                        column: x => x.ShopingId,
                        principalTable: "Shoppings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ShoppingItemsEntity_ShoppingCategories_SubCategoryId",
                        column: x => x.SubCategoryId,
                        principalTable: "ShoppingCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ShoppingItemsEntity_ShoppingUnits_UnitId",
                        column: x => x.UnitId,
                        principalTable: "ShoppingUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingTempItems_ShoppingEntityId",
                table: "ShoppingTempItems",
                column: "ShoppingEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingItemsEntity_CategoryId",
                table: "ShoppingItemsEntity",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingItemsEntity_MeasureId",
                table: "ShoppingItemsEntity",
                column: "MeasureId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingItemsEntity_ShopingId",
                table: "ShoppingItemsEntity",
                column: "ShopingId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingItemsEntity_SubCategoryId",
                table: "ShoppingItemsEntity",
                column: "SubCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingItemsEntity_UnitId",
                table: "ShoppingItemsEntity",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Shoppings_ProjectId",
                table: "Shoppings",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Shoppings_StateId",
                table: "Shoppings",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_Shoppings_UserAssignedId",
                table: "Shoppings",
                column: "UserAssignedId");

            migrationBuilder.CreateIndex(
                name: "IX_Shoppings_UserCreateId",
                table: "Shoppings",
                column: "UserCreateId");

            migrationBuilder.CreateIndex(
                name: "IX_Shoppings_UserProjectBossId",
                table: "Shoppings",
                column: "UserProjectBossId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingTempItems_Shoppings_ShoppingEntityId",
                table: "ShoppingTempItems",
                column: "ShoppingEntityId",
                principalTable: "Shoppings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingTempItems_Shoppings_ShoppingEntityId",
                table: "ShoppingTempItems");

            migrationBuilder.DropTable(
                name: "ShoppingItemsEntity");

            migrationBuilder.DropTable(
                name: "Shoppings");

            migrationBuilder.DropIndex(
                name: "IX_ShoppingTempItems_ShoppingEntityId",
                table: "ShoppingTempItems");

            migrationBuilder.DropColumn(
                name: "ShoppingEntityId",
                table: "ShoppingTempItems");
        }
    }
}
