using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _56_RefactorLocationEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PhoneNr",
                table: "Location",
                newName: "UploadSpeed");

            migrationBuilder.AddColumn<string>(
                name: "CityAbbreviation",
                table: "Location",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CountryAbbreviation",
                table: "Location",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DownloadSpeed",
                table: "Location",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameAbbreviation",
                table: "Location",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TimeZone",
                table: "Location",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Location",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CityAbbreviation",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "CountryAbbreviation",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "DownloadSpeed",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "NameAbbreviation",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "TimeZone",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Location");

            migrationBuilder.RenameColumn(
                name: "UploadSpeed",
                table: "Location",
                newName: "PhoneNr");
        }
    }
}
