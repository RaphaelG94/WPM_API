using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _111_AddAssetModelToClient : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AssetModelId",
                table: "Client",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Client_AssetModelId",
                table: "Client",
                column: "AssetModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Client_AssetModel_AssetModelId",
                table: "Client",
                column: "AssetModelId",
                principalTable: "AssetModel",
                principalColumn: "PK_AssetModel",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Client_AssetModel_AssetModelId",
                table: "Client");

            migrationBuilder.DropIndex(
                name: "IX_Client_AssetModelId",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "AssetModelId",
                table: "Client");
        }
    }
}
