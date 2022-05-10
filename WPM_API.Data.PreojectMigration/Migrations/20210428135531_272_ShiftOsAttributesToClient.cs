using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _272_ShiftOsAttributesToClient : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdminPasswordLinux",
                table: "CustomerImageStream");

            migrationBuilder.DropColumn(
                name: "KeyboardLayoutLinux",
                table: "CustomerImageStream");

            migrationBuilder.DropColumn(
                name: "KeyboardLayoutWindows",
                table: "CustomerImageStream");

            migrationBuilder.DropColumn(
                name: "PartitionEncryptionPassLinux",
                table: "CustomerImageStream");

            migrationBuilder.DropColumn(
                name: "TimeZoneLinux",
                table: "CustomerImageStream");

            migrationBuilder.DropColumn(
                name: "TimeZoneWindows",
                table: "CustomerImageStream");

            migrationBuilder.DropColumn(
                name: "UserPasswordLinux",
                table: "CustomerImageStream");

            migrationBuilder.DropColumn(
                name: "UsernameLinux",
                table: "CustomerImageStream");

            migrationBuilder.AddColumn<string>(
                name: "AdminPasswordLinux",
                table: "Client",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KeyboardLayoutLinux",
                table: "Client",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KeyboardLayoutWindows",
                table: "Client",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PartitionEncryptionPassLinux",
                table: "Client",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TimeZoneLinux",
                table: "Client",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TimeZoneWindows",
                table: "Client",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserPasswordLinux",
                table: "Client",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UsernameLinux",
                table: "Client",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdminPasswordLinux",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "KeyboardLayoutLinux",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "KeyboardLayoutWindows",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "PartitionEncryptionPassLinux",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "TimeZoneLinux",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "TimeZoneWindows",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "UserPasswordLinux",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "UsernameLinux",
                table: "Client");

            migrationBuilder.AddColumn<string>(
                name: "AdminPasswordLinux",
                table: "CustomerImageStream",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KeyboardLayoutLinux",
                table: "CustomerImageStream",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KeyboardLayoutWindows",
                table: "CustomerImageStream",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PartitionEncryptionPassLinux",
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

            migrationBuilder.AddColumn<string>(
                name: "UserPasswordLinux",
                table: "CustomerImageStream",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UsernameLinux",
                table: "CustomerImageStream",
                nullable: true);
        }
    }
}
