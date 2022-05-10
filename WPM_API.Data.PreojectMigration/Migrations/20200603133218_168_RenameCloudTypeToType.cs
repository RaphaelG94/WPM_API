using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _168_RenameCloudTypeToType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Base_AzureCredentials_CredentialsId",
                table: "Base");

            migrationBuilder.DropTable(
                name: "AzureCredentials");

            migrationBuilder.DropIndex(
                name: "IX_Base_CredentialsId",
                table: "Base");

            migrationBuilder.DropColumn(
                name: "CredentialsId",
                table: "Base");

            migrationBuilder.RenameColumn(
                name: "CloudType",
                table: "CloudEntryPoint",
                newName: "Type");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "CloudEntryPoint",
                newName: "CloudType");

            migrationBuilder.AddColumn<string>(
                name: "CredentialsId",
                table: "Base",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AzureCredentials",
                columns: table => new
                {
                    PK_AzureCredentials = table.Column<string>(nullable: false),
                    ClientId = table.Column<string>(nullable: true),
                    ClientSecret = table.Column<string>(nullable: true),
                    CustomerId = table.Column<string>(nullable: true),
                    TenantId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AzureCredentials", x => x.PK_AzureCredentials);
                    table.ForeignKey(
                        name: "FK_AzureCredentials_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "PK_Customer",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Base_CredentialsId",
                table: "Base",
                column: "CredentialsId");

            migrationBuilder.CreateIndex(
                name: "IX_AzureCredentials_CustomerId",
                table: "AzureCredentials",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Base_AzureCredentials_CredentialsId",
                table: "Base",
                column: "CredentialsId",
                principalTable: "AzureCredentials",
                principalColumn: "PK_AzureCredentials",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
