using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _263_AddPatchInfoToImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PatchInfo",
                table: "Image",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PatchInfo",
                table: "CustomerImage",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PatchInfo",
                table: "Image");

            migrationBuilder.DropColumn(
                name: "PatchInfo",
                table: "CustomerImage");
        }
    }
}
