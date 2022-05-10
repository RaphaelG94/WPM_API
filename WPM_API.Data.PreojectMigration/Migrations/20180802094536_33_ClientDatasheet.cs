using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _33_ClientDatasheet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropColumn(
                name: "Bios",
                table: "Client");

            migrationBuilder.AddColumn<string>(
                name: "UsageStatus",
                table: "Client",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BiosId",
                table: "Client",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HardwareId",
                table: "Client",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InstallationDate",
                table: "Client",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JoinedDomain",
                table: "Client",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Client",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MainUser",
                table: "Client",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NetworkId",
                table: "Client",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OsId",
                table: "Client",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PartitionId",
                table: "Client",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Proxy",
                table: "Client",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PurchaseId",
                table: "Client",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BiosSettings",
                columns: table => new
                {
                    PK_BiosSettingsId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BiosSettings", x => x.PK_BiosSettingsId);
                });

            migrationBuilder.CreateTable(
                name: "Hardware",
                columns: table => new
                {
                    PK_HardwareId = table.Column<string>(nullable: false),
                    ChipSet = table.Column<string>(nullable: true),
                    DisplayResolution = table.Column<string>(nullable: true),
                    HDD = table.Column<string>(nullable: true),
                    Manufacturer = table.Column<string>(nullable: true),
                    ModelNumber = table.Column<string>(nullable: true),
                    Processor = table.Column<string>(nullable: true),
                    RAM = table.Column<string>(nullable: true),
                    SerialNumber = table.Column<string>(nullable: true),
                    ServiceTag = table.Column<string>(nullable: true),
                    TPMChip = table.Column<string>(nullable: true),
                    TPMChipClear = table.Column<string>(nullable: true),
                    TPMChipOwnership = table.Column<string>(nullable: true),
                    TPMChipVersion = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hardware", x => x.PK_HardwareId);
                });

            migrationBuilder.CreateTable(
                name: "HDDPartition",
                columns: table => new
                {
                    PK_HDDPartitionId = table.Column<string>(nullable: false),
                    Gpt = table.Column<string>(nullable: true),
                    Overprovisioning = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HDDPartition", x => x.PK_HDDPartitionId);
                });

            migrationBuilder.CreateTable(
                name: "NetworkConfiguration",
                columns: table => new
                {
                    PK_NetworkConfigurationId = table.Column<string>(nullable: false),
                    DHCP = table.Column<string>(nullable: true),
                    DNS = table.Column<string>(nullable: true),
                    Gateway = table.Column<string>(nullable: true),
                    IPv4 = table.Column<string>(nullable: true),
                    IPv6 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NetworkConfiguration", x => x.PK_NetworkConfigurationId);
                });

            migrationBuilder.CreateTable(
                name: "OS",
                columns: table => new
                {
                    PK_OSId = table.Column<string>(nullable: false),
                    KeyboardLayout = table.Column<string>(nullable: true),
                    LanguagePackage = table.Column<string>(nullable: true),
                    OSVersion = table.Column<string>(nullable: true),
                    Uefi = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OS", x => x.PK_OSId);
                });

            migrationBuilder.CreateTable(
                name: "Purchase",
                columns: table => new
                {
                    PK_PurchaseId = table.Column<string>(nullable: false),
                    AcquisitionCost = table.Column<string>(nullable: true),
                    CostUnitAssignment = table.Column<string>(nullable: true),
                    DecommissioningDate = table.Column<string>(nullable: true),
                    PurchaseDate = table.Column<string>(nullable: true),
                    Vendor = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Purchase", x => x.PK_PurchaseId);
                });

            migrationBuilder.CreateTable(
                name: "Bios",
                columns: table => new
                {
                    PK_BiosId = table.Column<string>(nullable: false),
                    BiosSettingsId = table.Column<string>(nullable: true),
                    InstalledVersion = table.Column<string>(nullable: true),
                    InternalVersion = table.Column<string>(nullable: true),
                    Manufacturer = table.Column<string>(nullable: true),
                    ManufacturerVersion = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bios", x => x.PK_BiosId);
                    table.ForeignKey(
                        name: "FK_Bios_BiosSettings_BiosSettingsId",
                        column: x => x.BiosSettingsId,
                        principalTable: "BiosSettings",
                        principalColumn: "PK_BiosSettingsId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Client_BiosId",
                table: "Client",
                column: "BiosId");

            migrationBuilder.CreateIndex(
                name: "IX_Client_HardwareId",
                table: "Client",
                column: "HardwareId");

            migrationBuilder.CreateIndex(
                name: "IX_Client_NetworkId",
                table: "Client",
                column: "NetworkId");

            migrationBuilder.CreateIndex(
                name: "IX_Client_OsId",
                table: "Client",
                column: "OsId");

            migrationBuilder.CreateIndex(
                name: "IX_Client_PartitionId",
                table: "Client",
                column: "PartitionId");

            migrationBuilder.CreateIndex(
                name: "IX_Client_PurchaseId",
                table: "Client",
                column: "PurchaseId");

            migrationBuilder.CreateIndex(
                name: "IX_Bios_BiosSettingsId",
                table: "Bios",
                column: "BiosSettingsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Client_Bios_BiosId",
                table: "Client",
                column: "BiosId",
                principalTable: "Bios",
                principalColumn: "PK_BiosId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Client_Hardware_HardwareId",
                table: "Client",
                column: "HardwareId",
                principalTable: "Hardware",
                principalColumn: "PK_HardwareId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Client_NetworkConfiguration_NetworkId",
                table: "Client",
                column: "NetworkId",
                principalTable: "NetworkConfiguration",
                principalColumn: "PK_NetworkConfigurationId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Client_OS_OsId",
                table: "Client",
                column: "OsId",
                principalTable: "OS",
                principalColumn: "PK_OSId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Client_HDDPartition_PartitionId",
                table: "Client",
                column: "PartitionId",
                principalTable: "HDDPartition",
                principalColumn: "PK_HDDPartitionId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Client_Purchase_PurchaseId",
                table: "Client",
                column: "PurchaseId",
                principalTable: "Purchase",
                principalColumn: "PK_PurchaseId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Client_Bios_BiosId",
                table: "Client");

            migrationBuilder.DropForeignKey(
                name: "FK_Client_Hardware_HardwareId",
                table: "Client");

            migrationBuilder.DropForeignKey(
                name: "FK_Client_NetworkConfiguration_NetworkId",
                table: "Client");

            migrationBuilder.DropForeignKey(
                name: "FK_Client_OS_OsId",
                table: "Client");

            migrationBuilder.DropForeignKey(
                name: "FK_Client_HDDPartition_PartitionId",
                table: "Client");

            migrationBuilder.DropForeignKey(
                name: "FK_Client_Purchase_PurchaseId",
                table: "Client");

            migrationBuilder.DropTable(
                name: "Bios");

            migrationBuilder.DropTable(
                name: "Hardware");

            migrationBuilder.DropTable(
                name: "HDDPartition");

            migrationBuilder.DropTable(
                name: "NetworkConfiguration");

            migrationBuilder.DropTable(
                name: "OS");

            migrationBuilder.DropTable(
                name: "Purchase");

            migrationBuilder.DropTable(
                name: "BiosSettings");

            migrationBuilder.DropIndex(
                name: "IX_Client_BiosId",
                table: "Client");

            migrationBuilder.DropIndex(
                name: "IX_Client_HardwareId",
                table: "Client");

            migrationBuilder.DropIndex(
                name: "IX_Client_NetworkId",
                table: "Client");

            migrationBuilder.DropIndex(
                name: "IX_Client_OsId",
                table: "Client");

            migrationBuilder.DropIndex(
                name: "IX_Client_PartitionId",
                table: "Client");

            migrationBuilder.DropIndex(
                name: "IX_Client_PurchaseId",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "BiosId",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "HardwareId",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "InstallationDate",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "JoinedDomain",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "MainUser",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "NetworkId",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "OsId",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "PartitionId",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "Proxy",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "PurchaseId",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "UsageStatus",
                table: "Client");

            migrationBuilder.AddColumn<string>(
                name: "Bios",
                table: "Client",
                nullable: true);

        }
    }
}
