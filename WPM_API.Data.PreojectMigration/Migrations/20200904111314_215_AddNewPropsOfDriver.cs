using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _215_AddNewPropsOfDriver : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Architecture",
                table: "Driver",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BuildNr",
                table: "Driver",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Driver",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DownloadLinkget",
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
                name: "Name",
                table: "Driver",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubFolderName",
                table: "Driver",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Update",
                table: "Driver",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Vendor",
                table: "Driver",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Version",
                table: "Driver",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Architecture",
                table: "Driver");

            migrationBuilder.DropColumn(
                name: "BuildNr",
                table: "Driver");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Driver");

            migrationBuilder.DropColumn(
                name: "DownloadLinkget",
                table: "Driver");

            migrationBuilder.DropColumn(
                name: "FileName",
                table: "Driver");

            migrationBuilder.DropColumn(
                name: "Language",
                table: "Driver");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Driver");

            migrationBuilder.DropColumn(
                name: "SubFolderName",
                table: "Driver");

            migrationBuilder.DropColumn(
                name: "Update",
                table: "Driver");

            migrationBuilder.DropColumn(
                name: "Vendor",
                table: "Driver");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "Driver");
        }
    }
}
