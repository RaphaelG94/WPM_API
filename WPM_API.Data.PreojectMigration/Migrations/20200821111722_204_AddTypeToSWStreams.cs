using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _204_AddTypeToSWStreams : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "SoftwareStream",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "CustomerSoftwareStream",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "SoftwareStream");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "CustomerSoftwareStream");
        }
    }
}
