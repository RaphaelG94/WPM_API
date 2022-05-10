using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _44_ExtendHDDPartitionClass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Client_HDDPartition_PartitionId",
                table: "Client");

            migrationBuilder.DropIndex(
                name: "IX_Client_PartitionId",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "PartitionId",
                table: "Client");

            migrationBuilder.RenameColumn(
                name: "Gpt",
                table: "HDDPartition",
                newName: "isGpt");

            migrationBuilder.AddColumn<string>(
                name: "ClientId",
                table: "HDDPartition",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DriveLetter",
                table: "HDDPartition",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PartitionNumber",
                table: "HDDPartition",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SizeInBytes",
                table: "HDDPartition",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "HDDPartition",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_HDDPartition_ClientId",
                table: "HDDPartition",
                column: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_HDDPartition_Client_ClientId",
                table: "HDDPartition",
                column: "ClientId",
                principalTable: "Client",
                principalColumn: "PK_Client",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HDDPartition_Client_ClientId",
                table: "HDDPartition");

            migrationBuilder.DropIndex(
                name: "IX_HDDPartition_ClientId",
                table: "HDDPartition");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "HDDPartition");

            migrationBuilder.DropColumn(
                name: "DriveLetter",
                table: "HDDPartition");

            migrationBuilder.DropColumn(
                name: "PartitionNumber",
                table: "HDDPartition");

            migrationBuilder.DropColumn(
                name: "SizeInBytes",
                table: "HDDPartition");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "HDDPartition");

            migrationBuilder.RenameColumn(
                name: "isGpt",
                table: "HDDPartition",
                newName: "Gpt");

            migrationBuilder.AddColumn<string>(
                name: "PartitionId",
                table: "Client",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Client_PartitionId",
                table: "Client",
                column: "PartitionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Client_HDDPartition_PartitionId",
                table: "Client",
                column: "PartitionId",
                principalTable: "HDDPartition",
                principalColumn: "PK_HDDPartitionId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
