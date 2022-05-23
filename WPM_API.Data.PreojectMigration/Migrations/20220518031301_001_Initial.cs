using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _001_Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdminDeviceOption",
                columns: table => new
                {
                    PK_AdminDeviceOption = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PEOnly = table.Column<bool>(type: "bit", nullable: false),
                    OSType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminDeviceOption", x => x.PK_AdminDeviceOption);
                });

            migrationBuilder.CreateTable(
                name: "BaseStatus",
                columns: table => new
                {
                    PK_BaseStatus = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResourceGroupStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VirtualNetworkStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StorageAccountStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VPNStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseStatus", x => x.PK_BaseStatus);
                });

            migrationBuilder.CreateTable(
                name: "BiosSettings",
                columns: table => new
                {
                    PK_BiosSettingsId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BiosSettings", x => x.PK_BiosSettingsId);
                });

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    PK_Category = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.PK_Category);
                });

            migrationBuilder.CreateTable(
                name: "Certification",
                columns: table => new
                {
                    PK_Certification = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Certification", x => x.PK_Certification);
                });

            migrationBuilder.CreateTable(
                name: "ChangeLog",
                columns: table => new
                {
                    PK_ChangeLog = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EntityName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PropertyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PrimaryKeyValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OldValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NewValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateChanged = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChangeLog", x => x.PK_ChangeLog);
                });

            migrationBuilder.CreateTable(
                name: "CustomerDriver",
                columns: table => new
                {
                    PK_CustomerDriver = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubFolderPath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConnectionString = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Version = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Vendor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CustomerId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DriverId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerDriver", x => x.PK_CustomerDriver);
                });

            migrationBuilder.CreateTable(
                name: "CustomerHardwareModel",
                columns: table => new
                {
                    PK_CustomerHardwareModel = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Counter = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CustomerId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerHardwareModel", x => x.PK_CustomerHardwareModel);
                });

            migrationBuilder.CreateTable(
                name: "DomainRegistrationTemp",
                columns: table => new
                {
                    PK_DomainRegistrationTemp = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DomainRegistrationTemp", x => x.PK_DomainRegistrationTemp);
                });

            migrationBuilder.CreateTable(
                name: "Driver",
                columns: table => new
                {
                    PK_Driver = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubFolderPath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConnectionString = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Version = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Vendor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ContainerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PublishInShop = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Driver", x => x.PK_Driver);
                });

            migrationBuilder.CreateTable(
                name: "ErrorLog",
                columns: table => new
                {
                    PK_ErrorLog = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Error = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Source = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Data = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ErrorLog", x => x.PK_ErrorLog);
                });

            migrationBuilder.CreateTable(
                name: "Hardware",
                columns: table => new
                {
                    PK_HardwareId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Manufacturer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModelNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SerialNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ServiceTag = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Processor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RAM = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HDD = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChipSet = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DisplayResolution = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TPMChipData = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hardware", x => x.PK_HardwareId);
                });

            migrationBuilder.CreateTable(
                name: "NetworkConfiguration",
                columns: table => new
                {
                    PK_NetworkConfigurationId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IPv4 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IPv6 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DNS = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gateway = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DHCP = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NetworkConfiguration", x => x.PK_NetworkConfigurationId);
                });

            migrationBuilder.CreateTable(
                name: "OS",
                columns: table => new
                {
                    PK_OSId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Uefi = table.Column<bool>(type: "bit", nullable: false),
                    OSVersion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LanguagePackage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    KeyboardLayout = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OS", x => x.PK_OSId);
                });

            migrationBuilder.CreateTable(
                name: "Purchase",
                columns: table => new
                {
                    PK_PurchaseId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PurchaseDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CostUnitAssignment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Vendor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AcquisitionCost = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DecommissioningDate = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Purchase", x => x.PK_PurchaseId);
                });

            migrationBuilder.CreateTable(
                name: "RevisionMessage",
                columns: table => new
                {
                    PK_RevisionMessage = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SoftwareId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DisplayRevisionNumber = table.Column<int>(type: "int", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ImageId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RevisionMessage", x => x.PK_RevisionMessage);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    PK_Role = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.PK_Role);
                });

            migrationBuilder.CreateTable(
                name: "RuleType",
                columns: table => new
                {
                    PK_RuleType = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RuleType", x => x.PK_RuleType);
                });

            migrationBuilder.CreateTable(
                name: "Script",
                columns: table => new
                {
                    PK_Script = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    AuthorType = table.Column<int>(type: "int", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    showToCustomer = table.Column<bool>(type: "bit", nullable: false),
                    PEOnly = table.Column<bool>(type: "bit", nullable: false),
                    OSType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Script", x => x.PK_Script);
                });

            migrationBuilder.CreateTable(
                name: "ShopItem",
                columns: table => new
                {
                    PK_ShopItem = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescriptionShort = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ManagedServicePrice = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ManagedServiceLifecyclePrice = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopItem", x => x.PK_ShopItem);
                });

            migrationBuilder.CreateTable(
                name: "Systemhouse",
                columns: table => new
                {
                    PK_Systemhouse = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Deletable = table.Column<bool>(type: "bit", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Systemhouse", x => x.PK_Systemhouse);
                });

            migrationBuilder.CreateTable(
                name: "VendorModel",
                columns: table => new
                {
                    PK_VendorModel = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModelFamily = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModelType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VendorModel", x => x.PK_VendorModel);
                });

            migrationBuilder.CreateTable(
                name: "VirtualNetwork",
                columns: table => new
                {
                    PK_VirtualNetwork = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AddressRange = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Dns = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AzureId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
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
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LocalAddressRange = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LocalPublicIp = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VirtualNetwork = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VirtualPublicIp = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SharedKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vpn", x => x.PK_Vpn);
                });

            migrationBuilder.CreateTable(
                name: "Workflow",
                columns: table => new
                {
                    PK_Workflow = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workflow", x => x.PK_Workflow);
                });

            migrationBuilder.CreateTable(
                name: "Bios",
                columns: table => new
                {
                    PK_BiosId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Manufacturer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InstalledVersion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ManufacturerVersion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InternalVersion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BiosSettingsId = table.Column<string>(type: "nvarchar(450)", nullable: true)
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

            migrationBuilder.CreateTable(
                name: "ClientProperty",
                columns: table => new
                {
                    PK_ClientProperty = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PropertyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoryId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Command = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParameterName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientProperty", x => x.PK_ClientProperty);
                    table.ForeignKey(
                        name: "FK_ClientProperty_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "PK_Category",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ScriptVersion",
                columns: table => new
                {
                    PK_ScriptVersion = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Number = table.Column<int>(type: "int", nullable: false),
                    ContentUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdminDeviceOptionId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ScriptId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScriptVersion", x => x.PK_ScriptVersion);
                    table.ForeignKey(
                        name: "FK_ScriptVersion_AdminDeviceOption_AdminDeviceOptionId",
                        column: x => x.AdminDeviceOptionId,
                        principalTable: "AdminDeviceOption",
                        principalColumn: "PK_AdminDeviceOption",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ScriptVersion_Script_ScriptId",
                        column: x => x.ScriptId,
                        principalTable: "Script",
                        principalColumn: "PK_Script",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DriverShopItem",
                columns: table => new
                {
                    PK_DriverShopItem = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DriverId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ShopItemId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DriverShopItem", x => x.PK_DriverShopItem);
                    table.ForeignKey(
                        name: "FK_DriverShopItem_Driver_DriverId",
                        column: x => x.DriverId,
                        principalTable: "Driver",
                        principalColumn: "PK_Driver",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DriverShopItem_ShopItem_ShopItemId",
                        column: x => x.ShopItemId,
                        principalTable: "ShopItem",
                        principalColumn: "PK_ShopItem",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ShopItemCategory",
                columns: table => new
                {
                    PK_ShopItemCategory = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CategoryId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ShopItemId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopItemCategory", x => x.PK_ShopItemCategory);
                    table.ForeignKey(
                        name: "FK_ShopItemCategory_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "PK_Category",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ShopItemCategory_ShopItem_ShopItemId",
                        column: x => x.ShopItemId,
                        principalTable: "ShopItem",
                        principalColumn: "PK_ShopItem",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Subnet",
                columns: table => new
                {
                    PK_Subnet = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AddressRange = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Number = table.Column<int>(type: "int", nullable: false),
                    VirtualNetworkId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subnet", x => x.PK_Subnet);
                    table.ForeignKey(
                        name: "FK_Subnet_VirtualNetwork_VirtualNetworkId",
                        column: x => x.VirtualNetworkId,
                        principalTable: "VirtualNetwork",
                        principalColumn: "PK_VirtualNetwork",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ActivityLog",
                columns: table => new
                {
                    PK_ActivityLog = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClientId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityLog", x => x.PK_ActivityLog);
                });

            migrationBuilder.CreateTable(
                name: "AdvancedProperty",
                columns: table => new
                {
                    PK_AdvancedProperty = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BaseId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    isEditable = table.Column<bool>(type: "bit", nullable: false),
                    CategoryId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdvancedProperty", x => x.PK_AdvancedProperty);
                    table.ForeignKey(
                        name: "FK_AdvancedProperty_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "PK_Category",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Architecture",
                columns: table => new
                {
                    PK_Architecture = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Version = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RuleId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Architecture", x => x.PK_Architecture);
                });

            migrationBuilder.CreateTable(
                name: "AssetClass",
                columns: table => new
                {
                    PK_AssetClass = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AssetTypeId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CustomerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    fromAdmin = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetClass", x => x.PK_AssetClass);
                });

            migrationBuilder.CreateTable(
                name: "AssetModel",
                columns: table => new
                {
                    PK_AssetModel = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AssetID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AssetStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Building = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Floor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Room = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Coordinates = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AssetClassId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AssetTypeId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    InvoiceId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PurchaseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PurchaseValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CINumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PersonId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LocationId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClientId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DepreciationMonths = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VendorModelId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetModel", x => x.PK_AssetModel);
                    table.ForeignKey(
                        name: "FK_AssetModel_AssetClass_AssetClassId",
                        column: x => x.AssetClassId,
                        principalTable: "AssetClass",
                        principalColumn: "PK_AssetClass",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AssetModel_VendorModel_VendorModelId",
                        column: x => x.VendorModelId,
                        principalTable: "VendorModel",
                        principalColumn: "PK_VendorModel",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AssetType",
                columns: table => new
                {
                    PK_AssetType = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    fromAdmin = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetType", x => x.PK_AssetType);
                });

            migrationBuilder.CreateTable(
                name: "Attachment",
                columns: table => new
                {
                    PK_Attachment = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    GenFileName = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachment", x => x.PK_Attachment);
                });

            migrationBuilder.CreateTable(
                name: "AzureBlobStorage",
                columns: table => new
                {
                    PK_AzureBlob = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StorageAccountId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RessourceGroupId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CustomerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SubscriptionId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AzureBlobStorage", x => x.PK_AzureBlob);
                });

            migrationBuilder.CreateTable(
                name: "Base",
                columns: table => new
                {
                    PK_Base = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CredentialsId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubscriptionId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ResourceGroupId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    StorageAccountId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    VirtualNetworkId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    VpnId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BaseStatusId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Base", x => x.PK_Base);
                    table.ForeignKey(
                        name: "FK_Base_BaseStatus_BaseStatusId",
                        column: x => x.BaseStatusId,
                        principalTable: "BaseStatus",
                        principalColumn: "PK_BaseStatus",
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
                name: "VirtualMachine",
                columns: table => new
                {
                    PK_VirtualMachine = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AzureId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Subnet = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OperatingSystem = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdminUserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdminUserPassword = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LocalIp = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BaseId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CurrentCustomerId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubscriptionName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubscriptionId = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
                    PK_Disk = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SizeInGb = table.Column<int>(type: "int", nullable: false),
                    DiskType = table.Column<int>(type: "int", nullable: false),
                    VirtualMachineId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Disk", x => x.PK_Disk);
                    table.ForeignKey(
                        name: "FK_Disk_VirtualMachine_VirtualMachineId",
                        column: x => x.VirtualMachineId,
                        principalTable: "VirtualMachine",
                        principalColumn: "PK_VirtualMachine",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BIOSModel",
                columns: table => new
                {
                    PK_HardwareModel = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Vendor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Version = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ValidOS = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReleaseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ContentId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ReadMeId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BIOSModel", x => x.PK_HardwareModel);
                });

            migrationBuilder.CreateTable(
                name: "Client",
                columns: table => new
                {
                    PK_Client = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UUID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AssetId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WdsIp = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Unattend = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    OrganizationalUnitId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Vendor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UsageStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OsId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    BiosId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    HardwareId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    NetworkId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PurchaseId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    JoinedDomain = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Proxy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InstallationDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MainUser = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Subnet = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CloudFlag = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BaseId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LastInventoryUpdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InventoryId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AssetModelId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SerialNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HyperVisor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Manufacturer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsOnline = table.Column<bool>(type: "bit", nullable: false),
                    LastOnlineStatusUpdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OSSettingsImageId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UsernameLinux = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserPasswordLinux = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdminPasswordLinux = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PartitionEncryptionPassLinux = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    KeyboardLayoutWindows = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TimeZoneWindows = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TimeZoneLinux = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    KeyboardLayoutLinux = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OSEdition = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OSMemorySize = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OSOperatingSystemSKU = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OSArchitecture = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CSPvendor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OSInstallDateUTC = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OSType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OSVersion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModelSeries = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CSPversion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CSPname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OSLanguage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OSProductSuite = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Processor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MainFrequentUser = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DownloadSeedURL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LanguagePackLinux = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Timezone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InstallScript = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BaseLineFile1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BaseLineFile2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BaseLineFile3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OSTypeDevice = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LocalAdminUsername = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LocalAdminPassword = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Client", x => x.PK_Client);
                    table.ForeignKey(
                        name: "FK_Client_Base_BaseId",
                        column: x => x.BaseId,
                        principalTable: "Base",
                        principalColumn: "PK_Base",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Client_Bios_BiosId",
                        column: x => x.BiosId,
                        principalTable: "Bios",
                        principalColumn: "PK_BiosId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Client_Hardware_HardwareId",
                        column: x => x.HardwareId,
                        principalTable: "Hardware",
                        principalColumn: "PK_HardwareId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Client_NetworkConfiguration_NetworkId",
                        column: x => x.NetworkId,
                        principalTable: "NetworkConfiguration",
                        principalColumn: "PK_NetworkConfigurationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Client_OS_OsId",
                        column: x => x.OsId,
                        principalTable: "OS",
                        principalColumn: "PK_OSId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Client_Purchase_PurchaseId",
                        column: x => x.PurchaseId,
                        principalTable: "Purchase",
                        principalColumn: "PK_PurchaseId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ClientClientProperty",
                columns: table => new
                {
                    PK_ClientClientProperty = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClientId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClientPropertyId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientClientProperty", x => x.PK_ClientClientProperty);
                    table.ForeignKey(
                        name: "FK_ClientClientProperty_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "PK_Client",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClientClientProperty_ClientProperty_ClientPropertyId",
                        column: x => x.ClientPropertyId,
                        principalTable: "ClientProperty",
                        principalColumn: "PK_ClientProperty",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ClientOption",
                columns: table => new
                {
                    PK_ClientOption = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClientId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DeviceOptionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PEOnly = table.Column<bool>(type: "bit", nullable: false),
                    OSType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientOption", x => x.PK_ClientOption);
                    table.ForeignKey(
                        name: "FK_ClientOption_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "PK_Client",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClientOption_ScriptVersion_DeviceOptionId",
                        column: x => x.DeviceOptionId,
                        principalTable: "ScriptVersion",
                        principalColumn: "PK_ScriptVersion",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ClientParameter",
                columns: table => new
                {
                    PK_ClientParameter = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PropertyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParameterName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClientId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsEditable = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientParameter", x => x.PK_ClientParameter);
                    table.ForeignKey(
                        name: "FK_ClientParameter_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "PK_Client",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HDDPartition",
                columns: table => new
                {
                    PK_HDDPartitionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PartitionNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    isGpt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DriveLetter = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SizeInBytes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Overprovisioning = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClientId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HDDPartition", x => x.PK_HDDPartitionId);
                    table.ForeignKey(
                        name: "FK_HDDPartition_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "PK_Client",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Inventory",
                columns: table => new
                {
                    PK_Inventory = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClientId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OperationType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventory", x => x.PK_Inventory);
                    table.ForeignKey(
                        name: "FK_Inventory_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "PK_Client",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MacAddress",
                columns: table => new
                {
                    PK_MacAddress = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClientId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MacAddress", x => x.PK_MacAddress);
                    table.ForeignKey(
                        name: "FK_MacAddress_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "PK_Client",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PreinstalledSoftware",
                columns: table => new
                {
                    PK_PreinstalledSoftware = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Size = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Version = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InstalledAt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClientId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreinstalledSoftware", x => x.PK_PreinstalledSoftware);
                    table.ForeignKey(
                        name: "FK_PreinstalledSoftware_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "PK_Client",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WMIInvenotryCmds",
                columns: table => new
                {
                    PK_WMIInventoryCmds = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Command = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClientId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WMIInvenotryCmds", x => x.PK_WMIInventoryCmds);
                    table.ForeignKey(
                        name: "FK_WMIInvenotryCmds_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "PK_Client",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ClientSoftware",
                columns: table => new
                {
                    PK_ClientSoftware = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClientId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CustomerSoftwareId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Install = table.Column<bool>(type: "bit", nullable: true),
                    RunningContext = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RevisionNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AllWin10Versions = table.Column<bool>(type: "bit", nullable: false),
                    AllWin11Versions = table.Column<bool>(type: "bit", nullable: false),
                    SoftwareId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientSoftware", x => x.PK_ClientSoftware);
                    table.ForeignKey(
                        name: "FK_ClientSoftware_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "PK_Client",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ClientTask",
                columns: table => new
                {
                    PK_ClientTask = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClientId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TaskId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientTask", x => x.PK_ClientTask);
                    table.ForeignKey(
                        name: "FK_ClientTask_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "PK_Client",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CloudEntryPoint",
                columns: table => new
                {
                    PK_CloudEntryPoint = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClientSecret = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClientId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CustomerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsStandard = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CloudEntryPoint", x => x.PK_CloudEntryPoint);
                });

            migrationBuilder.CreateTable(
                name: "Company",
                columns: table => new
                {
                    PK_Company = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CorporateName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FormOfOrganization = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LinkWebsite = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpertId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CustomerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    HeadquarterId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company", x => x.PK_Company);
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    PK_Customer = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SystemhouseId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Deletable = table.Column<bool>(type: "bit", nullable: false),
                    MainCompanyId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IconRightId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IconLeftId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BannerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OpeningTimes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CmdBtn1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CmdBtn2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CmdBtn3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CmdBtn4 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Btn1Label = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Btn2Label = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Btn3Label = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Btn4Label = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CsdpRoot = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CsdpContainer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LtSASRead = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LtSASWríte = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LtSASExpireDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    WinPEDownloadLink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BannerLink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AutoRegisterPassword = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AutoRegisterClients = table.Column<bool>(type: "bit", nullable: false),
                    OfficeConfig = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UseCustomConfig = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.PK_Customer);
                    table.ForeignKey(
                        name: "FK_Customer_Company_MainCompanyId",
                        column: x => x.MainCompanyId,
                        principalTable: "Company",
                        principalColumn: "PK_Company",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Customer_Systemhouse_SystemhouseId",
                        column: x => x.SystemhouseId,
                        principalTable: "Systemhouse",
                        principalColumn: "PK_Systemhouse",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Default",
                columns: table => new
                {
                    PK_Default = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CustomerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Default", x => x.PK_Default);
                    table.ForeignKey(
                        name: "FK_Default_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "PK_Customer",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Location",
                columns: table => new
                {
                    PK_Location = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameAbbreviation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CityAbbreviation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CountryAbbreviation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TimeZone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Number = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Postcode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PublicIP = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UploadSpeed = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DownloadSpeed = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AzureLocation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CompanyId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Location", x => x.PK_Location);
                    table.ForeignKey(
                        name: "FK_Location_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "PK_Company",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Location_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "PK_Customer",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Person",
                columns: table => new
                {
                    PK_Person = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GivenName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MiddleName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AcademicDegree = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmployeeType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CostCenter = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FaxNr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MobileNr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmailPrimary = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmailOptional = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Domain = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DepartementName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DepartementShort = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoomNr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmployeeNr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CustomerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Person", x => x.PK_Person);
                    table.ForeignKey(
                        name: "FK_Person_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "PK_Company",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Person_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "PK_Customer",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReleasePlan",
                columns: table => new
                {
                    PK_ReleasePlan = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CustomerId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReleasePlan", x => x.PK_ReleasePlan);
                    table.ForeignKey(
                        name: "FK_ReleasePlan_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "PK_Customer",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ResourceGroup",
                columns: table => new
                {
                    PK_ResourceGroup = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AzureSubscriptionId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResourceGroup", x => x.PK_ResourceGroup);
                    table.ForeignKey(
                        name: "FK_ResourceGroup_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "PK_Customer",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StorageEntryPoint",
                columns: table => new
                {
                    PK_StorageEntryPoint = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ResourceGrpName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubscriptionId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsCSDP = table.Column<bool>(type: "bit", nullable: false),
                    StorageAccount = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StorageAccountType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BlobContainerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Kind = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Managed = table.Column<bool>(type: "bit", nullable: false),
                    CustomerId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StorageEntryPoint", x => x.PK_StorageEntryPoint);
                    table.ForeignKey(
                        name: "FK_StorageEntryPoint_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "PK_Customer",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Subscription",
                columns: table => new
                {
                    PK_Subscription = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AzureId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CustomerId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscription", x => x.PK_Subscription);
                    table.ForeignKey(
                        name: "FK_Subscription_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "PK_Customer",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Token",
                columns: table => new
                {
                    PK_Token = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CustomerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Hash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ValidTo = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Token", x => x.PK_Token);
                    table.ForeignKey(
                        name: "FK_Token_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "PK_Customer",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    PK_User = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Login = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CustomerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SystemhouseId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Deletable = table.Column<bool>(type: "bit", nullable: false),
                    UpdatedByUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.PK_User);
                    table.ForeignKey(
                        name: "FK_User_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "PK_Customer",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_User_Systemhouse_SystemhouseId",
                        column: x => x.SystemhouseId,
                        principalTable: "Systemhouse",
                        principalColumn: "PK_Systemhouse",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_User_User_DeletedByUserId",
                        column: x => x.DeletedByUserId,
                        principalTable: "User",
                        principalColumn: "PK_User",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_User_User_UpdatedByUserId",
                        column: x => x.UpdatedByUserId,
                        principalTable: "User",
                        principalColumn: "PK_User",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StorageAccount",
                columns: table => new
                {
                    PK_StorageAccount = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AzureId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ResourceGroupId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StorageAccount", x => x.PK_StorageAccount);
                    table.ForeignKey(
                        name: "FK_StorageAccount_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "PK_Customer",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StorageAccount_ResourceGroup_ResourceGroupId",
                        column: x => x.ResourceGroupId,
                        principalTable: "ResourceGroup",
                        principalColumn: "PK_ResourceGroup",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExecutionLog",
                columns: table => new
                {
                    PK_ExecutionLog = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    VirtualMachineId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClientId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ExecutionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Script = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ScriptVersionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExecutionLog", x => x.PK_ExecutionLog);
                    table.ForeignKey(
                        name: "FK_ExecutionLog_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "PK_Client",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExecutionLog_ScriptVersion_ScriptVersionId",
                        column: x => x.ScriptVersionId,
                        principalTable: "ScriptVersion",
                        principalColumn: "PK_ScriptVersion",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExecutionLog_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "PK_User",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExecutionLog_VirtualMachine_VirtualMachineId",
                        column: x => x.VirtualMachineId,
                        principalTable: "VirtualMachine",
                        principalColumn: "PK_VirtualMachine",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    PK_Order = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.PK_Order);
                    table.ForeignKey(
                        name: "FK_Order_User_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "User",
                        principalColumn: "PK_User",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Scheduler",
                columns: table => new
                {
                    PK_Scheduler = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SchedulerActionType = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ParentSchedulerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    OnDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsSynchronous = table.Column<bool>(type: "bit", nullable: false),
                    StartProcessDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndProcessDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EntityId1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EntityId2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EntityId3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EntityId4 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EntityData1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EntityData2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EntityData3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EntityData4 = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scheduler", x => x.PK_Scheduler);
                    table.ForeignKey(
                        name: "FK_Scheduler_Scheduler_ParentSchedulerId",
                        column: x => x.ParentSchedulerId,
                        principalTable: "Scheduler",
                        principalColumn: "PK_Scheduler",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Scheduler_User_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "User",
                        principalColumn: "PK_User",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserCustomer",
                columns: table => new
                {
                    PK_UserCustomer = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CustomerId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCustomer", x => x.PK_UserCustomer);
                    table.ForeignKey(
                        name: "FK_UserCustomer_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "PK_Customer",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserCustomer_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "PK_User",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserForgotPassword",
                columns: table => new
                {
                    PK_UserForgotPassword = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RequestGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorIpAddress = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    ApprovedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApproverIpAddress = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserForgotPassword", x => x.PK_UserForgotPassword);
                    table.ForeignKey(
                        name: "FK_UserForgotPassword_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "PK_User",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                columns: table => new
                {
                    PK_UserRole = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => x.PK_UserRole);
                    table.ForeignKey(
                        name: "FK_UserRole_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "PK_Role",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserRole_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "PK_User",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserSubscription",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SubscriptionId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSubscription", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSubscription_Subscription_SubscriptionId",
                        column: x => x.SubscriptionId,
                        principalTable: "Subscription",
                        principalColumn: "PK_Subscription",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserSubscription_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "PK_User",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Parameter",
                columns: table => new
                {
                    PK_Parameter = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsEditable = table.Column<bool>(type: "bit", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CustomerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClientOptionId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ExecutionLogId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parameter", x => x.PK_Parameter);
                    table.ForeignKey(
                        name: "FK_Parameter_ClientOption_ClientOptionId",
                        column: x => x.ClientOptionId,
                        principalTable: "ClientOption",
                        principalColumn: "PK_ClientOption",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Parameter_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "PK_Customer",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Parameter_ExecutionLog_ExecutionLogId",
                        column: x => x.ExecutionLogId,
                        principalTable: "ExecutionLog",
                        principalColumn: "PK_ExecutionLog",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderShopItem",
                columns: table => new
                {
                    PK_OrderShopItem = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    OrderId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ShopItemId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderShopItem", x => x.PK_OrderShopItem);
                    table.ForeignKey(
                        name: "FK_OrderShopItem_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "PK_Order",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderShopItem_ShopItem_ShopItemId",
                        column: x => x.ShopItemId,
                        principalTable: "ShopItem",
                        principalColumn: "PK_ShopItem",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NotificationEmail",
                columns: table => new
                {
                    PK_NotificationEmail = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SchedulerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProcessedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Subject = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ToEmailAddresses = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ToCcEmailAddresses = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ToBccEmailAddresses = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AttemptsCount = table.Column<int>(type: "int", nullable: false),
                    LastAttemptDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastAttemptError = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationEmail", x => x.PK_NotificationEmail);
                    table.ForeignKey(
                        name: "FK_NotificationEmail_Scheduler_SchedulerId",
                        column: x => x.SchedulerId,
                        principalTable: "Scheduler",
                        principalColumn: "PK_Scheduler",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NotificationEmailAttachment",
                columns: table => new
                {
                    PK_NotificationEmailAttachment = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NotificationEmailId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AttachmentId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationEmailAttachment", x => x.PK_NotificationEmailAttachment);
                    table.ForeignKey(
                        name: "FK_NotificationEmailAttachment_Attachment_AttachmentId",
                        column: x => x.AttachmentId,
                        principalTable: "Attachment",
                        principalColumn: "PK_Attachment",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NotificationEmailAttachment_NotificationEmail_NotificationEmailId",
                        column: x => x.NotificationEmailId,
                        principalTable: "NotificationEmail",
                        principalColumn: "PK_NotificationEmail",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CustomerImage",
                columns: table => new
                {
                    PK_CustomerImage = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Update = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BuildNr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PublishInShop = table.Column<bool>(type: "bit", nullable: false),
                    CustomerId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UnattendId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    OEMPartitionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CustomerImageStreamId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RevisionNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DisplayRevisionNumber = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerImage", x => x.PK_CustomerImage);
                });

            migrationBuilder.CreateTable(
                name: "CustomerImageStream",
                columns: table => new
                {
                    PK_ImageStream = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Vendor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescriptionShort = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Architecture = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Website = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IconId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CustomerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ImageStreamId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubFolderName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Edition = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PrefixUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SASKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LocalSettingLinux = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerImageStream", x => x.PK_ImageStream);
                    table.ForeignKey(
                        name: "FK_CustomerImageStream_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "PK_Customer",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CustomerSoftware",
                columns: table => new
                {
                    PK_CustomerSoftware = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Version = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RuleDetectionId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RuleApplicabilityId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TaskInstallId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TaskUninstallId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TaskUpdateId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InstallationTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PackageSize = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Prerequisites = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VendorReleaseDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompliancyRule = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Checksum = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerSoftwareStreamId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SoftwareId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RunningContext = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NextSoftwareId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PrevSoftwareId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MinimalSoftwareId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DedicatedDownloadLink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RevisionNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DisplayRevisionNumber = table.Column<int>(type: "int", nullable: false),
                    AllWin10Versions = table.Column<bool>(type: "bit", nullable: false),
                    AllWin11Versions = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerSoftware", x => x.PK_CustomerSoftware);
                });

            migrationBuilder.CreateTable(
                name: "CustomerSoftwareStream",
                columns: table => new
                {
                    PK_CustomerSoftwareStream = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdateSettings = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CustomerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SoftwareStreamId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Vendor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescriptionShort = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GnuLicence = table.Column<bool>(type: "bit", nullable: false),
                    Architecture = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Website = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DownloadLink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApplicationOwnerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ReleasePlanId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IconId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsEnterpriseStream = table.Column<bool>(type: "bit", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    ClientOrServer = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerSoftwareStream", x => x.PK_CustomerSoftwareStream);
                    table.ForeignKey(
                        name: "FK_CustomerSoftwareStream_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "PK_Customer",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerSoftwareStream_Person_ApplicationOwnerId",
                        column: x => x.ApplicationOwnerId,
                        principalTable: "Person",
                        principalColumn: "PK_Person",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerSoftwareStream_ReleasePlan_ReleasePlanId",
                        column: x => x.ReleasePlanId,
                        principalTable: "ReleasePlan",
                        principalColumn: "PK_ReleasePlan",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DNS",
                columns: table => new
                {
                    PK_DNS = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Forwarder = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DomainId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DNS", x => x.PK_DNS);
                });

            migrationBuilder.CreateTable(
                name: "Domain",
                columns: table => new
                {
                    PK_Domain = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CustomerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BaseId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tld = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GpoId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExecutionVMId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Office365ConfigurationXMLId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    DomainUserCSVId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Domain", x => x.PK_Domain);
                    table.ForeignKey(
                        name: "FK_Domain_Base_BaseId",
                        column: x => x.BaseId,
                        principalTable: "Base",
                        principalColumn: "PK_Base",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Domain_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "PK_Customer",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DomainUser",
                columns: table => new
                {
                    PK_DomainUser = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserGivenName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserLastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SamAccountName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserPrincipalName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MemberOf = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Displayname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Workphone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DomainId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DomainUser", x => x.PK_DomainUser);
                    table.ForeignKey(
                        name: "FK_DomainUser_Domain_DomainId",
                        column: x => x.DomainId,
                        principalTable: "Domain",
                        principalColumn: "PK_Domain",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrganizationalUnit",
                columns: table => new
                {
                    PK_OrganizationalUnit = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsLeaf = table.Column<bool>(type: "bit", nullable: false),
                    DomainId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    OrganizationalUnitId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationalUnit", x => x.PK_OrganizationalUnit);
                    table.ForeignKey(
                        name: "FK_OrganizationalUnit_Domain_DomainId",
                        column: x => x.DomainId,
                        principalTable: "Domain",
                        principalColumn: "PK_Domain",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrganizationalUnit_OrganizationalUnit_OrganizationalUnitId",
                        column: x => x.OrganizationalUnitId,
                        principalTable: "OrganizationalUnit",
                        principalColumn: "PK_OrganizationalUnit",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Server",
                columns: table => new
                {
                    PK_Server = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DomainId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    VirtualMachineId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    OrganizationalUnitId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Server", x => x.PK_Server);
                    table.ForeignKey(
                        name: "FK_Server_Domain_DomainId",
                        column: x => x.DomainId,
                        principalTable: "Domain",
                        principalColumn: "PK_Domain",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Server_OrganizationalUnit_OrganizationalUnitId",
                        column: x => x.OrganizationalUnitId,
                        principalTable: "OrganizationalUnit",
                        principalColumn: "PK_OrganizationalUnit",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Server_VirtualMachine_VirtualMachineId",
                        column: x => x.VirtualMachineId,
                        principalTable: "VirtualMachine",
                        principalColumn: "PK_VirtualMachine",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "File",
                columns: table => new
                {
                    PK_File = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Guid = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TaskId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CustomerHardwareModelId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ScriptVersionId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ShopItemId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    VendorModelId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_File", x => x.PK_File);
                    table.ForeignKey(
                        name: "FK_File_CustomerHardwareModel_CustomerHardwareModelId",
                        column: x => x.CustomerHardwareModelId,
                        principalTable: "CustomerHardwareModel",
                        principalColumn: "PK_CustomerHardwareModel",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_File_ScriptVersion_ScriptVersionId",
                        column: x => x.ScriptVersionId,
                        principalTable: "ScriptVersion",
                        principalColumn: "PK_ScriptVersion",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_File_ShopItem_ShopItemId",
                        column: x => x.ShopItemId,
                        principalTable: "ShopItem",
                        principalColumn: "PK_ShopItem",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_File_VendorModel_VendorModelId",
                        column: x => x.VendorModelId,
                        principalTable: "VendorModel",
                        principalColumn: "PK_VendorModel",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GroupPolicyObject",
                columns: table => new
                {
                    PK_GPO = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BsiCertified = table.Column<bool>(type: "bit", nullable: false),
                    WallpaperId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LockscreenId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Settings = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupPolicyObject", x => x.PK_GPO);
                    table.ForeignKey(
                        name: "FK_GroupPolicyObject_File_LockscreenId",
                        column: x => x.LockscreenId,
                        principalTable: "File",
                        principalColumn: "PK_File",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GroupPolicyObject_File_WallpaperId",
                        column: x => x.WallpaperId,
                        principalTable: "File",
                        principalColumn: "PK_File",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ImageStream",
                columns: table => new
                {
                    PK_ImageStream = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Vendor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescriptionShort = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Architecture = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Website = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IconId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    SubFolderName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Edition = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PrefixUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SASKey = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageStream", x => x.PK_ImageStream);
                    table.ForeignKey(
                        name: "FK_ImageStream_File_IconId",
                        column: x => x.IconId,
                        principalTable: "File",
                        principalColumn: "PK_File",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OSModel",
                columns: table => new
                {
                    PK_OSModel = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Vendor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OSName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Architecture = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OSType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Version = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReleaseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SupportEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ContentId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
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

            migrationBuilder.CreateTable(
                name: "Rule",
                columns: table => new
                {
                    PK_Rule = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TypeId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    DataId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Successon = table.Column<bool>(type: "bit", nullable: false),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    VersionNr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CheckVersionNr = table.Column<bool>(type: "bit", nullable: false),
                    OsType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rule", x => x.PK_Rule);
                    table.ForeignKey(
                        name: "FK_Rule_File_DataId",
                        column: x => x.DataId,
                        principalTable: "File",
                        principalColumn: "PK_File",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Rule_RuleType_TypeId",
                        column: x => x.TypeId,
                        principalTable: "RuleType",
                        principalColumn: "PK_RuleType",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SoftwareStream",
                columns: table => new
                {
                    PK_SoftwareStream = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdateSettings = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Vendor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescriptionShort = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GnuLicence = table.Column<bool>(type: "bit", nullable: false),
                    Architecture = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Website = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DownloadLink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IconId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoftwareStream", x => x.PK_SoftwareStream);
                    table.ForeignKey(
                        name: "FK_SoftwareStream_File_IconId",
                        column: x => x.IconId,
                        principalTable: "File",
                        principalColumn: "PK_File",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Task",
                columns: table => new
                {
                    PK_Task = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescriptionShort = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CommandLine = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SuccessValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EstimatedExecutionTime = table.Column<int>(type: "int", nullable: false),
                    UseShellExecute = table.Column<bool>(type: "bit", nullable: false),
                    BlockFilePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExecutionFileId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    VersionNr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CheckVersionNr = table.Column<bool>(type: "bit", nullable: false),
                    ExePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExecutionContext = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Visibility = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RestartRequired = table.Column<bool>(type: "bit", nullable: false),
                    RunningContext = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InstallationType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Task", x => x.PK_Task);
                    table.ForeignKey(
                        name: "FK_Task_File_ExecutionFileId",
                        column: x => x.ExecutionFileId,
                        principalTable: "File",
                        principalColumn: "PK_File",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Image",
                columns: table => new
                {
                    PK_Image = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Update = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BuildNr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PublishInShop = table.Column<bool>(type: "bit", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UnattendId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    OEMPartitionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ImageStreamId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RevisionNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DisplayRevisionNumber = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Image", x => x.PK_Image);
                    table.ForeignKey(
                        name: "FK_Image_File_OEMPartitionId",
                        column: x => x.OEMPartitionId,
                        principalTable: "File",
                        principalColumn: "PK_File",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Image_File_UnattendId",
                        column: x => x.UnattendId,
                        principalTable: "File",
                        principalColumn: "PK_File",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Image_ImageStream_ImageStreamId",
                        column: x => x.ImageStreamId,
                        principalTable: "ImageStream",
                        principalColumn: "PK_ImageStream",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HardwareModel",
                columns: table => new
                {
                    PK_HardwareModel = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Vendor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModelFamily = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModelType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductionStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProductionEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BIOSModelId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    OSModelId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HardwareModel", x => x.PK_HardwareModel);
                    table.ForeignKey(
                        name: "FK_HardwareModel_BIOSModel_BIOSModelId",
                        column: x => x.BIOSModelId,
                        principalTable: "BIOSModel",
                        principalColumn: "PK_HardwareModel",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HardwareModel_OSModel_OSModelId",
                        column: x => x.OSModelId,
                        principalTable: "OSModel",
                        principalColumn: "PK_OSModel",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OsVersionName",
                columns: table => new
                {
                    PK_OSVersion = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Version = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RuleId = table.Column<string>(type: "nvarchar(450)", nullable: true)
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
                    PK_Win10Version = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Version = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RuleId = table.Column<string>(type: "nvarchar(450)", nullable: true)
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

            migrationBuilder.CreateTable(
                name: "Win11Version",
                columns: table => new
                {
                    PK_Win11Version = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Version = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RuleId = table.Column<string>(type: "nvarchar(450)", nullable: true)
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

            migrationBuilder.CreateTable(
                name: "Software",
                columns: table => new
                {
                    PK_Software = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Version = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RuleDetectionId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RuleApplicabilityId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TaskInstallId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TaskUninstallId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TaskUpdateId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InstallationTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PackageSize = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Prerequisites = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VendorReleaseDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompliancyRule = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Checksum = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RunningContext = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SoftwareStreamId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PublishInShop = table.Column<bool>(type: "bit", nullable: false),
                    NextSoftwareId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PrevSoftwareId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MinimalSoftwareId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DedicatedDownloadLink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RevisionNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DisplayRevisionNumber = table.Column<int>(type: "int", nullable: false),
                    AllWin10Versions = table.Column<bool>(type: "bit", nullable: false),
                    AllWin11Versions = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Software", x => x.PK_Software);
                    table.ForeignKey(
                        name: "FK_Software_Rule_RuleApplicabilityId",
                        column: x => x.RuleApplicabilityId,
                        principalTable: "Rule",
                        principalColumn: "PK_Rule",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Software_Rule_RuleDetectionId",
                        column: x => x.RuleDetectionId,
                        principalTable: "Rule",
                        principalColumn: "PK_Rule",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Software_SoftwareStream_SoftwareStreamId",
                        column: x => x.SoftwareStreamId,
                        principalTable: "SoftwareStream",
                        principalColumn: "PK_SoftwareStream",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Software_Task_TaskInstallId",
                        column: x => x.TaskInstallId,
                        principalTable: "Task",
                        principalColumn: "PK_Task",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Software_Task_TaskUninstallId",
                        column: x => x.TaskUninstallId,
                        principalTable: "Task",
                        principalColumn: "PK_Task",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Software_Task_TaskUpdateId",
                        column: x => x.TaskUpdateId,
                        principalTable: "Task",
                        principalColumn: "PK_Task",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ImagesClient",
                columns: table => new
                {
                    PK_SoftwaresClient = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ImageId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClientId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImagesClient", x => x.PK_SoftwaresClient);
                    table.ForeignKey(
                        name: "FK_ImagesClient_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "PK_Client",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ImagesClient_Image_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Image",
                        principalColumn: "PK_Image",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ImagesCustomer",
                columns: table => new
                {
                    PK_SoftwaresCustomer = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ImageId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CustomerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImagesCustomer", x => x.PK_SoftwaresCustomer);
                    table.ForeignKey(
                        name: "FK_ImagesCustomer_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "PK_Customer",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ImagesCustomer_Image_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Image",
                        principalColumn: "PK_Image",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ImagesSystemhouse",
                columns: table => new
                {
                    PK_SoftwaresSystemhouse = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ImageId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SystemhouseId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImagesSystemhouse", x => x.PK_SoftwaresSystemhouse);
                    table.ForeignKey(
                        name: "FK_ImagesSystemhouse_Image_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Image",
                        principalColumn: "PK_Image",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ImagesSystemhouse_Systemhouse_SystemhouseId",
                        column: x => x.SystemhouseId,
                        principalTable: "Systemhouse",
                        principalColumn: "PK_Systemhouse",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SoftwaresClient",
                columns: table => new
                {
                    PK_SoftwaresClient = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SoftwareId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClientId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoftwaresClient", x => x.PK_SoftwaresClient);
                    table.ForeignKey(
                        name: "FK_SoftwaresClient_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "PK_Client",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SoftwaresClient_Software_SoftwareId",
                        column: x => x.SoftwareId,
                        principalTable: "Software",
                        principalColumn: "PK_Software",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SoftwaresCustomer",
                columns: table => new
                {
                    PK_SoftwaresCustomer = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SoftwareId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CustomerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoftwaresCustomer", x => x.PK_SoftwaresCustomer);
                    table.ForeignKey(
                        name: "FK_SoftwaresCustomer_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "PK_Customer",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SoftwaresCustomer_Software_SoftwareId",
                        column: x => x.SoftwareId,
                        principalTable: "Software",
                        principalColumn: "PK_Software",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SoftwaresSystemhouse",
                columns: table => new
                {
                    PK_SoftwaresSystemhouse = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SoftwareId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SystemhouseId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoftwaresSystemhouse", x => x.PK_SoftwaresSystemhouse);
                    table.ForeignKey(
                        name: "FK_SoftwaresSystemhouse_Software_SoftwareId",
                        column: x => x.SoftwareId,
                        principalTable: "Software",
                        principalColumn: "PK_Software",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SoftwaresSystemhouse_Systemhouse_SystemhouseId",
                        column: x => x.SystemhouseId,
                        principalTable: "Systemhouse",
                        principalColumn: "PK_Systemhouse",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLog_ClientId",
                table: "ActivityLog",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_AdvancedProperty_BaseId",
                table: "AdvancedProperty",
                column: "BaseId");

            migrationBuilder.CreateIndex(
                name: "IX_AdvancedProperty_CategoryId",
                table: "AdvancedProperty",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Architecture_RuleId",
                table: "Architecture",
                column: "RuleId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetClass_AssetTypeId",
                table: "AssetClass",
                column: "AssetTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetClass_CustomerId",
                table: "AssetClass",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetModel_AssetClassId",
                table: "AssetModel",
                column: "AssetClassId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetModel_AssetTypeId",
                table: "AssetModel",
                column: "AssetTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetModel_ClientId",
                table: "AssetModel",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetModel_CustomerId",
                table: "AssetModel",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetModel_InvoiceId",
                table: "AssetModel",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetModel_LocationId",
                table: "AssetModel",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetModel_PersonId",
                table: "AssetModel",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetModel_VendorModelId",
                table: "AssetModel",
                column: "VendorModelId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetType_CustomerId",
                table: "AssetType",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Attachment_CreatedByUserId",
                table: "Attachment",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AzureBlobStorage_CustomerId",
                table: "AzureBlobStorage",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_AzureBlobStorage_RessourceGroupId",
                table: "AzureBlobStorage",
                column: "RessourceGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_AzureBlobStorage_StorageAccountId",
                table: "AzureBlobStorage",
                column: "StorageAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AzureBlobStorage_SubscriptionId",
                table: "AzureBlobStorage",
                column: "SubscriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Base_BaseStatusId",
                table: "Base",
                column: "BaseStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Base_CustomerId",
                table: "Base",
                column: "CustomerId");

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
                name: "IX_Bios_BiosSettingsId",
                table: "Bios",
                column: "BiosSettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_BIOSModel_ContentId",
                table: "BIOSModel",
                column: "ContentId");

            migrationBuilder.CreateIndex(
                name: "IX_BIOSModel_ReadMeId",
                table: "BIOSModel",
                column: "ReadMeId");

            migrationBuilder.CreateIndex(
                name: "IX_Client_BaseId",
                table: "Client",
                column: "BaseId");

            migrationBuilder.CreateIndex(
                name: "IX_Client_BiosId",
                table: "Client",
                column: "BiosId");

            migrationBuilder.CreateIndex(
                name: "IX_Client_CustomerId",
                table: "Client",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Client_HardwareId",
                table: "Client",
                column: "HardwareId");

            migrationBuilder.CreateIndex(
                name: "IX_Client_NetworkId",
                table: "Client",
                column: "NetworkId");

            migrationBuilder.CreateIndex(
                name: "IX_Client_OrganizationalUnitId",
                table: "Client",
                column: "OrganizationalUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Client_OsId",
                table: "Client",
                column: "OsId");

            migrationBuilder.CreateIndex(
                name: "IX_Client_PurchaseId",
                table: "Client",
                column: "PurchaseId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientClientProperty_ClientId",
                table: "ClientClientProperty",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientClientProperty_ClientPropertyId",
                table: "ClientClientProperty",
                column: "ClientPropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientOption_ClientId",
                table: "ClientOption",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientOption_DeviceOptionId",
                table: "ClientOption",
                column: "DeviceOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientParameter_ClientId",
                table: "ClientParameter",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientProperty_CategoryId",
                table: "ClientProperty",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientSoftware_ClientId",
                table: "ClientSoftware",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientSoftware_CustomerSoftwareId",
                table: "ClientSoftware",
                column: "CustomerSoftwareId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientSoftware_SoftwareId",
                table: "ClientSoftware",
                column: "SoftwareId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientTask_ClientId",
                table: "ClientTask",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientTask_TaskId",
                table: "ClientTask",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_CloudEntryPoint_CustomerId",
                table: "CloudEntryPoint",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Company_CustomerId",
                table: "Company",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Company_ExpertId",
                table: "Company",
                column: "ExpertId");

            migrationBuilder.CreateIndex(
                name: "IX_Company_HeadquarterId",
                table: "Company",
                column: "HeadquarterId");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_BannerId",
                table: "Customer",
                column: "BannerId");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_IconLeftId",
                table: "Customer",
                column: "IconLeftId");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_IconRightId",
                table: "Customer",
                column: "IconRightId");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_MainCompanyId",
                table: "Customer",
                column: "MainCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_SystemhouseId",
                table: "Customer",
                column: "SystemhouseId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerImage_CustomerImageStreamId",
                table: "CustomerImage",
                column: "CustomerImageStreamId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerImage_OEMPartitionId",
                table: "CustomerImage",
                column: "OEMPartitionId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerImage_UnattendId",
                table: "CustomerImage",
                column: "UnattendId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerImageStream_CustomerId",
                table: "CustomerImageStream",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerImageStream_IconId",
                table: "CustomerImageStream",
                column: "IconId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerSoftware_CustomerSoftwareStreamId",
                table: "CustomerSoftware",
                column: "CustomerSoftwareStreamId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerSoftware_RuleApplicabilityId",
                table: "CustomerSoftware",
                column: "RuleApplicabilityId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerSoftware_RuleDetectionId",
                table: "CustomerSoftware",
                column: "RuleDetectionId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerSoftware_TaskInstallId",
                table: "CustomerSoftware",
                column: "TaskInstallId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerSoftware_TaskUninstallId",
                table: "CustomerSoftware",
                column: "TaskUninstallId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerSoftware_TaskUpdateId",
                table: "CustomerSoftware",
                column: "TaskUpdateId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerSoftwareStream_ApplicationOwnerId",
                table: "CustomerSoftwareStream",
                column: "ApplicationOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerSoftwareStream_CustomerId",
                table: "CustomerSoftwareStream",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerSoftwareStream_IconId",
                table: "CustomerSoftwareStream",
                column: "IconId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerSoftwareStream_ReleasePlanId",
                table: "CustomerSoftwareStream",
                column: "ReleasePlanId");

            migrationBuilder.CreateIndex(
                name: "IX_Default_CustomerId",
                table: "Default",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Disk_VirtualMachineId",
                table: "Disk",
                column: "VirtualMachineId");

            migrationBuilder.CreateIndex(
                name: "IX_DNS_DomainId",
                table: "DNS",
                column: "DomainId");

            migrationBuilder.CreateIndex(
                name: "IX_Domain_BaseId",
                table: "Domain",
                column: "BaseId");

            migrationBuilder.CreateIndex(
                name: "IX_Domain_CustomerId",
                table: "Domain",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Domain_DomainUserCSVId",
                table: "Domain",
                column: "DomainUserCSVId");

            migrationBuilder.CreateIndex(
                name: "IX_Domain_GpoId",
                table: "Domain",
                column: "GpoId");

            migrationBuilder.CreateIndex(
                name: "IX_Domain_Office365ConfigurationXMLId",
                table: "Domain",
                column: "Office365ConfigurationXMLId");

            migrationBuilder.CreateIndex(
                name: "IX_DomainUser_DomainId",
                table: "DomainUser",
                column: "DomainId");

            migrationBuilder.CreateIndex(
                name: "IX_DriverShopItem_DriverId",
                table: "DriverShopItem",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_DriverShopItem_ShopItemId",
                table: "DriverShopItem",
                column: "ShopItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ExecutionLog_ClientId",
                table: "ExecutionLog",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ExecutionLog_ScriptVersionId",
                table: "ExecutionLog",
                column: "ScriptVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_ExecutionLog_UserId",
                table: "ExecutionLog",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ExecutionLog_VirtualMachineId",
                table: "ExecutionLog",
                column: "VirtualMachineId");

            migrationBuilder.CreateIndex(
                name: "IX_File_CustomerHardwareModelId",
                table: "File",
                column: "CustomerHardwareModelId");

            migrationBuilder.CreateIndex(
                name: "IX_File_ScriptVersionId",
                table: "File",
                column: "ScriptVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_File_ShopItemId",
                table: "File",
                column: "ShopItemId");

            migrationBuilder.CreateIndex(
                name: "IX_File_TaskId",
                table: "File",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_File_VendorModelId",
                table: "File",
                column: "VendorModelId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupPolicyObject_LockscreenId",
                table: "GroupPolicyObject",
                column: "LockscreenId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupPolicyObject_WallpaperId",
                table: "GroupPolicyObject",
                column: "WallpaperId");

            migrationBuilder.CreateIndex(
                name: "IX_HardwareModel_BIOSModelId",
                table: "HardwareModel",
                column: "BIOSModelId");

            migrationBuilder.CreateIndex(
                name: "IX_HardwareModel_OSModelId",
                table: "HardwareModel",
                column: "OSModelId");

            migrationBuilder.CreateIndex(
                name: "IX_HDDPartition_ClientId",
                table: "HDDPartition",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Image_ImageStreamId",
                table: "Image",
                column: "ImageStreamId");

            migrationBuilder.CreateIndex(
                name: "IX_Image_OEMPartitionId",
                table: "Image",
                column: "OEMPartitionId");

            migrationBuilder.CreateIndex(
                name: "IX_Image_UnattendId",
                table: "Image",
                column: "UnattendId");

            migrationBuilder.CreateIndex(
                name: "IX_ImagesClient_ClientId",
                table: "ImagesClient",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ImagesClient_ImageId",
                table: "ImagesClient",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_ImagesCustomer_CustomerId",
                table: "ImagesCustomer",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_ImagesCustomer_ImageId",
                table: "ImagesCustomer",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_ImagesSystemhouse_ImageId",
                table: "ImagesSystemhouse",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_ImagesSystemhouse_SystemhouseId",
                table: "ImagesSystemhouse",
                column: "SystemhouseId");

            migrationBuilder.CreateIndex(
                name: "IX_ImageStream_IconId",
                table: "ImageStream",
                column: "IconId");

            migrationBuilder.CreateIndex(
                name: "IX_Inventory_ClientId",
                table: "Inventory",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Location_CompanyId",
                table: "Location",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Location_CustomerId",
                table: "Location",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_MacAddress_ClientId",
                table: "MacAddress",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationEmail_SchedulerId",
                table: "NotificationEmail",
                column: "SchedulerId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationEmailAttachment_AttachmentId",
                table: "NotificationEmailAttachment",
                column: "AttachmentId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationEmailAttachment_NotificationEmailId",
                table: "NotificationEmailAttachment",
                column: "NotificationEmailId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_CreatedByUserId",
                table: "Order",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderShopItem_OrderId",
                table: "OrderShopItem",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderShopItem_ShopItemId",
                table: "OrderShopItem",
                column: "ShopItemId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationalUnit_DomainId",
                table: "OrganizationalUnit",
                column: "DomainId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationalUnit_OrganizationalUnitId",
                table: "OrganizationalUnit",
                column: "OrganizationalUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_OSModel_ContentId",
                table: "OSModel",
                column: "ContentId");

            migrationBuilder.CreateIndex(
                name: "IX_OsVersionName_RuleId",
                table: "OsVersionName",
                column: "RuleId");

            migrationBuilder.CreateIndex(
                name: "IX_Parameter_ClientOptionId",
                table: "Parameter",
                column: "ClientOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Parameter_CustomerId",
                table: "Parameter",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Parameter_ExecutionLogId",
                table: "Parameter",
                column: "ExecutionLogId");

            migrationBuilder.CreateIndex(
                name: "IX_Person_CompanyId",
                table: "Person",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Person_CustomerId",
                table: "Person",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_PreinstalledSoftware_ClientId",
                table: "PreinstalledSoftware",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ReleasePlan_CustomerId",
                table: "ReleasePlan",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_ResourceGroup_CustomerId",
                table: "ResourceGroup",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Rule_DataId",
                table: "Rule",
                column: "DataId");

            migrationBuilder.CreateIndex(
                name: "IX_Rule_TypeId",
                table: "Rule",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Scheduler_CreatedByUserId",
                table: "Scheduler",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Scheduler_ParentSchedulerId",
                table: "Scheduler",
                column: "ParentSchedulerId");

            migrationBuilder.CreateIndex(
                name: "IX_ScriptVersion_AdminDeviceOptionId",
                table: "ScriptVersion",
                column: "AdminDeviceOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_ScriptVersion_ScriptId",
                table: "ScriptVersion",
                column: "ScriptId");

            migrationBuilder.CreateIndex(
                name: "IX_Server_DomainId",
                table: "Server",
                column: "DomainId");

            migrationBuilder.CreateIndex(
                name: "IX_Server_OrganizationalUnitId",
                table: "Server",
                column: "OrganizationalUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Server_VirtualMachineId",
                table: "Server",
                column: "VirtualMachineId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopItemCategory_CategoryId",
                table: "ShopItemCategory",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopItemCategory_ShopItemId",
                table: "ShopItemCategory",
                column: "ShopItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Software_RuleApplicabilityId",
                table: "Software",
                column: "RuleApplicabilityId");

            migrationBuilder.CreateIndex(
                name: "IX_Software_RuleDetectionId",
                table: "Software",
                column: "RuleDetectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Software_SoftwareStreamId",
                table: "Software",
                column: "SoftwareStreamId");

            migrationBuilder.CreateIndex(
                name: "IX_Software_TaskInstallId",
                table: "Software",
                column: "TaskInstallId");

            migrationBuilder.CreateIndex(
                name: "IX_Software_TaskUninstallId",
                table: "Software",
                column: "TaskUninstallId");

            migrationBuilder.CreateIndex(
                name: "IX_Software_TaskUpdateId",
                table: "Software",
                column: "TaskUpdateId");

            migrationBuilder.CreateIndex(
                name: "IX_SoftwaresClient_ClientId",
                table: "SoftwaresClient",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_SoftwaresClient_SoftwareId",
                table: "SoftwaresClient",
                column: "SoftwareId");

            migrationBuilder.CreateIndex(
                name: "IX_SoftwaresCustomer_CustomerId",
                table: "SoftwaresCustomer",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_SoftwaresCustomer_SoftwareId",
                table: "SoftwaresCustomer",
                column: "SoftwareId");

            migrationBuilder.CreateIndex(
                name: "IX_SoftwaresSystemhouse_SoftwareId",
                table: "SoftwaresSystemhouse",
                column: "SoftwareId");

            migrationBuilder.CreateIndex(
                name: "IX_SoftwaresSystemhouse_SystemhouseId",
                table: "SoftwaresSystemhouse",
                column: "SystemhouseId");

            migrationBuilder.CreateIndex(
                name: "IX_SoftwareStream_IconId",
                table: "SoftwareStream",
                column: "IconId");

            migrationBuilder.CreateIndex(
                name: "IX_StorageAccount_CustomerId",
                table: "StorageAccount",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_StorageAccount_ResourceGroupId",
                table: "StorageAccount",
                column: "ResourceGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_StorageEntryPoint_CustomerId",
                table: "StorageEntryPoint",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Subnet_VirtualNetworkId",
                table: "Subnet",
                column: "VirtualNetworkId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscription_CustomerId",
                table: "Subscription",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Task_ExecutionFileId",
                table: "Task",
                column: "ExecutionFileId");

            migrationBuilder.CreateIndex(
                name: "IX_Token_CustomerId",
                table: "Token",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_User_CustomerId",
                table: "User",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_User_DeletedByUserId",
                table: "User",
                column: "DeletedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_User_SystemhouseId",
                table: "User",
                column: "SystemhouseId");

            migrationBuilder.CreateIndex(
                name: "IX_User_UpdatedByUserId",
                table: "User",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCustomer_CustomerId",
                table: "UserCustomer",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCustomer_UserId",
                table: "UserCustomer",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserForgotPassword_UserId",
                table: "UserForgotPassword",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_RoleId",
                table: "UserRole",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_UserId",
                table: "UserRole",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSubscription_SubscriptionId",
                table: "UserSubscription",
                column: "SubscriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSubscription_UserId",
                table: "UserSubscription",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_VirtualMachine_BaseId",
                table: "VirtualMachine",
                column: "BaseId");

            migrationBuilder.CreateIndex(
                name: "IX_Win10Version_RuleId",
                table: "Win10Version",
                column: "RuleId");

            migrationBuilder.CreateIndex(
                name: "IX_Win11Version_RuleId",
                table: "Win11Version",
                column: "RuleId");

            migrationBuilder.CreateIndex(
                name: "IX_WMIInvenotryCmds_ClientId",
                table: "WMIInvenotryCmds",
                column: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityLog_Client_ClientId",
                table: "ActivityLog",
                column: "ClientId",
                principalTable: "Client",
                principalColumn: "PK_Client",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AdvancedProperty_Base_BaseId",
                table: "AdvancedProperty",
                column: "BaseId",
                principalTable: "Base",
                principalColumn: "PK_Base",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Architecture_Rule_RuleId",
                table: "Architecture",
                column: "RuleId",
                principalTable: "Rule",
                principalColumn: "PK_Rule",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AssetClass_AssetType_AssetTypeId",
                table: "AssetClass",
                column: "AssetTypeId",
                principalTable: "AssetType",
                principalColumn: "PK_AssetType",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AssetClass_Customer_CustomerId",
                table: "AssetClass",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "PK_Customer",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AssetModel_AssetType_AssetTypeId",
                table: "AssetModel",
                column: "AssetTypeId",
                principalTable: "AssetType",
                principalColumn: "PK_AssetType",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AssetModel_Client_ClientId",
                table: "AssetModel",
                column: "ClientId",
                principalTable: "Client",
                principalColumn: "PK_Client",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AssetModel_Customer_CustomerId",
                table: "AssetModel",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "PK_Customer",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AssetModel_File_InvoiceId",
                table: "AssetModel",
                column: "InvoiceId",
                principalTable: "File",
                principalColumn: "PK_File",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AssetModel_Location_LocationId",
                table: "AssetModel",
                column: "LocationId",
                principalTable: "Location",
                principalColumn: "PK_Location",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AssetModel_Person_PersonId",
                table: "AssetModel",
                column: "PersonId",
                principalTable: "Person",
                principalColumn: "PK_Person",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AssetType_Customer_CustomerId",
                table: "AssetType",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "PK_Customer",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Attachment_User_CreatedByUserId",
                table: "Attachment",
                column: "CreatedByUserId",
                principalTable: "User",
                principalColumn: "PK_User",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AzureBlobStorage_Customer_CustomerId",
                table: "AzureBlobStorage",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "PK_Customer",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AzureBlobStorage_ResourceGroup_RessourceGroupId",
                table: "AzureBlobStorage",
                column: "RessourceGroupId",
                principalTable: "ResourceGroup",
                principalColumn: "PK_ResourceGroup",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AzureBlobStorage_StorageAccount_StorageAccountId",
                table: "AzureBlobStorage",
                column: "StorageAccountId",
                principalTable: "StorageAccount",
                principalColumn: "PK_StorageAccount",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AzureBlobStorage_Subscription_SubscriptionId",
                table: "AzureBlobStorage",
                column: "SubscriptionId",
                principalTable: "Subscription",
                principalColumn: "PK_Subscription",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Base_Customer_CustomerId",
                table: "Base",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "PK_Customer",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Base_ResourceGroup_ResourceGroupId",
                table: "Base",
                column: "ResourceGroupId",
                principalTable: "ResourceGroup",
                principalColumn: "PK_ResourceGroup",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Base_StorageAccount_StorageAccountId",
                table: "Base",
                column: "StorageAccountId",
                principalTable: "StorageAccount",
                principalColumn: "PK_StorageAccount",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Base_Subscription_SubscriptionId",
                table: "Base",
                column: "SubscriptionId",
                principalTable: "Subscription",
                principalColumn: "PK_Subscription",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BIOSModel_File_ContentId",
                table: "BIOSModel",
                column: "ContentId",
                principalTable: "File",
                principalColumn: "PK_File",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BIOSModel_File_ReadMeId",
                table: "BIOSModel",
                column: "ReadMeId",
                principalTable: "File",
                principalColumn: "PK_File",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Client_Customer_CustomerId",
                table: "Client",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "PK_Customer",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Client_OrganizationalUnit_OrganizationalUnitId",
                table: "Client",
                column: "OrganizationalUnitId",
                principalTable: "OrganizationalUnit",
                principalColumn: "PK_OrganizationalUnit",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientSoftware_CustomerSoftware_CustomerSoftwareId",
                table: "ClientSoftware",
                column: "CustomerSoftwareId",
                principalTable: "CustomerSoftware",
                principalColumn: "PK_CustomerSoftware",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientSoftware_Software_SoftwareId",
                table: "ClientSoftware",
                column: "SoftwareId",
                principalTable: "Software",
                principalColumn: "PK_Software",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientTask_Task_TaskId",
                table: "ClientTask",
                column: "TaskId",
                principalTable: "Task",
                principalColumn: "PK_Task",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CloudEntryPoint_Customer_CustomerId",
                table: "CloudEntryPoint",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "PK_Customer",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Company_Customer_CustomerId",
                table: "Company",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "PK_Customer",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Company_Location_HeadquarterId",
                table: "Company",
                column: "HeadquarterId",
                principalTable: "Location",
                principalColumn: "PK_Location",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Company_Person_ExpertId",
                table: "Company",
                column: "ExpertId",
                principalTable: "Person",
                principalColumn: "PK_Person",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_File_BannerId",
                table: "Customer",
                column: "BannerId",
                principalTable: "File",
                principalColumn: "PK_File",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_File_IconLeftId",
                table: "Customer",
                column: "IconLeftId",
                principalTable: "File",
                principalColumn: "PK_File",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_File_IconRightId",
                table: "Customer",
                column: "IconRightId",
                principalTable: "File",
                principalColumn: "PK_File",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerImage_CustomerImageStream_CustomerImageStreamId",
                table: "CustomerImage",
                column: "CustomerImageStreamId",
                principalTable: "CustomerImageStream",
                principalColumn: "PK_ImageStream",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerImage_File_OEMPartitionId",
                table: "CustomerImage",
                column: "OEMPartitionId",
                principalTable: "File",
                principalColumn: "PK_File",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerImage_File_UnattendId",
                table: "CustomerImage",
                column: "UnattendId",
                principalTable: "File",
                principalColumn: "PK_File",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerImageStream_File_IconId",
                table: "CustomerImageStream",
                column: "IconId",
                principalTable: "File",
                principalColumn: "PK_File",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerSoftware_CustomerSoftwareStream_CustomerSoftwareStreamId",
                table: "CustomerSoftware",
                column: "CustomerSoftwareStreamId",
                principalTable: "CustomerSoftwareStream",
                principalColumn: "PK_CustomerSoftwareStream",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerSoftware_Rule_RuleApplicabilityId",
                table: "CustomerSoftware",
                column: "RuleApplicabilityId",
                principalTable: "Rule",
                principalColumn: "PK_Rule",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerSoftware_Rule_RuleDetectionId",
                table: "CustomerSoftware",
                column: "RuleDetectionId",
                principalTable: "Rule",
                principalColumn: "PK_Rule",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerSoftware_Task_TaskInstallId",
                table: "CustomerSoftware",
                column: "TaskInstallId",
                principalTable: "Task",
                principalColumn: "PK_Task",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerSoftware_Task_TaskUninstallId",
                table: "CustomerSoftware",
                column: "TaskUninstallId",
                principalTable: "Task",
                principalColumn: "PK_Task",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerSoftware_Task_TaskUpdateId",
                table: "CustomerSoftware",
                column: "TaskUpdateId",
                principalTable: "Task",
                principalColumn: "PK_Task",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerSoftwareStream_File_IconId",
                table: "CustomerSoftwareStream",
                column: "IconId",
                principalTable: "File",
                principalColumn: "PK_File",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DNS_Domain_DomainId",
                table: "DNS",
                column: "DomainId",
                principalTable: "Domain",
                principalColumn: "PK_Domain",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Domain_File_DomainUserCSVId",
                table: "Domain",
                column: "DomainUserCSVId",
                principalTable: "File",
                principalColumn: "PK_File",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Domain_File_Office365ConfigurationXMLId",
                table: "Domain",
                column: "Office365ConfigurationXMLId",
                principalTable: "File",
                principalColumn: "PK_File",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Domain_GroupPolicyObject_GpoId",
                table: "Domain",
                column: "GpoId",
                principalTable: "GroupPolicyObject",
                principalColumn: "PK_GPO",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_File_Task_TaskId",
                table: "File",
                column: "TaskId",
                principalTable: "Task",
                principalColumn: "PK_Task",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Company_Customer_CustomerId",
                table: "Company");

            migrationBuilder.DropForeignKey(
                name: "FK_Location_Customer_CustomerId",
                table: "Location");

            migrationBuilder.DropForeignKey(
                name: "FK_Person_Customer_CustomerId",
                table: "Person");

            migrationBuilder.DropForeignKey(
                name: "FK_Task_File_ExecutionFileId",
                table: "Task");

            migrationBuilder.DropForeignKey(
                name: "FK_Company_Location_HeadquarterId",
                table: "Company");

            migrationBuilder.DropForeignKey(
                name: "FK_Company_Person_ExpertId",
                table: "Company");

            migrationBuilder.DropTable(
                name: "ActivityLog");

            migrationBuilder.DropTable(
                name: "AdvancedProperty");

            migrationBuilder.DropTable(
                name: "Architecture");

            migrationBuilder.DropTable(
                name: "AssetModel");

            migrationBuilder.DropTable(
                name: "AzureBlobStorage");

            migrationBuilder.DropTable(
                name: "Certification");

            migrationBuilder.DropTable(
                name: "ChangeLog");

            migrationBuilder.DropTable(
                name: "ClientClientProperty");

            migrationBuilder.DropTable(
                name: "ClientParameter");

            migrationBuilder.DropTable(
                name: "ClientSoftware");

            migrationBuilder.DropTable(
                name: "ClientTask");

            migrationBuilder.DropTable(
                name: "CloudEntryPoint");

            migrationBuilder.DropTable(
                name: "CustomerDriver");

            migrationBuilder.DropTable(
                name: "CustomerImage");

            migrationBuilder.DropTable(
                name: "Default");

            migrationBuilder.DropTable(
                name: "Disk");

            migrationBuilder.DropTable(
                name: "DNS");

            migrationBuilder.DropTable(
                name: "DomainRegistrationTemp");

            migrationBuilder.DropTable(
                name: "DomainUser");

            migrationBuilder.DropTable(
                name: "DriverShopItem");

            migrationBuilder.DropTable(
                name: "ErrorLog");

            migrationBuilder.DropTable(
                name: "HardwareModel");

            migrationBuilder.DropTable(
                name: "HDDPartition");

            migrationBuilder.DropTable(
                name: "ImagesClient");

            migrationBuilder.DropTable(
                name: "ImagesCustomer");

            migrationBuilder.DropTable(
                name: "ImagesSystemhouse");

            migrationBuilder.DropTable(
                name: "Inventory");

            migrationBuilder.DropTable(
                name: "MacAddress");

            migrationBuilder.DropTable(
                name: "NotificationEmailAttachment");

            migrationBuilder.DropTable(
                name: "OrderShopItem");

            migrationBuilder.DropTable(
                name: "OsVersionName");

            migrationBuilder.DropTable(
                name: "Parameter");

            migrationBuilder.DropTable(
                name: "PreinstalledSoftware");

            migrationBuilder.DropTable(
                name: "RevisionMessage");

            migrationBuilder.DropTable(
                name: "Server");

            migrationBuilder.DropTable(
                name: "ShopItemCategory");

            migrationBuilder.DropTable(
                name: "SoftwaresClient");

            migrationBuilder.DropTable(
                name: "SoftwaresCustomer");

            migrationBuilder.DropTable(
                name: "SoftwaresSystemhouse");

            migrationBuilder.DropTable(
                name: "StorageEntryPoint");

            migrationBuilder.DropTable(
                name: "Subnet");

            migrationBuilder.DropTable(
                name: "Token");

            migrationBuilder.DropTable(
                name: "UserCustomer");

            migrationBuilder.DropTable(
                name: "UserForgotPassword");

            migrationBuilder.DropTable(
                name: "UserRole");

            migrationBuilder.DropTable(
                name: "UserSubscription");

            migrationBuilder.DropTable(
                name: "Win10Version");

            migrationBuilder.DropTable(
                name: "Win11Version");

            migrationBuilder.DropTable(
                name: "WMIInvenotryCmds");

            migrationBuilder.DropTable(
                name: "Workflow");

            migrationBuilder.DropTable(
                name: "AssetClass");

            migrationBuilder.DropTable(
                name: "ClientProperty");

            migrationBuilder.DropTable(
                name: "CustomerSoftware");

            migrationBuilder.DropTable(
                name: "CustomerImageStream");

            migrationBuilder.DropTable(
                name: "Driver");

            migrationBuilder.DropTable(
                name: "BIOSModel");

            migrationBuilder.DropTable(
                name: "OSModel");

            migrationBuilder.DropTable(
                name: "Image");

            migrationBuilder.DropTable(
                name: "Attachment");

            migrationBuilder.DropTable(
                name: "NotificationEmail");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "ClientOption");

            migrationBuilder.DropTable(
                name: "ExecutionLog");

            migrationBuilder.DropTable(
                name: "Software");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "AssetType");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "CustomerSoftwareStream");

            migrationBuilder.DropTable(
                name: "ImageStream");

            migrationBuilder.DropTable(
                name: "Scheduler");

            migrationBuilder.DropTable(
                name: "Client");

            migrationBuilder.DropTable(
                name: "VirtualMachine");

            migrationBuilder.DropTable(
                name: "Rule");

            migrationBuilder.DropTable(
                name: "SoftwareStream");

            migrationBuilder.DropTable(
                name: "ReleasePlan");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Bios");

            migrationBuilder.DropTable(
                name: "Hardware");

            migrationBuilder.DropTable(
                name: "NetworkConfiguration");

            migrationBuilder.DropTable(
                name: "OrganizationalUnit");

            migrationBuilder.DropTable(
                name: "OS");

            migrationBuilder.DropTable(
                name: "Purchase");

            migrationBuilder.DropTable(
                name: "RuleType");

            migrationBuilder.DropTable(
                name: "BiosSettings");

            migrationBuilder.DropTable(
                name: "Domain");

            migrationBuilder.DropTable(
                name: "Base");

            migrationBuilder.DropTable(
                name: "GroupPolicyObject");

            migrationBuilder.DropTable(
                name: "BaseStatus");

            migrationBuilder.DropTable(
                name: "StorageAccount");

            migrationBuilder.DropTable(
                name: "Subscription");

            migrationBuilder.DropTable(
                name: "VirtualNetwork");

            migrationBuilder.DropTable(
                name: "Vpn");

            migrationBuilder.DropTable(
                name: "ResourceGroup");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "Systemhouse");

            migrationBuilder.DropTable(
                name: "File");

            migrationBuilder.DropTable(
                name: "CustomerHardwareModel");

            migrationBuilder.DropTable(
                name: "ScriptVersion");

            migrationBuilder.DropTable(
                name: "ShopItem");

            migrationBuilder.DropTable(
                name: "Task");

            migrationBuilder.DropTable(
                name: "VendorModel");

            migrationBuilder.DropTable(
                name: "AdminDeviceOption");

            migrationBuilder.DropTable(
                name: "Script");

            migrationBuilder.DropTable(
                name: "Location");

            migrationBuilder.DropTable(
                name: "Person");

            migrationBuilder.DropTable(
                name: "Company");
        }
    }
}
