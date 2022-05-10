using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _08_AddAzureIds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubscriptionId",
                table: "Subscription");

            migrationBuilder.AddColumn<string>(
                name: "AzureId",
                table: "Subscription",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AzureId",
                table: "StorageAccount",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExecutionVMId",
                table: "Domain",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AzureId",
                table: "Subscription");

            migrationBuilder.DropColumn(
                name: "AzureId",
                table: "StorageAccount");

            migrationBuilder.DropColumn(
                name: "ExecutionVMId",
                table: "Domain");

            migrationBuilder.AddColumn<string>(
                name: "SubscriptionId",
                table: "Subscription",
                nullable: true);
        }
    }
}
