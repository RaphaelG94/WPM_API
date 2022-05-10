using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _47_ClientdatasheetRenaming : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientClientProperties_ClientProperties_ClientPropertyId",
                table: "ClientClientProperties");

            migrationBuilder.DropTable(
                name: "ClientProperties");

            migrationBuilder.RenameColumn(
                name: "PK_Categories",
                table: "ClientDatasheetCategories",
                newName: "PK_ClientDatasheetCategoriesId");

            migrationBuilder.CreateTable(
                name: "ClientProperty",
                columns: table => new
                {
                    PK_ClientProperties = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    CategoryId = table.Column<string>(nullable: true),
                    Command = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientProperty", x => x.PK_ClientProperties);
                    table.ForeignKey(
                        name: "FK_ClientProperty_ClientDatasheetCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "ClientDatasheetCategories",
                        principalColumn: "PK_ClientDatasheetCategoriesId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClientProperty_CategoryId",
                table: "ClientProperty",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientClientProperties_ClientProperty_ClientPropertyId",
                table: "ClientClientProperties",
                column: "ClientPropertyId",
                principalTable: "ClientProperty",
                principalColumn: "PK_ClientProperties",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientClientProperties_ClientProperty_ClientPropertyId",
                table: "ClientClientProperties");

            migrationBuilder.DropTable(
                name: "ClientProperty");

            migrationBuilder.RenameColumn(
                name: "PK_ClientDatasheetCategoriesId",
                table: "ClientDatasheetCategories",
                newName: "PK_Categories");

            migrationBuilder.CreateTable(
                name: "ClientProperties",
                columns: table => new
                {
                    PK_ClientProperties = table.Column<string>(nullable: false),
                    CategoryId = table.Column<string>(nullable: true),
                    Command = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
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

            migrationBuilder.CreateIndex(
                name: "IX_ClientProperties_CategoryId",
                table: "ClientProperties",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientClientProperties_ClientProperties_ClientPropertyId",
                table: "ClientClientProperties",
                column: "ClientPropertyId",
                principalTable: "ClientProperties",
                principalColumn: "PK_ClientProperties",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
