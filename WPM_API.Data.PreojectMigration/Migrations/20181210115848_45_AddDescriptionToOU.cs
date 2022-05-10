﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _45_AddDescriptionToOU : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "OrganizationalUnit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "OrganizationalUnit");
        }
    }
}
