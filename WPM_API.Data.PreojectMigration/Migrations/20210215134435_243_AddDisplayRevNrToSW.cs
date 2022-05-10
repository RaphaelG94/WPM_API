using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _243_AddDisplayRevNrToSW : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DisplayRevisionNumber",
                table: "Software",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DisplayRevisionNumber",
                table: "CustomerSoftware",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisplayRevisionNumber",
                table: "Software");

            migrationBuilder.DropColumn(
                name: "DisplayRevisionNumber",
                table: "CustomerSoftware");
        }
    }
}
