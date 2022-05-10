using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _61_AddAzureBlobStorageEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AzureBlobStorage",
                columns: table => new
                {
                    PK_AzureBlob = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    StorageAccountId = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    RessourceGroupId = table.Column<string>(nullable: true),
                    CreatedByUserId = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedByUserId = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    DeletedByUserId = table.Column<string>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AzureBlobStorage", x => x.PK_AzureBlob);
                    table.ForeignKey(
                        name: "FK_AzureBlobStorage_ResourceGroup_RessourceGroupId",
                        column: x => x.RessourceGroupId,
                        principalTable: "ResourceGroup",
                        principalColumn: "PK_ResourceGroup",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AzureBlobStorage_StorageAccount_StorageAccountId",
                        column: x => x.StorageAccountId,
                        principalTable: "StorageAccount",
                        principalColumn: "PK_StorageAccount",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AzureBlobStorage_RessourceGroupId",
                table: "AzureBlobStorage",
                column: "RessourceGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_AzureBlobStorage_StorageAccountId",
                table: "AzureBlobStorage",
                column: "StorageAccountId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AzureBlobStorage");
        }
    }
}
