using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _07_BaseInDomain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BaseId",
                table: "Domain",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Domain_BaseId",
                table: "Domain",
                column: "BaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Domain_Base_BaseId",
                table: "Domain",
                column: "BaseId",
                principalTable: "Base",
                principalColumn: "PK_Base",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Domain_Base_BaseId",
                table: "Domain");

            migrationBuilder.DropIndex(
                name: "IX_Domain_BaseId",
                table: "Domain");

            migrationBuilder.DropColumn(
                name: "BaseId",
                table: "Domain");
        }
    }
}
