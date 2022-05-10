using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _254_AddIdOfImageStream : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CustomerId",
                table: "CustomerImageStream",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageStreamId",
                table: "CustomerImageStream",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerImageStream_CustomerId",
                table: "CustomerImageStream",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerImageStream_Customer_CustomerId",
                table: "CustomerImageStream",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "PK_Customer",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerImageStream_Customer_CustomerId",
                table: "CustomerImageStream");

            migrationBuilder.DropIndex(
                name: "IX_CustomerImageStream_CustomerId",
                table: "CustomerImageStream");

            migrationBuilder.DropColumn(
                name: "ImageStreamId",
                table: "CustomerImageStream");

            migrationBuilder.AlterColumn<string>(
                name: "CustomerId",
                table: "CustomerImageStream",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
