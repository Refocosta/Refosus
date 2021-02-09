using Microsoft.EntityFrameworkCore.Migrations;

namespace Refosus.Web.Migrations
{
    public partial class AddtablesGroups2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Total",
                table: "Shoppings");

            migrationBuilder.AddColumn<bool>(
                name: "State",
                table: "ShoppingStates",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "TotalValue",
                table: "Shoppings",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "State",
                table: "ShoppingStates");

            migrationBuilder.DropColumn(
                name: "TotalValue",
                table: "Shoppings");

            migrationBuilder.AddColumn<double>(
                name: "Total",
                table: "Shoppings",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
