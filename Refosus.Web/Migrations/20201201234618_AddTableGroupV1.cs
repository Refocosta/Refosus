using Microsoft.EntityFrameworkCore.Migrations;

namespace Refosus.Web.Migrations
{
    public partial class AddTableGroupV1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shoppings_GroupEntity_GroupId",
                table: "Shoppings");

            migrationBuilder.DropTable(
                name: "GroupEntity");

            migrationBuilder.CreateTable(
                name: "TM_GroupEntity",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Stade = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TM_GroupEntity", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Shoppings_TM_GroupEntity_GroupId",
                table: "Shoppings",
                column: "GroupId",
                principalTable: "TM_GroupEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shoppings_TM_GroupEntity_GroupId",
                table: "Shoppings");

            migrationBuilder.DropTable(
                name: "TM_GroupEntity");

            migrationBuilder.CreateTable(
                name: "GroupEntity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupEntity", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Shoppings_GroupEntity_GroupId",
                table: "Shoppings",
                column: "GroupId",
                principalTable: "GroupEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
