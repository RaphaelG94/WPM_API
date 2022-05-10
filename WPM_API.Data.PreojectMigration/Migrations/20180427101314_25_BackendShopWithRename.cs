using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _25_BackendShopWithRename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ShopItemId",
                table: "File",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ShopItem",
                columns: table => new
                {
                    PK_ShopItem = table.Column<string>(nullable: false),
                    CreatedByUserId = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    DeletedByUserId = table.Column<string>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    DescriptionShort = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Price = table.Column<string>(nullable: true),
                    UpdatedByUserId = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopItem", x => x.PK_ShopItem);
                });

            migrationBuilder.CreateIndex(
                name: "IX_File_ShopItemId",
                table: "File",
                column: "ShopItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_File_ShopItem_ShopItemId",
                table: "File",
                column: "ShopItemId",
                principalTable: "ShopItem",
                principalColumn: "PK_ShopItem",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_File_ShopItem_ShopItemId",
                table: "File");

            migrationBuilder.DropTable(
                name: "ShopItem");

            migrationBuilder.DropIndex(
                name: "IX_File_ShopItemId",
                table: "File");

            migrationBuilder.DropColumn(
                name: "ShopItemId",
                table: "File");
        }
    }
}
