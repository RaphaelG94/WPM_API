using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _173_AddMissingPropsToSEP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CustomerId",
                table: "StorageEntryPoint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "StorageEntryPoint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StorageAccountType",
                table: "StorageEntryPoint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StorageEntryPoint_CustomerId",
                table: "StorageEntryPoint",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_StorageEntryPoint_Customer_CustomerId",
                table: "StorageEntryPoint",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "PK_Customer",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StorageEntryPoint_Customer_CustomerId",
                table: "StorageEntryPoint");

            migrationBuilder.DropIndex(
                name: "IX_StorageEntryPoint_CustomerId",
                table: "StorageEntryPoint");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "StorageEntryPoint");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "StorageEntryPoint");

            migrationBuilder.DropColumn(
                name: "StorageAccountType",
                table: "StorageEntryPoint");
        }
    }
}
