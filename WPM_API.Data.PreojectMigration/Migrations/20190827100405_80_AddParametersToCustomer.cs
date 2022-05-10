using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _80_AddParametersToCustomer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CustomerId",
                table: "Parameter",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsEditable",
                table: "Parameter",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Parameter_CustomerId",
                table: "Parameter",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Parameter_Customer_CustomerId",
                table: "Parameter",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "PK_Customer",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Parameter_Customer_CustomerId",
                table: "Parameter");

            migrationBuilder.DropIndex(
                name: "IX_Parameter_CustomerId",
                table: "Parameter");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "Parameter");

            migrationBuilder.DropColumn(
                name: "IsEditable",
                table: "Parameter");
        }
    }
}
