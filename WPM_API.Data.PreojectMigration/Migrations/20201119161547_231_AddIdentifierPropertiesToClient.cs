using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _231_AddIdentifierPropertiesToClient : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HyperVisor",
                table: "Client",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MACAddress",
                table: "Client",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Manufacturer",
                table: "Client",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Model",
                table: "Client",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SerialNumber",
                table: "Client",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Client",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HyperVisor",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "MACAddress",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "Manufacturer",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "Model",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "SerialNumber",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Client");
        }
    }
}
