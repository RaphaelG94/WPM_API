using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _197_AddCustomerReferenceToReleasePlan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CustomerId",
                table: "ReleasePlan",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReleasePlan_CustomerId",
                table: "ReleasePlan",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReleasePlan_Customer_CustomerId",
                table: "ReleasePlan",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "PK_Customer",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReleasePlan_Customer_CustomerId",
                table: "ReleasePlan");

            migrationBuilder.DropIndex(
                name: "IX_ReleasePlan_CustomerId",
                table: "ReleasePlan");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "ReleasePlan");
        }
    }
}
