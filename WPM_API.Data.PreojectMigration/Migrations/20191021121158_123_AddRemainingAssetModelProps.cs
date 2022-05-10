using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _123_AddRemainingAssetModelProps : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InvoiceId",
                table: "AssetModel",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PurchaseDate",
                table: "AssetModel",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "PurchaseValue",
                table: "AssetModel",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AssetModel_InvoiceId",
                table: "AssetModel",
                column: "InvoiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_AssetModel_File_InvoiceId",
                table: "AssetModel",
                column: "InvoiceId",
                principalTable: "File",
                principalColumn: "PK_File",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssetModel_File_InvoiceId",
                table: "AssetModel");

            migrationBuilder.DropIndex(
                name: "IX_AssetModel_InvoiceId",
                table: "AssetModel");

            migrationBuilder.DropColumn(
                name: "InvoiceId",
                table: "AssetModel");

            migrationBuilder.DropColumn(
                name: "PurchaseDate",
                table: "AssetModel");

            migrationBuilder.DropColumn(
                name: "PurchaseValue",
                table: "AssetModel");
        }
    }
}
