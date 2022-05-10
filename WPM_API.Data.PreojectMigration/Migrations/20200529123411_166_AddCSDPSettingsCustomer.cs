using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _166_AddCSDPSettingsCustomer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CsdpContainer",
                table: "Customer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CsdpRoot",
                table: "Customer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LtSASRead",
                table: "Customer",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CsdpContainer",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "CsdpRoot",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "LtSASRead",
                table: "Customer");
        }
    }
}
