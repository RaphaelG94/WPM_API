using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _16_AddUserInExecutionLog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExecutionLog_User_UserIdId",
                table: "ExecutionLog");

            migrationBuilder.RenameColumn(
                name: "UserIdId",
                table: "ExecutionLog",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ExecutionLog_UserIdId",
                table: "ExecutionLog",
                newName: "IX_ExecutionLog_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExecutionLog_User_UserId",
                table: "ExecutionLog",
                column: "UserId",
                principalTable: "User",
                principalColumn: "PK_User",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExecutionLog_User_UserId",
                table: "ExecutionLog");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "ExecutionLog",
                newName: "UserIdId");

            migrationBuilder.RenameIndex(
                name: "IX_ExecutionLog_UserId",
                table: "ExecutionLog",
                newName: "IX_ExecutionLog_UserIdId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExecutionLog_User_UserIdId",
                table: "ExecutionLog",
                column: "UserIdId",
                principalTable: "User",
                principalColumn: "PK_User",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
