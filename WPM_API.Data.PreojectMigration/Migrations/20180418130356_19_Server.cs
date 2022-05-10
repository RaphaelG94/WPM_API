using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _19_Server : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Server",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    DomainId = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    VirtualMachineId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Server", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Server_Domain_DomainId",
                        column: x => x.DomainId,
                        principalTable: "Domain",
                        principalColumn: "PK_Domain",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Server_VirtualMachine_VirtualMachineId",
                        column: x => x.VirtualMachineId,
                        principalTable: "VirtualMachine",
                        principalColumn: "PK_VirtualMachine",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Server_DomainId",
                table: "Server",
                column: "DomainId");

            migrationBuilder.CreateIndex(
                name: "IX_Server_VirtualMachineId",
                table: "Server",
                column: "VirtualMachineId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Server");
        }
    }
}
