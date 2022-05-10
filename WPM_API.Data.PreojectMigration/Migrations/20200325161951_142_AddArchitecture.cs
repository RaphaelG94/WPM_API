using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _142_AddArchitecture : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RuleId",
                table: "Architecture",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Architecture_RuleId",
                table: "Architecture",
                column: "RuleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Architecture_Rule_RuleId",
                table: "Architecture",
                column: "RuleId",
                principalTable: "Rule",
                principalColumn: "PK_Rule",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Architecture_Rule_RuleId",
                table: "Architecture");

            migrationBuilder.DropIndex(
                name: "IX_Architecture_RuleId",
                table: "Architecture");

            migrationBuilder.DropColumn(
                name: "RuleId",
                table: "Architecture");
        }
    }
}
