using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _61_AddCustomerReferenceToAzureBlob : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CustomerId",
                table: "AzureBlobStorage",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AzureBlobStorage_CustomerId",
                table: "AzureBlobStorage",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_AzureBlobStorage_Customer_CustomerId",
                table: "AzureBlobStorage",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "PK_Customer",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AzureBlobStorage_Customer_CustomerId",
                table: "AzureBlobStorage");

            migrationBuilder.DropIndex(
                name: "IX_AzureBlobStorage_CustomerId",
                table: "AzureBlobStorage");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "AzureBlobStorage");
        }
    }
}
