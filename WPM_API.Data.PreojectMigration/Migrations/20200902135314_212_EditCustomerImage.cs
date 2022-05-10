using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _212_EditCustomerImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "CustomerImage",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubFolderName",
                table: "CustomerImage",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileName",
                table: "CustomerImage");

            migrationBuilder.DropColumn(
                name: "SubFolderName",
                table: "CustomerImage");
        }
    }
}
