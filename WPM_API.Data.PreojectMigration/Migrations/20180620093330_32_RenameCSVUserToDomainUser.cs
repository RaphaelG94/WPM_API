using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _32_RenameCSVUserToDomainUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CSVUser");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "DomainUser",
                newName: "Workphone");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "DomainUser",
                newName: "UserPrincipalName");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "DomainUser",
                newName: "UserLastName");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "DomainUser",
                newName: "UserGivenName");

            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserId",
                table: "DomainUser",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "DomainUser",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DeletedByUserId",
                table: "DomainUser",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "DomainUser",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "DomainUser",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Displayname",
                table: "DomainUser",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MemberOf",
                table: "DomainUser",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "DomainUser",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SamAccountName",
                table: "DomainUser",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedByUserId",
                table: "DomainUser",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "DomainUser",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "DomainUser");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "DomainUser");

            migrationBuilder.DropColumn(
                name: "DeletedByUserId",
                table: "DomainUser");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "DomainUser");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "DomainUser");

            migrationBuilder.DropColumn(
                name: "Displayname",
                table: "DomainUser");

            migrationBuilder.DropColumn(
                name: "MemberOf",
                table: "DomainUser");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "DomainUser");

            migrationBuilder.DropColumn(
                name: "SamAccountName",
                table: "DomainUser");

            migrationBuilder.DropColumn(
                name: "UpdatedByUserId",
                table: "DomainUser");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "DomainUser");

            migrationBuilder.RenameColumn(
                name: "Workphone",
                table: "DomainUser",
                newName: "UserName");

            migrationBuilder.RenameColumn(
                name: "UserPrincipalName",
                table: "DomainUser",
                newName: "Password");

            migrationBuilder.RenameColumn(
                name: "UserLastName",
                table: "DomainUser",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "UserGivenName",
                table: "DomainUser",
                newName: "FirstName");

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
    }
}
