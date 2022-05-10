using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _15_ExecutionLog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExecutionLog",
                columns: table => new
                {
                    PK_ExecutionLog = table.Column<string>(nullable: false),
                    ExecutionDate = table.Column<DateTime>(nullable: false),
                    Script = table.Column<string>(nullable: true),
                    ScriptVersionId = table.Column<string>(nullable: true),
                    UserIdId = table.Column<string>(nullable: true),
                    VirtualMachineId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExecutionLog", x => x.PK_ExecutionLog);
                    table.ForeignKey(
                        name: "FK_ExecutionLog_ScriptVersion_ScriptVersionId",
                        column: x => x.ScriptVersionId,
                        principalTable: "ScriptVersion",
                        principalColumn: "PK_ScriptVersion",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExecutionLog_User_UserIdId",
                        column: x => x.UserIdId,
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
                name: "Parameter",
                columns: table => new
                {
                    PK_Parameter = table.Column<string>(nullable: false),
                    ExecutionLogId = table.Column<string>(nullable: true),
                    Key = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parameter", x => x.PK_Parameter);
                    table.ForeignKey(
                        name: "FK_Parameter_ExecutionLog_ExecutionLogId",
                        column: x => x.ExecutionLogId,
                        principalTable: "ExecutionLog",
                        principalColumn: "PK_ExecutionLog",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExecutionLog_ScriptVersionId",
                table: "ExecutionLog",
                column: "ScriptVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_ExecutionLog_UserIdId",
                table: "ExecutionLog",
                column: "UserIdId");

            migrationBuilder.CreateIndex(
                name: "IX_ExecutionLog_VirtualMachineId",
                table: "ExecutionLog",
                column: "VirtualMachineId");

            migrationBuilder.CreateIndex(
                name: "IX_Parameter_ExecutionLogId",
                table: "Parameter",
                column: "ExecutionLogId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Parameter");

            migrationBuilder.DropTable(
                name: "ExecutionLog");
        }
    }
}
