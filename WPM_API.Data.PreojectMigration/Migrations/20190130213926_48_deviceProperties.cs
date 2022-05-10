using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _48_deviceProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientProperty_ClientDatasheetCategories_CategoryId",
                table: "ClientProperty");

            migrationBuilder.DropTable(
                name: "ClientClientProperties");

            migrationBuilder.DropTable(
                name: "ClientDatasheetCategories");

            migrationBuilder.RenameColumn(
                name: "PK_ClientProperties",
                table: "ClientProperty",
                newName: "PK_ClientProperty");

            migrationBuilder.CreateTable(
                name: "ClientClientProperty",
                columns: table => new
                {
                    PK_ClientClientProperty = table.Column<string>(nullable: false),
                    ClientId = table.Column<string>(nullable: true),
                    ClientPropertyId = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientClientProperty", x => x.PK_ClientClientProperty);
                    table.ForeignKey(
                        name: "FK_ClientClientProperty_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "PK_Client",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClientClientProperty_ClientProperty_ClientPropertyId",
                        column: x => x.ClientPropertyId,
                        principalTable: "ClientProperty",
                        principalColumn: "PK_ClientProperty",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClientClientProperty_ClientId",
                table: "ClientClientProperty",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientClientProperty_ClientPropertyId",
                table: "ClientClientProperty",
                column: "ClientPropertyId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientProperty_Category_CategoryId",
                table: "ClientProperty",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "PK_Category",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientProperty_Category_CategoryId",
                table: "ClientProperty");

            migrationBuilder.DropTable(
                name: "ClientClientProperty");

            migrationBuilder.RenameColumn(
                name: "PK_ClientProperty",
                table: "ClientProperty",
                newName: "PK_ClientProperties");

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
                        name: "FK_ClientClientProperties_ClientProperty_ClientPropertyId",
                        column: x => x.ClientPropertyId,
                        principalTable: "ClientProperty",
                        principalColumn: "PK_ClientProperties",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ClientDatasheetCategories",
                columns: table => new
                {
                    PK_ClientDatasheetCategoriesId = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientDatasheetCategories", x => x.PK_ClientDatasheetCategoriesId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClientClientProperties_ClientId",
                table: "ClientClientProperties",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientClientProperties_ClientPropertyId",
                table: "ClientClientProperties",
                column: "ClientPropertyId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientProperty_ClientDatasheetCategories_CategoryId",
                table: "ClientProperty",
                column: "CategoryId",
                principalTable: "ClientDatasheetCategories",
                principalColumn: "PK_ClientDatasheetCategoriesId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
