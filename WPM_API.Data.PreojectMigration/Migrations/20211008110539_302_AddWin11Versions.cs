using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _302_AddWin11Versions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AllWin11Versions",
                table: "Software",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Win11Version",
                columns: table => new
                {
                    PK_Win11Version = table.Column<string>(nullable: false),
                    Version = table.Column<string>(nullable: true),
                    CreatedByUserId = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedByUserId = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    DeletedByUserId = table.Column<string>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    RuleId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Win11Version", x => x.PK_Win11Version);
                    table.ForeignKey(
                        name: "FK_Win11Version_Rule_RuleId",
                        column: x => x.RuleId,
                        principalTable: "Rule",
                        principalColumn: "PK_Rule",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Win11Version_RuleId",
                table: "Win11Version",
                column: "RuleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Win11Version");

            migrationBuilder.DropColumn(
                name: "AllWin11Versions",
                table: "Software");
        }
    }
}
