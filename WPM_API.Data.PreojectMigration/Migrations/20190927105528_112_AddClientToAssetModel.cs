using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _112_AddClientToAssetModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Client_AssetModelId",
                table: "Client");

            migrationBuilder.CreateIndex(
                name: "IX_Client_AssetModelId",
                table: "Client",
                column: "AssetModelId",
                unique: true,
                filter: "[AssetModelId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Client_AssetModelId",
                table: "Client");

            migrationBuilder.CreateIndex(
                name: "IX_Client_AssetModelId",
                table: "Client",
                column: "AssetModelId");
        }
    }
}
