using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _47_AddParameterToDeviceOptions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClientOptionId",
                table: "Parameter",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Parameter_ClientOptionId",
                table: "Parameter",
                column: "ClientOptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Parameter_ClientOption_ClientOptionId",
                table: "Parameter",
                column: "ClientOptionId",
                principalTable: "ClientOption",
                principalColumn: "PK_ClientOption",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Parameter_ClientOption_ClientOptionId",
                table: "Parameter");

            migrationBuilder.DropIndex(
                name: "IX_Parameter_ClientOptionId",
                table: "Parameter");

            migrationBuilder.DropColumn(
                name: "ClientOptionId",
                table: "Parameter");
        }
    }
}
