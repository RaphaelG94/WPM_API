using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _159_AddCustomersToSW : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SoftwaresCustomer",
                columns: table => new
                {
                    PK_SoftwaresSystemhouse = table.Column<string>(nullable: false),
                    SoftwareId = table.Column<string>(nullable: true),
                    CustomerId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoftwaresCustomer", x => x.PK_SoftwaresSystemhouse);
                    table.ForeignKey(
                        name: "FK_SoftwaresCustomer_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "PK_Customer",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SoftwaresCustomer_Software_SoftwareId",
                        column: x => x.SoftwareId,
                        principalTable: "Software",
                        principalColumn: "PK_Software",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SoftwaresCustomer_CustomerId",
                table: "SoftwaresCustomer",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_SoftwaresCustomer_SoftwareId",
                table: "SoftwaresCustomer",
                column: "SoftwareId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SoftwaresCustomer");
        }
    }
}
