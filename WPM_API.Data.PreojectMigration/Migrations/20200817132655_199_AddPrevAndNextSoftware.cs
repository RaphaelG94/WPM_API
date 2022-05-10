using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _199_AddPrevAndNextSoftware : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InstallationType",
                table: "Software",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NextSoftwareId",
                table: "Software",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Software_NextSoftwareId",
                table: "Software",
                column: "NextSoftwareId",
                unique: true,
                filter: "[NextSoftwareId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Software_Software_NextSoftwareId",
                table: "Software",
                column: "NextSoftwareId",
                principalTable: "Software",
                principalColumn: "PK_Software",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Software_Software_NextSoftwareId",
                table: "Software");

            migrationBuilder.DropIndex(
                name: "IX_Software_NextSoftwareId",
                table: "Software");

            migrationBuilder.DropColumn(
                name: "InstallationType",
                table: "Software");

            migrationBuilder.DropColumn(
                name: "NextSoftwareId",
                table: "Software");
        }
    }
}
