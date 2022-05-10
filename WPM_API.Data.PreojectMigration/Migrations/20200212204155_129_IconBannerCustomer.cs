using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _129_IconBannerCustomer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BannerId",
                table: "Customer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IconId",
                table: "Customer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customer_BannerId",
                table: "Customer",
                column: "BannerId");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_IconId",
                table: "Customer",
                column: "IconId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_File_BannerId",
                table: "Customer",
                column: "BannerId",
                principalTable: "File",
                principalColumn: "PK_File",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_File_IconId",
                table: "Customer",
                column: "IconId",
                principalTable: "File",
                principalColumn: "PK_File",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customer_File_BannerId",
                table: "Customer");

            migrationBuilder.DropForeignKey(
                name: "FK_Customer_File_IconId",
                table: "Customer");

            migrationBuilder.DropIndex(
                name: "IX_Customer_BannerId",
                table: "Customer");

            migrationBuilder.DropIndex(
                name: "IX_Customer_IconId",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "BannerId",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "IconId",
                table: "Customer");
        }
    }
}
