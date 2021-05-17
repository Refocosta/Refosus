using Microsoft.EntityFrameworkCore.Migrations;

namespace Refosus.Web.Migrations
{
    public partial class shoppingtempitem5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Refence",
                table: "TP_Shopping_ItemSAPEntity");

            migrationBuilder.AddColumn<string>(
                name: "Reference",
                table: "TP_Shopping_ItemSAPEntity",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Reference",
                table: "TP_Shopping_ItemSAPEntity");

            migrationBuilder.AddColumn<string>(
                name: "Refence",
                table: "TP_Shopping_ItemSAPEntity",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
