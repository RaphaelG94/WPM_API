using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _62_AddCustomerReferenceToStorageAccounts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CustomerId",
                table: "StorageAccount",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StorageAccount_CustomerId",
                table: "StorageAccount",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_StorageAccount_Customer_CustomerId",
                table: "StorageAccount",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "PK_Customer",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StorageAccount_Customer_CustomerId",
                table: "StorageAccount");

            migrationBuilder.DropIndex(
                name: "IX_StorageAccount_CustomerId",
                table: "StorageAccount");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "StorageAccount");
        }
    }
}
