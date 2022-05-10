using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _51_ConnectPersonCompanyCustomer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customer_Person_ExpertId",
                table: "Customer");

            migrationBuilder.DropIndex(
                name: "IX_Customer_ExpertId",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "ExpertId",
                table: "Customer");

            migrationBuilder.AddColumn<string>(
                name: "CustomerId",
                table: "Person",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Person_CustomerId",
                table: "Person",
                column: "CustomerId",
                unique: true,
                filter: "[CustomerId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Person_Customer_CustomerId",
                table: "Person",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "PK_Customer",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Person_Customer_CustomerId",
                table: "Person");

            migrationBuilder.DropIndex(
                name: "IX_Person_CustomerId",
                table: "Person");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "Person");

            migrationBuilder.AddColumn<string>(
                name: "ExpertId",
                table: "Customer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customer_ExpertId",
                table: "Customer",
                column: "ExpertId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_Person_ExpertId",
                table: "Customer",
                column: "ExpertId",
                principalTable: "Person",
                principalColumn: "PK_Person",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
