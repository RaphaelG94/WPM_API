using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    public partial class _00_Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GroupPolicyObject",
                columns: table => new
                {
                    PK_GPO = table.Column<string>(nullable: false),
                    BsiCertified = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupPolicyObject", x => x.PK_GPO);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    PK_Role = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.PK_Role);
                });

            migrationBuilder.CreateTable(
                name: "Systemhouse",
                columns: table => new
                {
                    PK_Systemhouse = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Systemhouse", x => x.PK_Systemhouse);
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    PK_Customer = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    FK_Systemhouse = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.PK_Customer);
                    table.ForeignKey(
                        name: "FK_Customer_Systemhouse_FK_Systemhouse",
                        column: x => x.FK_Systemhouse,
                        principalTable: "Systemhouse",
                        principalColumn: "PK_Systemhouse",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AzureCredentials",
                columns: table => new
                {
                    PK_AzureCredentials = table.Column<string>(nullable: false),
                    ClientId = table.Column<string>(nullable: true),
                    ClientSecret = table.Column<string>(nullable: true),
                    FK_Customer = table.Column<string>(nullable: true),
                    TenantId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AzureCredentials", x => x.PK_AzureCredentials);
                    table.ForeignKey(
                        name: "FK_AzureCredentials_Customer_FK_Customer",
                        column: x => x.FK_Customer,
                        principalTable: "Customer",
                        principalColumn: "PK_Customer",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Domain",
                columns: table => new
                {
                    PK_Domain = table.Column<string>(nullable: false),
                    FK_Customer = table.Column<string>(nullable: true),
                    GpoId = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    Tld = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Domain", x => x.PK_Domain);
                    table.ForeignKey(
                        name: "FK_Domain_Customer_FK_Customer",
                        column: x => x.FK_Customer,
                        principalTable: "Customer",
                        principalColumn: "PK_Customer",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Domain_GroupPolicyObject_GpoId",
                        column: x => x.GpoId,
                        principalTable: "GroupPolicyObject",
                        principalColumn: "PK_GPO",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Subscription",
                columns: table => new
                {
                    PK_Subscription = table.Column<string>(nullable: false),
                    FK_Customer = table.Column<string>(nullable: true),
                    SubscriptionId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscription", x => x.PK_Subscription);
                    table.ForeignKey(
                        name: "FK_Subscription_Customer_FK_Customer",
                        column: x => x.FK_Customer,
                        principalTable: "Customer",
                        principalColumn: "PK_Customer",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    PK_User = table.Column<string>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    FK_Customer = table.Column<string>(nullable: true),
                    DeletedByUserId = table.Column<string>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    Email = table.Column<string>(maxLength: 64, nullable: false),
                    Login = table.Column<string>(maxLength: 64, nullable: false),
                    Password = table.Column<string>(maxLength: 256, nullable: false),
                    FK_Systemhouse = table.Column<string>(nullable: false),
                    UpdatedByUserId = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    UserName = table.Column<string>(maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.PK_User);
                    table.ForeignKey(
                        name: "FK_User_Customer_FK_Customer",
                        column: x => x.FK_Customer,
                        principalTable: "Customer",
                        principalColumn: "PK_Customer",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_User_User_DeletedByUserId",
                        column: x => x.DeletedByUserId,
                        principalTable: "User",
                        principalColumn: "PK_User",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_User_Systemhouse_FK_Systemhouse",
                        column: x => x.FK_Systemhouse,
                        principalTable: "Systemhouse",
                        principalColumn: "PK_Systemhouse",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_User_User_UpdatedByUserId",
                        column: x => x.UpdatedByUserId,
                        principalTable: "User",
                        principalColumn: "PK_User",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DomainUser",
                columns: table => new
                {
                    PK_DomainUser = table.Column<string>(nullable: false),
                    DomainId = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DomainUser", x => x.PK_DomainUser);
                    table.ForeignKey(
                        name: "FK_DomainUser_Domain_DomainId",
                        column: x => x.DomainId,
                        principalTable: "Domain",
                        principalColumn: "PK_Domain",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrganizationalUnit",
                columns: table => new
                {
                    PK_OrganizationalUnit = table.Column<string>(nullable: false),
                    DomainId = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    OrganizationalUnitId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationalUnit", x => x.PK_OrganizationalUnit);
                    table.ForeignKey(
                        name: "FK_OrganizationalUnit_Domain_DomainId",
                        column: x => x.DomainId,
                        principalTable: "Domain",
                        principalColumn: "PK_Domain",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrganizationalUnit_OrganizationalUnit_OrganizationalUnitId",
                        column: x => x.OrganizationalUnitId,
                        principalTable: "OrganizationalUnit",
                        principalColumn: "PK_OrganizationalUnit",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Attachment",
                columns: table => new
                {
                    PK_Attachment = table.Column<string>(nullable: false),
                    ContentType = table.Column<string>(maxLength: 256, nullable: false),
                    CreatedByUserId = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    FileName = table.Column<string>(maxLength: 256, nullable: false),
                    FileSize = table.Column<long>(nullable: false),
                    GenFileName = table.Column<string>(maxLength: 512, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachment", x => x.PK_Attachment);
                    table.ForeignKey(
                        name: "FK_Attachment_User_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "User",
                        principalColumn: "PK_User",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Scheduler",
                columns: table => new
                {
                    PK_Scheduler = table.Column<string>(nullable: false),
                    CreatedByUserId = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    EndProcessDate = table.Column<DateTime>(nullable: true),
                    EntityData1 = table.Column<string>(nullable: true),
                    EntityData2 = table.Column<string>(nullable: true),
                    EntityData3 = table.Column<string>(nullable: true),
                    EntityData4 = table.Column<string>(nullable: true),
                    EntityId1 = table.Column<string>(nullable: true),
                    EntityId2 = table.Column<string>(nullable: true),
                    EntityId3 = table.Column<string>(nullable: true),
                    EntityId4 = table.Column<string>(nullable: true),
                    ErrorMessage = table.Column<string>(nullable: true),
                    IsSynchronous = table.Column<bool>(nullable: false),
                    OnDate = table.Column<DateTime>(nullable: false),
                    ParentSchedulerId = table.Column<string>(nullable: true),
                    SchedulerActionType = table.Column<int>(nullable: false),
                    StartProcessDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scheduler", x => x.PK_Scheduler);
                    table.ForeignKey(
                        name: "FK_Scheduler_User_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "User",
                        principalColumn: "PK_User",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Scheduler_Scheduler_ParentSchedulerId",
                        column: x => x.ParentSchedulerId,
                        principalTable: "Scheduler",
                        principalColumn: "PK_Scheduler",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserCustomer",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CustomerId = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCustomer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserCustomer_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "PK_Customer",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserCustomer_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "PK_User",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserForgotPassword",
                columns: table => new
                {
                    PK_UserForgotPassword = table.Column<string>(nullable: false),
                    ApprovedDateTime = table.Column<DateTime>(nullable: true),
                    ApproverIpAddress = table.Column<string>(maxLength: 64, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatorIpAddress = table.Column<string>(maxLength: 64, nullable: true),
                    RequestGuid = table.Column<Guid>(nullable: false),
                    FK_User = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserForgotPassword", x => x.PK_UserForgotPassword);
                    table.ForeignKey(
                        name: "FK_UserForgotPassword_User_FK_User",
                        column: x => x.FK_User,
                        principalTable: "User",
                        principalColumn: "PK_User",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                columns: table => new
                {
                    PK_UserRole = table.Column<string>(nullable: false),
                    FK_Role = table.Column<string>(nullable: true),
                    FK_User = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => x.PK_UserRole);
                    table.ForeignKey(
                        name: "FK_UserRole_Role_FK_Role",
                        column: x => x.FK_Role,
                        principalTable: "Role",
                        principalColumn: "PK_Role",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserRole_User_FK_User",
                        column: x => x.FK_User,
                        principalTable: "User",
                        principalColumn: "PK_User",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserSubscription",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    SubscriptionId = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSubscription", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSubscription_Subscription_SubscriptionId",
                        column: x => x.SubscriptionId,
                        principalTable: "Subscription",
                        principalColumn: "PK_Subscription",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserSubscription_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "PK_User",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NotificationEmail",
                columns: table => new
                {
                    PK_NotificationEmail = table.Column<string>(nullable: false),
                    AttemptsCount = table.Column<int>(nullable: false),
                    Body = table.Column<string>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    LastAttemptDate = table.Column<DateTime>(nullable: true),
                    LastAttemptError = table.Column<string>(nullable: true),
                    ProcessedDate = table.Column<DateTime>(nullable: true),
                    FK_Scheduler = table.Column<string>(nullable: true),
                    Subject = table.Column<string>(maxLength: 1024, nullable: false),
                    ToBccEmailAddresses = table.Column<string>(nullable: true),
                    ToCcEmailAddresses = table.Column<string>(nullable: true),
                    ToEmailAddresses = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationEmail", x => x.PK_NotificationEmail);
                    table.ForeignKey(
                        name: "FK_NotificationEmail_Scheduler_FK_Scheduler",
                        column: x => x.FK_Scheduler,
                        principalTable: "Scheduler",
                        principalColumn: "PK_Scheduler",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NotificationEmailAttachment",
                columns: table => new
                {
                    PK_NotificationEmailAttachment = table.Column<string>(nullable: false),
                    FK_Attachment = table.Column<string>(nullable: true),
                    FK_Email = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationEmailAttachment", x => x.PK_NotificationEmailAttachment);
                    table.ForeignKey(
                        name: "FK_NotificationEmailAttachment_Attachment_FK_Attachment",
                        column: x => x.FK_Attachment,
                        principalTable: "Attachment",
                        principalColumn: "PK_Attachment",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NotificationEmailAttachment_NotificationEmail_FK_Email",
                        column: x => x.FK_Email,
                        principalTable: "NotificationEmail",
                        principalColumn: "PK_NotificationEmail",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attachment_CreatedByUserId",
                table: "Attachment",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AzureCredentials_FK_Customer",
                table: "AzureCredentials",
                column: "FK_Customer",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customer_FK_Systemhouse",
                table: "Customer",
                column: "FK_Systemhouse");

            migrationBuilder.CreateIndex(
                name: "IX_Domain_FK_Customer",
                table: "Domain",
                column: "FK_Customer");

            migrationBuilder.CreateIndex(
                name: "IX_Domain_GpoId",
                table: "Domain",
                column: "GpoId");

            migrationBuilder.CreateIndex(
                name: "IX_DomainUser_DomainId",
                table: "DomainUser",
                column: "DomainId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationEmail_FK_Scheduler",
                table: "NotificationEmail",
                column: "FK_Scheduler");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationEmailAttachment_FK_Attachment",
                table: "NotificationEmailAttachment",
                column: "FK_Attachment");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationEmailAttachment_FK_Email",
                table: "NotificationEmailAttachment",
                column: "FK_Email");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationalUnit_DomainId",
                table: "OrganizationalUnit",
                column: "DomainId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationalUnit_OrganizationalUnitId",
                table: "OrganizationalUnit",
                column: "OrganizationalUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Scheduler_CreatedByUserId",
                table: "Scheduler",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Scheduler_ParentSchedulerId",
                table: "Scheduler",
                column: "ParentSchedulerId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscription_FK_Customer",
                table: "Subscription",
                column: "FK_Customer");

            migrationBuilder.CreateIndex(
                name: "IX_User_FK_Customer",
                table: "User",
                column: "FK_Customer");

            migrationBuilder.CreateIndex(
                name: "IX_User_DeletedByUserId",
                table: "User",
                column: "DeletedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_User_FK_Systemhouse",
                table: "User",
                column: "FK_Systemhouse");

            migrationBuilder.CreateIndex(
                name: "IX_User_UpdatedByUserId",
                table: "User",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCustomer_CustomerId",
                table: "UserCustomer",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCustomer_UserId",
                table: "UserCustomer",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserForgotPassword_FK_User",
                table: "UserForgotPassword",
                column: "FK_User");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_FK_Role",
                table: "UserRole",
                column: "FK_Role");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_FK_User",
                table: "UserRole",
                column: "FK_User");

            migrationBuilder.CreateIndex(
                name: "IX_UserSubscription_SubscriptionId",
                table: "UserSubscription",
                column: "SubscriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSubscription_UserId",
                table: "UserSubscription",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AzureCredentials");

            migrationBuilder.DropTable(
                name: "DomainUser");

            migrationBuilder.DropTable(
                name: "NotificationEmailAttachment");

            migrationBuilder.DropTable(
                name: "OrganizationalUnit");

            migrationBuilder.DropTable(
                name: "UserCustomer");

            migrationBuilder.DropTable(
                name: "UserForgotPassword");

            migrationBuilder.DropTable(
                name: "UserRole");

            migrationBuilder.DropTable(
                name: "UserSubscription");

            migrationBuilder.DropTable(
                name: "Attachment");

            migrationBuilder.DropTable(
                name: "NotificationEmail");

            migrationBuilder.DropTable(
                name: "Domain");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "Subscription");

            migrationBuilder.DropTable(
                name: "Scheduler");

            migrationBuilder.DropTable(
                name: "GroupPolicyObject");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "Systemhouse");
        }
    }
}
