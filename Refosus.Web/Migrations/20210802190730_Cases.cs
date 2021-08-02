using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Refosus.Web.Migrations
{
    public partial class Cases : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cases",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Issue = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Sender = table.Column<string>(nullable: true),
                    TypesCasesId = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    Priority = table.Column<int>(nullable: false),
                    Code = table.Column<string>(nullable: true),
                    Solution = table.Column<string>(nullable: true),
                    Responsable = table.Column<string>(nullable: true),
                    Fulfillment = table.Column<int>(nullable: false),
                    Hours = table.Column<double>(nullable: false),
                    BusinessUnitsId = table.Column<int>(nullable: false),
                    Ubication = table.Column<string>(nullable: true),
                    DeadLine = table.Column<DateTime>(nullable: false),
                    ClosingDate = table.Column<DateTime>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cases_TypesCases_TypesCasesId",
                        column: x => x.TypesCasesId,
                        principalTable: "TypesCases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict
                    );
                    table.ForeignKey(
                        name: "FK_Cases_BusinessUnits_BusinessUnitsId",
                        column: x => x.BusinessUnitsId,
                        principalTable: "BusinessUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict
                    );
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cases");
        }
    }
}
