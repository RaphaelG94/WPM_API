using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _185_RemoveCustomerFromSoftwareStream : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SoftwareStream_Customer_CustomerId",
                table: "SoftwareStream");

            migrationBuilder.DropIndex(
                name: "IX_SoftwareStream_CustomerId",
                table: "SoftwareStream");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "SoftwareStream");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CustomerId",
                table: "SoftwareStream",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SoftwareStream_CustomerId",
                table: "SoftwareStream",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_SoftwareStream_Customer_CustomerId",
                table: "SoftwareStream",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "PK_Customer",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
