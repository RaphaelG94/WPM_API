using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _228_AddFilesToImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
                name: "OEMPartitionId",
                table: "Image",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UnattendId",
                table: "Image",
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
                name: "IX_Image_OEMPartitionId",
                table: "Image",
                column: "OEMPartitionId");

            migrationBuilder.CreateIndex(
                name: "IX_Image_UnattendId",
                table: "Image",
                column: "UnattendId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Image_File_OEMPartitionId",
                table: "Image",
                column: "OEMPartitionId",
                principalTable: "File",
                principalColumn: "PK_File",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Image_File_UnattendId",
                table: "Image",
                column: "UnattendId",
                principalTable: "File",
                principalColumn: "PK_File",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Image_File_DesktopFileId",
                table: "Image");

            migrationBuilder.DropForeignKey(
                name: "FK_Image_File_OEMLogoId",
                table: "Image");

            migrationBuilder.DropForeignKey(
                name: "FK_Image_File_OEMPartitionId",
                table: "Image");

            migrationBuilder.DropForeignKey(
                name: "FK_Image_File_UnattendId",
                table: "Image");

            migrationBuilder.DropIndex(
                name: "IX_Image_DesktopFileId",
                table: "Image");

            migrationBuilder.DropIndex(
                name: "IX_Image_OEMLogoId",
                table: "Image");

            migrationBuilder.DropIndex(
                name: "IX_Image_OEMPartitionId",
                table: "Image");

            migrationBuilder.DropIndex(
                name: "IX_Image_UnattendId",
                table: "Image");

            migrationBuilder.DropColumn(
                name: "DesktopFileId",
                table: "Image");

            migrationBuilder.DropColumn(
                name: "OEMLogoId",
                table: "Image");

            migrationBuilder.DropColumn(
                name: "OEMPartitionId",
                table: "Image");

            migrationBuilder.DropColumn(
                name: "UnattendId",
                table: "Image");
        }
    }
}
