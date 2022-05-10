using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _251_AddMissingPropsImageStream : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Architecture",
                table: "ImageStream",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionShort",
                table: "ImageStream",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IconId",
                table: "ImageStream",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Language",
                table: "ImageStream",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Vendor",
                table: "ImageStream",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Website",
                table: "ImageStream",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ImageStream_IconId",
                table: "ImageStream",
                column: "IconId");

            migrationBuilder.AddForeignKey(
                name: "FK_ImageStream_File_IconId",
                table: "ImageStream",
                column: "IconId",
                principalTable: "File",
                principalColumn: "PK_File",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImageStream_File_IconId",
                table: "ImageStream");

            migrationBuilder.DropIndex(
                name: "IX_ImageStream_IconId",
                table: "ImageStream");

            migrationBuilder.DropColumn(
                name: "Architecture",
                table: "ImageStream");

            migrationBuilder.DropColumn(
                name: "DescriptionShort",
                table: "ImageStream");

            migrationBuilder.DropColumn(
                name: "IconId",
                table: "ImageStream");

            migrationBuilder.DropColumn(
                name: "Language",
                table: "ImageStream");

            migrationBuilder.DropColumn(
                name: "Vendor",
                table: "ImageStream");

            migrationBuilder.DropColumn(
                name: "Website",
                table: "ImageStream");
        }
    }
}
