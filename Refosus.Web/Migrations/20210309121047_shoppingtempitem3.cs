using Microsoft.EntityFrameworkCore.Migrations;

namespace Refosus.Web.Migrations
{
    public partial class shoppingtempitem3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ResponsableId",
                table: "ShoppingCategories",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TP_Shopping_ItemSAPEntity",
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
                    table.PrimaryKey("PK_TP_Shopping_ItemSAPEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TP_Shopping_ItemSAPEntity_ShoppingCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "ShoppingCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TP_Shopping_ItemSAPEntity_ShoppingMeasures_MeasureId",
                        column: x => x.MeasureId,
                        principalTable: "ShoppingMeasures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TP_Shopping_ItemSAPEntity_ShoppingCategories_SubCategoryId",
                        column: x => x.SubCategoryId,
                        principalTable: "ShoppingCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TP_Shopping_ItemSAPEntity_ShoppingUnits_UnitId",
                        column: x => x.UnitId,
                        principalTable: "ShoppingUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCategories_ResponsableId",
                table: "ShoppingCategories",
                column: "ResponsableId");

            migrationBuilder.CreateIndex(
                name: "IX_TP_Shopping_ItemSAPEntity_CategoryId",
                table: "TP_Shopping_ItemSAPEntity",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_TP_Shopping_ItemSAPEntity_MeasureId",
                table: "TP_Shopping_ItemSAPEntity",
                column: "MeasureId");

            migrationBuilder.CreateIndex(
                name: "IX_TP_Shopping_ItemSAPEntity_SubCategoryId",
                table: "TP_Shopping_ItemSAPEntity",
                column: "SubCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_TP_Shopping_ItemSAPEntity_UnitId",
                table: "TP_Shopping_ItemSAPEntity",
                column: "UnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCategories_AspNetUsers_ResponsableId",
                table: "ShoppingCategories",
                column: "ResponsableId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCategories_AspNetUsers_ResponsableId",
                table: "ShoppingCategories");

            migrationBuilder.DropTable(
                name: "TP_Shopping_ItemSAPEntity");

            migrationBuilder.DropIndex(
                name: "IX_ShoppingCategories_ResponsableId",
                table: "ShoppingCategories");

            migrationBuilder.DropColumn(
                name: "ResponsableId",
                table: "ShoppingCategories");
        }
    }
}
