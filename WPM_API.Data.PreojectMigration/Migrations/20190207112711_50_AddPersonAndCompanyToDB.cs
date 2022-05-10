using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _50_AddPersonAndCompanyToDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OptionalEmail",
                table: "Person",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Person",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OptionalEmail",
                table: "Person");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Person");
        }
    }
}
