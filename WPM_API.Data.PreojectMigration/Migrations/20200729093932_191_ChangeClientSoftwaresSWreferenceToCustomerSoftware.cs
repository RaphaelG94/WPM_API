using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _191_ChangeClientSoftwaresSWreferenceToCustomerSoftware : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientSoftware_Software_SoftwareId",
                table: "ClientSoftware");

            migrationBuilder.AddColumn<string>(
                name: "SoftwareId1",
                table: "ClientSoftware",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClientSoftware_SoftwareId1",
                table: "ClientSoftware",
                column: "SoftwareId1");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientSoftware_CustomerSoftware_SoftwareId",
                table: "ClientSoftware");

            migrationBuilder.DropForeignKey(
                name: "FK_ClientSoftware_Software_SoftwareId1",
                table: "ClientSoftware");

            migrationBuilder.DropIndex(
                name: "IX_ClientSoftware_SoftwareId1",
                table: "ClientSoftware");

            migrationBuilder.DropColumn(
                name: "SoftwareId1",
                table: "ClientSoftware");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientSoftware_Software_SoftwareId",
                table: "ClientSoftware",
                column: "SoftwareId",
                principalTable: "Software",
                principalColumn: "PK_Software",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
