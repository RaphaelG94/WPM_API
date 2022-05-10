using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _241_REmoveInventoryFromClient : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Client_Inventory_InventoryId",
                table: "Client");

            migrationBuilder.DropIndex(
                name: "IX_Client_InventoryId",
                table: "Client");

            migrationBuilder.AlterColumn<string>(
                name: "InventoryId",
                table: "Client",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "InventoryId",
                table: "Client",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

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
    }
}
