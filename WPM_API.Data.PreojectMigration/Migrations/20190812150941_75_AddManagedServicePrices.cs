using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _75_AddManagedServicePrices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "ManagedServiceLifecylcePrice",
                table: "ShopItem",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ManagedServicePrice",
                table: "ShopItem",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ManagedServiceLifecylcePrice",
                table: "ShopItem");

            migrationBuilder.DropColumn(
                name: "ManagedServicePrice",
                table: "ShopItem");
        }
    }
}
