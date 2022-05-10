using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _54_SetCompanyHQDataToLocationReference : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Country",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "PhoneNr",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "Postcode",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "Street",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "StreetNr",
                table: "Company");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Location",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "Location");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Company",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNr",
                table: "Company",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Postcode",
                table: "Company",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Street",
                table: "Company",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StreetNr",
                table: "Company",
                nullable: true);
        }
    }
}
