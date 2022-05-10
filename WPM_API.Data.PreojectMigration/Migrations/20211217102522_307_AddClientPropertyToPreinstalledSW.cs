using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _307_AddClientPropertyToPreinstalledSW : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "UserCustomer",
                newName: "PK_UserCustomer");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Server",
                newName: "PK_Server");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "PreinstalledSoftware",
                newName: "PK_PreinstalledSoftware");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PK_UserCustomer",
                table: "UserCustomer",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "PK_Server",
                table: "Server",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "PK_PreinstalledSoftware",
                table: "PreinstalledSoftware",
                newName: "Id");
        }
    }
}
