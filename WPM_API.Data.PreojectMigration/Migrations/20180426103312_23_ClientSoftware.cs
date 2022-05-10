using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _23_ClientSoftware : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClientSoftware",
                columns: table => new
                {
                    PK_ClientSoftware = table.Column<string>(nullable: false),
                    ClientId = table.Column<string>(nullable: true),
                    SoftwareId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientSoftware", x => x.PK_ClientSoftware);
                    table.ForeignKey(
                        name: "FK_ClientSoftware_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "PK_Client",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClientSoftware_Software_SoftwareId",
                        column: x => x.SoftwareId,
                        principalTable: "Software",
                        principalColumn: "PK_Software",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClientSoftware_ClientId",
                table: "ClientSoftware",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientSoftware_SoftwareId",
                table: "ClientSoftware",
                column: "SoftwareId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientSoftware");
        }
    }
}
