using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _156_RemoveListsDueToWrongRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Client_Software_SoftwareId",
                table: "Client");

            migrationBuilder.DropForeignKey(
                name: "FK_Customer_Software_SoftwareId",
                table: "Customer");

            migrationBuilder.DropForeignKey(
                name: "FK_Systemhouse_Software_SoftwareId",
                table: "Systemhouse");

            migrationBuilder.DropIndex(
                name: "IX_Systemhouse_SoftwareId",
                table: "Systemhouse");

            migrationBuilder.DropIndex(
                name: "IX_Customer_SoftwareId",
                table: "Customer");

            migrationBuilder.DropIndex(
                name: "IX_Client_SoftwareId",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "SoftwareId",
                table: "Systemhouse");

            migrationBuilder.DropColumn(
                name: "SoftwareId",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "SoftwareId",
                table: "Client");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SoftwareId",
                table: "Systemhouse",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SoftwareId",
                table: "Customer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SoftwareId",
                table: "Client",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Systemhouse_SoftwareId",
                table: "Systemhouse",
                column: "SoftwareId");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_SoftwareId",
                table: "Customer",
                column: "SoftwareId");

            migrationBuilder.CreateIndex(
                name: "IX_Client_SoftwareId",
                table: "Client",
                column: "SoftwareId");

            migrationBuilder.AddForeignKey(
                name: "FK_Client_Software_SoftwareId",
                table: "Client",
                column: "SoftwareId",
                principalTable: "Software",
                principalColumn: "PK_Software",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_Software_SoftwareId",
                table: "Customer",
                column: "SoftwareId",
                principalTable: "Software",
                principalColumn: "PK_Software",
                onDelete: ReferentialAction.Restrict);

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
