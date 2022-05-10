using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _120_AddAssetClassAndAssetType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AssetClass",
                columns: table => new
                {
                    PK_AssetClass = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetClass", x => x.PK_AssetClass);
                });

            migrationBuilder.CreateTable(
                name: "AssetType",
                columns: table => new
                {
                    PK_AssetType = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetType", x => x.PK_AssetType);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssetClass");

            migrationBuilder.DropTable(
                name: "AssetType");
        }
    }
}
