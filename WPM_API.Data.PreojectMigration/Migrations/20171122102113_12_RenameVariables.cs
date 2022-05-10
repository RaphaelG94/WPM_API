using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _12_RenameVariables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Key",
                table: "Variable");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "Variable");

            migrationBuilder.AddColumn<string>(
                name: "Default",
                table: "Variable",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Variable",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Default",
                table: "Variable");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Variable");

            migrationBuilder.AddColumn<string>(
                name: "Key",
                table: "Variable",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Value",
                table: "Variable",
                nullable: true);
        }
    }
}
