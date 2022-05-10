using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _264_RemovePropertiesFromImageCustomerImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Architecture",
                table: "Image");

            migrationBuilder.DropColumn(
                name: "Edition",
                table: "Image");

            migrationBuilder.DropColumn(
                name: "Language",
                table: "Image");

            migrationBuilder.DropColumn(
                name: "PatchInfo",
                table: "Image");

            migrationBuilder.DropColumn(
                name: "Vendor",
                table: "Image");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "Image");

            migrationBuilder.DropColumn(
                name: "Architecture",
                table: "CustomerImage");

            migrationBuilder.DropColumn(
                name: "Edition",
                table: "CustomerImage");

            migrationBuilder.DropColumn(
                name: "Language",
                table: "CustomerImage");

            migrationBuilder.DropColumn(
                name: "PatchInfo",
                table: "CustomerImage");

            migrationBuilder.DropColumn(
                name: "Vendor",
                table: "CustomerImage");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "CustomerImage");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Architecture",
                table: "Image",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Edition",
                table: "Image",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Language",
                table: "Image",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PatchInfo",
                table: "Image",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Vendor",
                table: "Image",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Version",
                table: "Image",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Architecture",
                table: "CustomerImage",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Edition",
                table: "CustomerImage",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Language",
                table: "CustomerImage",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PatchInfo",
                table: "CustomerImage",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Vendor",
                table: "CustomerImage",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Version",
                table: "CustomerImage",
                nullable: true);
        }
    }
}
