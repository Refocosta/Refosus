using Microsoft.EntityFrameworkCore.Migrations;

namespace Refosus.Web.Migrations
{
    public partial class Adjunto3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MessageFileEntity_Messages_messageId",
                table: "MessageFileEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MessageFileEntity",
                table: "MessageFileEntity");

            migrationBuilder.RenameTable(
                name: "MessageFileEntity",
                newName: "MessagesFile");

            migrationBuilder.RenameIndex(
                name: "IX_MessageFileEntity_messageId",
                table: "MessagesFile",
                newName: "IX_MessagesFile_messageId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MessagesFile",
                table: "MessagesFile",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MessagesFile_Messages_messageId",
                table: "MessagesFile",
                column: "messageId",
                principalTable: "Messages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MessagesFile_Messages_messageId",
                table: "MessagesFile");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MessagesFile",
                table: "MessagesFile");

            migrationBuilder.RenameTable(
                name: "MessagesFile",
                newName: "MessageFileEntity");

            migrationBuilder.RenameIndex(
                name: "IX_MessagesFile_messageId",
                table: "MessageFileEntity",
                newName: "IX_MessageFileEntity_messageId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MessageFileEntity",
                table: "MessageFileEntity",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MessageFileEntity_Messages_messageId",
                table: "MessageFileEntity",
                column: "messageId",
                principalTable: "Messages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
