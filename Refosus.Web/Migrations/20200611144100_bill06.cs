using Microsoft.EntityFrameworkCore.Migrations;

namespace Refosus.Web.Migrations
{
    public partial class bill06 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MessagesTransaction_AspNetUsers_UserBillCreateId",
                table: "MessagesTransaction");

            migrationBuilder.DropForeignKey(
                name: "FK_MessagesTransaction_AspNetUsers_UserBillUpdateId",
                table: "MessagesTransaction");

            migrationBuilder.DropIndex(
                name: "IX_MessagesTransaction_UserBillCreateId",
                table: "MessagesTransaction");

            migrationBuilder.DropIndex(
                name: "IX_MessagesTransaction_UserBillUpdateId",
                table: "MessagesTransaction");

            migrationBuilder.DropColumn(
                name: "UserBillCreateId",
                table: "MessagesTransaction");

            migrationBuilder.DropColumn(
                name: "UserBillUpdateId",
                table: "MessagesTransaction");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MessagesTransaction",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "UserBillAuthoId",
                table: "MessagesTransaction",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserBillFinishedId",
                table: "MessagesTransaction",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MessagesTransaction_UserBillAuthoId",
                table: "MessagesTransaction",
                column: "UserBillAuthoId");

            migrationBuilder.CreateIndex(
                name: "IX_MessagesTransaction_UserBillFinishedId",
                table: "MessagesTransaction",
                column: "UserBillFinishedId");

            migrationBuilder.AddForeignKey(
                name: "FK_MessagesTransaction_AspNetUsers_UserBillAuthoId",
                table: "MessagesTransaction",
                column: "UserBillAuthoId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MessagesTransaction_AspNetUsers_UserBillFinishedId",
                table: "MessagesTransaction",
                column: "UserBillFinishedId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MessagesTransaction_AspNetUsers_UserBillAuthoId",
                table: "MessagesTransaction");

            migrationBuilder.DropForeignKey(
                name: "FK_MessagesTransaction_AspNetUsers_UserBillFinishedId",
                table: "MessagesTransaction");

            migrationBuilder.DropIndex(
                name: "IX_MessagesTransaction_UserBillAuthoId",
                table: "MessagesTransaction");

            migrationBuilder.DropIndex(
                name: "IX_MessagesTransaction_UserBillFinishedId",
                table: "MessagesTransaction");

            migrationBuilder.DropColumn(
                name: "UserBillAuthoId",
                table: "MessagesTransaction");

            migrationBuilder.DropColumn(
                name: "UserBillFinishedId",
                table: "MessagesTransaction");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MessagesTransaction",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserBillCreateId",
                table: "MessagesTransaction",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserBillUpdateId",
                table: "MessagesTransaction",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MessagesTransaction_UserBillCreateId",
                table: "MessagesTransaction",
                column: "UserBillCreateId");

            migrationBuilder.CreateIndex(
                name: "IX_MessagesTransaction_UserBillUpdateId",
                table: "MessagesTransaction",
                column: "UserBillUpdateId");

            migrationBuilder.AddForeignKey(
                name: "FK_MessagesTransaction_AspNetUsers_UserBillCreateId",
                table: "MessagesTransaction",
                column: "UserBillCreateId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MessagesTransaction_AspNetUsers_UserBillUpdateId",
                table: "MessagesTransaction",
                column: "UserBillUpdateId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
