using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _186_AddCustomerSoftwareEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomerSoftwareStream",
                columns: table => new
                {
                    PK_CustomerSoftwareStream = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    UpdateSettings = table.Column<string>(nullable: true),
                    CreatedByUserId = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedByUserId = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    DeletedByUserId = table.Column<string>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    CustomerId = table.Column<string>(nullable: true)
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
                });

            migrationBuilder.CreateTable(
                name: "CustomerSoftware",
                columns: table => new
                {
                    PK_CustomerSoftware = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    DescriptionShort = table.Column<string>(nullable: true),
                    Version = table.Column<string>(nullable: true),
                    Vendor = table.Column<string>(nullable: true),
                    RuleDetectionId = table.Column<string>(nullable: true),
                    RuleApplicabilityId = table.Column<string>(nullable: true),
                    TaskInstallId = table.Column<string>(nullable: true),
                    TaskUninstallId = table.Column<string>(nullable: true),
                    TaskUpdateId = table.Column<string>(nullable: true),
                    IconId = table.Column<string>(nullable: true),
                    CreatedByUserId = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedByUserId = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    DeletedByUserId = table.Column<string>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    Website = table.Column<string>(nullable: true),
                    DownloadLink = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    Language = table.Column<string>(nullable: true),
                    Architecture = table.Column<string>(nullable: true),
                    InstallationTime = table.Column<string>(nullable: true),
                    PackageSize = table.Column<string>(nullable: true),
                    Prerequisites = table.Column<string>(nullable: true),
                    VendorReleaseDate = table.Column<string>(nullable: true),
                    GnuLicence = table.Column<bool>(nullable: false),
                    CompliancyRule = table.Column<string>(nullable: true),
                    Checksum = table.Column<string>(nullable: true),
                    CustomerSoftwareStreamId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerSoftware", x => x.PK_CustomerSoftware);
                    table.ForeignKey(
                        name: "FK_CustomerSoftware_CustomerSoftwareStream_CustomerSoftwareStreamId",
                        column: x => x.CustomerSoftwareStreamId,
                        principalTable: "CustomerSoftwareStream",
                        principalColumn: "PK_CustomerSoftwareStream",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerSoftware_File_IconId",
                        column: x => x.IconId,
                        principalTable: "File",
                        principalColumn: "PK_File",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerSoftware_Rule_RuleApplicabilityId",
                        column: x => x.RuleApplicabilityId,
                        principalTable: "Rule",
                        principalColumn: "PK_Rule",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerSoftware_Rule_RuleDetectionId",
                        column: x => x.RuleDetectionId,
                        principalTable: "Rule",
                        principalColumn: "PK_Rule",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerSoftware_Task_TaskInstallId",
                        column: x => x.TaskInstallId,
                        principalTable: "Task",
                        principalColumn: "PK_Task",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerSoftware_Task_TaskUninstallId",
                        column: x => x.TaskUninstallId,
                        principalTable: "Task",
                        principalColumn: "PK_Task",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerSoftware_Task_TaskUpdateId",
                        column: x => x.TaskUpdateId,
                        principalTable: "Task",
                        principalColumn: "PK_Task",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerSoftware_CustomerSoftwareStreamId",
                table: "CustomerSoftware",
                column: "CustomerSoftwareStreamId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerSoftware_IconId",
                table: "CustomerSoftware",
                column: "IconId");

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
                name: "IX_CustomerSoftwareStream_CustomerId",
                table: "CustomerSoftwareStream",
                column: "CustomerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerSoftware");

            migrationBuilder.DropTable(
                name: "CustomerSoftwareStream");
        }
    }
}
