using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _177_AddRunningContextToSWAndTask : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RunningContext",
                table: "Task",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RunningContext",
                table: "Software",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RunningContext",
                table: "Task");

            migrationBuilder.DropColumn(
                name: "RunningContext",
                table: "Software");
        }
    }
}
