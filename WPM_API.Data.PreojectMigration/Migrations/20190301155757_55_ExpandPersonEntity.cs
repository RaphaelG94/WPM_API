using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _55_ExpandPersonEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DepartementName",
                table: "Person",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DepartementShort",
                table: "Person",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Domain",
                table: "Person",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmployeeNr",
                table: "Person",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoomNr",
                table: "Person",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DepartementName",
                table: "Person");

            migrationBuilder.DropColumn(
                name: "DepartementShort",
                table: "Person");

            migrationBuilder.DropColumn(
                name: "Domain",
                table: "Person");

            migrationBuilder.DropColumn(
                name: "EmployeeNr",
                table: "Person");

            migrationBuilder.DropColumn(
                name: "RoomNr",
                table: "Person");
        }
    }
}
