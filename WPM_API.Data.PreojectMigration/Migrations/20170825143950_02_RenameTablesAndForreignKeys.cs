using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _02_RenameTablesAndForreignKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AzureCredentials_Customer_FK_Customer",
                table: "AzureCredentials");

            migrationBuilder.DropForeignKey(
                name: "FK_Base_AzureCredentials_FK_Credentials",
                table: "Base");

            migrationBuilder.DropForeignKey(
                name: "FK_Customer_Systemhouse_FK_Systemhouse",
                table: "Customer");

            migrationBuilder.DropForeignKey(
                name: "FK_Domain_Customer_FK_Customer",
                table: "Domain");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscription_Customer_FK_Customer",
                table: "Subscription");

            migrationBuilder.DropIndex(
                name: "IX_Subscription_FK_Customer",
                table: "Subscription");

            migrationBuilder.DropIndex(
                name: "IX_Domain_FK_Customer",
                table: "Domain");

            migrationBuilder.DropIndex(
                name: "IX_Customer_FK_Systemhouse",
                table: "Customer");

            migrationBuilder.DropIndex(
                name: "IX_Base_FK_Credentials",
                table: "Base");

            migrationBuilder.DropIndex(
                name: "IX_AzureCredentials_FK_Customer",
                table: "AzureCredentials");

            migrationBuilder.DropColumn(
                name: "FK_Customer",
                table: "Subscription");

            migrationBuilder.DropColumn(
                name: "FK_Customer",
                table: "Domain");

            migrationBuilder.DropColumn(
                name: "FK_Systemhouse",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "FK_Credentials",
                table: "Base");

            migrationBuilder.DropColumn(
                name: "FK_Customer",
                table: "AzureCredentials");

            migrationBuilder.RenameColumn(
                name: "PK_AzureCredentials",
                table: "AzureCredentials",
                newName: "Id");

            migrationBuilder.AddColumn<string>(
                name: "CustomerId",
                table: "Subscription",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Subscription",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerId",
                table: "Domain",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SystemhouseId",
                table: "Customer",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CredentialsId",
                table: "Base",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerId",
                table: "AzureCredentials",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subscription_CustomerId",
                table: "Subscription",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Domain_CustomerId",
                table: "Domain",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_SystemhouseId",
                table: "Customer",
                column: "SystemhouseId");

            migrationBuilder.CreateIndex(
                name: "IX_Base_CredentialsId",
                table: "Base",
                column: "CredentialsId");

            migrationBuilder.CreateIndex(
                name: "IX_AzureCredentials_CustomerId",
                table: "AzureCredentials",
                column: "CustomerId",
                unique: true,
                filter: "[CustomerId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_AzureCredentials_Customer_CustomerId",
                table: "AzureCredentials",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "PK_Customer",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Base_AzureCredentials_CredentialsId",
                table: "Base",
                column: "CredentialsId",
                principalTable: "AzureCredentials",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_Systemhouse_SystemhouseId",
                table: "Customer",
                column: "SystemhouseId",
                principalTable: "Systemhouse",
                principalColumn: "PK_Systemhouse",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Domain_Customer_CustomerId",
                table: "Domain",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "PK_Customer",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscription_Customer_CustomerId",
                table: "Subscription",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "PK_Customer",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AzureCredentials_Customer_CustomerId",
                table: "AzureCredentials");

            migrationBuilder.DropForeignKey(
                name: "FK_Base_AzureCredentials_CredentialsId",
                table: "Base");

            migrationBuilder.DropForeignKey(
                name: "FK_Customer_Systemhouse_SystemhouseId",
                table: "Customer");

            migrationBuilder.DropForeignKey(
                name: "FK_Domain_Customer_CustomerId",
                table: "Domain");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscription_Customer_CustomerId",
                table: "Subscription");

            migrationBuilder.DropIndex(
                name: "IX_Subscription_CustomerId",
                table: "Subscription");

            migrationBuilder.DropIndex(
                name: "IX_Domain_CustomerId",
                table: "Domain");

            migrationBuilder.DropIndex(
                name: "IX_Customer_SystemhouseId",
                table: "Customer");

            migrationBuilder.DropIndex(
                name: "IX_Base_CredentialsId",
                table: "Base");

            migrationBuilder.DropIndex(
                name: "IX_AzureCredentials_CustomerId",
                table: "AzureCredentials");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "Subscription");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Subscription");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "Domain");

            migrationBuilder.DropColumn(
                name: "SystemhouseId",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "CredentialsId",
                table: "Base");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "AzureCredentials");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "AzureCredentials",
                newName: "PK_AzureCredentials");

            migrationBuilder.AddColumn<string>(
                name: "FK_Customer",
                table: "Subscription",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FK_Customer",
                table: "Domain",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FK_Systemhouse",
                table: "Customer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FK_Credentials",
                table: "Base",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FK_Customer",
                table: "AzureCredentials",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subscription_FK_Customer",
                table: "Subscription",
                column: "FK_Customer");

            migrationBuilder.CreateIndex(
                name: "IX_Domain_FK_Customer",
                table: "Domain",
                column: "FK_Customer");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_FK_Systemhouse",
                table: "Customer",
                column: "FK_Systemhouse");

            migrationBuilder.CreateIndex(
                name: "IX_Base_FK_Credentials",
                table: "Base",
                column: "FK_Credentials");

            migrationBuilder.CreateIndex(
                name: "IX_AzureCredentials_FK_Customer",
                table: "AzureCredentials",
                column: "FK_Customer",
                unique: true,
                filter: "[FK_Customer] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_AzureCredentials_Customer_FK_Customer",
                table: "AzureCredentials",
                column: "FK_Customer",
                principalTable: "Customer",
                principalColumn: "PK_Customer",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Base_AzureCredentials_FK_Credentials",
                table: "Base",
                column: "FK_Credentials",
                principalTable: "AzureCredentials",
                principalColumn: "PK_AzureCredentials",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_Systemhouse_FK_Systemhouse",
                table: "Customer",
                column: "FK_Systemhouse",
                principalTable: "Systemhouse",
                principalColumn: "PK_Systemhouse",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Domain_Customer_FK_Customer",
                table: "Domain",
                column: "FK_Customer",
                principalTable: "Customer",
                principalColumn: "PK_Customer",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscription_Customer_FK_Customer",
                table: "Subscription",
                column: "FK_Customer",
                principalTable: "Customer",
                principalColumn: "PK_Customer",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
