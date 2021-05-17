using Microsoft.EntityFrameworkCore.Migrations;

namespace Refosus.Web.Migrations
{
    public partial class shoppingtempitem7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ext",
                table: "TP_Shopping_ItemProvedorEntity");

            migrationBuilder.DropColumn(
                name: "FileName",
                table: "TP_Shopping_ItemProvedorEntity");

            migrationBuilder.AlterColumn<double>(
                name: "PrecioUnidad",
                table: "TP_Shopping_ItemProvedorEntity",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "PrecioTotal",
                table: "TP_Shopping_ItemProvedorEntity",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Cantidad",
                table: "TP_Shopping_ItemProvedorEntity",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "Telefono",
                table: "TP_Shopping_ItemProvedorEntity",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cantidad",
                table: "TP_Shopping_ItemProvedorEntity");

            migrationBuilder.DropColumn(
                name: "Telefono",
                table: "TP_Shopping_ItemProvedorEntity");

            migrationBuilder.AlterColumn<string>(
                name: "PrecioUnidad",
                table: "TP_Shopping_ItemProvedorEntity",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<string>(
                name: "PrecioTotal",
                table: "TP_Shopping_ItemProvedorEntity",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AddColumn<string>(
                name: "Ext",
                table: "TP_Shopping_ItemProvedorEntity",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "TP_Shopping_ItemProvedorEntity",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
