using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _90_AddClientToWMIInventoryCmds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClientId",
                table: "WMIInvenotryCmds",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WMIInvenotryCmds_ClientId",
                table: "WMIInvenotryCmds",
                column: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_WMIInvenotryCmds_Client_ClientId",
                table: "WMIInvenotryCmds",
                column: "ClientId",
                principalTable: "Client",
                principalColumn: "PK_Client",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WMIInvenotryCmds_Client_ClientId",
                table: "WMIInvenotryCmds");

            migrationBuilder.DropIndex(
                name: "IX_WMIInvenotryCmds_ClientId",
                table: "WMIInvenotryCmds");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "WMIInvenotryCmds");
        }
    }
}
