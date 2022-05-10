using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _53_FixErrorsInPerson : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PrimaryEmail",
                table: "Person",
                newName: "EmailPrimary");

            migrationBuilder.RenameColumn(
                name: "OptionalEmail",
                table: "Person",
                newName: "EmailOptional");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EmailPrimary",
                table: "Person",
                newName: "PrimaryEmail");

            migrationBuilder.RenameColumn(
                name: "EmailOptional",
                table: "Person",
                newName: "OptionalEmail");
        }
    }
}
