using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _119_AddFurtherPropertiesToAssetModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Building",
                table: "AssetModel",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Coordinates",
                table: "AssetModel",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Floor",
                table: "AssetModel",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Room",
                table: "AssetModel",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Building",
                table: "AssetModel");

            migrationBuilder.DropColumn(
                name: "Coordinates",
                table: "AssetModel");

            migrationBuilder.DropColumn(
                name: "Floor",
                table: "AssetModel");

            migrationBuilder.DropColumn(
                name: "Room",
                table: "AssetModel");
        }
    }
}
