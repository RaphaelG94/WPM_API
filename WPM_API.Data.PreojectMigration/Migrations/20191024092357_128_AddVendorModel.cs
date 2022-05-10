using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _128_AddVendorModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VendorModelId",
                table: "File",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VendorModelId",
                table: "AssetModel",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "VendorModel",
                columns: table => new
                {
                    PK_VendorModel = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    ModelFamily = table.Column<string>(nullable: true),
                    ModelType = table.Column<string>(nullable: true),
                    CreatedByUserId = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedByUserId = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    DeletedByUserId = table.Column<string>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VendorModel", x => x.PK_VendorModel);
                });

            migrationBuilder.CreateIndex(
                name: "IX_File_VendorModelId",
                table: "File",
                column: "VendorModelId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetModel_VendorModelId",
                table: "AssetModel",
                column: "VendorModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_AssetModel_VendorModel_VendorModelId",
                table: "AssetModel",
                column: "VendorModelId",
                principalTable: "VendorModel",
                principalColumn: "PK_VendorModel",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_File_VendorModel_VendorModelId",
                table: "File",
                column: "VendorModelId",
                principalTable: "VendorModel",
                principalColumn: "PK_VendorModel",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssetModel_VendorModel_VendorModelId",
                table: "AssetModel");

            migrationBuilder.DropForeignKey(
                name: "FK_File_VendorModel_VendorModelId",
                table: "File");

            migrationBuilder.DropTable(
                name: "VendorModel");

            migrationBuilder.DropIndex(
                name: "IX_File_VendorModelId",
                table: "File");

            migrationBuilder.DropIndex(
                name: "IX_AssetModel_VendorModelId",
                table: "AssetModel");

            migrationBuilder.DropColumn(
                name: "VendorModelId",
                table: "File");

            migrationBuilder.DropColumn(
                name: "VendorModelId",
                table: "AssetModel");
        }
    }
}
