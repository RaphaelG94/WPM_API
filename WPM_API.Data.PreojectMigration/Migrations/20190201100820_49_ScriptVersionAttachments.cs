using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _49_ScriptVersionAttachments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<string>(
                name: "ScriptVersionId",
                table: "File",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_File_ScriptVersionId",
                table: "File",
                column: "ScriptVersionId");


            migrationBuilder.AddForeignKey(
                name: "FK_File_ScriptVersion_ScriptVersionId",
                table: "File",
                column: "ScriptVersionId",
                principalTable: "ScriptVersion",
                principalColumn: "PK_ScriptVersion",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_File_ScriptVersion_ScriptVersionId",
                table: "File");

            migrationBuilder.DropIndex(
                name: "IX_File_ScriptVersionId",
                table: "File");

            migrationBuilder.DropColumn(
                name: "ScriptVersionId",
                table: "File");
        }
    }
}
