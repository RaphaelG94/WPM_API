using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _158_AddSyshouseList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SoftwaresSystemhouse",
                columns: table => new
                {
                    PK_SoftwaresSystemhouse = table.Column<string>(nullable: false),
                    SoftwareId = table.Column<string>(nullable: true),
                    SystemhouseId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoftwaresSystemhouse", x => x.PK_SoftwaresSystemhouse);
                    table.ForeignKey(
                        name: "FK_SoftwaresSystemhouse_Software_SoftwareId",
                        column: x => x.SoftwareId,
                        principalTable: "Software",
                        principalColumn: "PK_Software",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SoftwaresSystemhouse_Systemhouse_SystemhouseId",
                        column: x => x.SystemhouseId,
                        principalTable: "Systemhouse",
                        principalColumn: "PK_Systemhouse",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SoftwaresSystemhouse_SoftwareId",
                table: "SoftwaresSystemhouse",
                column: "SoftwareId");

            migrationBuilder.CreateIndex(
                name: "IX_SoftwaresSystemhouse_SystemhouseId",
                table: "SoftwaresSystemhouse",
                column: "SystemhouseId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SoftwaresSystemhouse");
        }
    }
}
