using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _132_ChangeInventory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ValueWrapper");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "Inventory");

            migrationBuilder.DropColumn(
                name: "Class",
                table: "Inventory");

            migrationBuilder.DropColumn(
                name: "IsMultiValue",
                table: "Inventory");

            migrationBuilder.DropColumn(
                name: "KeyName",
                table: "Inventory");

            migrationBuilder.RenameColumn(
                name: "Method",
                table: "Inventory",
                newName: "Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Inventory",
                newName: "Method");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Inventory",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Class",
                table: "Inventory",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsMultiValue",
                table: "Inventory",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "KeyName",
                table: "Inventory",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ValueWrapper",
                columns: table => new
                {
                    PK_ValiueWrapper = table.Column<string>(nullable: false),
                    CreatedByUserId = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    DeletedByUserId = table.Column<string>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    InventoryId = table.Column<string>(nullable: true),
                    UpdatedByUserId = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ValueWrapper", x => x.PK_ValiueWrapper);
                    table.ForeignKey(
                        name: "FK_ValueWrapper_Inventory_InventoryId",
                        column: x => x.InventoryId,
                        principalTable: "Inventory",
                        principalColumn: "PK_Inventory",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ValueWrapper_InventoryId",
                table: "ValueWrapper",
                column: "InventoryId");
        }
    }
}
