using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _46_NewClientDatasheet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClientDatasheetCategories",
                columns: table => new
                {
                    PK_Categories = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientDatasheetCategories", x => x.PK_Categories);
                });

            migrationBuilder.CreateTable(
                name: "ClientProperties",
                columns: table => new
                {
                    PK_ClientProperties = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    CategoryId = table.Column<string>(nullable: true),
                    Command = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientProperties", x => x.PK_ClientProperties);
                    table.ForeignKey(
                        name: "FK_ClientProperties_ClientDatasheetCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "ClientDatasheetCategories",
                        principalColumn: "PK_Categories",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ClientClientProperties",
                columns: table => new
                {
                    PK_ClientClientPropertiesId = table.Column<string>(nullable: false),
                    ClientId = table.Column<string>(nullable: true),
                    ClientPropertyId = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientClientProperties", x => x.PK_ClientClientPropertiesId);
                    table.ForeignKey(
                        name: "FK_ClientClientProperties_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "PK_Client",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClientClientProperties_ClientProperties_ClientPropertyId",
                        column: x => x.ClientPropertyId,
                        principalTable: "ClientProperties",
                        principalColumn: "PK_ClientProperties",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClientClientProperties_ClientId",
                table: "ClientClientProperties",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientClientProperties_ClientPropertyId",
                table: "ClientClientProperties",
                column: "ClientPropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientProperties_CategoryId",
                table: "ClientProperties",
                column: "CategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientClientProperties");

            migrationBuilder.DropTable(
                name: "ClientProperties");

            migrationBuilder.DropTable(
                name: "ClientDatasheetCategories");
        }
    }
}
