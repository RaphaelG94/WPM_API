using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _206_AddEnterpriseFlag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsEnterpriseStream",
                table: "CustomerSoftwareStream",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEnterpriseStream",
                table: "CustomerSoftwareStream");
        }
    }
}
