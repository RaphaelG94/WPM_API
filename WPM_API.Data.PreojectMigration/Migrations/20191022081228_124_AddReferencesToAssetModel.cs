using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _124_AddReferencesToAssetModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Client_AssetModelId",
                table: "Client");

            migrationBuilder.AddColumn<string>(
                name: "CINumber",
                table: "AssetModel",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClientId",
                table: "AssetModel",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LocationId",
                table: "AssetModel",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PersonId",
                table: "AssetModel",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Client_AssetModelId",
                table: "Client",
                column: "AssetModelId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetModel_ClientId",
                table: "AssetModel",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetModel_LocationId",
                table: "AssetModel",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetModel_PersonId",
                table: "AssetModel",
                column: "PersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_AssetModel_Client_ClientId",
                table: "AssetModel",
                column: "ClientId",
                principalTable: "Client",
                principalColumn: "PK_Client",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AssetModel_Location_LocationId",
                table: "AssetModel",
                column: "LocationId",
                principalTable: "Location",
                principalColumn: "PK_Location",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AssetModel_Person_PersonId",
                table: "AssetModel",
                column: "PersonId",
                principalTable: "Person",
                principalColumn: "PK_Person",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssetModel_Client_ClientId",
                table: "AssetModel");

            migrationBuilder.DropForeignKey(
                name: "FK_AssetModel_Location_LocationId",
                table: "AssetModel");

            migrationBuilder.DropForeignKey(
                name: "FK_AssetModel_Person_PersonId",
                table: "AssetModel");

            migrationBuilder.DropIndex(
                name: "IX_Client_AssetModelId",
                table: "Client");

            migrationBuilder.DropIndex(
                name: "IX_AssetModel_ClientId",
                table: "AssetModel");

            migrationBuilder.DropIndex(
                name: "IX_AssetModel_LocationId",
                table: "AssetModel");

            migrationBuilder.DropIndex(
                name: "IX_AssetModel_PersonId",
                table: "AssetModel");

            migrationBuilder.DropColumn(
                name: "CINumber",
                table: "AssetModel");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "AssetModel");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "AssetModel");

            migrationBuilder.DropColumn(
                name: "PersonId",
                table: "AssetModel");

            migrationBuilder.CreateIndex(
                name: "IX_Client_AssetModelId",
                table: "Client",
                column: "AssetModelId",
                unique: true,
                filter: "[AssetModelId] IS NOT NULL");
        }
    }
}
