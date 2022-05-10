using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _261_AddImageSyshouseandCustomerReferences : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ImagesClient",
                columns: table => new
                {
                    PK_SoftwaresClient = table.Column<string>(nullable: false),
                    ImageId = table.Column<string>(nullable: true),
                    ClientId = table.Column<string>(nullable: true),
                    CreatedByUserId = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedByUserId = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    DeletedByUserId = table.Column<string>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImagesClient", x => x.PK_SoftwaresClient);
                    table.ForeignKey(
                        name: "FK_ImagesClient_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "PK_Client",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ImagesClient_Image_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Image",
                        principalColumn: "PK_Image",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ImagesCustomer",
                columns: table => new
                {
                    PK_SoftwaresCustomer = table.Column<string>(nullable: false),
                    ImageId = table.Column<string>(nullable: true),
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
                    table.PrimaryKey("PK_ImagesCustomer", x => x.PK_SoftwaresCustomer);
                    table.ForeignKey(
                        name: "FK_ImagesCustomer_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "PK_Customer",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ImagesCustomer_Image_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Image",
                        principalColumn: "PK_Image",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ImagesSystemhouse",
                columns: table => new
                {
                    PK_SoftwaresSystemhouse = table.Column<string>(nullable: false),
                    ImageId = table.Column<string>(nullable: true),
                    SystemhouseId = table.Column<string>(nullable: true),
                    CreatedByUserId = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedByUserId = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    DeletedByUserId = table.Column<string>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImagesSystemhouse", x => x.PK_SoftwaresSystemhouse);
                    table.ForeignKey(
                        name: "FK_ImagesSystemhouse_Image_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Image",
                        principalColumn: "PK_Image",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ImagesSystemhouse_Systemhouse_SystemhouseId",
                        column: x => x.SystemhouseId,
                        principalTable: "Systemhouse",
                        principalColumn: "PK_Systemhouse",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ImagesClient_ClientId",
                table: "ImagesClient",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ImagesClient_ImageId",
                table: "ImagesClient",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_ImagesCustomer_CustomerId",
                table: "ImagesCustomer",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_ImagesCustomer_ImageId",
                table: "ImagesCustomer",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_ImagesSystemhouse_ImageId",
                table: "ImagesSystemhouse",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_ImagesSystemhouse_SystemhouseId",
                table: "ImagesSystemhouse",
                column: "SystemhouseId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ImagesClient");

            migrationBuilder.DropTable(
                name: "ImagesCustomer");

            migrationBuilder.DropTable(
                name: "ImagesSystemhouse");
        }
    }
}
