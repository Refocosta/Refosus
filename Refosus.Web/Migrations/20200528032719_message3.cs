using Microsoft.EntityFrameworkCore.Migrations;

namespace Refosus.Web.Migrations
{
    public partial class message3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MessagetransactionEntity_Messages_MessageId",
                table: "MessagetransactionEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_MessagetransactionEntity_MessagesStates_StateCreateId",
                table: "MessagetransactionEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_MessagetransactionEntity_MessagesStates_StateUpdateId",
                table: "MessagetransactionEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_MessagetransactionEntity_AspNetUsers_UserCreateId",
                table: "MessagetransactionEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_MessagetransactionEntity_AspNetUsers_UserUpdateId",
                table: "MessagetransactionEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MessagetransactionEntity",
                table: "MessagetransactionEntity");

            migrationBuilder.RenameTable(
                name: "MessagetransactionEntity",
                newName: "MessagesTransaction");

            migrationBuilder.RenameIndex(
                name: "IX_MessagetransactionEntity_UserUpdateId",
                table: "MessagesTransaction",
                newName: "IX_MessagesTransaction_UserUpdateId");

            migrationBuilder.RenameIndex(
                name: "IX_MessagetransactionEntity_UserCreateId",
                table: "MessagesTransaction",
                newName: "IX_MessagesTransaction_UserCreateId");

            migrationBuilder.RenameIndex(
                name: "IX_MessagetransactionEntity_StateUpdateId",
                table: "MessagesTransaction",
                newName: "IX_MessagesTransaction_StateUpdateId");

            migrationBuilder.RenameIndex(
                name: "IX_MessagetransactionEntity_StateCreateId",
                table: "MessagesTransaction",
                newName: "IX_MessagesTransaction_StateCreateId");

            migrationBuilder.RenameIndex(
                name: "IX_MessagetransactionEntity_MessageId",
                table: "MessagesTransaction",
                newName: "IX_MessagesTransaction_MessageId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MessagesTransaction",
                table: "MessagesTransaction",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MessagesTransaction_Messages_MessageId",
                table: "MessagesTransaction",
                column: "MessageId",
                principalTable: "Messages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MessagesTransaction_MessagesStates_StateCreateId",
                table: "MessagesTransaction",
                column: "StateCreateId",
                principalTable: "MessagesStates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MessagesTransaction_MessagesStates_StateUpdateId",
                table: "MessagesTransaction",
                column: "StateUpdateId",
                principalTable: "MessagesStates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MessagesTransaction_AspNetUsers_UserCreateId",
                table: "MessagesTransaction",
                column: "UserCreateId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MessagesTransaction_AspNetUsers_UserUpdateId",
                table: "MessagesTransaction",
                column: "UserUpdateId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MessagesTransaction_Messages_MessageId",
                table: "MessagesTransaction");

            migrationBuilder.DropForeignKey(
                name: "FK_MessagesTransaction_MessagesStates_StateCreateId",
                table: "MessagesTransaction");

            migrationBuilder.DropForeignKey(
                name: "FK_MessagesTransaction_MessagesStates_StateUpdateId",
                table: "MessagesTransaction");

            migrationBuilder.DropForeignKey(
                name: "FK_MessagesTransaction_AspNetUsers_UserCreateId",
                table: "MessagesTransaction");

            migrationBuilder.DropForeignKey(
                name: "FK_MessagesTransaction_AspNetUsers_UserUpdateId",
                table: "MessagesTransaction");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MessagesTransaction",
                table: "MessagesTransaction");

            migrationBuilder.RenameTable(
                name: "MessagesTransaction",
                newName: "MessagetransactionEntity");

            migrationBuilder.RenameIndex(
                name: "IX_MessagesTransaction_UserUpdateId",
                table: "MessagetransactionEntity",
                newName: "IX_MessagetransactionEntity_UserUpdateId");

            migrationBuilder.RenameIndex(
                name: "IX_MessagesTransaction_UserCreateId",
                table: "MessagetransactionEntity",
                newName: "IX_MessagetransactionEntity_UserCreateId");

            migrationBuilder.RenameIndex(
                name: "IX_MessagesTransaction_StateUpdateId",
                table: "MessagetransactionEntity",
                newName: "IX_MessagetransactionEntity_StateUpdateId");

            migrationBuilder.RenameIndex(
                name: "IX_MessagesTransaction_StateCreateId",
                table: "MessagetransactionEntity",
                newName: "IX_MessagetransactionEntity_StateCreateId");

            migrationBuilder.RenameIndex(
                name: "IX_MessagesTransaction_MessageId",
                table: "MessagetransactionEntity",
                newName: "IX_MessagetransactionEntity_MessageId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MessagetransactionEntity",
                table: "MessagetransactionEntity",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MessagetransactionEntity_Messages_MessageId",
                table: "MessagetransactionEntity",
                column: "MessageId",
                principalTable: "Messages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MessagetransactionEntity_MessagesStates_StateCreateId",
                table: "MessagetransactionEntity",
                column: "StateCreateId",
                principalTable: "MessagesStates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MessagetransactionEntity_MessagesStates_StateUpdateId",
                table: "MessagetransactionEntity",
                column: "StateUpdateId",
                principalTable: "MessagesStates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MessagetransactionEntity_AspNetUsers_UserCreateId",
                table: "MessagetransactionEntity",
                column: "UserCreateId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MessagetransactionEntity_AspNetUsers_UserUpdateId",
                table: "MessagetransactionEntity",
                column: "UserUpdateId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
