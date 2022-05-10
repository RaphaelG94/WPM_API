using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _160_AddClientsToSW : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PK_SoftwaresSystemhouse",
                table: "SoftwaresCustomer",
                newName: "PK_SoftwaresCustomer");

            migrationBuilder.CreateTable(
                name: "SoftwaresClient",
                columns: table => new
                {
                    PK_SoftwaresClient = table.Column<string>(nullable: false),
                    SoftwareId = table.Column<string>(nullable: true),
                    ClientId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoftwaresClient", x => x.PK_SoftwaresClient);
                    table.ForeignKey(
                        name: "FK_SoftwaresClient_Customer_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Customer",
                        principalColumn: "PK_Customer",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SoftwaresClient_Software_SoftwareId",
                        column: x => x.SoftwareId,
                        principalTable: "Software",
                        principalColumn: "PK_Software",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SoftwaresClient_ClientId",
                table: "SoftwaresClient",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_SoftwaresClient_SoftwareId",
                table: "SoftwaresClient",
                column: "SoftwareId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SoftwaresClient");

            migrationBuilder.RenameColumn(
                name: "PK_SoftwaresCustomer",
                table: "SoftwaresCustomer",
                newName: "PK_SoftwaresSystemhouse");
        }
    }
}
