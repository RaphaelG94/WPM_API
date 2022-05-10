using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _198_RemoveIconFromSoftwareAndShiftToStream : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Software_File_IconId",
                table: "Software");

            migrationBuilder.DropIndex(
                name: "IX_Software_IconId",
                table: "Software");

            migrationBuilder.DropColumn(
                name: "IconId",
                table: "Software");

            migrationBuilder.AddColumn<string>(
                name: "IconId",
                table: "SoftwareStream",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IconId",
                table: "CustomerSoftwareStream",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SoftwareStream_IconId",
                table: "SoftwareStream",
                column: "IconId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerSoftwareStream_IconId",
                table: "CustomerSoftwareStream",
                column: "IconId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerSoftwareStream_File_IconId",
                table: "CustomerSoftwareStream",
                column: "IconId",
                principalTable: "File",
                principalColumn: "PK_File",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SoftwareStream_File_IconId",
                table: "SoftwareStream",
                column: "IconId",
                principalTable: "File",
                principalColumn: "PK_File",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerSoftwareStream_File_IconId",
                table: "CustomerSoftwareStream");

            migrationBuilder.DropForeignKey(
                name: "FK_SoftwareStream_File_IconId",
                table: "SoftwareStream");

            migrationBuilder.DropIndex(
                name: "IX_SoftwareStream_IconId",
                table: "SoftwareStream");

            migrationBuilder.DropIndex(
                name: "IX_CustomerSoftwareStream_IconId",
                table: "CustomerSoftwareStream");

            migrationBuilder.DropColumn(
                name: "IconId",
                table: "SoftwareStream");

            migrationBuilder.DropColumn(
                name: "IconId",
                table: "CustomerSoftwareStream");

            migrationBuilder.AddColumn<string>(
                name: "IconId",
                table: "Software",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Software_IconId",
                table: "Software",
                column: "IconId");

            migrationBuilder.AddForeignKey(
                name: "FK_Software_File_IconId",
                table: "Software",
                column: "IconId",
                principalTable: "File",
                principalColumn: "PK_File",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
