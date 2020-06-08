using Microsoft.EntityFrameworkCore.Migrations;

namespace Refosus.Web.Migrations
{
    public partial class message6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_MessagesTypes_TypeId",
                table: "Messages");

            migrationBuilder.AlterColumn<int>(
                name: "TypeId",
                table: "Messages",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_MessagesTypes_TypeId",
                table: "Messages",
                column: "TypeId",
                principalTable: "MessagesTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_MessagesTypes_TypeId",
                table: "Messages");

            migrationBuilder.AlterColumn<int>(
                name: "TypeId",
                table: "Messages",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_MessagesTypes_TypeId",
                table: "Messages",
                column: "TypeId",
                principalTable: "MessagesTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
