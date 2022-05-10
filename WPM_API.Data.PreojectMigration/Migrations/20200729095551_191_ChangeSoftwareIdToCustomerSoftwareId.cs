using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _191_ChangeSoftwareIdToCustomerSoftwareId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientSoftware_CustomerSoftware_SoftwareId",
                table: "ClientSoftware");

            migrationBuilder.DropForeignKey(
                name: "FK_ClientSoftware_Software_SoftwareId1",
                table: "ClientSoftware");

            migrationBuilder.RenameColumn(
                name: "SoftwareId1",
                table: "ClientSoftware",
                newName: "CustomerSoftwareId");

            migrationBuilder.RenameIndex(
                name: "IX_ClientSoftware_SoftwareId1",
                table: "ClientSoftware",
                newName: "IX_ClientSoftware_CustomerSoftwareId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientSoftware_CustomerSoftware_CustomerSoftwareId",
                table: "ClientSoftware",
                column: "CustomerSoftwareId",
                principalTable: "CustomerSoftware",
                principalColumn: "PK_CustomerSoftware",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientSoftware_Software_SoftwareId",
                table: "ClientSoftware",
                column: "SoftwareId",
                principalTable: "Software",
                principalColumn: "PK_Software",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientSoftware_CustomerSoftware_CustomerSoftwareId",
                table: "ClientSoftware");

            migrationBuilder.DropForeignKey(
                name: "FK_ClientSoftware_Software_SoftwareId",
                table: "ClientSoftware");

            migrationBuilder.RenameColumn(
                name: "CustomerSoftwareId",
                table: "ClientSoftware",
                newName: "SoftwareId1");

            migrationBuilder.RenameIndex(
                name: "IX_ClientSoftware_CustomerSoftwareId",
                table: "ClientSoftware",
                newName: "IX_ClientSoftware_SoftwareId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientSoftware_CustomerSoftware_SoftwareId",
                table: "ClientSoftware",
                column: "SoftwareId",
                principalTable: "CustomerSoftware",
                principalColumn: "PK_CustomerSoftware",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientSoftware_Software_SoftwareId1",
                table: "ClientSoftware",
                column: "SoftwareId1",
                principalTable: "Software",
                principalColumn: "PK_Software",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
