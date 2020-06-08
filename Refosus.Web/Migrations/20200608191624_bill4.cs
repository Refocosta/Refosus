using Microsoft.EntityFrameworkCore.Migrations;

namespace Refosus.Web.Migrations
{
    public partial class bill4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StateBillCreateId",
                table: "MessagesTransaction",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StateBillUpdateId",
                table: "MessagesTransaction",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserBillCreateId",
                table: "MessagesTransaction",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserBillUpdateId",
                table: "MessagesTransaction",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MessageFileEntity",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    messageId = table.Column<int>(nullable: true),
                    FilePath = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageFileEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MessageFileEntity_Messages_messageId",
                        column: x => x.messageId,
                        principalTable: "Messages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MessagesTransaction_StateBillCreateId",
                table: "MessagesTransaction",
                column: "StateBillCreateId");

            migrationBuilder.CreateIndex(
                name: "IX_MessagesTransaction_StateBillUpdateId",
                table: "MessagesTransaction",
                column: "StateBillUpdateId");

            migrationBuilder.CreateIndex(
                name: "IX_MessagesTransaction_UserBillCreateId",
                table: "MessagesTransaction",
                column: "UserBillCreateId");

            migrationBuilder.CreateIndex(
                name: "IX_MessagesTransaction_UserBillUpdateId",
                table: "MessagesTransaction",
                column: "UserBillUpdateId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageFileEntity_messageId",
                table: "MessageFileEntity",
                column: "messageId");

            migrationBuilder.AddForeignKey(
                name: "FK_MessagesTransaction_MessagesBillState_StateBillCreateId",
                table: "MessagesTransaction",
                column: "StateBillCreateId",
                principalTable: "MessagesBillState",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MessagesTransaction_MessagesBillState_StateBillUpdateId",
                table: "MessagesTransaction",
                column: "StateBillUpdateId",
                principalTable: "MessagesBillState",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MessagesTransaction_MessagesBillState_StateBillCreateId",
                table: "MessagesTransaction");

            migrationBuilder.DropForeignKey(
                name: "FK_MessagesTransaction_MessagesBillState_StateBillUpdateId",
                table: "MessagesTransaction");

            migrationBuilder.DropForeignKey(
                name: "FK_MessagesTransaction_AspNetUsers_UserBillCreateId",
                table: "MessagesTransaction");

            migrationBuilder.DropForeignKey(
                name: "FK_MessagesTransaction_AspNetUsers_UserBillUpdateId",
                table: "MessagesTransaction");

            migrationBuilder.DropTable(
                name: "MessageFileEntity");

            migrationBuilder.DropIndex(
                name: "IX_MessagesTransaction_StateBillCreateId",
                table: "MessagesTransaction");

            migrationBuilder.DropIndex(
                name: "IX_MessagesTransaction_StateBillUpdateId",
                table: "MessagesTransaction");

            migrationBuilder.DropIndex(
                name: "IX_MessagesTransaction_UserBillCreateId",
                table: "MessagesTransaction");

            migrationBuilder.DropIndex(
                name: "IX_MessagesTransaction_UserBillUpdateId",
                table: "MessagesTransaction");

            migrationBuilder.DropColumn(
                name: "StateBillCreateId",
                table: "MessagesTransaction");

            migrationBuilder.DropColumn(
                name: "StateBillUpdateId",
                table: "MessagesTransaction");

            migrationBuilder.DropColumn(
                name: "UserBillCreateId",
                table: "MessagesTransaction");

            migrationBuilder.DropColumn(
                name: "UserBillUpdateId",
                table: "MessagesTransaction");
        }
    }
}
