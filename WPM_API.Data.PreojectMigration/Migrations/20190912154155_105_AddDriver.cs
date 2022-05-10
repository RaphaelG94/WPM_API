using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _105_AddDriver : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DriverId",
                table: "HardwareModel",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Driver",
                columns: table => new
                {
                    PK_Driver = table.Column<string>(nullable: false),
                    Vendor = table.Column<string>(nullable: true),
                    DriverName = table.Column<string>(nullable: true),
                    Version = table.Column<string>(nullable: true),
                    ValidOS = table.Column<string>(nullable: true),
                    ReleaseDate = table.Column<DateTime>(nullable: false),
                    DeviceName = table.Column<string>(nullable: true),
                    DeviceNameDriverManager = table.Column<string>(nullable: true),
                    ContentId = table.Column<string>(nullable: true),
                    ReadeMeId = table.Column<string>(nullable: true),
                    VendorId = table.Column<string>(nullable: true),
                    CreatedByUserId = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedByUserId = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    DeletedByUserId = table.Column<string>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Driver", x => x.PK_Driver);
                    table.ForeignKey(
                        name: "FK_Driver_File_ContentId",
                        column: x => x.ContentId,
                        principalTable: "File",
                        principalColumn: "PK_File",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Driver_File_ReadeMeId",
                        column: x => x.ReadeMeId,
                        principalTable: "File",
                        principalColumn: "PK_File",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HardwareModel_DriverId",
                table: "HardwareModel",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_Driver_ContentId",
                table: "Driver",
                column: "ContentId");

            migrationBuilder.CreateIndex(
                name: "IX_Driver_ReadeMeId",
                table: "Driver",
                column: "ReadeMeId");

            migrationBuilder.AddForeignKey(
                name: "FK_HardwareModel_Driver_DriverId",
                table: "HardwareModel",
                column: "DriverId",
                principalTable: "Driver",
                principalColumn: "PK_Driver",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HardwareModel_Driver_DriverId",
                table: "HardwareModel");

            migrationBuilder.DropTable(
                name: "Driver");

            migrationBuilder.DropIndex(
                name: "IX_HardwareModel_DriverId",
                table: "HardwareModel");

            migrationBuilder.DropColumn(
                name: "DriverId",
                table: "HardwareModel");
        }
    }
}
