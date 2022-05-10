using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _93_MakeValueWrapperDeletable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserId",
                table: "ValueWrapper",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "ValueWrapper",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DeletedByUserId",
                table: "ValueWrapper",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "ValueWrapper",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedByUserId",
                table: "ValueWrapper",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "ValueWrapper",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "ValueWrapper");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "ValueWrapper");

            migrationBuilder.DropColumn(
                name: "DeletedByUserId",
                table: "ValueWrapper");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "ValueWrapper");

            migrationBuilder.DropColumn(
                name: "UpdatedByUserId",
                table: "ValueWrapper");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "ValueWrapper");
        }
    }
}
