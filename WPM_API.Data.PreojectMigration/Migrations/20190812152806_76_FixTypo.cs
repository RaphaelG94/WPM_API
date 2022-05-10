using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _76_FixTypo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ManagedServiceLifecylcePrice",
                table: "ShopItem",
                newName: "ManagedServiceLifecyclePrice");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ManagedServiceLifecyclePrice",
                table: "ShopItem",
                newName: "ManagedServiceLifecylcePrice");
        }
    }
}
