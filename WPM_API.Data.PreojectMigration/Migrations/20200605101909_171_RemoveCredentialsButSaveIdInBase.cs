using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _171_RemoveCredentialsButSaveIdInBase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Base_CloudEntryPoint_CredentialsId",
                table: "Base");

            migrationBuilder.DropIndex(
                name: "IX_Base_CredentialsId",
                table: "Base");

            migrationBuilder.AlterColumn<string>(
                name: "CredentialsId",
                table: "Base",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CredentialsId",
                table: "Base",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Base_CredentialsId",
                table: "Base",
                column: "CredentialsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Base_CloudEntryPoint_CredentialsId",
                table: "Base",
                column: "CredentialsId",
                principalTable: "CloudEntryPoint",
                principalColumn: "PK_CloudEntryPoint",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
