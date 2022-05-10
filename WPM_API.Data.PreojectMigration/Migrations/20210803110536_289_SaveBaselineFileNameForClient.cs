using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _289_SaveBaselineFileNameForClient : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BaselineResult",
                table: "Client",
                newName: "BaseLineFile3");

            migrationBuilder.AddColumn<string>(
                name: "BaseLineFile1",
                table: "Client",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BaseLineFile2",
                table: "Client",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BaseLineFile1",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "BaseLineFile2",
                table: "Client");

            migrationBuilder.RenameColumn(
                name: "BaseLineFile3",
                table: "Client",
                newName: "BaselineResult");
        }
    }
}
