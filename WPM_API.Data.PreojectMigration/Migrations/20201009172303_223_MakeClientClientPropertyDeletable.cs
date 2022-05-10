using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _223_MakeClientClientPropertyDeletable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserId",
                table: "ClientClientProperty",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "ClientClientProperty",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DeletedByUserId",
                table: "ClientClientProperty",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "ClientClientProperty",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedByUserId",
                table: "ClientClientProperty",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "ClientClientProperty",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "ClientClientProperty");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "ClientClientProperty");

            migrationBuilder.DropColumn(
                name: "DeletedByUserId",
                table: "ClientClientProperty");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "ClientClientProperty");

            migrationBuilder.DropColumn(
                name: "UpdatedByUserId",
                table: "ClientClientProperty");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "ClientClientProperty");
        }
    }
}
