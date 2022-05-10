using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _151_AddCmdLinesToCustomer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CmdBtn1",
                table: "Customer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CmdBtn2",
                table: "Customer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CmdBtn3",
                table: "Customer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CmdBtn4",
                table: "Customer",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CmdBtn1",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "CmdBtn2",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "CmdBtn3",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "CmdBtn4",
                table: "Customer");
        }
    }
}
