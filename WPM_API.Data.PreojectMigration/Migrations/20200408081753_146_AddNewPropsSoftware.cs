using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _146_AddNewPropsSoftware : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DownloadLink",
                table: "Software",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Software",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Website",
                table: "Software",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DownloadLink",
                table: "Software");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Software");

            migrationBuilder.DropColumn(
                name: "Website",
                table: "Software");
        }
    }
}
