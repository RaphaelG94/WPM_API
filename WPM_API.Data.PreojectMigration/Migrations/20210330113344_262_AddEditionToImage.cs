using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _262_AddEditionToImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Edition",
                table: "Image",
                defaultValue: "Enterprise",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Edition",
                table: "CustomerImage",
                defaultValue: "Enterprise",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Edition",
                table: "Image");

            migrationBuilder.DropColumn(
                name: "Edition",
                table: "CustomerImage");
        }
    }
}
