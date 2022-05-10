using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _184_AddSoftwareStream : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SoftwareStreamId",
                table: "Software",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SoftwareStream",
                columns: table => new
                {
                    PK_SoftwareStream = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    UpdateSettings = table.Column<string>(nullable: true),
                    CreatedByUserId = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedByUserId = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    DeletedByUserId = table.Column<string>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    CustomerId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoftwareStream", x => x.PK_SoftwareStream);
                    table.ForeignKey(
                        name: "FK_SoftwareStream_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "PK_Customer",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Software_SoftwareStreamId",
                table: "Software",
                column: "SoftwareStreamId");

            migrationBuilder.CreateIndex(
                name: "IX_SoftwareStream_CustomerId",
                table: "SoftwareStream",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Software_SoftwareStream_SoftwareStreamId",
                table: "Software",
                column: "SoftwareStreamId",
                principalTable: "SoftwareStream",
                principalColumn: "PK_SoftwareStream",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Software_SoftwareStream_SoftwareStreamId",
                table: "Software");

            migrationBuilder.DropTable(
                name: "SoftwareStream");

            migrationBuilder.DropIndex(
                name: "IX_Software_SoftwareStreamId",
                table: "Software");

            migrationBuilder.DropColumn(
                name: "SoftwareStreamId",
                table: "Software");
        }
    }
}
