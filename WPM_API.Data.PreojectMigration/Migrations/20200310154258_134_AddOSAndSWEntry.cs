using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _134_AddOSAndSWEntry : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OSInventory",
                table: "Inventory",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SWInventory",
                table: "Inventory",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OSInventory",
                table: "Inventory");

            migrationBuilder.DropColumn(
                name: "SWInventory",
                table: "Inventory");
        }
    }
}
