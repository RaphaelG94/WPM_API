using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _270_AddOsSettingsCustomerStream : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DownloadLink",
                table: "Image");

            migrationBuilder.DropColumn(
                name: "DownloadLink",
                table: "CustomerImage");

            migrationBuilder.AddColumn<string>(
                name: "KeyboardLayout",
                table: "CustomerImageStream",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductKey",
                table: "CustomerImageStream",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TimeZoneLinux",
                table: "CustomerImageStream",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TimeZoneWindows",
                table: "CustomerImageStream",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KeyboardLayout",
                table: "CustomerImageStream");

            migrationBuilder.DropColumn(
                name: "ProductKey",
                table: "CustomerImageStream");

            migrationBuilder.DropColumn(
                name: "TimeZoneLinux",
                table: "CustomerImageStream");

            migrationBuilder.DropColumn(
                name: "TimeZoneWindows",
                table: "CustomerImageStream");

            migrationBuilder.AddColumn<string>(
                name: "DownloadLink",
                table: "Image",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DownloadLink",
                table: "CustomerImage",
                nullable: true);
        }
    }
}
