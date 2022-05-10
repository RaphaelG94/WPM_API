using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _46_DefaultList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Variable");

            migrationBuilder.CreateTable(
                name: "Default",
                columns: table => new
                {
                    PK_Default = table.Column<string>(nullable: false),
                    CustomerId = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true),
                    Category = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Default", x => x.PK_Default);
                    table.ForeignKey(
                        name: "FK_Default_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "PK_Customer",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Default_CustomerId",
                table: "Default",
                column: "CustomerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Default");

            migrationBuilder.CreateTable(
                name: "Variable",
                columns: table => new
                {
                    PK_Variable = table.Column<string>(nullable: false),
                    CustomerId = table.Column<string>(nullable: true),
                    Default = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Variable", x => x.PK_Variable);
                    table.ForeignKey(
                        name: "FK_Variable_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "PK_Customer",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Variable_CustomerId",
                table: "Variable",
                column: "CustomerId");
        }
    }
}
