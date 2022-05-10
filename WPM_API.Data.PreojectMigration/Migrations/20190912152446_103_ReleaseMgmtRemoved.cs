using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _103_ReleaseMgmtRemoved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            /*
            migrationBuilder.DropTable(
                name: "HardwareModel");

            migrationBuilder.DropTable(
                name: "BIOSReleaseMgmt");

            migrationBuilder.DropTable(
                name: "Driver");

            migrationBuilder.DropTable(
                name: "OperatingSystem");
                */
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            /*
            migrationBuilder.CreateTable(
                name: "BIOSReleaseMgmt",
                columns: table => new
                {
                    PK_BiosReleaseMgmt = table.Column<string>(nullable: false),
                    ContentId = table.Column<string>(nullable: true),
                    CreatedByUserId = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    DeletedByUserId = table.Column<string>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    ReadMeId = table.Column<string>(nullable: true),
                    ReleaseDate = table.Column<DateTime>(nullable: false),
                    UpdatedByUserId = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    ValidOS = table.Column<string>(nullable: true),
                    Vendor = table.Column<string>(nullable: true),
                    Version = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BIOSReleaseMgmt", x => x.PK_BiosReleaseMgmt);
                    table.ForeignKey(
                        name: "FK_BIOSReleaseMgmt_File_ContentId",
                        column: x => x.ContentId,
                        principalTable: "File",
                        principalColumn: "PK_File",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BIOSReleaseMgmt_File_ReadMeId",
                        column: x => x.ReadMeId,
                        principalTable: "File",
                        principalColumn: "PK_File",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Driver",
                columns: table => new
                {
                    PK_Driver = table.Column<string>(nullable: false),
                    ContentId = table.Column<string>(nullable: true),
                    CreatedByUserId = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    DeletedByUserId = table.Column<string>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    DeviceName = table.Column<string>(nullable: true),
                    DeviceNameDriverManager = table.Column<string>(nullable: true),
                    DriverName = table.Column<string>(nullable: true),
                    ReadMeId = table.Column<string>(nullable: true),
                    ReleaseDate = table.Column<DateTime>(nullable: false),
                    UpdatedByUserId = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    ValidOS = table.Column<string>(nullable: true),
                    Vendor = table.Column<string>(nullable: true),
                    VendorId = table.Column<string>(nullable: true),
                    Version = table.Column<string>(nullable: true)
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
                        name: "FK_Driver_File_ReadMeId",
                        column: x => x.ReadMeId,
                        principalTable: "File",
                        principalColumn: "PK_File",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OperatingSystem",
                columns: table => new
                {
                    PK_OperatingSystem = table.Column<string>(nullable: false),
                    Architecture = table.Column<string>(nullable: true),
                    CreatedByUserId = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    DeletedByUserId = table.Column<string>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    NameAddition = table.Column<string>(nullable: true),
                    ReleaseDate = table.Column<DateTime>(nullable: false),
                    SupportEnd = table.Column<DateTime>(nullable: false),
                    UpdatedByUserId = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    Vendor = table.Column<string>(nullable: true),
                    Version = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperatingSystem", x => x.PK_OperatingSystem);
                });

            migrationBuilder.CreateTable(
                name: "HardwareModel",
                columns: table => new
                {
                    PK_HardwareModel = table.Column<string>(nullable: false),
                    BIOSReleaseMgmtId = table.Column<string>(nullable: true),
                    CreatedByUserId = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    DeletedByUserId = table.Column<string>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    DriverId = table.Column<string>(nullable: true),
                    ModelFamily = table.Column<string>(nullable: true),
                    ModelType = table.Column<string>(nullable: true),
                    OperatingSystemId = table.Column<string>(nullable: true),
                    ProductionEnd = table.Column<DateTime>(nullable: false),
                    ProductionStart = table.Column<DateTime>(nullable: false),
                    UpdatedByUserId = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    Vendor = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HardwareModel", x => x.PK_HardwareModel);
                    table.ForeignKey(
                        name: "FK_HardwareModel_BIOSReleaseMgmt_BIOSReleaseMgmtId",
                        column: x => x.BIOSReleaseMgmtId,
                        principalTable: "BIOSReleaseMgmt",
                        principalColumn: "PK_BiosReleaseMgmt",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HardwareModel_Driver_DriverId",
                        column: x => x.DriverId,
                        principalTable: "Driver",
                        principalColumn: "PK_Driver",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HardwareModel_OperatingSystem_OperatingSystemId",
                        column: x => x.OperatingSystemId,
                        principalTable: "OperatingSystem",
                        principalColumn: "PK_OperatingSystem",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BIOSReleaseMgmt_ContentId",
                table: "BIOSReleaseMgmt",
                column: "ContentId");

            migrationBuilder.CreateIndex(
                name: "IX_BIOSReleaseMgmt_ReadMeId",
                table: "BIOSReleaseMgmt",
                column: "ReadMeId");

            migrationBuilder.CreateIndex(
                name: "IX_Driver_ContentId",
                table: "Driver",
                column: "ContentId");

            migrationBuilder.CreateIndex(
                name: "IX_Driver_ReadMeId",
                table: "Driver",
                column: "ReadMeId");

            migrationBuilder.CreateIndex(
                name: "IX_HardwareModel_BIOSReleaseMgmtId",
                table: "HardwareModel",
                column: "BIOSReleaseMgmtId");

            migrationBuilder.CreateIndex(
                name: "IX_HardwareModel_DriverId",
                table: "HardwareModel",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_HardwareModel_OperatingSystemId",
                table: "HardwareModel",
                column: "OperatingSystemId");
                */
        }
    }
}
