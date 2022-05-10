using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _252_AddMissingPropsToCustomerImageStream : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Architecture",
                table: "CustomerImageStream",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "CustomerImageStream",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionShort",
                table: "CustomerImageStream",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IconId",
                table: "CustomerImageStream",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Language",
                table: "CustomerImageStream",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Vendor",
                table: "CustomerImageStream",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Website",
                table: "CustomerImageStream",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerImageStream_IconId",
                table: "CustomerImageStream",
                column: "IconId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerImageStream_File_IconId",
                table: "CustomerImageStream",
                column: "IconId",
                principalTable: "File",
                principalColumn: "PK_File",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerImageStream_File_IconId",
                table: "CustomerImageStream");

            migrationBuilder.DropIndex(
                name: "IX_CustomerImageStream_IconId",
                table: "CustomerImageStream");

            migrationBuilder.DropColumn(
                name: "Architecture",
                table: "CustomerImageStream");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "CustomerImageStream");

            migrationBuilder.DropColumn(
                name: "DescriptionShort",
                table: "CustomerImageStream");

            migrationBuilder.DropColumn(
                name: "IconId",
                table: "CustomerImageStream");

            migrationBuilder.DropColumn(
                name: "Language",
                table: "CustomerImageStream");

            migrationBuilder.DropColumn(
                name: "Vendor",
                table: "CustomerImageStream");

            migrationBuilder.DropColumn(
                name: "Website",
                table: "CustomerImageStream");
        }
    }
}
