using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _49_AddPersonAndCompanyToCustomer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExpertId",
                table: "Customer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Person",
                columns: table => new
                {
                    PK_Person = table.Column<string>(nullable: false),
                    GivenName = table.Column<string>(nullable: true),
                    MiddleName = table.Column<string>(nullable: true),
                    Surname = table.Column<string>(nullable: true),
                    AcademicDegree = table.Column<string>(nullable: true),
                    EmployeeType = table.Column<string>(nullable: true),
                    CostCenter = table.Column<string>(nullable: true),
                    PhoneNr = table.Column<string>(nullable: true),
                    FaxNr = table.Column<string>(nullable: true),
                    MobileNr = table.Column<string>(nullable: true),
                    PrimaryEmail = table.Column<string>(nullable: true),
                    State = table.Column<string>(nullable: true),
                    CompanyId = table.Column<string>(nullable: true),
                    CreatedByUserId = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedByUserId = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    DeletedByUserId = table.Column<string>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Person", x => x.PK_Person);
                });

            migrationBuilder.CreateTable(
                name: "Company",
                columns: table => new
                {
                    PK_Company = table.Column<string>(nullable: false),
                    CorporateName = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    FormOfOrganization = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    PhoneNr = table.Column<string>(nullable: true),
                    LinkWebsite = table.Column<string>(nullable: true),
                    Street = table.Column<string>(nullable: true),
                    StreetNr = table.Column<string>(nullable: true),
                    Postcode = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    ExpertId = table.Column<string>(nullable: true),
                    CustomerId = table.Column<string>(nullable: true),
                    CreatedByUserId = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedByUserId = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    DeletedByUserId = table.Column<string>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company", x => x.PK_Company);
                    table.ForeignKey(
                        name: "FK_Company_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "PK_Customer",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Company_Person_ExpertId",
                        column: x => x.ExpertId,
                        principalTable: "Person",
                        principalColumn: "PK_Person",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customer_ExpertId",
                table: "Customer",
                column: "ExpertId");

            migrationBuilder.CreateIndex(
                name: "IX_Company_CustomerId",
                table: "Company",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Company_ExpertId",
                table: "Company",
                column: "ExpertId");

            migrationBuilder.CreateIndex(
                name: "IX_Person_CompanyId",
                table: "Person",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_Person_ExpertId",
                table: "Customer",
                column: "ExpertId",
                principalTable: "Person",
                principalColumn: "PK_Person",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Person_Company_CompanyId",
                table: "Person",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "PK_Company",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customer_Person_ExpertId",
                table: "Customer");

            migrationBuilder.DropForeignKey(
                name: "FK_Company_Person_ExpertId",
                table: "Company");

            migrationBuilder.DropTable(
                name: "Person");

            migrationBuilder.DropTable(
                name: "Company");

            migrationBuilder.DropIndex(
                name: "IX_Customer_ExpertId",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "ExpertId",
                table: "Customer");
        }
    }
}
