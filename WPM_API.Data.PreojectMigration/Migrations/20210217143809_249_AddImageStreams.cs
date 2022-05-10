using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _249_AddImageStreams : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageStreamId",
                table: "Image",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerImageStreamId",
                table: "CustomerImage",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CustomerImageStream",
                columns: table => new
                {
                    PK_ImageStream = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    CreatedByUserId = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedByUserId = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    DeletedByUserId = table.Column<string>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerImageStream", x => x.PK_ImageStream);
                });

            migrationBuilder.CreateTable(
                name: "ImageStream",
                columns: table => new
                {
                    PK_ImageStream = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    CreatedByUserId = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedByUserId = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    DeletedByUserId = table.Column<string>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageStream", x => x.PK_ImageStream);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Image_ImageStreamId",
                table: "Image",
                column: "ImageStreamId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerImage_CustomerImageStreamId",
                table: "CustomerImage",
                column: "CustomerImageStreamId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerImage_CustomerImageStream_CustomerImageStreamId",
                table: "CustomerImage",
                column: "CustomerImageStreamId",
                principalTable: "CustomerImageStream",
                principalColumn: "PK_ImageStream",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Image_ImageStream_ImageStreamId",
                table: "Image",
                column: "ImageStreamId",
                principalTable: "ImageStream",
                principalColumn: "PK_ImageStream",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerImage_CustomerImageStream_CustomerImageStreamId",
                table: "CustomerImage");

            migrationBuilder.DropForeignKey(
                name: "FK_Image_ImageStream_ImageStreamId",
                table: "Image");

            migrationBuilder.DropTable(
                name: "CustomerImageStream");

            migrationBuilder.DropTable(
                name: "ImageStream");

            migrationBuilder.DropIndex(
                name: "IX_Image_ImageStreamId",
                table: "Image");

            migrationBuilder.DropIndex(
                name: "IX_CustomerImage_CustomerImageStreamId",
                table: "CustomerImage");

            migrationBuilder.DropColumn(
                name: "ImageStreamId",
                table: "Image");

            migrationBuilder.DropColumn(
                name: "CustomerImageStreamId",
                table: "CustomerImage");
        }
    }
}
