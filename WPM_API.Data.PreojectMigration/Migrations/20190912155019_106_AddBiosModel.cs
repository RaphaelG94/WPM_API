using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _106_AddBiosModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BIOSModelId",
                table: "HardwareModel",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BIOSModel",
                columns: table => new
                {
                    PK_HardwareModel = table.Column<string>(nullable: false),
                    Vendor = table.Column<string>(nullable: true),
                    Version = table.Column<string>(nullable: true),
                    ValidOS = table.Column<string>(nullable: true),
                    ReleaseDate = table.Column<DateTime>(nullable: false),
                    ContentId = table.Column<string>(nullable: true),
                    ReadMeId = table.Column<string>(nullable: true),
                    CreatedByUserId = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedByUserId = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    DeletedByUserId = table.Column<string>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BIOSModel", x => x.PK_HardwareModel);
                    table.ForeignKey(
                        name: "FK_BIOSModel_File_ContentId",
                        column: x => x.ContentId,
                        principalTable: "File",
                        principalColumn: "PK_File",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BIOSModel_File_ReadMeId",
                        column: x => x.ReadMeId,
                        principalTable: "File",
                        principalColumn: "PK_File",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HardwareModel_BIOSModelId",
                table: "HardwareModel",
                column: "BIOSModelId");

            migrationBuilder.CreateIndex(
                name: "IX_BIOSModel_ContentId",
                table: "BIOSModel",
                column: "ContentId");

            migrationBuilder.CreateIndex(
                name: "IX_BIOSModel_ReadMeId",
                table: "BIOSModel",
                column: "ReadMeId");

            migrationBuilder.AddForeignKey(
                name: "FK_HardwareModel_BIOSModel_BIOSModelId",
                table: "HardwareModel",
                column: "BIOSModelId",
                principalTable: "BIOSModel",
                principalColumn: "PK_HardwareModel",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HardwareModel_BIOSModel_BIOSModelId",
                table: "HardwareModel");

            migrationBuilder.DropTable(
                name: "BIOSModel");

            migrationBuilder.DropIndex(
                name: "IX_HardwareModel_BIOSModelId",
                table: "HardwareModel");

            migrationBuilder.DropColumn(
                name: "BIOSModelId",
                table: "HardwareModel");
        }
    }
}
