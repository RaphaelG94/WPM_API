using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _41_AddWDSUnattend : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "WdsIp",
                table: "Client",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "unattend",
                table: "Client",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WdsIp",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "unattend",
                table: "Client");
        }
    }
}
