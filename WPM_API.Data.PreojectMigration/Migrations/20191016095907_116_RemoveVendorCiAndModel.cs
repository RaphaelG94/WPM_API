using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _116_RemoveVendorCiAndModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VendorCI");

            migrationBuilder.DropTable(
                name: "VendorModel");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VendorModel",
                columns: table => new
                {
                    PK_VendorModel = table.Column<string>(nullable: false),
                    CreatedByUserId = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    DeletedByUserId = table.Column<string>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    ManualId = table.Column<string>(nullable: true),
                    ModelFamily = table.Column<string>(nullable: true),
                    ModelType = table.Column<string>(nullable: true),
                    UpdatedByUserId = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    VendorName = table.Column<string>(nullable: true)
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

            migrationBuilder.CreateTable(
                name: "VendorCI",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreatedByUserId = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    DeletedByUserId = table.Column<string>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    MACAddress = table.Column<string>(nullable: true),
                    ProductionDate = table.Column<DateTime>(nullable: false),
                    SerialNumber = table.Column<string>(nullable: true),
                    UpdatedByUserId = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    VendorModelId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VendorCI", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VendorCI_VendorModel_VendorModelId",
                        column: x => x.VendorModelId,
                        principalTable: "VendorModel",
                        principalColumn: "PK_VendorModel",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VendorCI_VendorModelId",
                table: "VendorCI",
                column: "VendorModelId");

            migrationBuilder.CreateIndex(
                name: "IX_VendorModel_ManualId",
                table: "VendorModel",
                column: "ManualId");
        }
    }
}
