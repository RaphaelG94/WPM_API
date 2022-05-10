using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _114_VendorModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VendorModel",
                columns: table => new
                {
                    PK_VendorModel = table.Column<string>(nullable: false),
                    VendorName = table.Column<string>(nullable: true),
                    ModelFamily = table.Column<string>(nullable: true),
                    ModelType = table.Column<string>(nullable: true),
                    ManualId = table.Column<string>(nullable: true),
                    CreatedByUserId = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedByUserId = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    DeletedByUserId = table.Column<string>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VendorModel", x => x.PK_VendorModel);
                    table.ForeignKey(
                        name: "FK_VendorModel_File_ManualId",
                        column: x => x.ManualId,
                        principalTable: "File",
                        principalColumn: "PK_File",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VendorModel_ManualId",
                table: "VendorModel",
                column: "ManualId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VendorModel");
        }
    }
}
