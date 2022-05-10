using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _14_FKAdditionalDisks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VirtualMachine_Disk_SystemDiskId",
                table: "VirtualMachine");

            migrationBuilder.DropIndex(
                name: "IX_VirtualMachine_SystemDiskId",
                table: "VirtualMachine");

            migrationBuilder.DropColumn(
                name: "SystemDiskId",
                table: "VirtualMachine");

            migrationBuilder.RenameColumn(
                name: "PK_VirtualMachine",
                table: "Disk",
                newName: "PK_Disk");

            migrationBuilder.AddColumn<int>(
                name: "DiskType",
                table: "Disk",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiskType",
                table: "Disk");

            migrationBuilder.RenameColumn(
                name: "PK_Disk",
                table: "Disk",
                newName: "PK_VirtualMachine");

            migrationBuilder.AddColumn<string>(
                name: "SystemDiskId",
                table: "VirtualMachine",
                nullable: true);

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
    }
}
