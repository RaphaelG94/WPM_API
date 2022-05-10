using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _227_AddBaseStatusToBase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BaseStatusId",
                table: "Base",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BaseStatus",
                columns: table => new
                {
                    PK_BaseStatus = table.Column<string>(nullable: false),
                    Status = table.Column<string>(nullable: true),
                    ResourceGroupStatus = table.Column<string>(nullable: true),
                    VirtualNetworkStatus = table.Column<string>(nullable: true),
                    StorageAccountStatus = table.Column<string>(nullable: true),
                    VPNStatus = table.Column<string>(nullable: true),
                    CreatedByUserId = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedByUserId = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    DeletedByUserId = table.Column<string>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseStatus", x => x.PK_BaseStatus);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Base_BaseStatusId",
                table: "Base",
                column: "BaseStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Base_BaseStatus_BaseStatusId",
                table: "Base",
                column: "BaseStatusId",
                principalTable: "BaseStatus",
                principalColumn: "PK_BaseStatus",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Base_BaseStatus_BaseStatusId",
                table: "Base");

            migrationBuilder.DropTable(
                name: "BaseStatus");

            migrationBuilder.DropIndex(
                name: "IX_Base_BaseStatusId",
                table: "Base");

            migrationBuilder.DropColumn(
                name: "BaseStatusId",
                table: "Base");
        }
    }
}
