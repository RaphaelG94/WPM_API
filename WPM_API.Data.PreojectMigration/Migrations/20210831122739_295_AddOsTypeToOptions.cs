using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _295_AddOsTypeToOptions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OSType",
                table: "Script",
                defaultValue: "Windows",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OSType",
                table: "ClientOption",
                defaultValue: "Windows",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OSType",
                table: "Script");

            migrationBuilder.DropColumn(
                name: "OSType",
                table: "ClientOption");
        }
    }
}
