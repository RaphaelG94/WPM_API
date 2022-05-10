using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _210_AddFileNameToImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "Image",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerId",
                table: "CustomerImage",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileName",
                table: "Image");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "CustomerImage");
        }
    }
}
