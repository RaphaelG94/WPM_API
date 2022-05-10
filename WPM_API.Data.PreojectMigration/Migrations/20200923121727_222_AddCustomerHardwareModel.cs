using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _222_AddCustomerHardwareModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CustomerHardwareModelId",
                table: "File",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CustomerHardwareModel",
                columns: table => new
                {
                    PK_CustomerHardwareModel = table.Column<string>(nullable: false),
                    Counter = table.Column<int>(nullable: false),
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
                    table.PrimaryKey("PK_CustomerHardwareModel", x => x.PK_CustomerHardwareModel);
                });

            migrationBuilder.CreateIndex(
                name: "IX_File_CustomerHardwareModelId",
                table: "File",
                column: "CustomerHardwareModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_File_CustomerHardwareModel_CustomerHardwareModelId",
                table: "File",
                column: "CustomerHardwareModelId",
                principalTable: "CustomerHardwareModel",
                principalColumn: "PK_CustomerHardwareModel",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_File_CustomerHardwareModel_CustomerHardwareModelId",
                table: "File");

            migrationBuilder.DropTable(
                name: "CustomerHardwareModel");

            migrationBuilder.DropIndex(
                name: "IX_File_CustomerHardwareModelId",
                table: "File");

            migrationBuilder.DropColumn(
                name: "CustomerHardwareModelId",
                table: "File");
        }
    }
}
