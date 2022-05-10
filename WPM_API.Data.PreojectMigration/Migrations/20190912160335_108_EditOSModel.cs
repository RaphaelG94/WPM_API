using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _108_EditOSModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OSModelId",
                table: "HardwareModel",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_HardwareModel_OSModelId",
                table: "HardwareModel",
                column: "OSModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_HardwareModel_OSModel_OSModelId",
                table: "HardwareModel",
                column: "OSModelId",
                principalTable: "OSModel",
                principalColumn: "PK_OSModel",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HardwareModel_OSModel_OSModelId",
                table: "HardwareModel");

            migrationBuilder.DropIndex(
                name: "IX_HardwareModel_OSModelId",
                table: "HardwareModel");

            migrationBuilder.DropColumn(
                name: "OSModelId",
                table: "HardwareModel");
        }
    }
}
