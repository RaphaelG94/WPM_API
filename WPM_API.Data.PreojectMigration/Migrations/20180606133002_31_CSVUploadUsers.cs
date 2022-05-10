using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _31_CSVUploadUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CSVUser",
                columns: table => new
                {
                    PK_CSVUser = table.Column<string>(nullable: false),
                    CreatedByUserId = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    DeletedByUserId = table.Column<string>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Displayname = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    MemberOf = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    SamAccountName = table.Column<string>(nullable: true),
                    UpdatedByUserId = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    UserGivenName = table.Column<string>(nullable: true),
                    UserLastName = table.Column<string>(nullable: true),
                    UserPrincipalName = table.Column<string>(nullable: true),
                    Workphone = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CSVUser", x => x.PK_CSVUser);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CSVUser");
        }
    }
}
