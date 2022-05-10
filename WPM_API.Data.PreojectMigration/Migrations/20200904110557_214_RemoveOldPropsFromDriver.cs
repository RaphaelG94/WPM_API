using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _214_RemoveOldPropsFromDriver : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Driver_File_ContentId",
                table: "Driver");

            migrationBuilder.DropForeignKey(
                name: "FK_Driver_File_ReadeMeId",
                table: "Driver");

            migrationBuilder.DropForeignKey(
                name: "FK_HardwareModel_Driver_DriverId",
                table: "HardwareModel");

            migrationBuilder.DropIndex(
                name: "IX_HardwareModel_DriverId",
                table: "HardwareModel");

            migrationBuilder.DropIndex(
                name: "IX_Driver_ContentId",
                table: "Driver");

            migrationBuilder.DropIndex(
                name: "IX_Driver_ReadeMeId",
                table: "Driver");

            migrationBuilder.DropColumn(
                name: "DriverId",
                table: "HardwareModel");

            migrationBuilder.DropColumn(
                name: "ContentId",
                table: "Driver");

            migrationBuilder.DropColumn(
                name: "DeviceName",
                table: "Driver");

            migrationBuilder.DropColumn(
                name: "DeviceNameDriverManager",
                table: "Driver");

            migrationBuilder.DropColumn(
                name: "DriverName",
                table: "Driver");

            migrationBuilder.DropColumn(
                name: "ReadeMeId",
                table: "Driver");

            migrationBuilder.DropColumn(
                name: "ReleaseDate",
                table: "Driver");

            migrationBuilder.DropColumn(
                name: "ValidOS",
                table: "Driver");

            migrationBuilder.DropColumn(
                name: "Vendor",
                table: "Driver");

            migrationBuilder.DropColumn(
                name: "VendorId",
                table: "Driver");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "Driver");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DriverId",
                table: "HardwareModel",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContentId",
                table: "Driver",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeviceName",
                table: "Driver",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeviceNameDriverManager",
                table: "Driver",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DriverName",
                table: "Driver",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReadeMeId",
                table: "Driver",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReleaseDate",
                table: "Driver",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ValidOS",
                table: "Driver",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Vendor",
                table: "Driver",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VendorId",
                table: "Driver",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Version",
                table: "Driver",
                nullable: true);

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
                name: "FK_Driver_File_ContentId",
                table: "Driver",
                column: "ContentId",
                principalTable: "File",
                principalColumn: "PK_File",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Driver_File_ReadeMeId",
                table: "Driver",
                column: "ReadeMeId",
                principalTable: "File",
                principalColumn: "PK_File",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HardwareModel_Driver_DriverId",
                table: "HardwareModel",
                column: "DriverId",
                principalTable: "Driver",
                principalColumn: "PK_Driver",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
