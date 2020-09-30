using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Refosus.Web.Migrations
{
    public partial class shopping7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ShoppingTempItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    UserCreateId = table.Column<string>(nullable: true),
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
                    table.PrimaryKey("PK_ShoppingTempItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShoppingTempItems_ShoppingCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "ShoppingCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ShoppingTempItems_ShoppingMeasures_MeasureId",
                        column: x => x.MeasureId,
                        principalTable: "ShoppingMeasures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ShoppingTempItems_ShoppingCategories_SubCategoryId",
                        column: x => x.SubCategoryId,
                        principalTable: "ShoppingCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ShoppingTempItems_ShoppingUnits_UnitId",
                        column: x => x.UnitId,
                        principalTable: "ShoppingUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ShoppingTempItems_AspNetUsers_UserCreateId",
                        column: x => x.UserCreateId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingTempItems_CategoryId",
                table: "ShoppingTempItems",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingTempItems_MeasureId",
                table: "ShoppingTempItems",
                column: "MeasureId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingTempItems_SubCategoryId",
                table: "ShoppingTempItems",
                column: "SubCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingTempItems_UnitId",
                table: "ShoppingTempItems",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingTempItems_UserCreateId",
                table: "ShoppingTempItems",
                column: "UserCreateId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShoppingTempItems");
        }
    }
}
