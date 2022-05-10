using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _63_AddCustomerToResourceGroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CustomerId",
                table: "ResourceGroup",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ResourceGroup_CustomerId",
                table: "ResourceGroup",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_ResourceGroup_Customer_CustomerId",
                table: "ResourceGroup",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "PK_Customer",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ResourceGroup_Customer_CustomerId",
                table: "ResourceGroup");

            migrationBuilder.DropIndex(
                name: "IX_ResourceGroup_CustomerId",
                table: "ResourceGroup");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "ResourceGroup");
        }
    }
}
