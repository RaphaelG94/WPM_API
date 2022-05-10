using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _01_BaseDomainAzureModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_User_DeletedByUserId",
                table: "User");

            migrationBuilder.DropForeignKey(
                name: "FK_User_User_UpdatedByUserId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_DeletedByUserId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_UpdatedByUserId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_AzureCredentials_FK_Customer",
                table: "AzureCredentials");

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedByUserId",
                table: "User",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DeletedByUserId",
                table: "User",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Domain",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DeletedByUserId",
                table: "Domain",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "Domain",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedByUserId",
                table: "Domain",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Domain",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "ResourceGroup",
                columns: table => new
                {
                    PK_ResourceGroup = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResourceGroup", x => x.PK_ResourceGroup);
                });

            migrationBuilder.CreateTable(
                name: "StorageAccount",
                columns: table => new
                {
                    PK_StorageAccount = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StorageAccount", x => x.PK_StorageAccount);
                });

            migrationBuilder.CreateTable(
                name: "VirtualNetwork",
                columns: table => new
                {
                    PK_VirtualNetwork = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AddressRange = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Dns = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VirtualNetwork", x => x.PK_VirtualNetwork);
                });

            migrationBuilder.CreateTable(
                name: "Vpn",
                columns: table => new
                {
                    PK_Vpn = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LocalAddressRange = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LocalPublicIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VirtualNetwork = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VirtualPublicIp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vpn", x => x.PK_Vpn);
                });

            migrationBuilder.CreateTable(
                name: "Subnet",
                columns: table => new
                {
                    PK_VirtualNetwork = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AddressRange = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VirtualNetworkId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subnet", x => x.PK_VirtualNetwork);
                    table.ForeignKey(
                        name: "FK_Subnet_VirtualNetwork_VirtualNetworkId",
                        column: x => x.VirtualNetworkId,
                        principalTable: "VirtualNetwork",
                        principalColumn: "PK_VirtualNetwork",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Base",
                columns: table => new
                {
                    PK_Base = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FK_Credentials = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResourceGroupId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    StorageAccountId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    SubscriptionId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UpdatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VirtualNetworkId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    VpnId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Base", x => x.PK_Base);
                    table.ForeignKey(
                        name: "FK_Base_AzureCredentials_FK_Credentials",
                        column: x => x.FK_Credentials,
                        principalTable: "AzureCredentials",
                        principalColumn: "PK_AzureCredentials",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Base_ResourceGroup_ResourceGroupId",
                        column: x => x.ResourceGroupId,
                        principalTable: "ResourceGroup",
                        principalColumn: "PK_ResourceGroup",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Base_StorageAccount_StorageAccountId",
                        column: x => x.StorageAccountId,
                        principalTable: "StorageAccount",
                        principalColumn: "PK_StorageAccount",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Base_Subscription_SubscriptionId",
                        column: x => x.SubscriptionId,
                        principalTable: "Subscription",
                        principalColumn: "PK_Subscription",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Base_VirtualNetwork_VirtualNetworkId",
                        column: x => x.VirtualNetworkId,
                        principalTable: "VirtualNetwork",
                        principalColumn: "PK_VirtualNetwork",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Base_Vpn_VpnId",
                        column: x => x.VpnId,
                        principalTable: "Vpn",
                        principalColumn: "PK_Vpn",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Parameter",
                columns: table => new
                {
                    PK_Parameter = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BaseId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parameter", x => x.PK_Parameter);
                    table.ForeignKey(
                        name: "FK_Parameter_Base_BaseId",
                        column: x => x.BaseId,
                        principalTable: "Base",
                        principalColumn: "PK_Base",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VirtualMachine",
                columns: table => new
                {
                    PK_VirtualMachine = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AdminUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdminUserPassword = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AzureId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BaseId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OperatingSystem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SystemDiskId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VirtualMachine", x => x.PK_VirtualMachine);
                    table.ForeignKey(
                        name: "FK_VirtualMachine_Base_BaseId",
                        column: x => x.BaseId,
                        principalTable: "Base",
                        principalColumn: "PK_Base",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Disk",
                columns: table => new
                {
                    PK_VirtualMachine = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SizeInGb = table.Column<int>(type: "int", nullable: false),
                    VirtualMachineId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Disk", x => x.PK_VirtualMachine);
                    table.ForeignKey(
                        name: "FK_Disk_VirtualMachine_VirtualMachineId",
                        column: x => x.VirtualMachineId,
                        principalTable: "VirtualMachine",
                        principalColumn: "PK_VirtualMachine",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AzureCredentials_FK_Customer",
                table: "AzureCredentials",
                column: "FK_Customer",
                unique: true,
                filter: "[FK_Customer] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Base_FK_Credentials",
                table: "Base",
                column: "FK_Credentials");

            migrationBuilder.CreateIndex(
                name: "IX_Base_ResourceGroupId",
                table: "Base",
                column: "ResourceGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Base_StorageAccountId",
                table: "Base",
                column: "StorageAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Base_SubscriptionId",
                table: "Base",
                column: "SubscriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Base_VirtualNetworkId",
                table: "Base",
                column: "VirtualNetworkId");

            migrationBuilder.CreateIndex(
                name: "IX_Base_VpnId",
                table: "Base",
                column: "VpnId");

            migrationBuilder.CreateIndex(
                name: "IX_Disk_VirtualMachineId",
                table: "Disk",
                column: "VirtualMachineId");

            migrationBuilder.CreateIndex(
                name: "IX_Parameter_BaseId",
                table: "Parameter",
                column: "BaseId");

            migrationBuilder.CreateIndex(
                name: "IX_Subnet_VirtualNetworkId",
                table: "Subnet",
                column: "VirtualNetworkId");

            migrationBuilder.CreateIndex(
                name: "IX_VirtualMachine_BaseId",
                table: "VirtualMachine",
                column: "BaseId");

            migrationBuilder.CreateIndex(
                name: "IX_VirtualMachine_SystemDiskId",
                table: "VirtualMachine",
                column: "SystemDiskId");

            migrationBuilder.AddForeignKey(
                name: "FK_VirtualMachine_Disk_SystemDiskId",
                table: "VirtualMachine",
                column: "SystemDiskId",
                principalTable: "Disk",
                principalColumn: "PK_VirtualMachine",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Base_ResourceGroup_ResourceGroupId",
                table: "Base");

            migrationBuilder.DropForeignKey(
                name: "FK_Base_StorageAccount_StorageAccountId",
                table: "Base");

            migrationBuilder.DropForeignKey(
                name: "FK_Base_VirtualNetwork_VirtualNetworkId",
                table: "Base");

            migrationBuilder.DropForeignKey(
                name: "FK_Base_Vpn_VpnId",
                table: "Base");

            migrationBuilder.DropForeignKey(
                name: "FK_Disk_VirtualMachine_VirtualMachineId",
                table: "Disk");

            migrationBuilder.DropTable(
                name: "Parameter");

            migrationBuilder.DropTable(
                name: "Subnet");

            migrationBuilder.DropTable(
                name: "ResourceGroup");

            migrationBuilder.DropTable(
                name: "StorageAccount");

            migrationBuilder.DropTable(
                name: "VirtualNetwork");

            migrationBuilder.DropTable(
                name: "Vpn");

            migrationBuilder.DropTable(
                name: "VirtualMachine");

            migrationBuilder.DropTable(
                name: "Base");

            migrationBuilder.DropTable(
                name: "Disk");

            migrationBuilder.DropIndex(
                name: "IX_AzureCredentials_FK_Customer",
                table: "AzureCredentials");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Domain");

            migrationBuilder.DropColumn(
                name: "DeletedByUserId",
                table: "Domain");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "Domain");

            migrationBuilder.DropColumn(
                name: "UpdatedByUserId",
                table: "Domain");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Domain");

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedByUserId",
                table: "User",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DeletedByUserId",
                table: "User",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_DeletedByUserId",
                table: "User",
                column: "DeletedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_User_UpdatedByUserId",
                table: "User",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AzureCredentials_FK_Customer",
                table: "AzureCredentials",
                column: "FK_Customer",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_User_User_DeletedByUserId",
                table: "User",
                column: "DeletedByUserId",
                principalTable: "User",
                principalColumn: "PK_User",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_User_User_UpdatedByUserId",
                table: "User",
                column: "UpdatedByUserId",
                principalTable: "User",
                principalColumn: "PK_User",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
