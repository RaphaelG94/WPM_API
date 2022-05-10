using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _240_EditInventoryTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HWInventory",
                table: "Inventory");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Inventory");

            migrationBuilder.RenameColumn(
                name: "SWInventory",
                table: "Inventory",
                newName: "Value");

            migrationBuilder.RenameColumn(
                name: "OSInventory",
                table: "Inventory",
                newName: "OperationType");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Value",
                table: "Inventory",
                newName: "SWInventory");

            migrationBuilder.RenameColumn(
                name: "OperationType",
                table: "Inventory",
                newName: "OSInventory");

            migrationBuilder.AddColumn<string>(
                name: "HWInventory",
                table: "Inventory",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Inventory",
                nullable: true);
        }
    }
}
