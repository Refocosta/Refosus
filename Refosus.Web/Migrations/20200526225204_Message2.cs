using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Refosus.Web.Migrations
{
    public partial class Message2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MessagetransactionEntity_Messages_MessageEntityId",
                table: "MessagetransactionEntity");

            migrationBuilder.DropIndex(
                name: "IX_MessagetransactionEntity_MessageEntityId",
                table: "MessagetransactionEntity");

            migrationBuilder.DropColumn(
                name: "MessageEntityId",
                table: "MessagetransactionEntity");

            migrationBuilder.AddColumn<int>(
                name: "MessageId",
                table: "MessagetransactionEntity",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Observation",
                table: "MessagetransactionEntity",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StateCreateId",
                table: "MessagetransactionEntity",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StateUpdateId",
                table: "MessagetransactionEntity",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                table: "MessagetransactionEntity",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UserCreateId",
                table: "MessagetransactionEntity",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserUpdateId",
                table: "MessagetransactionEntity",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MessagetransactionEntity_MessageId",
                table: "MessagetransactionEntity",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "IX_MessagetransactionEntity_StateCreateId",
                table: "MessagetransactionEntity",
                column: "StateCreateId");

            migrationBuilder.CreateIndex(
                name: "IX_MessagetransactionEntity_StateUpdateId",
                table: "MessagetransactionEntity",
                column: "StateUpdateId");

            migrationBuilder.CreateIndex(
                name: "IX_MessagetransactionEntity_UserCreateId",
                table: "MessagetransactionEntity",
                column: "UserCreateId");

            migrationBuilder.CreateIndex(
                name: "IX_MessagetransactionEntity_UserUpdateId",
                table: "MessagetransactionEntity",
                column: "UserUpdateId");

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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropIndex(
                name: "IX_MessagetransactionEntity_MessageId",
                table: "MessagetransactionEntity");

            migrationBuilder.DropIndex(
                name: "IX_MessagetransactionEntity_StateCreateId",
                table: "MessagetransactionEntity");

            migrationBuilder.DropIndex(
                name: "IX_MessagetransactionEntity_StateUpdateId",
                table: "MessagetransactionEntity");

            migrationBuilder.DropIndex(
                name: "IX_MessagetransactionEntity_UserCreateId",
                table: "MessagetransactionEntity");

            migrationBuilder.DropIndex(
                name: "IX_MessagetransactionEntity_UserUpdateId",
                table: "MessagetransactionEntity");

            migrationBuilder.DropColumn(
                name: "MessageId",
                table: "MessagetransactionEntity");

            migrationBuilder.DropColumn(
                name: "Observation",
                table: "MessagetransactionEntity");

            migrationBuilder.DropColumn(
                name: "StateCreateId",
                table: "MessagetransactionEntity");

            migrationBuilder.DropColumn(
                name: "StateUpdateId",
                table: "MessagetransactionEntity");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                table: "MessagetransactionEntity");

            migrationBuilder.DropColumn(
                name: "UserCreateId",
                table: "MessagetransactionEntity");

            migrationBuilder.DropColumn(
                name: "UserUpdateId",
                table: "MessagetransactionEntity");

            migrationBuilder.AddColumn<int>(
                name: "MessageEntityId",
                table: "MessagetransactionEntity",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MessagetransactionEntity_MessageEntityId",
                table: "MessagetransactionEntity",
                column: "MessageEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_MessagetransactionEntity_Messages_MessageEntityId",
                table: "MessagetransactionEntity",
                column: "MessageEntityId",
                principalTable: "Messages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
