using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _224_MakeClientSoftwareDeletable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserId",
                table: "ClientSoftware",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "ClientSoftware",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DeletedByUserId",
                table: "ClientSoftware",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "ClientSoftware",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedByUserId",
                table: "ClientSoftware",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "ClientSoftware",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "ClientSoftware");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "ClientSoftware");

            migrationBuilder.DropColumn(
                name: "DeletedByUserId",
                table: "ClientSoftware");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "ClientSoftware");

            migrationBuilder.DropColumn(
                name: "UpdatedByUserId",
                table: "ClientSoftware");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "ClientSoftware");
        }
    }
}
