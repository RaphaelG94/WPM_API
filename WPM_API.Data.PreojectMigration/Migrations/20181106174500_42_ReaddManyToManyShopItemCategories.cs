using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _42_ReaddManyToManyShopItemCategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Category_ShopItem_ShopItemId",
                table: "Category");

            migrationBuilder.DropIndex(
                name: "IX_Category_ShopItemId",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "ShopItemId",
                table: "Category");

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedByUserId",
                table: "User",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DeletedByUserId",
                table: "User",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "ShopItemCategory",
                columns: table => new
                {
                    PK_ShopItemCategory = table.Column<string>(nullable: false),
                    CategoryId = table.Column<string>(nullable: true),
                    ShopItemId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopItemCategory", x => x.PK_ShopItemCategory);
                    table.ForeignKey(
                        name: "FK_ShopItemCategory_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "PK_Category",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ShopItemCategory_ShopItem_ShopItemId",
                        column: x => x.ShopItemId,
                        principalTable: "ShopItem",
                        principalColumn: "PK_ShopItem",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_User_DeletedByUserId",
                table: "User",
                column: "DeletedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_User_UpdatedByUserId",
                table: "User",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopItemCategory_CategoryId",
                table: "ShopItemCategory",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopItemCategory_ShopItemId",
                table: "ShopItemCategory",
                column: "ShopItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_User_DeletedByUserId",
                table: "User",
                column: "DeletedByUserId",
                principalTable: "User",
                principalColumn: "PK_User",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_User_User_UpdatedByUserId",
                table: "User",
                column: "UpdatedByUserId",
                principalTable: "User",
                principalColumn: "PK_User",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_User_DeletedByUserId",
                table: "User");

            migrationBuilder.DropForeignKey(
                name: "FK_User_User_UpdatedByUserId",
                table: "User");

            migrationBuilder.DropTable(
                name: "ShopItemCategory");

            migrationBuilder.DropIndex(
                name: "IX_User_DeletedByUserId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_UpdatedByUserId",
                table: "User");

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedByUserId",
                table: "User",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DeletedByUserId",
                table: "User",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShopItemId",
                table: "Category",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Category_ShopItemId",
                table: "Category",
                column: "ShopItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Category_ShopItem_ShopItemId",
                table: "Category",
                column: "ShopItemId",
                principalTable: "ShopItem",
                principalColumn: "PK_ShopItem",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
