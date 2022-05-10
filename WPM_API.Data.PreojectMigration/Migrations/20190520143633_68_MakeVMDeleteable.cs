using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _68_MakeVMDeleteable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserId",
                table: "VirtualMachine",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "VirtualMachine",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DeletedByUserId",
                table: "VirtualMachine",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "VirtualMachine",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedByUserId",
                table: "VirtualMachine",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "VirtualMachine",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "VirtualMachine");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "VirtualMachine");

            migrationBuilder.DropColumn(
                name: "DeletedByUserId",
                table: "VirtualMachine");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "VirtualMachine");

            migrationBuilder.DropColumn(
                name: "UpdatedByUserId",
                table: "VirtualMachine");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "VirtualMachine");
        }
    }
}
