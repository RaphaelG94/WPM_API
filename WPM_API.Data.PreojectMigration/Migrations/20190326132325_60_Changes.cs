using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _60_Changes : Migration
    {
       protected override void Up(MigrationBuilder migrationBuilder)
       {
            /*
           migrationBuilder.CreateTable(
               name: "SmbStorage",
               columns: table => new
               {
                   Id = table.Column<string>(nullable: false),
                   ServerName = table.Column<string>(nullable: true),
                   ServerIp = table.Column<string>(nullable: true),
                   Permission = table.Column<string>(nullable: true),
                   ShareName = table.Column<string>(nullable: true),
                   CreatedByUserId = table.Column<string>(nullable: true),
                   CreatedDate = table.Column<DateTime>(nullable: false),
                   UpdatedByUserId = table.Column<string>(nullable: true),
                   UpdatedDate = table.Column<DateTime>(nullable: false),
                   DeletedByUserId = table.Column<string>(nullable: true),
                   DeletedDate = table.Column<DateTime>(nullable: true),
                   CustomerId = table.Column<string>(nullable: true)
               },
               constraints: table =>
               {
                   table.PrimaryKey("PK_SmbStorage", x => x.Id);
                   table.ForeignKey(
                       name: "FK_SmbStorage_Customer_CustomerId",
                       column: x => x.CustomerId,
                       principalTable: "Customer",
                       principalColumn: "PK_Customer",
                       onDelete: ReferentialAction.Restrict);
               });

           migrationBuilder.CreateIndex(
               name: "IX_SmbStorage_CustomerId",
               table: "SmbStorage",
               column: "CustomerId");
               */
       }

       protected override void Down(MigrationBuilder migrationBuilder)
       {
            /*
           migrationBuilder.DropTable(
               name: "SmbStorage");
               */
       }
    }
}
