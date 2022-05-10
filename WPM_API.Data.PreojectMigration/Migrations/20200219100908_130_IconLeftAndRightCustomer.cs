using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _130_IconLeftAndRightCustomer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customer_File_IconId",
                table: "Customer");

            migrationBuilder.RenameColumn(
                name: "IconId",
                table: "Customer",
                newName: "IconRightId");

            migrationBuilder.RenameIndex(
                name: "IX_Customer_IconId",
                table: "Customer",
                newName: "IX_Customer_IconRightId");

            migrationBuilder.AddColumn<string>(
                name: "IconLeftId",
                table: "Customer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customer_IconLeftId",
                table: "Customer",
                column: "IconLeftId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_File_IconLeftId",
                table: "Customer",
                column: "IconLeftId",
                principalTable: "File",
                principalColumn: "PK_File",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_File_IconRightId",
                table: "Customer",
                column: "IconRightId",
                principalTable: "File",
                principalColumn: "PK_File",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customer_File_IconLeftId",
                table: "Customer");

            migrationBuilder.DropForeignKey(
                name: "FK_Customer_File_IconRightId",
                table: "Customer");

            migrationBuilder.DropIndex(
                name: "IX_Customer_IconLeftId",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "IconLeftId",
                table: "Customer");

            migrationBuilder.RenameColumn(
                name: "IconRightId",
                table: "Customer",
                newName: "IconId");

            migrationBuilder.RenameIndex(
                name: "IX_Customer_IconRightId",
                table: "Customer",
                newName: "IX_Customer_IconId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_File_IconId",
                table: "Customer",
                column: "IconId",
                principalTable: "File",
                principalColumn: "PK_File",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
