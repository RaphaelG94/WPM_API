using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _218_AddRiversToShopItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ShopItemId",
                table: "Driver",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Driver_ShopItemId",
                table: "Driver",
                column: "ShopItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Driver_ShopItem_ShopItemId",
                table: "Driver",
                column: "ShopItemId",
                principalTable: "ShopItem",
                principalColumn: "PK_ShopItem",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Driver_ShopItem_ShopItemId",
                table: "Driver");

            migrationBuilder.DropIndex(
                name: "IX_Driver_ShopItemId",
                table: "Driver");

            migrationBuilder.DropColumn(
                name: "ShopItemId",
                table: "Driver");
        }
    }
}
