using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class AssignDeviceOptions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClientOption",
                columns: table => new
                {
                    PK_ClientOption = table.Column<string>(nullable: false),
                    ClientId = table.Column<string>(nullable: true),
                    DeviceOptionId = table.Column<string>(nullable: true),
                    Order = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientOption", x => x.PK_ClientOption);
                    table.ForeignKey(
                        name: "FK_ClientOption_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "PK_Client",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClientOption_ScriptVersion_DeviceOptionId",
                        column: x => x.DeviceOptionId,
                        principalTable: "ScriptVersion",
                        principalColumn: "PK_ScriptVersion",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClientOption_ClientId",
                table: "ClientOption",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientOption_DeviceOptionId",
                table: "ClientOption",
                column: "DeviceOptionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientOption");
        }
    }
}
