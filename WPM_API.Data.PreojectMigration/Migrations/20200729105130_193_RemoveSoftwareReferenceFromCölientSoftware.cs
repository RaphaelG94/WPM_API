using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _193_RemoveSoftwareReferenceFromCölientSoftware : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientSoftware_CustomerSoftware_CustomerSoftwareId",
                table: "ClientSoftware");

            migrationBuilder.DropIndex(
                name: "IX_ClientSoftware_CustomerSoftwareId",
                table: "ClientSoftware");

            migrationBuilder.DropColumn(
                name: "CustomerSoftwareId",
                table: "ClientSoftware");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CustomerSoftwareId",
                table: "ClientSoftware",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClientSoftware_CustomerSoftwareId",
                table: "ClientSoftware",
                column: "CustomerSoftwareId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientSoftware_CustomerSoftware_CustomerSoftwareId",
                table: "ClientSoftware",
                column: "CustomerSoftwareId",
                principalTable: "CustomerSoftware",
                principalColumn: "PK_CustomerSoftware",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
