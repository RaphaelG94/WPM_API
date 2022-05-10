using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _282_AddAdminOptions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdminDeviceOptionId",
                table: "ScriptVersion",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AdminDeviceOption",
                columns: table => new
                {
                    PK_AdminDeviceOption = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    CreatedByUserId = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedByUserId = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    DeletedByUserId = table.Column<string>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminDeviceOption", x => x.PK_AdminDeviceOption);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ScriptVersion_AdminDeviceOptionId",
                table: "ScriptVersion",
                column: "AdminDeviceOptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_ScriptVersion_AdminDeviceOption_AdminDeviceOptionId",
                table: "ScriptVersion",
                column: "AdminDeviceOptionId",
                principalTable: "AdminDeviceOption",
                principalColumn: "PK_AdminDeviceOption",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScriptVersion_AdminDeviceOption_AdminDeviceOptionId",
                table: "ScriptVersion");

            migrationBuilder.DropTable(
                name: "AdminDeviceOption");

            migrationBuilder.DropIndex(
                name: "IX_ScriptVersion_AdminDeviceOptionId",
                table: "ScriptVersion");

            migrationBuilder.DropColumn(
                name: "AdminDeviceOptionId",
                table: "ScriptVersion");
        }
    }
}
