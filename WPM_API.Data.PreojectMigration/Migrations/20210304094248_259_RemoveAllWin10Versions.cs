﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _259_RemoveAllWin10Versions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AllWin10Versions",
                table: "Software");

            migrationBuilder.DropColumn(
                name: "AllWin10Versions",
                table: "CustomerSoftware");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AllWin10Versions",
                table: "Software",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AllWin10Versions",
                table: "CustomerSoftware",
                nullable: false,
                defaultValue: false);
        }
    }
}
