using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _107_AddOSModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OSModel",
                columns: table => new
                {
                    PK_OSModel = table.Column<string>(nullable: false),
                    Vendor = table.Column<string>(nullable: true),
                    OSName = table.Column<string>(nullable: true),
                    Architecture = table.Column<string>(nullable: true),
                    OSType = table.Column<string>(nullable: true),
                    Version = table.Column<string>(nullable: true),
                    ReleaseDate = table.Column<DateTime>(nullable: false),
                    SupportEnd = table.Column<string>(nullable: true),
                    ContentId = table.Column<string>(nullable: true),
                    CreatedByUserId = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedByUserId = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    DeletedByUserId = table.Column<string>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OSModel", x => x.PK_OSModel);
                    table.ForeignKey(
                        name: "FK_OSModel_File_ContentId",
                        column: x => x.ContentId,
                        principalTable: "File",
                        principalColumn: "PK_File",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OSModel_ContentId",
                table: "OSModel",
                column: "ContentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OSModel");
        }
    }
}
