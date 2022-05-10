using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _267_AddEditionToImageStreams : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Edition",
                table: "ImageStream",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Edition",
                table: "CustomerImageStream",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Edition",
                table: "ImageStream");

            migrationBuilder.DropColumn(
                name: "Edition",
                table: "CustomerImageStream");
        }
    }
}
