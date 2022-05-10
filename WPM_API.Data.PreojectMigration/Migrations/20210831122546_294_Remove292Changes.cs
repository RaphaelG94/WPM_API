using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _294_Remove292Changes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OSType",
                table: "Script");

            migrationBuilder.DropColumn(
                name: "OSType",
                table: "ClientOption");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OSType",
                table: "Script",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OSType",
                table: "ClientOption",
                nullable: true);
        }
    }
}
