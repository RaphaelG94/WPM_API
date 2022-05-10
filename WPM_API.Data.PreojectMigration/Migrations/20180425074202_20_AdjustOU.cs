using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _20_AdjustOU : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OrganizationalUnitId",
                table: "Server",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsLeaf",
                table: "OrganizationalUnit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Server_OrganizationalUnitId",
                table: "Server",
                column: "OrganizationalUnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_Server_OrganizationalUnit_OrganizationalUnitId",
                table: "Server",
                column: "OrganizationalUnitId",
                principalTable: "OrganizationalUnit",
                principalColumn: "PK_OrganizationalUnit",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Server_OrganizationalUnit_OrganizationalUnitId",
                table: "Server");

            migrationBuilder.DropIndex(
                name: "IX_Server_OrganizationalUnitId",
                table: "Server");

            migrationBuilder.DropColumn(
                name: "OrganizationalUnitId",
                table: "Server");

            migrationBuilder.DropColumn(
                name: "IsLeaf",
                table: "OrganizationalUnit");
        }
    }
}
