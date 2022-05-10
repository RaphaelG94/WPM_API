using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _153_AddTaskPropertiesForExecution : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExecutionPolicy",
                table: "Task",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "RestartRequired",
                table: "Task",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Visibility",
                table: "Task",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExecutionPolicy",
                table: "Task");

            migrationBuilder.DropColumn(
                name: "RestartRequired",
                table: "Task");

            migrationBuilder.DropColumn(
                name: "Visibility",
                table: "Task");
        }
    }
}
