﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _113_EditAssetModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AssetQRValue",
                table: "AssetModel",
                newName: "Price");

            migrationBuilder.AddColumn<string>(
                name: "Bill",
                table: "AssetModel",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DepreciationEnd",
                table: "AssetModel",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "DepreciationValue",
                table: "AssetModel",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "AssetModel",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PurchaseDate",
                table: "AssetModel",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "isBrutto",
                table: "AssetModel",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bill",
                table: "AssetModel");

            migrationBuilder.DropColumn(
                name: "DepreciationEnd",
                table: "AssetModel");

            migrationBuilder.DropColumn(
                name: "DepreciationValue",
                table: "AssetModel");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "AssetModel");

            migrationBuilder.DropColumn(
                name: "PurchaseDate",
                table: "AssetModel");

            migrationBuilder.DropColumn(
                name: "isBrutto",
                table: "AssetModel");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "AssetModel",
                newName: "AssetQRValue");
        }
    }
}
