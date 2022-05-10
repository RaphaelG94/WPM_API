using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _167_RemoveAzureCredsAndAllowMultipleCEPs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AzureCredentials_CustomerId",
                table: "AzureCredentials");

            migrationBuilder.CreateTable(
                name: "CloudEntryPoint",
                columns: table => new
                {
                    PK_CloudEntryPoint = table.Column<string>(nullable: false),
                    TenantId = table.Column<string>(nullable: true),
                    ClientSecret = table.Column<string>(nullable: true),
                    ClientId = table.Column<string>(nullable: true),
                    CloudType = table.Column<string>(nullable: true),
                    CreatedByUserId = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedByUserId = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    DeletedByUserId = table.Column<string>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    CustomerId = table.Column<string>(nullable: true),
                    IsCSDP = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CloudEntryPoint", x => x.PK_CloudEntryPoint);
                    table.ForeignKey(
                        name: "FK_CloudEntryPoint_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "PK_Customer",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AzureCredentials_CustomerId",
                table: "AzureCredentials",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CloudEntryPoint_CustomerId",
                table: "CloudEntryPoint",
                column: "CustomerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CloudEntryPoint");

            migrationBuilder.DropIndex(
                name: "IX_AzureCredentials_CustomerId",
                table: "AzureCredentials");

            migrationBuilder.CreateIndex(
                name: "IX_AzureCredentials_CustomerId",
                table: "AzureCredentials",
                column: "CustomerId",
                unique: true,
                filter: "[CustomerId] IS NOT NULL");
        }
    }
}
