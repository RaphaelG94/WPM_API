using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _298_AddBaselineFileContent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BaseLineFile1Content",
                table: "Client",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BaseLineFile2Content",
                table: "Client",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BaseLineFile3Content",
                table: "Client",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BaseLineFile1Content",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "BaseLineFile2Content",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "BaseLineFile3Content",
                table: "Client");
        }
    }
}
