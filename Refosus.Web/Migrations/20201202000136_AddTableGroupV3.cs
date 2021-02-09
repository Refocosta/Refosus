using Microsoft.EntityFrameworkCore.Migrations;

namespace Refosus.Web.Migrations
{
    public partial class AddTableGroupV3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shoppings_TM_Groups_GroupId",
                table: "Shoppings");

            migrationBuilder.DropTable(
                name: "TM_Groups");

            migrationBuilder.CreateTable(
                name: "TP_Groups",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Stade = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TP_Groups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TM_User_Groups",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: true),
                    GroupId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TM_User_Groups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TM_User_Groups_TP_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "TP_Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TM_User_Groups_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TM_User_Groups_GroupId",
                table: "TM_User_Groups",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_TM_User_Groups_UserId",
                table: "TM_User_Groups",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Shoppings_TP_Groups_GroupId",
                table: "Shoppings",
                column: "GroupId",
                principalTable: "TP_Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shoppings_TP_Groups_GroupId",
                table: "Shoppings");

            migrationBuilder.DropTable(
                name: "TM_User_Groups");

            migrationBuilder.DropTable(
                name: "TP_Groups");

            migrationBuilder.CreateTable(
                name: "TM_Groups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Stade = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TM_Groups", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Shoppings_TM_Groups_GroupId",
                table: "Shoppings",
                column: "GroupId",
                principalTable: "TM_Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
