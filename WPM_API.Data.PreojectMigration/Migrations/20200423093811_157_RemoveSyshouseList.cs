using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _157_RemoveSyshouseList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Systemhouse_Software_SoftwareId",
                table: "Systemhouse");

            migrationBuilder.DropIndex(
                name: "IX_Systemhouse_SoftwareId",
                table: "Systemhouse");

            migrationBuilder.DropColumn(
                name: "SoftwareId",
                table: "Systemhouse");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SoftwareId",
                table: "Systemhouse",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Systemhouse_SoftwareId",
                table: "Systemhouse",
                column: "SoftwareId");

            migrationBuilder.AddForeignKey(
                name: "FK_Systemhouse_Software_SoftwareId",
                table: "Systemhouse",
                column: "SoftwareId",
                principalTable: "Software",
                principalColumn: "PK_Software",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
