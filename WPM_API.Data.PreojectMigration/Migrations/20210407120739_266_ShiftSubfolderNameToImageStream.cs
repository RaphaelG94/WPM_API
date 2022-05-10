using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _266_ShiftSubfolderNameToImageStream : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubFolderName",
                table: "Image");

            migrationBuilder.DropColumn(
                name: "SubFolderName",
                table: "CustomerImage");

            migrationBuilder.AddColumn<string>(
                name: "SubFolderName",
                table: "ImageStream",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubFolderName",
                table: "CustomerImageStream",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubFolderName",
                table: "ImageStream");

            migrationBuilder.DropColumn(
                name: "SubFolderName",
                table: "CustomerImageStream");

            migrationBuilder.AddColumn<string>(
                name: "SubFolderName",
                table: "Image",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubFolderName",
                table: "CustomerImage",
                nullable: true);
        }
    }
}
