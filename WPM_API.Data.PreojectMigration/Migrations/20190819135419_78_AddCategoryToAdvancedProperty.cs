using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _78_AddCategoryToAdvancedProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CategoryId",
                table: "AdvancedProperty",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AdvancedProperty_CategoryId",
                table: "AdvancedProperty",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_AdvancedProperty_Category_CategoryId",
                table: "AdvancedProperty",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "PK_Category",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdvancedProperty_Category_CategoryId",
                table: "AdvancedProperty");

            migrationBuilder.DropIndex(
                name: "IX_AdvancedProperty_CategoryId",
                table: "AdvancedProperty");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "AdvancedProperty");
        }
    }
}
