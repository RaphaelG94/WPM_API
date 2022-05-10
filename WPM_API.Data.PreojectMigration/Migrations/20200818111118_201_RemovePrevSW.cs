using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _201_RemovePrevSW : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Software_Software_PreviousSoftwareId",
                table: "Software");

            migrationBuilder.DropIndex(
                name: "IX_Software_NextSoftwareId",
                table: "Software");

            migrationBuilder.DropIndex(
                name: "IX_Software_PreviousSoftwareId",
                table: "Software");

            migrationBuilder.RenameColumn(
                name: "PreviousSoftwareId",
                table: "Software",
                newName: "MinimalSoftwareId");

            migrationBuilder.CreateIndex(
                name: "IX_Software_MinimalSoftwareId",
                table: "Software",
                column: "MinimalSoftwareId",
                unique: true,
                filter: "[MinimalSoftwareId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Software_NextSoftwareId",
                table: "Software",
                column: "NextSoftwareId");

            migrationBuilder.AddForeignKey(
                name: "FK_Software_Software_MinimalSoftwareId",
                table: "Software",
                column: "MinimalSoftwareId",
                principalTable: "Software",
                principalColumn: "PK_Software",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Software_Software_MinimalSoftwareId",
                table: "Software");

            migrationBuilder.DropIndex(
                name: "IX_Software_MinimalSoftwareId",
                table: "Software");

            migrationBuilder.DropIndex(
                name: "IX_Software_NextSoftwareId",
                table: "Software");

            migrationBuilder.RenameColumn(
                name: "MinimalSoftwareId",
                table: "Software",
                newName: "PreviousSoftwareId");

            migrationBuilder.CreateIndex(
                name: "IX_Software_NextSoftwareId",
                table: "Software",
                column: "NextSoftwareId",
                unique: true,
                filter: "[NextSoftwareId] IS NOT NULL");

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
    }
}
