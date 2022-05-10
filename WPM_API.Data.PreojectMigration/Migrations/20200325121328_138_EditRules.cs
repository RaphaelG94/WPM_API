using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _138_EditRules : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Architecture",
                table: "Rule");

            migrationBuilder.CreateTable(
                name: "OsType",
                columns: table => new
                {
                    PK_OSType = table.Column<string>(nullable: false),
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
                    table.PrimaryKey("PK_OsType", x => x.PK_OSType);
                    table.ForeignKey(
                        name: "FK_OsType_Rule_RuleId",
                        column: x => x.RuleId,
                        principalTable: "Rule",
                        principalColumn: "PK_Rule",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OsVersionName",
                columns: table => new
                {
                    PK_OSVersion = table.Column<string>(nullable: false),
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
                    table.PrimaryKey("PK_OsVersionName", x => x.PK_OSVersion);
                    table.ForeignKey(
                        name: "FK_OsVersionName_Rule_RuleId",
                        column: x => x.RuleId,
                        principalTable: "Rule",
                        principalColumn: "PK_Rule",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Win10Version",
                columns: table => new
                {
                    PK_Win10Version = table.Column<string>(nullable: false),
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
                    table.PrimaryKey("PK_Win10Version", x => x.PK_Win10Version);
                    table.ForeignKey(
                        name: "FK_Win10Version_Rule_RuleId",
                        column: x => x.RuleId,
                        principalTable: "Rule",
                        principalColumn: "PK_Rule",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OsType_RuleId",
                table: "OsType",
                column: "RuleId");

            migrationBuilder.CreateIndex(
                name: "IX_OsVersionName_RuleId",
                table: "OsVersionName",
                column: "RuleId");

            migrationBuilder.CreateIndex(
                name: "IX_Win10Version_RuleId",
                table: "Win10Version",
                column: "RuleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OsType");

            migrationBuilder.DropTable(
                name: "OsVersionName");

            migrationBuilder.DropTable(
                name: "Win10Version");

            migrationBuilder.AddColumn<string>(
                name: "Architecture",
                table: "Rule",
                nullable: true);
        }
    }
}
