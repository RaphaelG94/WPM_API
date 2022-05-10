using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _37_AddAttributesToGroupPolicyObject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<string>(
                name: "LockscreenId",
                table: "GroupPolicyObject",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Settings",
                table: "GroupPolicyObject",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WallpaperId",
                table: "GroupPolicyObject",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GroupPolicyObject_LockscreenId",
                table: "GroupPolicyObject",
                column: "LockscreenId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupPolicyObject_WallpaperId",
                table: "GroupPolicyObject",
                column: "WallpaperId");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupPolicyObject_File_LockscreenId",
                table: "GroupPolicyObject",
                column: "LockscreenId",
                principalTable: "File",
                principalColumn: "PK_File",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupPolicyObject_File_WallpaperId",
                table: "GroupPolicyObject",
                column: "WallpaperId",
                principalTable: "File",
                principalColumn: "PK_File",
                onDelete: ReferentialAction.Restrict);

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupPolicyObject_File_LockscreenId",
                table: "GroupPolicyObject");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupPolicyObject_File_WallpaperId",
                table: "GroupPolicyObject");

       

            migrationBuilder.DropIndex(
                name: "IX_GroupPolicyObject_LockscreenId",
                table: "GroupPolicyObject");

            migrationBuilder.DropIndex(
                name: "IX_GroupPolicyObject_WallpaperId",
                table: "GroupPolicyObject");

            migrationBuilder.DropColumn(
                name: "LockscreenId",
                table: "GroupPolicyObject");

            migrationBuilder.DropColumn(
                name: "Settings",
                table: "GroupPolicyObject");

            migrationBuilder.DropColumn(
                name: "WallpaperId",
                table: "GroupPolicyObject");

 
        }
    }
}
