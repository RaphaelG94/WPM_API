using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _162_MakeSoftwareSysCustomerAndClientDeleteable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserId",
                table: "SoftwaresSystemhouse",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "SoftwaresSystemhouse",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DeletedByUserId",
                table: "SoftwaresSystemhouse",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "SoftwaresSystemhouse",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedByUserId",
                table: "SoftwaresSystemhouse",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "SoftwaresSystemhouse",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserId",
                table: "SoftwaresCustomer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "SoftwaresCustomer",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DeletedByUserId",
                table: "SoftwaresCustomer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "SoftwaresCustomer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedByUserId",
                table: "SoftwaresCustomer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "SoftwaresCustomer",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserId",
                table: "SoftwaresClient",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "SoftwaresClient",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DeletedByUserId",
                table: "SoftwaresClient",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "SoftwaresClient",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedByUserId",
                table: "SoftwaresClient",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "SoftwaresClient",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "SoftwaresSystemhouse");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "SoftwaresSystemhouse");

            migrationBuilder.DropColumn(
                name: "DeletedByUserId",
                table: "SoftwaresSystemhouse");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "SoftwaresSystemhouse");

            migrationBuilder.DropColumn(
                name: "UpdatedByUserId",
                table: "SoftwaresSystemhouse");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "SoftwaresSystemhouse");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "SoftwaresCustomer");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "SoftwaresCustomer");

            migrationBuilder.DropColumn(
                name: "DeletedByUserId",
                table: "SoftwaresCustomer");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "SoftwaresCustomer");

            migrationBuilder.DropColumn(
                name: "UpdatedByUserId",
                table: "SoftwaresCustomer");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "SoftwaresCustomer");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "SoftwaresClient");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "SoftwaresClient");

            migrationBuilder.DropColumn(
                name: "DeletedByUserId",
                table: "SoftwaresClient");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "SoftwaresClient");

            migrationBuilder.DropColumn(
                name: "UpdatedByUserId",
                table: "SoftwaresClient");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "SoftwaresClient");
        }
    }
}
