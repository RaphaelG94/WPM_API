using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _64_AddSubscriptionToAzureBlob : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SubscriptionId",
                table: "AzureBlobStorage",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AzureBlobStorage_SubscriptionId",
                table: "AzureBlobStorage",
                column: "SubscriptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_AzureBlobStorage_Subscription_SubscriptionId",
                table: "AzureBlobStorage",
                column: "SubscriptionId",
                principalTable: "Subscription",
                principalColumn: "PK_Subscription",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AzureBlobStorage_Subscription_SubscriptionId",
                table: "AzureBlobStorage");

            migrationBuilder.DropIndex(
                name: "IX_AzureBlobStorage_SubscriptionId",
                table: "AzureBlobStorage");

            migrationBuilder.DropColumn(
                name: "SubscriptionId",
                table: "AzureBlobStorage");
        }
    }
}
