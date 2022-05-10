using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _170_AddCustomerToBase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CustomerId",
                table: "Base",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Base_CustomerId",
                table: "Base",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Base_Customer_CustomerId",
                table: "Base",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "PK_Customer",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Base_Customer_CustomerId",
                table: "Base");

            migrationBuilder.DropIndex(
                name: "IX_Base_CustomerId",
                table: "Base");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "Base");
        }
    }
}
