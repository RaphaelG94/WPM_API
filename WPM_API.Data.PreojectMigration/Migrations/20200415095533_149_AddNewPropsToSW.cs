using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _149_AddNewPropsToSW : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Architecture",
                table: "Software",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Checksum",
                table: "Software",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompliancyRule",
                table: "Software",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "GnuLicence",
                table: "Software",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "InstallationTime",
                table: "Software",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Language",
                table: "Software",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PackageSize",
                table: "Software",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Prerequisites",
                table: "Software",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VendorReleaseDate",
                table: "Software",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Architecture",
                table: "Software");

            migrationBuilder.DropColumn(
                name: "Checksum",
                table: "Software");

            migrationBuilder.DropColumn(
                name: "CompliancyRule",
                table: "Software");

            migrationBuilder.DropColumn(
                name: "GnuLicence",
                table: "Software");

            migrationBuilder.DropColumn(
                name: "InstallationTime",
                table: "Software");

            migrationBuilder.DropColumn(
                name: "Language",
                table: "Software");

            migrationBuilder.DropColumn(
                name: "PackageSize",
                table: "Software");

            migrationBuilder.DropColumn(
                name: "Prerequisites",
                table: "Software");

            migrationBuilder.DropColumn(
                name: "VendorReleaseDate",
                table: "Software");
        }
    }
}
