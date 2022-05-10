using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _17_AddSmartDeploy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RuleType",
                columns: table => new
                {
                    PK_RuleType = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RuleType", x => x.PK_RuleType);
                });

            migrationBuilder.CreateTable(
                name: "Rule",
                columns: table => new
                {
                    PK_Rule = table.Column<string>(nullable: false),
                    Architecture = table.Column<string>(nullable: true),
                    CreatedByUserId = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    DataId = table.Column<string>(nullable: true),
                    DeletedByUserId = table.Column<string>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Successon = table.Column<bool>(nullable: false),
                    TypeId = table.Column<string>(nullable: true),
                    UpdatedByUserId = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rule", x => x.PK_Rule);
                    table.ForeignKey(
                        name: "FK_Rule_RuleType_TypeId",
                        column: x => x.TypeId,
                        principalTable: "RuleType",
                        principalColumn: "PK_RuleType",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Software",
                columns: table => new
                {
                    PK_Software = table.Column<string>(nullable: false),
                    CreatedByUserId = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    DeletedByUserId = table.Column<string>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    DescriptionShort = table.Column<string>(nullable: true),
                    IconId = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    RuleApplicabilityId = table.Column<string>(nullable: true),
                    RuleDetectionId = table.Column<string>(nullable: true),
                    TaskInstallId = table.Column<string>(nullable: true),
                    TaskUninstallId = table.Column<string>(nullable: true),
                    TaskUpdateId = table.Column<string>(nullable: true),
                    UpdatedByUserId = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    Vendor = table.Column<string>(nullable: true),
                    Version = table.Column<string>(nullable: true)
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
                });

            migrationBuilder.CreateTable(
                name: "Task",
                columns: table => new
                {
                    PK_Task = table.Column<string>(nullable: false),
                    BlockFilePath = table.Column<string>(nullable: true),
                    CommandLine = table.Column<string>(nullable: true),
                    CreatedByUserId = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    DeletedByUserId = table.Column<string>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    DescriptionShort = table.Column<string>(nullable: true),
                    EstimatedExecutionTime = table.Column<int>(nullable: false),
                    ExecutionFileId = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    SuccessValue = table.Column<string>(nullable: true),
                    UpdatedByUserId = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    UseShellExecute = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Task", x => x.PK_Task);
                });

            migrationBuilder.CreateTable(
                name: "File",
                columns: table => new
                {
                    PK_File = table.Column<string>(nullable: false),
                    Guid = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    TaskId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_File", x => x.PK_File);
                    table.ForeignKey(
                        name: "FK_File_Task_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Task",
                        principalColumn: "PK_Task",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_File_TaskId",
                table: "File",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Rule_DataId",
                table: "Rule",
                column: "DataId");

            migrationBuilder.CreateIndex(
                name: "IX_Rule_TypeId",
                table: "Rule",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Software_IconId",
                table: "Software",
                column: "IconId");

            migrationBuilder.CreateIndex(
                name: "IX_Software_RuleApplicabilityId",
                table: "Software",
                column: "RuleApplicabilityId");

            migrationBuilder.CreateIndex(
                name: "IX_Software_RuleDetectionId",
                table: "Software",
                column: "RuleDetectionId");

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
                name: "IX_Task_ExecutionFileId",
                table: "Task",
                column: "ExecutionFileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rule_File_DataId",
                table: "Rule",
                column: "DataId",
                principalTable: "File",
                principalColumn: "PK_File",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Software_Task_TaskInstallId",
                table: "Software",
                column: "TaskInstallId",
                principalTable: "Task",
                principalColumn: "PK_Task",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Software_Task_TaskUninstallId",
                table: "Software",
                column: "TaskUninstallId",
                principalTable: "Task",
                principalColumn: "PK_Task",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Software_Task_TaskUpdateId",
                table: "Software",
                column: "TaskUpdateId",
                principalTable: "Task",
                principalColumn: "PK_Task",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Software_File_IconId",
                table: "Software",
                column: "IconId",
                principalTable: "File",
                principalColumn: "PK_File",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Task_File_ExecutionFileId",
                table: "Task",
                column: "ExecutionFileId",
                principalTable: "File",
                principalColumn: "PK_File",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_File_Task_TaskId",
                table: "File");

            migrationBuilder.DropTable(
                name: "Software");

            migrationBuilder.DropTable(
                name: "Rule");

            migrationBuilder.DropTable(
                name: "RuleType");

            migrationBuilder.DropTable(
                name: "Task");

            migrationBuilder.DropTable(
                name: "File");
        }
    }
}
