using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _92_MakeInventoryValueToList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Value",
                table: "Inventory");

            migrationBuilder.CreateTable(
                name: "ValueWrapper",
                columns: table => new
                {
                    PK_ValiueWrapper = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true),
                    InventoryId = table.Column<string>(nullable: true)
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ValueWrapper");

            migrationBuilder.AddColumn<string>(
                name: "Value",
                table: "Inventory",
                nullable: true);
        }
    }
}
