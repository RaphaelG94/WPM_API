using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _122_AddReferencesToAssetTypeAssetClass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CustomerId",
                table: "AssetType",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "fromAdmin",
                table: "AssetType",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "AssetTypeId",
                table: "AssetClass",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerId",
                table: "AssetClass",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "fromAdmin",
                table: "AssetClass",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_AssetType_CustomerId",
                table: "AssetType",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetClass_AssetTypeId",
                table: "AssetClass",
                column: "AssetTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetClass_CustomerId",
                table: "AssetClass",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_AssetClass_AssetType_AssetTypeId",
                table: "AssetClass",
                column: "AssetTypeId",
                principalTable: "AssetType",
                principalColumn: "PK_AssetType",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AssetClass_Customer_CustomerId",
                table: "AssetClass",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "PK_Customer",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AssetType_Customer_CustomerId",
                table: "AssetType",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "PK_Customer",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssetClass_AssetType_AssetTypeId",
                table: "AssetClass");

            migrationBuilder.DropForeignKey(
                name: "FK_AssetClass_Customer_CustomerId",
                table: "AssetClass");

            migrationBuilder.DropForeignKey(
                name: "FK_AssetType_Customer_CustomerId",
                table: "AssetType");

            migrationBuilder.DropIndex(
                name: "IX_AssetType_CustomerId",
                table: "AssetType");

            migrationBuilder.DropIndex(
                name: "IX_AssetClass_AssetTypeId",
                table: "AssetClass");

            migrationBuilder.DropIndex(
                name: "IX_AssetClass_CustomerId",
                table: "AssetClass");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "AssetType");

            migrationBuilder.DropColumn(
                name: "fromAdmin",
                table: "AssetType");

            migrationBuilder.DropColumn(
                name: "AssetTypeId",
                table: "AssetClass");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "AssetClass");

            migrationBuilder.DropColumn(
                name: "fromAdmin",
                table: "AssetClass");
        }
    }
}
