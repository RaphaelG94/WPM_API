using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _104_AddHardwareModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HardwareModel",
                columns: table => new
                {
                    PK_HardwareModel = table.Column<string>(nullable: false),
                    Vendor = table.Column<string>(nullable: true),
                    ModelFamily = table.Column<string>(nullable: true),
                    ModelType = table.Column<string>(nullable: true),
                    ProductionStart = table.Column<DateTime>(nullable: false),
                    ProductionEnd = table.Column<DateTime>(nullable: false),
                    CreatedByUserId = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedByUserId = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    DeletedByUserId = table.Column<string>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HardwareModel", x => x.PK_HardwareModel);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HardwareModel");
        }
    }
}
