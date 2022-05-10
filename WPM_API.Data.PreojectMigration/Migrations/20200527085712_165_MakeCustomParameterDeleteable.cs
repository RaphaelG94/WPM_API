using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _165_MakeCustomParameterDeleteable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserId",
                table: "Parameter",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Parameter",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DeletedByUserId",
                table: "Parameter",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "Parameter",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedByUserId",
                table: "Parameter",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Parameter",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "Parameter");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Parameter");

            migrationBuilder.DropColumn(
                name: "DeletedByUserId",
                table: "Parameter");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "Parameter");

            migrationBuilder.DropColumn(
                name: "UpdatedByUserId",
                table: "Parameter");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Parameter");
        }
    }
}
