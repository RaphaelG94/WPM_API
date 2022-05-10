using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _72_AddResourceGroupToStorageAccount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ResourceGroupId",
                table: "StorageAccount",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StorageAccount_ResourceGroupId",
                table: "StorageAccount",
                column: "ResourceGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_StorageAccount_ResourceGroup_ResourceGroupId",
                table: "StorageAccount",
                column: "ResourceGroupId",
                principalTable: "ResourceGroup",
                principalColumn: "PK_ResourceGroup",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StorageAccount_ResourceGroup_ResourceGroupId",
                table: "StorageAccount");

            migrationBuilder.DropIndex(
                name: "IX_StorageAccount_ResourceGroupId",
                table: "StorageAccount");

            migrationBuilder.DropColumn(
                name: "ResourceGroupId",
                table: "StorageAccount");
        }
    }
}
