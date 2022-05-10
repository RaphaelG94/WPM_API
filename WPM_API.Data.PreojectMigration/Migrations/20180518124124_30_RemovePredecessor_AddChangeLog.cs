using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _30_RemovePredecessor_AddChangeLog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PredecessorId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "PredecessorId",
                table: "Task");

            migrationBuilder.DropColumn(
                name: "PredecessorId",
                table: "Systemhouse");

            migrationBuilder.DropColumn(
                name: "PredecessorId",
                table: "Status");

            migrationBuilder.DropColumn(
                name: "PredecessorId",
                table: "Software");

            migrationBuilder.DropColumn(
                name: "PredecessorId",
                table: "ShopItem");

            migrationBuilder.DropColumn(
                name: "PredecessorId",
                table: "Script");

            migrationBuilder.DropColumn(
                name: "PredecessorId",
                table: "Rule");

            migrationBuilder.DropColumn(
                name: "PredecessorId",
                table: "Domain");

            migrationBuilder.DropColumn(
                name: "PredecessorId",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "PredecessorId",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "PredecessorId",
                table: "Base");

            migrationBuilder.CreateTable(
                name: "ChangeLog",
                columns: table => new
                {
                    PK_ChangeLog = table.Column<string>(nullable: false),
                    DateChanged = table.Column<DateTime>(nullable: false),
                    EntityName = table.Column<string>(nullable: true),
                    NewValue = table.Column<string>(nullable: true),
                    OldValue = table.Column<string>(nullable: true),
                    PrimaryKeyValue = table.Column<string>(nullable: true),
                    PropertyName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChangeLog", x => x.PK_ChangeLog);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChangeLog");

            migrationBuilder.AddColumn<string>(
                name: "PredecessorId",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PredecessorId",
                table: "Task",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PredecessorId",
                table: "Systemhouse",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PredecessorId",
                table: "Status",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PredecessorId",
                table: "Software",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PredecessorId",
                table: "ShopItem",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PredecessorId",
                table: "Script",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PredecessorId",
                table: "Rule",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PredecessorId",
                table: "Domain",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PredecessorId",
                table: "Customer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PredecessorId",
                table: "Client",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PredecessorId",
                table: "Base",
                nullable: true);
        }
    }
}
