using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _300_RemoveOEMLogoAndDesktopThemeFromImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerImage_File_DesktopFileId",
                table: "CustomerImage");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerImage_File_OEMLogoId",
                table: "CustomerImage");

            migrationBuilder.DropForeignKey(
                name: "FK_Image_File_DesktopFileId",
                table: "Image");

            migrationBuilder.DropForeignKey(
                name: "FK_Image_File_OEMLogoId",
                table: "Image");

            migrationBuilder.DropIndex(
                name: "IX_Image_DesktopFileId",
                table: "Image");

            migrationBuilder.DropIndex(
                name: "IX_Image_OEMLogoId",
                table: "Image");

            migrationBuilder.DropIndex(
                name: "IX_CustomerImage_DesktopFileId",
                table: "CustomerImage");

            migrationBuilder.DropIndex(
                name: "IX_CustomerImage_OEMLogoId",
                table: "CustomerImage");

            migrationBuilder.DropColumn(
                name: "DesktopFileId",
                table: "Image");

            migrationBuilder.DropColumn(
                name: "OEMLogoId",
                table: "Image");

            migrationBuilder.DropColumn(
                name: "DesktopFileId",
                table: "CustomerImage");

            migrationBuilder.DropColumn(
                name: "OEMLogoId",
                table: "CustomerImage");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DesktopFileId",
                table: "Image",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OEMLogoId",
                table: "Image",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DesktopFileId",
                table: "CustomerImage",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OEMLogoId",
                table: "CustomerImage",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Image_DesktopFileId",
                table: "Image",
                column: "DesktopFileId");

            migrationBuilder.CreateIndex(
                name: "IX_Image_OEMLogoId",
                table: "Image",
                column: "OEMLogoId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerImage_DesktopFileId",
                table: "CustomerImage",
                column: "DesktopFileId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerImage_OEMLogoId",
                table: "CustomerImage",
                column: "OEMLogoId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerImage_File_DesktopFileId",
                table: "CustomerImage",
                column: "DesktopFileId",
                principalTable: "File",
                principalColumn: "PK_File",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerImage_File_OEMLogoId",
                table: "CustomerImage",
                column: "OEMLogoId",
                principalTable: "File",
                principalColumn: "PK_File",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Image_File_DesktopFileId",
                table: "Image",
                column: "DesktopFileId",
                principalTable: "File",
                principalColumn: "PK_File",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Image_File_OEMLogoId",
                table: "Image",
                column: "OEMLogoId",
                principalTable: "File",
                principalColumn: "PK_File",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
