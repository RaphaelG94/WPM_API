using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _197_AddReleasePlanAndApplOwnerCustomerSWStream : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Architecture",
                table: "CustomerSoftware");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "CustomerSoftware");

            migrationBuilder.DropColumn(
                name: "DescriptionShort",
                table: "CustomerSoftware");

            migrationBuilder.DropColumn(
                name: "DownloadLink",
                table: "CustomerSoftware");

            migrationBuilder.DropColumn(
                name: "GnuLicence",
                table: "CustomerSoftware");

            migrationBuilder.DropColumn(
                name: "Language",
                table: "CustomerSoftware");

            migrationBuilder.DropColumn(
                name: "Vendor",
                table: "CustomerSoftware");

            migrationBuilder.DropColumn(
                name: "Website",
                table: "CustomerSoftware");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationOwnerId",
                table: "CustomerSoftwareStream",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReleasePlanId",
                table: "CustomerSoftwareStream",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ReleasePlan",
                columns: table => new
                {
                    PK_ReleasePlan = table.Column<string>(nullable: false),
                    CreatedByUserId = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedByUserId = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    DeletedByUserId = table.Column<string>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReleasePlan", x => x.PK_ReleasePlan);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerSoftwareStream_ApplicationOwnerId",
                table: "CustomerSoftwareStream",
                column: "ApplicationOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerSoftwareStream_ReleasePlanId",
                table: "CustomerSoftwareStream",
                column: "ReleasePlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerSoftwareStream_Person_ApplicationOwnerId",
                table: "CustomerSoftwareStream",
                column: "ApplicationOwnerId",
                principalTable: "Person",
                principalColumn: "PK_Person",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerSoftwareStream_ReleasePlan_ReleasePlanId",
                table: "CustomerSoftwareStream",
                column: "ReleasePlanId",
                principalTable: "ReleasePlan",
                principalColumn: "PK_ReleasePlan",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerSoftwareStream_Person_ApplicationOwnerId",
                table: "CustomerSoftwareStream");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerSoftwareStream_ReleasePlan_ReleasePlanId",
                table: "CustomerSoftwareStream");

            migrationBuilder.DropTable(
                name: "ReleasePlan");

            migrationBuilder.DropIndex(
                name: "IX_CustomerSoftwareStream_ApplicationOwnerId",
                table: "CustomerSoftwareStream");

            migrationBuilder.DropIndex(
                name: "IX_CustomerSoftwareStream_ReleasePlanId",
                table: "CustomerSoftwareStream");

            migrationBuilder.DropColumn(
                name: "ApplicationOwnerId",
                table: "CustomerSoftwareStream");

            migrationBuilder.DropColumn(
                name: "ReleasePlanId",
                table: "CustomerSoftwareStream");

            migrationBuilder.AddColumn<string>(
                name: "Architecture",
                table: "CustomerSoftware",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "CustomerSoftware",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionShort",
                table: "CustomerSoftware",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DownloadLink",
                table: "CustomerSoftware",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "GnuLicence",
                table: "CustomerSoftware",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Language",
                table: "CustomerSoftware",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Vendor",
                table: "CustomerSoftware",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Website",
                table: "CustomerSoftware",
                nullable: true);
        }
    }
}
