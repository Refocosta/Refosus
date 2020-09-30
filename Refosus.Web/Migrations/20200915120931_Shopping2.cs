using Microsoft.EntityFrameworkCore.Migrations;

namespace Refosus.Web.Migrations
{
    public partial class Shopping2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingMeasures_ShoppingUnits_UnitId",
                table: "ShoppingMeasures");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShoppingMeasures",
                table: "ShoppingMeasures");

            migrationBuilder.RenameTable(
                name: "ShoppingMeasures",
                newName: "ShoppingMeasureEntity");

            migrationBuilder.RenameIndex(
                name: "IX_ShoppingMeasures_UnitId",
                table: "ShoppingMeasureEntity",
                newName: "IX_ShoppingMeasureEntity_UnitId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShoppingMeasureEntity",
                table: "ShoppingMeasureEntity",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingMeasureEntity_ShoppingUnits_UnitId",
                table: "ShoppingMeasureEntity",
                column: "UnitId",
                principalTable: "ShoppingUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingMeasureEntity_ShoppingUnits_UnitId",
                table: "ShoppingMeasureEntity");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShoppingMeasureEntity",
                table: "ShoppingMeasureEntity");

            migrationBuilder.RenameTable(
                name: "ShoppingMeasureEntity",
                newName: "ShoppingMeasures");

            migrationBuilder.RenameIndex(
                name: "IX_ShoppingMeasureEntity_UnitId",
                table: "ShoppingMeasures",
                newName: "IX_ShoppingMeasures_UnitId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShoppingMeasures",
                table: "ShoppingMeasures",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingMeasures_ShoppingUnits_UnitId",
                table: "ShoppingMeasures",
                column: "UnitId",
                principalTable: "ShoppingUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
