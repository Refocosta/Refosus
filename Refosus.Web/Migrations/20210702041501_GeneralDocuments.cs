using Microsoft.EntityFrameworkCore.Migrations;
using System;

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
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Alias = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Path = table.Column<string>(nullable: true),
                    Ext = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: true),
                    GeneralDocumentsCategoriesId = table.Column<int>(nullable: true),
                    CreateAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GeneralDocuments_GeneralDocumentsCategories_GeneralDocumentsCategoriesId",
                        column: x => x.GeneralDocumentsCategoriesId,
                        principalTable: "GeneralDocumentsCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GeneralDocuments");
        }
    }
}
