using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _71_EditSMBShareAndClient : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Permission",
                table: "SmbStorage");

            migrationBuilder.DropColumn(
                name: "LSDPInstalled",
                table: "Client");

            migrationBuilder.RenameColumn(
                name: "ServerName",
                table: "SmbStorage",
                newName: "Path");

            migrationBuilder.RenameColumn(
                name: "ServerIp",
                table: "SmbStorage",
                newName: "DataDriveLetter");

            migrationBuilder.AddColumn<bool>(
                name: "ExistedAlready",
                table: "SmbStorage",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExistedAlready",
                table: "SmbStorage");

            migrationBuilder.RenameColumn(
                name: "Path",
                table: "SmbStorage",
                newName: "ServerName");

            migrationBuilder.RenameColumn(
                name: "DataDriveLetter",
                table: "SmbStorage",
                newName: "ServerIp");

            migrationBuilder.AddColumn<string>(
                name: "Permission",
                table: "SmbStorage",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "LSDPInstalled",
                table: "Client",
                nullable: false,
                defaultValue: false);
        }
    }
}
