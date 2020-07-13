using Microsoft.EntityFrameworkCore.Migrations;

namespace Refosus.Web.Migrations
{
    public partial class messagechange1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserCreateId",
                table: "Messages",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserSenderId",
                table: "Messages",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Messages_UserCreateId",
                table: "Messages",
                column: "UserCreateId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_UserSenderId",
                table: "Messages",
                column: "UserSenderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_AspNetUsers_UserCreateId",
                table: "Messages",
                column: "UserCreateId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_AspNetUsers_UserSenderId",
                table: "Messages",
                column: "UserSenderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_AspNetUsers_UserCreateId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_AspNetUsers_UserSenderId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_UserCreateId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_UserSenderId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "UserCreateId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "UserSenderId",
                table: "Messages");
        }
    }
}
