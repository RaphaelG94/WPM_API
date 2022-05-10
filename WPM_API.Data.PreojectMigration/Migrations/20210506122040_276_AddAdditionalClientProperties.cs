using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _276_AddAdditionalClientProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CSPname",
                table: "Client",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CSPvendor",
                table: "Client",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CSPversion",
                table: "Client",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MainFrequentUser",
                table: "Client",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModelSeries",
                table: "Client",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OSArchitecture",
                table: "Client",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OSEdition",
                table: "Client",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "OSInstallDateUTC",
                table: "Client",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "OSLanguage",
                table: "Client",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OSMemorySize",
                table: "Client",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OSOperatingSystemSKU",
                table: "Client",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OSProductSuite",
                table: "Client",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OSType",
                table: "Client",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OSVersion",
                table: "Client",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Processor",
                table: "Client",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CSPname",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "CSPvendor",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "CSPversion",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "MainFrequentUser",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "ModelSeries",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "OSArchitecture",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "OSEdition",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "OSInstallDateUTC",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "OSLanguage",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "OSMemorySize",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "OSOperatingSystemSKU",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "OSProductSuite",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "OSType",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "OSVersion",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "Processor",
                table: "Client");
        }
    }
}
