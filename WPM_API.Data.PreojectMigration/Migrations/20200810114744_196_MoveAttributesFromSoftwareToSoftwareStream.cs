using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _196_MoveAttributesFromSoftwareToSoftwareStream : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Architecture",
                table: "Software");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Software");

            migrationBuilder.DropColumn(
                name: "DescriptionShort",
                table: "Software");

            migrationBuilder.DropColumn(
                name: "DownloadLink",
                table: "Software");

            migrationBuilder.DropColumn(
                name: "GnuLicence",
                table: "Software");

            migrationBuilder.DropColumn(
                name: "Language",
                table: "Software");

            migrationBuilder.DropColumn(
                name: "Vendor",
                table: "Software");

            migrationBuilder.DropColumn(
                name: "Website",
                table: "Software");

            migrationBuilder.AddColumn<string>(
                name: "Architecture",
                table: "SoftwareStream",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "SoftwareStream",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionShort",
                table: "SoftwareStream",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DownloadLink",
                table: "SoftwareStream",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "GnuLicence",
                table: "SoftwareStream",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Language",
                table: "SoftwareStream",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Vendor",
                table: "SoftwareStream",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Website",
                table: "SoftwareStream",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Architecture",
                table: "CustomerSoftwareStream",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "CustomerSoftwareStream",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionShort",
                table: "CustomerSoftwareStream",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DownloadLink",
                table: "CustomerSoftwareStream",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "GnuLicence",
                table: "CustomerSoftwareStream",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Language",
                table: "CustomerSoftwareStream",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Vendor",
                table: "CustomerSoftwareStream",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Website",
                table: "CustomerSoftwareStream",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Architecture",
                table: "SoftwareStream");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "SoftwareStream");

            migrationBuilder.DropColumn(
                name: "DescriptionShort",
                table: "SoftwareStream");

            migrationBuilder.DropColumn(
                name: "DownloadLink",
                table: "SoftwareStream");

            migrationBuilder.DropColumn(
                name: "GnuLicence",
                table: "SoftwareStream");

            migrationBuilder.DropColumn(
                name: "Language",
                table: "SoftwareStream");

            migrationBuilder.DropColumn(
                name: "Vendor",
                table: "SoftwareStream");

            migrationBuilder.DropColumn(
                name: "Website",
                table: "SoftwareStream");

            migrationBuilder.DropColumn(
                name: "Architecture",
                table: "CustomerSoftwareStream");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "CustomerSoftwareStream");

            migrationBuilder.DropColumn(
                name: "DescriptionShort",
                table: "CustomerSoftwareStream");

            migrationBuilder.DropColumn(
                name: "DownloadLink",
                table: "CustomerSoftwareStream");

            migrationBuilder.DropColumn(
                name: "GnuLicence",
                table: "CustomerSoftwareStream");

            migrationBuilder.DropColumn(
                name: "Language",
                table: "CustomerSoftwareStream");

            migrationBuilder.DropColumn(
                name: "Vendor",
                table: "CustomerSoftwareStream");

            migrationBuilder.DropColumn(
                name: "Website",
                table: "CustomerSoftwareStream");

            migrationBuilder.AddColumn<string>(
                name: "Architecture",
                table: "Software",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Software",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionShort",
                table: "Software",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DownloadLink",
                table: "Software",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "GnuLicence",
                table: "Software",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Language",
                table: "Software",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Vendor",
                table: "Software",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Website",
                table: "Software",
                nullable: true);
        }
    }
}
