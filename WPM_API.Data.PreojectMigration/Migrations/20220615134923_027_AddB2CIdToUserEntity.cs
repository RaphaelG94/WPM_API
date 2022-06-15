using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _027_AddB2CIdToUserEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "B2CID",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "B2CID",
                table: "User");
        }
    }
}
