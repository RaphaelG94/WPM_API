using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _182_MakeBaseRelationsDeleteable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserId",
                table: "Vpn",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Vpn",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DeletedByUserId",
                table: "Vpn",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "Vpn",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedByUserId",
                table: "Vpn",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Vpn",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserId",
                table: "VirtualNetwork",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "VirtualNetwork",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DeletedByUserId",
                table: "VirtualNetwork",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "VirtualNetwork",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedByUserId",
                table: "VirtualNetwork",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "VirtualNetwork",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserId",
                table: "Subscription",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Subscription",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DeletedByUserId",
                table: "Subscription",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "Subscription",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedByUserId",
                table: "Subscription",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Subscription",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserId",
                table: "StorageAccount",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "StorageAccount",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DeletedByUserId",
                table: "StorageAccount",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "StorageAccount",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedByUserId",
                table: "StorageAccount",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "StorageAccount",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserId",
                table: "ResourceGroup",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "ResourceGroup",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DeletedByUserId",
                table: "ResourceGroup",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "ResourceGroup",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedByUserId",
                table: "ResourceGroup",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "ResourceGroup",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserId",
                table: "AdvancedProperty",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "AdvancedProperty",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DeletedByUserId",
                table: "AdvancedProperty",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "AdvancedProperty",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedByUserId",
                table: "AdvancedProperty",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "AdvancedProperty",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "Vpn");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Vpn");

            migrationBuilder.DropColumn(
                name: "DeletedByUserId",
                table: "Vpn");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "Vpn");

            migrationBuilder.DropColumn(
                name: "UpdatedByUserId",
                table: "Vpn");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Vpn");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "VirtualNetwork");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "VirtualNetwork");

            migrationBuilder.DropColumn(
                name: "DeletedByUserId",
                table: "VirtualNetwork");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "VirtualNetwork");

            migrationBuilder.DropColumn(
                name: "UpdatedByUserId",
                table: "VirtualNetwork");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "VirtualNetwork");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "Subscription");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Subscription");

            migrationBuilder.DropColumn(
                name: "DeletedByUserId",
                table: "Subscription");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "Subscription");

            migrationBuilder.DropColumn(
                name: "UpdatedByUserId",
                table: "Subscription");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Subscription");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "StorageAccount");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "StorageAccount");

            migrationBuilder.DropColumn(
                name: "DeletedByUserId",
                table: "StorageAccount");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "StorageAccount");

            migrationBuilder.DropColumn(
                name: "UpdatedByUserId",
                table: "StorageAccount");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "StorageAccount");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "ResourceGroup");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "ResourceGroup");

            migrationBuilder.DropColumn(
                name: "DeletedByUserId",
                table: "ResourceGroup");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "ResourceGroup");

            migrationBuilder.DropColumn(
                name: "UpdatedByUserId",
                table: "ResourceGroup");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "ResourceGroup");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "AdvancedProperty");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "AdvancedProperty");

            migrationBuilder.DropColumn(
                name: "DeletedByUserId",
                table: "AdvancedProperty");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "AdvancedProperty");

            migrationBuilder.DropColumn(
                name: "UpdatedByUserId",
                table: "AdvancedProperty");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "AdvancedProperty");
        }
    }
}
