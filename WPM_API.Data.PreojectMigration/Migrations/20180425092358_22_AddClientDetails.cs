using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _22_AddClientDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Bios",
                table: "Client",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Vendor",
                table: "Client",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MacAddress",
                columns: table => new
                {
                    PK_MacAddress = table.Column<string>(nullable: false),
                    Address = table.Column<string>(nullable: true),
                    ClientId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MacAddress", x => x.PK_MacAddress);
                    table.ForeignKey(
                        name: "FK_MacAddress_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "PK_Client",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MacAddress_ClientId",
                table: "MacAddress",
                column: "ClientId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MacAddress");

            migrationBuilder.DropColumn(
                name: "Bios",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "Vendor",
                table: "Client");
        }
    }
}
