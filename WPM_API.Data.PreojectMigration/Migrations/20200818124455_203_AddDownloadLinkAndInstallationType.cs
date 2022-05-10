using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _203_AddDownloadLinkAndInstallationType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DedicatedDownloadLink",
                table: "Software",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InstallationType",
                table: "Software",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DedicatedDownloadLink",
                table: "Software");

            migrationBuilder.DropColumn(
                name: "InstallationType",
                table: "Software");
        }
    }
}
