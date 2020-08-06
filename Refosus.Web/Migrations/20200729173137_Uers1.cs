using Microsoft.EntityFrameworkCore.Migrations;

namespace Refosus.Web.Migrations
{
    public partial class Uers1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_DocumentTypeEntity_TypeDocumentId",
                table: "AspNetUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DocumentTypeEntity",
                table: "DocumentTypeEntity");

            migrationBuilder.RenameTable(
                name: "DocumentTypeEntity",
                newName: "DocumentTypes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DocumentTypes",
                table: "DocumentTypes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_DocumentTypes_TypeDocumentId",
                table: "AspNetUsers",
                column: "TypeDocumentId",
                principalTable: "DocumentTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_DocumentTypes_TypeDocumentId",
                table: "AspNetUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DocumentTypes",
                table: "DocumentTypes");

            migrationBuilder.RenameTable(
                name: "DocumentTypes",
                newName: "DocumentTypeEntity");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DocumentTypeEntity",
                table: "DocumentTypeEntity",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_DocumentTypeEntity_TypeDocumentId",
                table: "AspNetUsers",
                column: "TypeDocumentId",
                principalTable: "DocumentTypeEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
