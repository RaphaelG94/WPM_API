using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _205_ShiftInstallationTypeToTask : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InstallationType",
                table: "Software");

            migrationBuilder.DropColumn(
                name: "InstallationType",
                table: "CustomerSoftware");

            migrationBuilder.AddColumn<string>(
                name: "InstallationType",
                table: "Task",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InstallationType",
                table: "Task");

            migrationBuilder.AddColumn<string>(
                name: "InstallationType",
                table: "Software",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InstallationType",
                table: "CustomerSoftware",
                nullable: true);
        }
    }
}
