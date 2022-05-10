using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _26_AddOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OrderId",
                table: "ShopItem",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    PK_Order = table.Column<string>(nullable: false),
                    CreatedByUserId = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    DeletedByUserId = table.Column<string>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    UpdatedByUserId = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.PK_Order);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShopItem_OrderId",
                table: "ShopItem",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShopItem_Order_OrderId",
                table: "ShopItem",
                column: "OrderId",
                principalTable: "Order",
                principalColumn: "PK_Order",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShopItem_Order_OrderId",
                table: "ShopItem");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropIndex(
                name: "IX_ShopItem_OrderId",
                table: "ShopItem");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "ShopItem");
        }
    }
}
