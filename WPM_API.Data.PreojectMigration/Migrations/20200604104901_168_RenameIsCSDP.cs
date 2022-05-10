using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _168_RenameIsCSDP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsCSDP",
                table: "CloudEntryPoint",
                newName: "IsStandard");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsStandard",
                table: "CloudEntryPoint",
                newName: "IsCSDP");
        }
    }
}
