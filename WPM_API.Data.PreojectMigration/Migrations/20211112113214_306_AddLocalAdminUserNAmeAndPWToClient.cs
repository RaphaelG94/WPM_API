using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _306_AddLocalAdminUserNAmeAndPWToClient : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LocalAdminPassword",
                table: "Client",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LocalAdminUsername",
                table: "Client",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LocalAdminPassword",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "LocalAdminUsername",
                table: "Client");
        }
    }
}
