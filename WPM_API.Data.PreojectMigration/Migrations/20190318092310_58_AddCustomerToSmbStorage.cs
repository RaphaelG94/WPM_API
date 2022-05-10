using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _58_AddCustomerToSmbStorage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CustomerId",
                table: "SmbStorage",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SmbStorage_CustomerId",
                table: "SmbStorage",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_SmbStorage_Customer_CustomerId",
                table: "SmbStorage",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "PK_Customer",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SmbStorage_Customer_CustomerId",
                table: "SmbStorage");

            migrationBuilder.DropIndex(
                name: "IX_SmbStorage_CustomerId",
                table: "SmbStorage");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "SmbStorage");
        }
    }
}
