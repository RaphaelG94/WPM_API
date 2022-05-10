using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _133_AddInventoryToClient : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HWInventory",
                table: "Inventory",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InventoryId",
                table: "Client",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Client_InventoryId",
                table: "Client",
                column: "InventoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Client_Inventory_InventoryId",
                table: "Client",
                column: "InventoryId",
                principalTable: "Inventory",
                principalColumn: "PK_Inventory",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Client_Inventory_InventoryId",
                table: "Client");

            migrationBuilder.DropIndex(
                name: "IX_Client_InventoryId",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "HWInventory",
                table: "Inventory");

            migrationBuilder.DropColumn(
                name: "InventoryId",
                table: "Client");
        }
    }
}
