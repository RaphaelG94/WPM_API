using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _242_AddSubscritpionInfoToVM : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SubscriptionId",
                table: "VirtualMachine",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubscriptionName",
                table: "VirtualMachine",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubscriptionId",
                table: "VirtualMachine");

            migrationBuilder.DropColumn(
                name: "SubscriptionName",
                table: "VirtualMachine");
        }
    }
}
