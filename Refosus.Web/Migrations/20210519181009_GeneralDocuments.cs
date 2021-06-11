using Microsoft.EntityFrameworkCore.Migrations;

namespace Refosus.Web.Migrations
{
    public partial class GeneralDocuments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GeneralDocuments",
                columns: table => new
                {
                    Alias = table.Column<string>(nullable: false),
                    Name  = table.Column<string>(nullable: false),
                    Path  = table.Column<string>(nullable: false),
                    Ext   = table.Column<string>(nullable: false)
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GeneralDocuments");
        }
    }
}
