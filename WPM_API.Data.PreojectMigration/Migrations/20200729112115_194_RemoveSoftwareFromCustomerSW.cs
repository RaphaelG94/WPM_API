using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _194_RemoveSoftwareFromCustomerSW : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerSoftware_Software_SoftwareId",
                table: "CustomerSoftware");

            migrationBuilder.DropIndex(
                name: "IX_CustomerSoftware_SoftwareId",
                table: "CustomerSoftware");

            migrationBuilder.AlterColumn<string>(
                name: "SoftwareId",
                table: "CustomerSoftware",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SoftwareId",
                table: "CustomerSoftware",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerSoftware_SoftwareId",
                table: "CustomerSoftware",
                column: "SoftwareId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerSoftware_Software_SoftwareId",
                table: "CustomerSoftware",
                column: "SoftwareId",
                principalTable: "Software",
                principalColumn: "PK_Software",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
