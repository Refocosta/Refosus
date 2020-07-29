using Microsoft.EntityFrameworkCore.Migrations;

namespace Refosus.Web.Migrations
{
    public partial class MessageCheck2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MessageCheckes_AspNetUsers_UserId",
                table: "MessageCheckes");

            migrationBuilder.DropForeignKey(
                name: "FK_MessageCheckes_Messages_messageId",
                table: "MessageCheckes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MessageCheckes",
                table: "MessageCheckes");

            migrationBuilder.RenameTable(
                name: "MessageCheckes",
                newName: "MessagesCheckes");

            migrationBuilder.RenameIndex(
                name: "IX_MessageCheckes_messageId",
                table: "MessagesCheckes",
                newName: "IX_MessagesCheckes_messageId");

            migrationBuilder.RenameIndex(
                name: "IX_MessageCheckes_UserId",
                table: "MessagesCheckes",
                newName: "IX_MessagesCheckes_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MessagesCheckes",
                table: "MessagesCheckes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MessagesCheckes_AspNetUsers_UserId",
                table: "MessagesCheckes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MessagesCheckes_Messages_messageId",
                table: "MessagesCheckes",
                column: "messageId",
                principalTable: "Messages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MessagesCheckes_AspNetUsers_UserId",
                table: "MessagesCheckes");

            migrationBuilder.DropForeignKey(
                name: "FK_MessagesCheckes_Messages_messageId",
                table: "MessagesCheckes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MessagesCheckes",
                table: "MessagesCheckes");

            migrationBuilder.RenameTable(
                name: "MessagesCheckes",
                newName: "MessageCheckes");

            migrationBuilder.RenameIndex(
                name: "IX_MessagesCheckes_messageId",
                table: "MessageCheckes",
                newName: "IX_MessageCheckes_messageId");

            migrationBuilder.RenameIndex(
                name: "IX_MessagesCheckes_UserId",
                table: "MessageCheckes",
                newName: "IX_MessageCheckes_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MessageCheckes",
                table: "MessageCheckes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MessageCheckes_AspNetUsers_UserId",
                table: "MessageCheckes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MessageCheckes_Messages_messageId",
                table: "MessageCheckes",
                column: "messageId",
                principalTable: "Messages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
