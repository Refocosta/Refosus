using Microsoft.EntityFrameworkCore.Migrations;

namespace Refosus.Web.Migrations
{
    public partial class AddTableGroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GroupId",
                table: "Shoppings",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "GroupEntity",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    State = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupEntity", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Shoppings_GroupId",
                table: "Shoppings",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Shoppings_GroupEntity_GroupId",
                table: "Shoppings",
                column: "GroupId",
                principalTable: "GroupEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shoppings_GroupEntity_GroupId",
                table: "Shoppings");

            migrationBuilder.DropTable(
                name: "GroupEntity");

            migrationBuilder.DropIndex(
                name: "IX_Shoppings_GroupId",
                table: "Shoppings");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "Shoppings");
        }
    }
}
