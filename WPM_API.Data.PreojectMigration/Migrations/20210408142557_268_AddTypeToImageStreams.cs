using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _268_AddTypeToImageStreams : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "ImageStream",
                defaultValue: "Windows",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "CustomerImageStream",
                defaultValue: "Windows",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "ImageStream");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "CustomerImageStream");
        }
    }
}
