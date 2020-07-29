using Microsoft.EntityFrameworkCore.Migrations;

namespace Refosus.Web.Migrations
{
    public partial class MessageCheck1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Check_AspNetUsers_UserId",
                table: "Check");

            migrationBuilder.DropForeignKey(
                name: "FK_Check_Messages_messageId",
                table: "Check");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Check",
                table: "Check");

            migrationBuilder.RenameTable(
                name: "Check",
                newName: "MessageCheckes");

            migrationBuilder.RenameIndex(
                name: "IX_Check_messageId",
                table: "MessageCheckes",
                newName: "IX_MessageCheckes_messageId");

            migrationBuilder.RenameIndex(
                name: "IX_Check_UserId",
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

        protected override void Down(MigrationBuilder migrationBuilder)
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
                newName: "Check");

            migrationBuilder.RenameIndex(
                name: "IX_MessageCheckes_messageId",
                table: "Check",
                newName: "IX_Check_messageId");

            migrationBuilder.RenameIndex(
                name: "IX_MessageCheckes_UserId",
                table: "Check",
                newName: "IX_Check_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Check",
                table: "Check",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Check_AspNetUsers_UserId",
                table: "Check",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Check_Messages_messageId",
                table: "Check",
                column: "messageId",
                principalTable: "Messages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
