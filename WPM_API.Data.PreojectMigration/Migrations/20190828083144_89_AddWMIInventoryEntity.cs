using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _89_AddWMIInventoryEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WMIInvenotryCmds",
                columns: table => new
                {
                    PK_WMIInventoryCmds = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Command = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WMIInvenotryCmds", x => x.PK_WMIInventoryCmds);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WMIInvenotryCmds");
        }
    }
}
