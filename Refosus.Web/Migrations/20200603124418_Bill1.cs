using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Refosus.Web.Migrations
{
    public partial class Bill1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateAut",
                table: "Messages",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateProcess",
                table: "Messages",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "StateBillId",
                table: "Messages",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserAutId",
                table: "Messages",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserProsId",
                table: "Messages",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Messages_StateBillId",
                table: "Messages",
                column: "StateBillId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_UserAutId",
                table: "Messages",
                column: "UserAutId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_UserProsId",
                table: "Messages",
                column: "UserProsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_MessagesBillState_StateBillId",
                table: "Messages",
                column: "StateBillId",
                principalTable: "MessagesBillState",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_AspNetUsers_UserAutId",
                table: "Messages",
                column: "UserAutId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_AspNetUsers_UserProsId",
                table: "Messages",
                column: "UserProsId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_MessagesBillState_StateBillId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_AspNetUsers_UserAutId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_AspNetUsers_UserProsId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_StateBillId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_UserAutId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_UserProsId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "DateAut",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "DateProcess",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "StateBillId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "UserAutId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "UserProsId",
                table: "Messages");
        }
    }
}
