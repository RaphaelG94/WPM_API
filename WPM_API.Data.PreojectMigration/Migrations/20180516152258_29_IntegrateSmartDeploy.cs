using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _29_IntegrateSmartDeploy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateTable(
                name: "ClientTask",
                columns: table => new
                {
                    PK_ClientTask = table.Column<string>(nullable: false),
                    ClientId = table.Column<string>(nullable: true),
                    TaskId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientTask", x => x.PK_ClientTask);
                    table.ForeignKey(
                        name: "FK_ClientTask_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "PK_Client",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClientTask_Task_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Task",
                        principalColumn: "PK_Task",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Status",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ClientTaskId = table.Column<string>(nullable: true),
                    CreatedByUserId = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    DeletedByUserId = table.Column<string>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    Message = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    PredecessorId = table.Column<string>(nullable: true),
                    UpdatedByUserId = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Status", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Status_ClientTask_ClientTaskId",
                        column: x => x.ClientTaskId,
                        principalTable: "ClientTask",
                        principalColumn: "PK_ClientTask",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClientTask_ClientId",
                table: "ClientTask",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientTask_TaskId",
                table: "ClientTask",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Status_ClientTaskId",
                table: "Status",
                column: "ClientTaskId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Status");

            migrationBuilder.DropTable(
                name: "ClientTask");

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
        }
    }
}
