using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _34_TPMChipDataFromFile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TPMChip",
                table: "Hardware");

            migrationBuilder.DropColumn(
                name: "TPMChipClear",
                table: "Hardware");

            migrationBuilder.DropColumn(
                name: "TPMChipOwnership",
                table: "Hardware");

            migrationBuilder.RenameColumn(
                name: "TPMChipVersion",
                table: "Hardware",
                newName: "TPMChipData");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TPMChipData",
                table: "Hardware",
                newName: "TPMChipVersion");

            migrationBuilder.AddColumn<string>(
                name: "TPMChip",
                table: "Hardware",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TPMChipClear",
                table: "Hardware",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TPMChipOwnership",
                table: "Hardware",
                nullable: true);
        }
    }
}
