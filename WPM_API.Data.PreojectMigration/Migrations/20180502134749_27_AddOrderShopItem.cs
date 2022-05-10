using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _27_AddOrderShopItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShopItem_Order_OrderId",
                table: "ShopItem");

            migrationBuilder.DropIndex(
                name: "IX_ShopItem_OrderId",
                table: "ShopItem");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "ShopItem");

            migrationBuilder.DropColumn(
                name: "DeletedByUserId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "UpdatedByUserId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Order");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedByUserId",
                table: "Order",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "OrderShopItem",
                columns: table => new
                {
                    PK_OrderShopItem = table.Column<string>(nullable: false),
                    OrderId = table.Column<string>(nullable: true),
                    ShopItemId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderShopItem", x => x.PK_OrderShopItem);
                    table.ForeignKey(
                        name: "FK_OrderShopItem_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "PK_Order",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderShopItem_ShopItem_ShopItemId",
                        column: x => x.ShopItemId,
                        principalTable: "ShopItem",
                        principalColumn: "PK_ShopItem",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Order_CreatedByUserId",
                table: "Order",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderShopItem_OrderId",
                table: "OrderShopItem",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderShopItem_ShopItemId",
                table: "OrderShopItem",
                column: "ShopItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_User_CreatedByUserId",
                table: "Order",
                column: "CreatedByUserId",
                principalTable: "User",
                principalColumn: "PK_User",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_User_CreatedByUserId",
                table: "Order");

            migrationBuilder.DropTable(
                name: "OrderShopItem");

            migrationBuilder.DropIndex(
                name: "IX_Order_CreatedByUserId",
                table: "Order");

            migrationBuilder.AddColumn<string>(
                name: "OrderId",
                table: "ShopItem",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedByUserId",
                table: "Order",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedByUserId",
                table: "Order",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "Order",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedByUserId",
                table: "Order",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Order",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

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
    }
}
