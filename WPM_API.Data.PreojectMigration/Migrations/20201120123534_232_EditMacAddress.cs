using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _232_EditMacAddress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserId",
                table: "MacAddress",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "MacAddress",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DeletedByUserId",
                table: "MacAddress",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "MacAddress",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedByUserId",
                table: "MacAddress",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "MacAddress",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "MacAddress");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "MacAddress");

            migrationBuilder.DropColumn(
                name: "DeletedByUserId",
                table: "MacAddress");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "MacAddress");

            migrationBuilder.DropColumn(
                name: "UpdatedByUserId",
                table: "MacAddress");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "MacAddress");
        }
    }
}
