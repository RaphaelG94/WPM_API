using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _161_AddClientsToSWv2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SoftwaresClient_Customer_ClientId",
                table: "SoftwaresClient");

            migrationBuilder.AddForeignKey(
                name: "FK_SoftwaresClient_Client_ClientId",
                table: "SoftwaresClient",
                column: "ClientId",
                principalTable: "Client",
                principalColumn: "PK_Client",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SoftwaresClient_Client_ClientId",
                table: "SoftwaresClient");

            migrationBuilder.AddForeignKey(
                name: "FK_SoftwaresClient_Customer_ClientId",
                table: "SoftwaresClient",
                column: "ClientId",
                principalTable: "Customer",
                principalColumn: "PK_Customer",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
