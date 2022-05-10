using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _152_AddCmdLinesLabelsToCustomer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Btn1Label",
                table: "Customer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Btn2Label",
                table: "Customer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Btn3Label",
                table: "Customer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Btn4Label",
                table: "Customer",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Btn1Label",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "Btn2Label",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "Btn3Label",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "Btn4Label",
                table: "Customer");
        }
    }
}
