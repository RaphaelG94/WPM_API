using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _75_AddPropertyNameToClientParameter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "ClientParameter",
                newName: "PropertyName");

            migrationBuilder.AddColumn<string>(
                name: "ParameterName",
                table: "ClientParameter",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParameterName",
                table: "ClientParameter");

            migrationBuilder.RenameColumn(
                name: "PropertyName",
                table: "ClientParameter",
                newName: "Name");
        }
    }
}
