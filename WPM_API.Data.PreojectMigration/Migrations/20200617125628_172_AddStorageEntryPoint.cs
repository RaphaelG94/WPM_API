using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _172_AddStorageEntryPoint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SmbStorage");

            migrationBuilder.CreateTable(
                name: "StorageEntryPoint",
                columns: table => new
                {
                    PK_StorageEntryPoint = table.Column<string>(nullable: false),
                    ResourceGrpName = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    SubscriptionId = table.Column<string>(nullable: true),
                    IsCSDP = table.Column<bool>(nullable: false),
                    StorageAccount = table.Column<string>(nullable: true),
                    CreatedByUserId = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedByUserId = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    DeletedByUserId = table.Column<string>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StorageEntryPoint", x => x.PK_StorageEntryPoint);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StorageEntryPoint");

            migrationBuilder.CreateTable(
                name: "SmbStorage",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ClientId = table.Column<string>(nullable: true),
                    CreatedByUserId = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CustomerId = table.Column<string>(nullable: true),
                    DataDriveLetter = table.Column<string>(nullable: true),
                    DeletedByUserId = table.Column<string>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    ExistedAlready = table.Column<bool>(nullable: false),
                    Path = table.Column<string>(nullable: true),
                    ShareName = table.Column<string>(nullable: true),
                    UpdatedByUserId = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmbStorage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SmbStorage_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "PK_Client",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SmbStorage_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "PK_Customer",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SmbStorage_ClientId",
                table: "SmbStorage",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_SmbStorage_CustomerId",
                table: "SmbStorage",
                column: "CustomerId");
        }
    }
}
