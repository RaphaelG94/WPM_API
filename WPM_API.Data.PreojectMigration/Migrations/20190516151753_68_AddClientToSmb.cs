using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _68_AddClientToSmb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClientId",
                table: "SmbStorage",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SmbStorage_ClientId",
                table: "SmbStorage",
                column: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_SmbStorage_Client_ClientId",
                table: "SmbStorage",
                column: "ClientId",
                principalTable: "Client",
                principalColumn: "PK_Client",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SmbStorage_Client_ClientId",
                table: "SmbStorage");

            migrationBuilder.DropIndex(
                name: "IX_SmbStorage_ClientId",
                table: "SmbStorage");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "SmbStorage");
        }
    }
}
