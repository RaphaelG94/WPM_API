using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _203_AddSomePropertiesToCustomerSoftware : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DedicatedDownloadLink",
                table: "CustomerSoftware",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InstallationType",
                table: "CustomerSoftware",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MinimalSoftwareId",
                table: "CustomerSoftware",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NextSoftwareId",
                table: "CustomerSoftware",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PrevSoftwareId",
                table: "CustomerSoftware",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DedicatedDownloadLink",
                table: "CustomerSoftware");

            migrationBuilder.DropColumn(
                name: "InstallationType",
                table: "CustomerSoftware");

            migrationBuilder.DropColumn(
                name: "MinimalSoftwareId",
                table: "CustomerSoftware");

            migrationBuilder.DropColumn(
                name: "NextSoftwareId",
                table: "CustomerSoftware");

            migrationBuilder.DropColumn(
                name: "PrevSoftwareId",
                table: "CustomerSoftware");
        }
    }
}
