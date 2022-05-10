using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _200_AddPrevSW : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PreviousSoftwareId",
                table: "Software",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Software_PreviousSoftwareId",
                table: "Software",
                column: "PreviousSoftwareId");

            migrationBuilder.AddForeignKey(
                name: "FK_Software_Software_PreviousSoftwareId",
                table: "Software",
                column: "PreviousSoftwareId",
                principalTable: "Software",
                principalColumn: "PK_Software",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Software_Software_PreviousSoftwareId",
                table: "Software");

            migrationBuilder.DropIndex(
                name: "IX_Software_PreviousSoftwareId",
                table: "Software");

            migrationBuilder.DropColumn(
                name: "PreviousSoftwareId",
                table: "Software");
        }
    }
}
