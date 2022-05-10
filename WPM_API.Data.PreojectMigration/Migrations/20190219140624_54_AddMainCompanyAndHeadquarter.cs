using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _54_AddMainCompanyAndHeadquarter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MainCompanyId",
                table: "Customer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HeadquarterId",
                table: "Company",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customer_MainCompanyId",
                table: "Customer",
                column: "MainCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Company_HeadquarterId",
                table: "Company",
                column: "HeadquarterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Company_Location_HeadquarterId",
                table: "Company",
                column: "HeadquarterId",
                principalTable: "Location",
                principalColumn: "PK_Location",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_Company_MainCompanyId",
                table: "Customer",
                column: "MainCompanyId",
                principalTable: "Company",
                principalColumn: "PK_Company",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Company_Location_HeadquarterId",
                table: "Company");

            migrationBuilder.DropForeignKey(
                name: "FK_Customer_Company_MainCompanyId",
                table: "Customer");

            migrationBuilder.DropIndex(
                name: "IX_Customer_MainCompanyId",
                table: "Customer");

            migrationBuilder.DropIndex(
                name: "IX_Company_HeadquarterId",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "MainCompanyId",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "HeadquarterId",
                table: "Company");
        }
    }
}
