using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _74_ChangeSubnet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PK_VirtualNetwork",
                table: "Subnet",
                newName: "PK_Subnet");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PK_Subnet",
                table: "Subnet",
                newName: "PK_VirtualNetwork");
        }
    }
}
