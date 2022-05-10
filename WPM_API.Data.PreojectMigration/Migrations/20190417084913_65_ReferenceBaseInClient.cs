using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _65_ReferenceBaseInClient : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BaseId",
                table: "Client",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Client_BaseId",
                table: "Client",
                column: "BaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Client_Base_BaseId",
                table: "Client",
                column: "BaseId",
                principalTable: "Base",
                principalColumn: "PK_Base",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Client_Base_BaseId",
                table: "Client");

            migrationBuilder.DropIndex(
                name: "IX_Client_BaseId",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "BaseId",
                table: "Client");
        }
    }
}
