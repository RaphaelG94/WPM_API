using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _202_OnlySaveIdsOfPrevNextAndMinimalSW : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Software_Software_MinimalSoftwareId",
                table: "Software");

            migrationBuilder.DropForeignKey(
                name: "FK_Software_Software_NextSoftwareId",
                table: "Software");

            migrationBuilder.DropIndex(
                name: "IX_Software_MinimalSoftwareId",
                table: "Software");

            migrationBuilder.DropIndex(
                name: "IX_Software_NextSoftwareId",
                table: "Software");

            migrationBuilder.RenameColumn(
                name: "InstallationType",
                table: "Software",
                newName: "PrevSoftwareId");

            migrationBuilder.AlterColumn<string>(
                name: "NextSoftwareId",
                table: "Software",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MinimalSoftwareId",
                table: "Software",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PrevSoftwareId",
                table: "Software",
                newName: "InstallationType");

            migrationBuilder.AlterColumn<string>(
                name: "NextSoftwareId",
                table: "Software",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MinimalSoftwareId",
                table: "Software",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

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

            migrationBuilder.AddForeignKey(
                name: "FK_Software_Software_NextSoftwareId",
                table: "Software",
                column: "NextSoftwareId",
                principalTable: "Software",
                principalColumn: "PK_Software",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
