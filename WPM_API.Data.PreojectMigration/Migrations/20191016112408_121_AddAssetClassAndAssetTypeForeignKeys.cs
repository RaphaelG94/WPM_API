using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _121_AddAssetClassAndAssetTypeForeignKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AssetType",
                table: "AssetModel",
                newName: "AssetTypeId");

            migrationBuilder.RenameColumn(
                name: "AssetClass",
                table: "AssetModel",
                newName: "AssetClassId");

            migrationBuilder.AlterColumn<string>(
                name: "AssetTypeId",
                table: "AssetModel",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AssetClassId",
                table: "AssetModel",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AssetModel_AssetClassId",
                table: "AssetModel",
                column: "AssetClassId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetModel_AssetTypeId",
                table: "AssetModel",
                column: "AssetTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_AssetModel_AssetClass_AssetClassId",
                table: "AssetModel",
                column: "AssetClassId",
                principalTable: "AssetClass",
                principalColumn: "PK_AssetClass",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AssetModel_AssetType_AssetTypeId",
                table: "AssetModel",
                column: "AssetTypeId",
                principalTable: "AssetType",
                principalColumn: "PK_AssetType",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssetModel_AssetClass_AssetClassId",
                table: "AssetModel");

            migrationBuilder.DropForeignKey(
                name: "FK_AssetModel_AssetType_AssetTypeId",
                table: "AssetModel");

            migrationBuilder.DropIndex(
                name: "IX_AssetModel_AssetClassId",
                table: "AssetModel");

            migrationBuilder.DropIndex(
                name: "IX_AssetModel_AssetTypeId",
                table: "AssetModel");

            migrationBuilder.RenameColumn(
                name: "AssetTypeId",
                table: "AssetModel",
                newName: "AssetType");

            migrationBuilder.RenameColumn(
                name: "AssetClassId",
                table: "AssetModel",
                newName: "AssetClass");

            migrationBuilder.AlterColumn<string>(
                name: "AssetType",
                table: "AssetModel",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AssetClass",
                table: "AssetModel",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
