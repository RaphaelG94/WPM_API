using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _238_AddRevisionNumberToCustomerSoftwareAndSoftware : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RevisionNumber",
                table: "Software",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RevisionNumber",
                table: "CustomerSoftware",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RevisionNumber",
                table: "Software");

            migrationBuilder.DropColumn(
                name: "RevisionNumber",
                table: "CustomerSoftware");
        }
    }
}
