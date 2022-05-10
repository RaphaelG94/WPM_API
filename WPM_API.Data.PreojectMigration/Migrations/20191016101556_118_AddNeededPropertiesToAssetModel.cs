using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _118_AddNeededPropertiesToAssetModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AssetClass",
                table: "AssetModel",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AssetStatus",
                table: "AssetModel",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "AssetModel",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssetClass",
                table: "AssetModel");

            migrationBuilder.DropColumn(
                name: "AssetStatus",
                table: "AssetModel");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "AssetModel");
        }
    }
}
