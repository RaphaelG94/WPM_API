using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _269_AddUrlPrefixAndSASToImageStream : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PrefixUrl",
                table: "ImageStream",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SASKey",
                table: "ImageStream",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PrefixUrl",
                table: "CustomerImageStream",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SASKey",
                table: "CustomerImageStream",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrefixUrl",
                table: "ImageStream");

            migrationBuilder.DropColumn(
                name: "SASKey",
                table: "ImageStream");

            migrationBuilder.DropColumn(
                name: "PrefixUrl",
                table: "CustomerImageStream");

            migrationBuilder.DropColumn(
                name: "SASKey",
                table: "CustomerImageStream");
        }
    }
}
