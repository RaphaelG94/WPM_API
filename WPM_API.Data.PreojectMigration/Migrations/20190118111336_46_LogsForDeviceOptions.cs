using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _46_LogsForDeviceOptions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClientId",
                table: "ExecutionLog",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExecutionLog_ClientId",
                table: "ExecutionLog",
                column: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExecutionLog_Client_ClientId",
                table: "ExecutionLog",
                column: "ClientId",
                principalTable: "Client",
                principalColumn: "PK_Client",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExecutionLog_Client_ClientId",
                table: "ExecutionLog");

            migrationBuilder.DropIndex(
                name: "IX_ExecutionLog_ClientId",
                table: "ExecutionLog");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "ExecutionLog");
        }
    }
}
