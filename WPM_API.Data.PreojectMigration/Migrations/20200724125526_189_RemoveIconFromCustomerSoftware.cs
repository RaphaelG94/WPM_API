using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _189_RemoveIconFromCustomerSoftware : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerSoftware_File_IconId",
                table: "CustomerSoftware");

            migrationBuilder.DropIndex(
                name: "IX_CustomerSoftware_IconId",
                table: "CustomerSoftware");

            migrationBuilder.DropColumn(
                name: "IconId",
                table: "CustomerSoftware");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IconId",
                table: "CustomerSoftware",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerSoftware_IconId",
                table: "CustomerSoftware",
                column: "IconId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerSoftware_File_IconId",
                table: "CustomerSoftware",
                column: "IconId",
                principalTable: "File",
                principalColumn: "PK_File",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
