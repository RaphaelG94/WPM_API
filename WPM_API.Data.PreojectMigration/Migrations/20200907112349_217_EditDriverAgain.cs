using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _217_EditDriverAgain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Architecture",
                table: "Driver");

            migrationBuilder.DropColumn(
                name: "BuildNr",
                table: "Driver");

            migrationBuilder.DropColumn(
                name: "FileName",
                table: "Driver");

            migrationBuilder.DropColumn(
                name: "Language",
                table: "Driver");

            migrationBuilder.DropColumn(
                name: "SubFolderName",
                table: "Driver");

            migrationBuilder.RenameColumn(
                name: "Update",
                table: "Driver",
                newName: "SubFolderPath");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SubFolderPath",
                table: "Driver",
                newName: "Update");

            migrationBuilder.AddColumn<string>(
                name: "Architecture",
                table: "Driver",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BuildNr",
                table: "Driver",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "Driver",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Language",
                table: "Driver",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubFolderName",
                table: "Driver",
                nullable: true);
        }
    }
}
