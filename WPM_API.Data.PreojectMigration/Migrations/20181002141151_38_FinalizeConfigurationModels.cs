using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _38_FinalizeConfigurationModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<string>(
                name: "DomainUserCSVId",
                table: "Domain",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Office365ConfigurationXMLId",
                table: "Domain",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Domain_DomainUserCSVId",
                table: "Domain",
                column: "DomainUserCSVId");

            migrationBuilder.CreateIndex(
                name: "IX_Domain_Office365ConfigurationXMLId",
                table: "Domain",
                column: "Office365ConfigurationXMLId");

            migrationBuilder.AddForeignKey(
                name: "FK_Domain_File_DomainUserCSVId",
                table: "Domain",
                column: "DomainUserCSVId",
                principalTable: "File",
                principalColumn: "PK_File",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Domain_File_Office365ConfigurationXMLId",
                table: "Domain",
                column: "Office365ConfigurationXMLId",
                principalTable: "File",
                principalColumn: "PK_File",
                onDelete: ReferentialAction.Restrict);
           
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Domain_File_DomainUserCSVId",
                table: "Domain");

            migrationBuilder.DropForeignKey(
                name: "FK_Domain_File_Office365ConfigurationXMLId",
                table: "Domain");

            migrationBuilder.DropIndex(
                name: "IX_Domain_DomainUserCSVId",
                table: "Domain");

            migrationBuilder.DropIndex(
                name: "IX_Domain_Office365ConfigurationXMLId",
                table: "Domain");

            migrationBuilder.DropColumn(
                name: "DomainUserCSVId",
                table: "Domain");

            migrationBuilder.DropColumn(
                name: "Office365ConfigurationXMLId",
                table: "Domain");
        }
    }
}
