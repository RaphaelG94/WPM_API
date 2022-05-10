using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _255_AddRevisionNrToImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageId",
                table: "RevisionMessage",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DisplayRevisionNumber",
                table: "Image",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RevisionNumber",
                table: "Image",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DisplayRevisionNumber",
                table: "CustomerImage",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<string>(
                name: "RevisionNumber",
                table: "CustomerImage",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "RevisionMessage");

            migrationBuilder.DropColumn(
                name: "DisplayRevisionNumber",
                table: "Image");

            migrationBuilder.DropColumn(
                name: "RevisionNumber",
                table: "Image");

            migrationBuilder.DropColumn(
                name: "DisplayRevisionNumber",
                table: "CustomerImage");

            migrationBuilder.DropColumn(
                name: "RevisionNumber",
                table: "CustomerImage");
        }
    }
}
