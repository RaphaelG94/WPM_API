using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _181_MakeExecutionLogDeleteable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserId",
                table: "ExecutionLog",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "ExecutionLog",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DeletedByUserId",
                table: "ExecutionLog",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "ExecutionLog",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedByUserId",
                table: "ExecutionLog",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "ExecutionLog",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "ExecutionLog");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "ExecutionLog");

            migrationBuilder.DropColumn(
                name: "DeletedByUserId",
                table: "ExecutionLog");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "ExecutionLog");

            migrationBuilder.DropColumn(
                name: "UpdatedByUserId",
                table: "ExecutionLog");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "ExecutionLog");
        }
    }
}
