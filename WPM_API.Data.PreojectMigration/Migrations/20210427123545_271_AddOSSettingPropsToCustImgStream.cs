using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _271_AddOSSettingPropsToCustImgStream : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "KeyboardLayout",
                table: "CustomerImageStream",
                newName: "UsernameLinux");

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
                name: "LocalSettingLinux",
                table: "CustomerImageStream",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PartitionEncryptionPassLinux",
                table: "CustomerImageStream",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserPasswordLinux",
                table: "CustomerImageStream",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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
                name: "LocalSettingLinux",
                table: "CustomerImageStream");

            migrationBuilder.DropColumn(
                name: "PartitionEncryptionPassLinux",
                table: "CustomerImageStream");

            migrationBuilder.DropColumn(
                name: "UserPasswordLinux",
                table: "CustomerImageStream");

            migrationBuilder.RenameColumn(
                name: "UsernameLinux",
                table: "CustomerImageStream",
                newName: "KeyboardLayout");
        }
    }
}
