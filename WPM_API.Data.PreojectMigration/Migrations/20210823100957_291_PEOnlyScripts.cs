using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _291_PEOnlyScripts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InPESession",
                table: "Client");

            migrationBuilder.AddColumn<bool>(
                name: "PEOnly",
                table: "Script",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "PEOnly",
                table: "ClientOption",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PEOnly",
                table: "Script");

            migrationBuilder.DropColumn(
                name: "PEOnly",
                table: "ClientOption");

            migrationBuilder.AddColumn<bool>(
                name: "InPESession",
                table: "Client",
                nullable: false,
                defaultValue: false);
        }
    }
}
