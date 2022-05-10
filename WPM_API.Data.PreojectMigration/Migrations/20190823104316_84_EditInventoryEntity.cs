using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _84_EditInventoryEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Inventory",
                newName: "Type");

            migrationBuilder.AddColumn<string>(
                name: "Class",
                table: "Inventory",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsMultiValue",
                table: "Inventory",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "KeyName",
                table: "Inventory",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Method",
                table: "Inventory",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Class",
                table: "Inventory");

            migrationBuilder.DropColumn(
                name: "IsMultiValue",
                table: "Inventory");

            migrationBuilder.DropColumn(
                name: "KeyName",
                table: "Inventory");

            migrationBuilder.DropColumn(
                name: "Method",
                table: "Inventory");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Inventory",
                newName: "Name");
        }
    }
}
