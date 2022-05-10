using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _03_RenameForreignKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NotificationEmail_Scheduler_FK_Scheduler",
                table: "NotificationEmail");

            migrationBuilder.DropForeignKey(
                name: "FK_NotificationEmailAttachment_Attachment_FK_Attachment",
                table: "NotificationEmailAttachment");

            migrationBuilder.DropForeignKey(
                name: "FK_NotificationEmailAttachment_NotificationEmail_FK_Email",
                table: "NotificationEmailAttachment");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Customer_FK_Customer",
                table: "User");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Systemhouse_FK_Systemhouse",
                table: "User");

            migrationBuilder.DropForeignKey(
                name: "FK_UserForgotPassword_User_FK_User",
                table: "UserForgotPassword");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRole_Role_FK_Role",
                table: "UserRole");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRole_User_FK_User",
                table: "UserRole");

            migrationBuilder.RenameColumn(
                name: "FK_User",
                table: "UserRole",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "FK_Role",
                table: "UserRole",
                newName: "RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_UserRole_FK_User",
                table: "UserRole",
                newName: "IX_UserRole_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserRole_FK_Role",
                table: "UserRole",
                newName: "IX_UserRole_RoleId");

            migrationBuilder.RenameColumn(
                name: "FK_User",
                table: "UserForgotPassword",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserForgotPassword_FK_User",
                table: "UserForgotPassword",
                newName: "IX_UserForgotPassword_UserId");

            migrationBuilder.RenameColumn(
                name: "FK_Systemhouse",
                table: "User",
                newName: "SystemhouseId");

            migrationBuilder.RenameColumn(
                name: "FK_Customer",
                table: "User",
                newName: "CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_User_FK_Systemhouse",
                table: "User",
                newName: "IX_User_SystemhouseId");

            migrationBuilder.RenameIndex(
                name: "IX_User_FK_Customer",
                table: "User",
                newName: "IX_User_CustomerId");

            migrationBuilder.RenameColumn(
                name: "FK_Email",
                table: "NotificationEmailAttachment",
                newName: "NotificationEmailId");

            migrationBuilder.RenameColumn(
                name: "FK_Attachment",
                table: "NotificationEmailAttachment",
                newName: "AttachmentId");

            migrationBuilder.RenameIndex(
                name: "IX_NotificationEmailAttachment_FK_Email",
                table: "NotificationEmailAttachment",
                newName: "IX_NotificationEmailAttachment_NotificationEmailId");

            migrationBuilder.RenameIndex(
                name: "IX_NotificationEmailAttachment_FK_Attachment",
                table: "NotificationEmailAttachment",
                newName: "IX_NotificationEmailAttachment_AttachmentId");

            migrationBuilder.RenameColumn(
                name: "FK_Scheduler",
                table: "NotificationEmail",
                newName: "SchedulerId");

            migrationBuilder.RenameIndex(
                name: "IX_NotificationEmail_FK_Scheduler",
                table: "NotificationEmail",
                newName: "IX_NotificationEmail_SchedulerId");

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationEmail_Scheduler_SchedulerId",
                table: "NotificationEmail",
                column: "SchedulerId",
                principalTable: "Scheduler",
                principalColumn: "PK_Scheduler",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationEmailAttachment_Attachment_AttachmentId",
                table: "NotificationEmailAttachment",
                column: "AttachmentId",
                principalTable: "Attachment",
                principalColumn: "PK_Attachment",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationEmailAttachment_NotificationEmail_NotificationEmailId",
                table: "NotificationEmailAttachment",
                column: "NotificationEmailId",
                principalTable: "NotificationEmail",
                principalColumn: "PK_NotificationEmail",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_User_Customer_CustomerId",
                table: "User",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "PK_Customer",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_User_Systemhouse_SystemhouseId",
                table: "User",
                column: "SystemhouseId",
                principalTable: "Systemhouse",
                principalColumn: "PK_Systemhouse",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserForgotPassword_User_UserId",
                table: "UserForgotPassword",
                column: "UserId",
                principalTable: "User",
                principalColumn: "PK_User",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRole_Role_RoleId",
                table: "UserRole",
                column: "RoleId",
                principalTable: "Role",
                principalColumn: "PK_Role",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRole_User_UserId",
                table: "UserRole",
                column: "UserId",
                principalTable: "User",
                principalColumn: "PK_User",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NotificationEmail_Scheduler_SchedulerId",
                table: "NotificationEmail");

            migrationBuilder.DropForeignKey(
                name: "FK_NotificationEmailAttachment_Attachment_AttachmentId",
                table: "NotificationEmailAttachment");

            migrationBuilder.DropForeignKey(
                name: "FK_NotificationEmailAttachment_NotificationEmail_NotificationEmailId",
                table: "NotificationEmailAttachment");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Customer_CustomerId",
                table: "User");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Systemhouse_SystemhouseId",
                table: "User");

            migrationBuilder.DropForeignKey(
                name: "FK_UserForgotPassword_User_UserId",
                table: "UserForgotPassword");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRole_Role_RoleId",
                table: "UserRole");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRole_User_UserId",
                table: "UserRole");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "UserRole",
                newName: "FK_User");

            migrationBuilder.RenameColumn(
                name: "RoleId",
                table: "UserRole",
                newName: "FK_Role");

            migrationBuilder.RenameIndex(
                name: "IX_UserRole_UserId",
                table: "UserRole",
                newName: "IX_UserRole_FK_User");

            migrationBuilder.RenameIndex(
                name: "IX_UserRole_RoleId",
                table: "UserRole",
                newName: "IX_UserRole_FK_Role");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "UserForgotPassword",
                newName: "FK_User");

            migrationBuilder.RenameIndex(
                name: "IX_UserForgotPassword_UserId",
                table: "UserForgotPassword",
                newName: "IX_UserForgotPassword_FK_User");

            migrationBuilder.RenameColumn(
                name: "SystemhouseId",
                table: "User",
                newName: "FK_Systemhouse");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "User",
                newName: "FK_Customer");

            migrationBuilder.RenameIndex(
                name: "IX_User_SystemhouseId",
                table: "User",
                newName: "IX_User_FK_Systemhouse");

            migrationBuilder.RenameIndex(
                name: "IX_User_CustomerId",
                table: "User",
                newName: "IX_User_FK_Customer");

            migrationBuilder.RenameColumn(
                name: "NotificationEmailId",
                table: "NotificationEmailAttachment",
                newName: "FK_Email");

            migrationBuilder.RenameColumn(
                name: "AttachmentId",
                table: "NotificationEmailAttachment",
                newName: "FK_Attachment");

            migrationBuilder.RenameIndex(
                name: "IX_NotificationEmailAttachment_NotificationEmailId",
                table: "NotificationEmailAttachment",
                newName: "IX_NotificationEmailAttachment_FK_Email");

            migrationBuilder.RenameIndex(
                name: "IX_NotificationEmailAttachment_AttachmentId",
                table: "NotificationEmailAttachment",
                newName: "IX_NotificationEmailAttachment_FK_Attachment");

            migrationBuilder.RenameColumn(
                name: "SchedulerId",
                table: "NotificationEmail",
                newName: "FK_Scheduler");

            migrationBuilder.RenameIndex(
                name: "IX_NotificationEmail_SchedulerId",
                table: "NotificationEmail",
                newName: "IX_NotificationEmail_FK_Scheduler");

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationEmail_Scheduler_FK_Scheduler",
                table: "NotificationEmail",
                column: "FK_Scheduler",
                principalTable: "Scheduler",
                principalColumn: "PK_Scheduler",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationEmailAttachment_Attachment_FK_Attachment",
                table: "NotificationEmailAttachment",
                column: "FK_Attachment",
                principalTable: "Attachment",
                principalColumn: "PK_Attachment",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationEmailAttachment_NotificationEmail_FK_Email",
                table: "NotificationEmailAttachment",
                column: "FK_Email",
                principalTable: "NotificationEmail",
                principalColumn: "PK_NotificationEmail",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_User_Customer_FK_Customer",
                table: "User",
                column: "FK_Customer",
                principalTable: "Customer",
                principalColumn: "PK_Customer",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_User_Systemhouse_FK_Systemhouse",
                table: "User",
                column: "FK_Systemhouse",
                principalTable: "Systemhouse",
                principalColumn: "PK_Systemhouse",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserForgotPassword_User_FK_User",
                table: "UserForgotPassword",
                column: "FK_User",
                principalTable: "User",
                principalColumn: "PK_User",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRole_Role_FK_Role",
                table: "UserRole",
                column: "FK_Role",
                principalTable: "Role",
                principalColumn: "PK_Role",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRole_User_FK_User",
                table: "UserRole",
                column: "FK_User",
                principalTable: "User",
                principalColumn: "PK_User",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
