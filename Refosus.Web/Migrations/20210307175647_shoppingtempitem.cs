using Microsoft.EntityFrameworkCore.Migrations;

namespace Refosus.Web.Migrations
{
    public partial class shoppingtempitem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Refence",
                table: "ShoppingTempItems");

            migrationBuilder.DropColumn(
                name: "Refence",
                table: "ShoppingItems");

            migrationBuilder.AddColumn<string>(
                name: "Reference",
                table: "ShoppingTempItems",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Reference",
                table: "ShoppingItems",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Reference",
                table: "ShoppingTempItems");

            migrationBuilder.DropColumn(
                name: "Reference",
                table: "ShoppingItems");

            migrationBuilder.AddColumn<string>(
                name: "Refence",
                table: "ShoppingTempItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Refence",
                table: "ShoppingItems",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
