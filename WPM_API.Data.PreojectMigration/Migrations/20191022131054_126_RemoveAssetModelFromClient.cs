using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _126_RemoveAssetModelFromClient : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Client_AssetModel_AssetModelId",
                table: "Client");

            migrationBuilder.DropIndex(
                name: "IX_Client_AssetModelId",
                table: "Client");

            migrationBuilder.AlterColumn<string>(
                name: "AssetModelId",
                table: "Client",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "AssetModelId",
                table: "Client",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

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
    }
}
