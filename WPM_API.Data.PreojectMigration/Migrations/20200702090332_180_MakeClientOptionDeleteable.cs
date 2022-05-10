using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _180_MakeClientOptionDeleteable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserId",
                table: "ClientOption",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "ClientOption",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DeletedByUserId",
                table: "ClientOption",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "ClientOption",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedByUserId",
                table: "ClientOption",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "ClientOption",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "ClientOption");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "ClientOption");

            migrationBuilder.DropColumn(
                name: "DeletedByUserId",
                table: "ClientOption");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "ClientOption");

            migrationBuilder.DropColumn(
                name: "UpdatedByUserId",
                table: "ClientOption");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "ClientOption");
        }
    }
}
