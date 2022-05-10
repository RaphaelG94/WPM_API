using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _229_AddFilesToCustomerImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DesktopFileId",
                table: "CustomerImage",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OEMLogoId",
                table: "CustomerImage",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OEMPartitionId",
                table: "CustomerImage",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UnattendId",
                table: "CustomerImage",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerImage_DesktopFileId",
                table: "CustomerImage",
                column: "DesktopFileId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerImage_OEMLogoId",
                table: "CustomerImage",
                column: "OEMLogoId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerImage_OEMPartitionId",
                table: "CustomerImage",
                column: "OEMPartitionId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerImage_UnattendId",
                table: "CustomerImage",
                column: "UnattendId");

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
                name: "FK_CustomerImage_File_OEMPartitionId",
                table: "CustomerImage",
                column: "OEMPartitionId",
                principalTable: "File",
                principalColumn: "PK_File",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerImage_File_UnattendId",
                table: "CustomerImage",
                column: "UnattendId",
                principalTable: "File",
                principalColumn: "PK_File",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerImage_File_DesktopFileId",
                table: "CustomerImage");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerImage_File_OEMLogoId",
                table: "CustomerImage");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerImage_File_OEMPartitionId",
                table: "CustomerImage");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerImage_File_UnattendId",
                table: "CustomerImage");

            migrationBuilder.DropIndex(
                name: "IX_CustomerImage_DesktopFileId",
                table: "CustomerImage");

            migrationBuilder.DropIndex(
                name: "IX_CustomerImage_OEMLogoId",
                table: "CustomerImage");

            migrationBuilder.DropIndex(
                name: "IX_CustomerImage_OEMPartitionId",
                table: "CustomerImage");

            migrationBuilder.DropIndex(
                name: "IX_CustomerImage_UnattendId",
                table: "CustomerImage");

            migrationBuilder.DropColumn(
                name: "DesktopFileId",
                table: "CustomerImage");

            migrationBuilder.DropColumn(
                name: "OEMLogoId",
                table: "CustomerImage");

            migrationBuilder.DropColumn(
                name: "OEMPartitionId",
                table: "CustomerImage");

            migrationBuilder.DropColumn(
                name: "UnattendId",
                table: "CustomerImage");
        }
    }
}
