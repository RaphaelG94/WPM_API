using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _208_AddPropsToImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BuildNr",
                table: "Image",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Language",
                table: "Image",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PublishInShop",
                table: "Image",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Update",
                table: "Image",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Vendor",
                table: "Image",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Version",
                table: "Image",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BuildNr",
                table: "Image");

            migrationBuilder.DropColumn(
                name: "Language",
                table: "Image");

            migrationBuilder.DropColumn(
                name: "PublishInShop",
                table: "Image");

            migrationBuilder.DropColumn(
                name: "Update",
                table: "Image");

            migrationBuilder.DropColumn(
                name: "Vendor",
                table: "Image");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "Image");
        }
    }
}
