﻿// <auto-generated />
using WPM_API.Common;
using WPM_API.Data.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    [DbContext(typeof(DBData))]
    [Migration("20171122123859_13_VmLocalIp")]
    partial class _13_VmLocalIp
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Bitstream.Data.DataContext.Entities.Attachment", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("PK_Attachment");

                    b.Property<string>("ContentType")
                        .IsRequired()
                        .HasMaxLength(256);

                    b.Property<string>("CreatedByUserId");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasMaxLength(256);

                    b.Property<long>("FileSize");

                    b.Property<string>("GenFileName")
                        .IsRequired()
                        .HasMaxLength(512);

                    b.HasKey("Id");

                    b.HasIndex("CreatedByUserId");

                    b.ToTable("Attachment");
                });

            modelBuilder.Entity("Bitstream.Data.DataContext.Entities.AzureCredentials", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClientId");

                    b.Property<string>("ClientSecret");

                    b.Property<string>("CustomerId");

                    b.Property<string>("TenantId");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId")
                        .IsUnique()
                        .HasFilter("[CustomerId] IS NOT NULL");

                    b.ToTable("AzureCredentials");
                });

            modelBuilder.Entity("Bitstream.Data.DataContext.Entities.Base", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("PK_Base");

                    b.Property<string>("CreatedByUserId");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("CredentialsId");

                    b.Property<string>("DeletedByUserId");

                    b.Property<DateTime?>("DeletedDate");

                    b.Property<string>("Name");

                    b.Property<string>("ResourceGroupId");

                    b.Property<string>("Status");

                    b.Property<string>("StorageAccountId");

                    b.Property<string>("SubscriptionId");

                    b.Property<string>("UpdatedByUserId");

                    b.Property<DateTime>("UpdatedDate");

                    b.Property<string>("VirtualNetworkId");

                    b.Property<string>("VpnId");

                    b.HasKey("Id");

                    b.HasIndex("CredentialsId");

                    b.HasIndex("ResourceGroupId");

                    b.HasIndex("StorageAccountId");

                    b.HasIndex("SubscriptionId");

                    b.HasIndex("VirtualNetworkId");

                    b.HasIndex("VpnId");

                    b.ToTable("Base");
                });

            modelBuilder.Entity("Bitstream.Data.DataContext.Entities.Customer", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("PK_Customer");

                    b.Property<string>("CreatedByUserId");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("DeletedByUserId");

                    b.Property<DateTime?>("DeletedDate");

                    b.Property<string>("Name");

                    b.Property<string>("SystemhouseId");

                    b.Property<string>("UpdatedByUserId");

                    b.Property<DateTime>("UpdatedDate");

                    b.HasKey("Id");

                    b.HasIndex("SystemhouseId");

                    b.ToTable("Customer");
                });

            modelBuilder.Entity("Bitstream.Data.DataContext.Entities.Disk", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("PK_VirtualMachine");

                    b.Property<string>("Name");

                    b.Property<int>("SizeInGb");

                    b.Property<string>("VirtualMachineId");

                    b.HasKey("Id");

                    b.HasIndex("VirtualMachineId");

                    b.ToTable("Disk");
                });

            modelBuilder.Entity("Bitstream.Data.DataContext.Entities.Domain", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("PK_Domain");

                    b.Property<string>("BaseId");

                    b.Property<string>("CreatedByUserId");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("CustomerId");

                    b.Property<string>("DeletedByUserId");

                    b.Property<DateTime?>("DeletedDate");

                    b.Property<string>("ExecutionVMId");

                    b.Property<string>("GpoId");

                    b.Property<string>("Name");

                    b.Property<string>("Status");

                    b.Property<string>("Tld");

                    b.Property<string>("UpdatedByUserId");

                    b.Property<DateTime>("UpdatedDate");

                    b.HasKey("Id");

                    b.HasIndex("BaseId");

                    b.HasIndex("CustomerId");

                    b.HasIndex("GpoId");

                    b.ToTable("Domain");
                });

            modelBuilder.Entity("Bitstream.Data.DataContext.Entities.DomainUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("PK_DomainUser");

                    b.Property<string>("DomainId");

                    b.Property<string>("Email");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<string>("Password");

                    b.Property<string>("UserName");

                    b.HasKey("Id");

                    b.HasIndex("DomainId");

                    b.ToTable("DomainUser");
                });

            modelBuilder.Entity("Bitstream.Data.DataContext.Entities.GroupPolicyObject", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("PK_GPO");

                    b.Property<bool>("BsiCertified");

                    b.HasKey("Id");

                    b.ToTable("GroupPolicyObject");
                });

            modelBuilder.Entity("Bitstream.Data.DataContext.Entities.NotificationEmail", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("PK_NotificationEmail");

                    b.Property<int>("AttemptsCount");

                    b.Property<string>("Body")
                        .IsRequired();

                    b.Property<DateTime>("CreatedDate");

                    b.Property<DateTime?>("LastAttemptDate");

                    b.Property<string>("LastAttemptError");

                    b.Property<DateTime?>("ProcessedDate");

                    b.Property<string>("SchedulerId");

                    b.Property<string>("Subject")
                        .IsRequired()
                        .HasMaxLength(1024);

                    b.Property<string>("ToBccEmailAddresses");

                    b.Property<string>("ToCcEmailAddresses");

                    b.Property<string>("ToEmailAddresses")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("SchedulerId");

                    b.ToTable("NotificationEmail");
                });

            modelBuilder.Entity("Bitstream.Data.DataContext.Entities.NotificationEmailAttachment", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("PK_NotificationEmailAttachment");

                    b.Property<string>("AttachmentId");

                    b.Property<string>("NotificationEmailId");

                    b.HasKey("Id");

                    b.HasIndex("AttachmentId");

                    b.HasIndex("NotificationEmailId");

                    b.ToTable("NotificationEmailAttachment");
                });

            modelBuilder.Entity("Bitstream.Data.DataContext.Entities.OrganizationalUnit", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("PK_OrganizationalUnit");

                    b.Property<string>("DomainId");

                    b.Property<string>("Name");

                    b.Property<string>("OrganizationalUnitId");

                    b.HasKey("Id");

                    b.HasIndex("DomainId");

                    b.HasIndex("OrganizationalUnitId");

                    b.ToTable("OrganizationalUnit");
                });

            modelBuilder.Entity("Bitstream.Data.DataContext.Entities.ResourceGroup", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("PK_ResourceGroup");

                    b.Property<string>("Location");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("ResourceGroup");
                });

            modelBuilder.Entity("Bitstream.Data.DataContext.Entities.Role", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("PK_Role");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.HasKey("Id");

                    b.ToTable("Role");
                });

            modelBuilder.Entity("Bitstream.Data.DataContext.Entities.Scheduler", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("PK_Scheduler");

                    b.Property<string>("CreatedByUserId");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<DateTime?>("EndProcessDate");

                    b.Property<string>("EntityData1");

                    b.Property<string>("EntityData2");

                    b.Property<string>("EntityData3");

                    b.Property<string>("EntityData4");

                    b.Property<string>("EntityId1");

                    b.Property<string>("EntityId2");

                    b.Property<string>("EntityId3");

                    b.Property<string>("EntityId4");

                    b.Property<string>("ErrorMessage");

                    b.Property<bool>("IsSynchronous");

                    b.Property<DateTime>("OnDate");

                    b.Property<string>("ParentSchedulerId");

                    b.Property<int>("SchedulerActionType");

                    b.Property<DateTime?>("StartProcessDate");

                    b.HasKey("Id");

                    b.HasIndex("CreatedByUserId");

                    b.HasIndex("ParentSchedulerId");

                    b.ToTable("Scheduler");
                });

            modelBuilder.Entity("Bitstream.Data.DataContext.Entities.Script", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("PK_Script");

                    b.Property<string>("CreatedByUserId");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("DeletedByUserId");

                    b.Property<DateTime?>("DeletedDate");

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.Property<string>("UpdatedByUserId");

                    b.Property<DateTime>("UpdatedDate");

                    b.HasKey("Id");

                    b.ToTable("Script");
                });

            modelBuilder.Entity("Bitstream.Data.DataContext.Entities.ScriptVersion", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("PK_ScriptVersion");

                    b.Property<string>("ContentUrl");

                    b.Property<int>("Number");

                    b.Property<string>("ScriptId");

                    b.HasKey("Id");

                    b.HasIndex("ScriptId");

                    b.ToTable("ScriptVersion");
                });

            modelBuilder.Entity("Bitstream.Data.DataContext.Entities.StorageAccount", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("PK_StorageAccount");

                    b.Property<string>("AzureId");

                    b.Property<string>("Name");

                    b.Property<string>("Type");

                    b.HasKey("Id");

                    b.ToTable("StorageAccount");
                });

            modelBuilder.Entity("Bitstream.Data.DataContext.Entities.Subnet", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("PK_VirtualNetwork");

                    b.Property<string>("AddressRange");

                    b.Property<string>("Name");

                    b.Property<string>("VirtualNetworkId");

                    b.HasKey("Id");

                    b.HasIndex("VirtualNetworkId");

                    b.ToTable("Subnet");
                });

            modelBuilder.Entity("Bitstream.Data.DataContext.Entities.Subscription", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("PK_Subscription");

                    b.Property<string>("AzureId");

                    b.Property<string>("CustomerId");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.ToTable("Subscription");
                });

            modelBuilder.Entity("Bitstream.Data.DataContext.Entities.Systemhouse", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("PK_Systemhouse");

                    b.Property<string>("CreatedByUserId");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("DeletedByUserId");

                    b.Property<DateTime?>("DeletedDate");

                    b.Property<string>("Name");

                    b.Property<string>("UpdatedByUserId");

                    b.Property<DateTime>("UpdatedDate");

                    b.HasKey("Id");

                    b.ToTable("Systemhouse");
                });

            modelBuilder.Entity("Bitstream.Data.DataContext.Entities.User", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("PK_User");

                    b.Property<bool>("Active");

                    b.Property<string>("CreatedByUserId");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("CustomerId");

                    b.Property<string>("DeletedByUserId");

                    b.Property<DateTime?>("DeletedDate");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(256);

                    b.Property<string>("SystemhouseId")
                        .IsRequired();

                    b.Property<string>("UpdatedByUserId");

                    b.Property<DateTime>("UpdatedDate");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.HasIndex("SystemhouseId");

                    b.ToTable("User");
                });

            modelBuilder.Entity("Bitstream.Data.DataContext.Entities.UserCustomer", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CustomerId");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.HasIndex("UserId");

                    b.ToTable("UserCustomer");
                });

            modelBuilder.Entity("Bitstream.Data.DataContext.Entities.UserForgotPassword", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("PK_UserForgotPassword");

                    b.Property<DateTime?>("ApprovedDateTime");

                    b.Property<string>("ApproverIpAddress")
                        .HasMaxLength(64);

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("CreatorIpAddress")
                        .HasMaxLength(64);

                    b.Property<Guid>("RequestGuid");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserForgotPassword");
                });

            modelBuilder.Entity("Bitstream.Data.DataContext.Entities.UserRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("PK_UserRole");

                    b.Property<string>("RoleId");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId");

                    b.ToTable("UserRole");
                });

            modelBuilder.Entity("Bitstream.Data.DataContext.Entities.UserSubscription", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("SubscriptionId");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("SubscriptionId");

                    b.HasIndex("UserId");

                    b.ToTable("UserSubscription");
                });

            modelBuilder.Entity("Bitstream.Data.DataContext.Entities.Variable", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("PK_Variable");

                    b.Property<string>("CustomerId");

                    b.Property<string>("Default");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.ToTable("Variable");
                });

            modelBuilder.Entity("Bitstream.Data.DataContext.Entities.VirtualMachine", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("PK_VirtualMachine");

                    b.Property<string>("AdminUserName");

                    b.Property<string>("AdminUserPassword");

                    b.Property<string>("AzureId");

                    b.Property<string>("BaseId");

                    b.Property<string>("LocalIp");

                    b.Property<string>("Location");

                    b.Property<string>("Name");

                    b.Property<string>("OperatingSystem");

                    b.Property<string>("Subnet");

                    b.Property<string>("SystemDiskId");

                    b.Property<string>("Type");

                    b.HasKey("Id");

                    b.HasIndex("BaseId");

                    b.HasIndex("SystemDiskId");

                    b.ToTable("VirtualMachine");
                });

            modelBuilder.Entity("Bitstream.Data.DataContext.Entities.VirtualNetwork", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("PK_VirtualNetwork");

                    b.Property<string>("AddressRange");

                    b.Property<string>("AzureId");

                    b.Property<string>("Dns");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("VirtualNetwork");
                });

            modelBuilder.Entity("Bitstream.Data.DataContext.Entities.Vpn", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("PK_Vpn");

                    b.Property<string>("LocalAddressRange");

                    b.Property<string>("LocalPublicIp");

                    b.Property<string>("Name");

                    b.Property<string>("VirtualNetwork");

                    b.Property<string>("VirtualPublicIp");

                    b.HasKey("Id");

                    b.ToTable("Vpn");
                });

            modelBuilder.Entity("Bitstream.Data.DataContext.Entities.Attachment", b =>
                {
                    b.HasOne("Bitstream.Data.DataContext.Entities.User", "CreatedByUser")
                        .WithMany()
                        .HasForeignKey("CreatedByUserId");
                });

            modelBuilder.Entity("Bitstream.Data.DataContext.Entities.AzureCredentials", b =>
                {
                    b.HasOne("Bitstream.Data.DataContext.Entities.Customer", "Customer")
                        .WithOne("AzureCredentials")
                        .HasForeignKey("Bitstream.Data.DataContext.Entities.AzureCredentials", "CustomerId");
                });

            modelBuilder.Entity("Bitstream.Data.DataContext.Entities.Base", b =>
                {
                    b.HasOne("Bitstream.Data.DataContext.Entities.AzureCredentials", "Credentials")
                        .WithMany()
                        .HasForeignKey("CredentialsId");

                    b.HasOne("Bitstream.Data.DataContext.Entities.ResourceGroup", "ResourceGroup")
                        .WithMany()
                        .HasForeignKey("ResourceGroupId");

                    b.HasOne("Bitstream.Data.DataContext.Entities.StorageAccount", "StorageAccount")
                        .WithMany()
                        .HasForeignKey("StorageAccountId");

                    b.HasOne("Bitstream.Data.DataContext.Entities.Subscription", "Subscription")
                        .WithMany()
                        .HasForeignKey("SubscriptionId");

                    b.HasOne("Bitstream.Data.DataContext.Entities.VirtualNetwork", "VirtualNetwork")
                        .WithMany()
                        .HasForeignKey("VirtualNetworkId");

                    b.HasOne("Bitstream.Data.DataContext.Entities.Vpn", "Vpn")
                        .WithMany()
                        .HasForeignKey("VpnId");
                });

            modelBuilder.Entity("Bitstream.Data.DataContext.Entities.Customer", b =>
                {
                    b.HasOne("Bitstream.Data.DataContext.Entities.Systemhouse", "Systemhouse")
                        .WithMany("Customer")
                        .HasForeignKey("SystemhouseId");
                });

            modelBuilder.Entity("Bitstream.Data.DataContext.Entities.Disk", b =>
                {
                    b.HasOne("Bitstream.Data.DataContext.Entities.VirtualMachine")
                        .WithMany("AdditionalDisks")
                        .HasForeignKey("VirtualMachineId");
                });

            modelBuilder.Entity("Bitstream.Data.DataContext.Entities.Domain", b =>
                {
                    b.HasOne("Bitstream.Data.DataContext.Entities.Base", "Base")
                        .WithMany()
                        .HasForeignKey("BaseId");

                    b.HasOne("Bitstream.Data.DataContext.Entities.Customer", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId");

                    b.HasOne("Bitstream.Data.DataContext.Entities.GroupPolicyObject", "Gpo")
                        .WithMany()
                        .HasForeignKey("GpoId");
                });

            modelBuilder.Entity("Bitstream.Data.DataContext.Entities.DomainUser", b =>
                {
                    b.HasOne("Bitstream.Data.DataContext.Entities.Domain")
                        .WithMany("DomainUsers")
                        .HasForeignKey("DomainId");
                });

            modelBuilder.Entity("Bitstream.Data.DataContext.Entities.NotificationEmail", b =>
                {
                    b.HasOne("Bitstream.Data.DataContext.Entities.Scheduler", "Scheduler")
                        .WithMany("NotificationEmails")
                        .HasForeignKey("SchedulerId");
                });

            modelBuilder.Entity("Bitstream.Data.DataContext.Entities.NotificationEmailAttachment", b =>
                {
                    b.HasOne("Bitstream.Data.DataContext.Entities.Attachment", "Attachment")
                        .WithMany()
                        .HasForeignKey("AttachmentId");

                    b.HasOne("Bitstream.Data.DataContext.Entities.NotificationEmail", "NotificationEmail")
                        .WithMany("NotificationEmailAttachments")
                        .HasForeignKey("NotificationEmailId");
                });

            modelBuilder.Entity("Bitstream.Data.DataContext.Entities.OrganizationalUnit", b =>
                {
                    b.HasOne("Bitstream.Data.DataContext.Entities.Domain")
                        .WithMany("OrganizationalUnits")
                        .HasForeignKey("DomainId");

                    b.HasOne("Bitstream.Data.DataContext.Entities.OrganizationalUnit")
                        .WithMany("OrganizationalUnits")
                        .HasForeignKey("OrganizationalUnitId");
                });

            modelBuilder.Entity("Bitstream.Data.DataContext.Entities.Scheduler", b =>
                {
                    b.HasOne("Bitstream.Data.DataContext.Entities.User", "CreatedByUser")
                        .WithMany()
                        .HasForeignKey("CreatedByUserId");

                    b.HasOne("Bitstream.Data.DataContext.Entities.Scheduler", "ParentScheduler")
                        .WithMany("ChildSchedulers")
                        .HasForeignKey("ParentSchedulerId");
                });

            modelBuilder.Entity("Bitstream.Data.DataContext.Entities.ScriptVersion", b =>
                {
                    b.HasOne("Bitstream.Data.DataContext.Entities.Script")
                        .WithMany("Versions")
                        .HasForeignKey("ScriptId");
                });

            modelBuilder.Entity("Bitstream.Data.DataContext.Entities.Subnet", b =>
                {
                    b.HasOne("Bitstream.Data.DataContext.Entities.VirtualNetwork")
                        .WithMany("Subnets")
                        .HasForeignKey("VirtualNetworkId");
                });

            modelBuilder.Entity("Bitstream.Data.DataContext.Entities.Subscription", b =>
                {
                    b.HasOne("Bitstream.Data.DataContext.Entities.Customer")
                        .WithMany("Subscriptions")
                        .HasForeignKey("CustomerId");
                });

            modelBuilder.Entity("Bitstream.Data.DataContext.Entities.User", b =>
                {
                    b.HasOne("Bitstream.Data.DataContext.Entities.Customer", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId");

                    b.HasOne("Bitstream.Data.DataContext.Entities.Systemhouse", "Systemhouse")
                        .WithMany()
                        .HasForeignKey("SystemhouseId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Bitstream.Data.DataContext.Entities.UserCustomer", b =>
                {
                    b.HasOne("Bitstream.Data.DataContext.Entities.Customer", "Customer")
                        .WithMany("UserCustomer")
                        .HasForeignKey("CustomerId");

                    b.HasOne("Bitstream.Data.DataContext.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Bitstream.Data.DataContext.Entities.UserForgotPassword", b =>
                {
                    b.HasOne("Bitstream.Data.DataContext.Entities.User", "User")
                        .WithMany("UserForgotPasswords")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Bitstream.Data.DataContext.Entities.UserRole", b =>
                {
                    b.HasOne("Bitstream.Data.DataContext.Entities.Role", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId");

                    b.HasOne("Bitstream.Data.DataContext.Entities.User", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Bitstream.Data.DataContext.Entities.UserSubscription", b =>
                {
                    b.HasOne("Bitstream.Data.DataContext.Entities.Subscription", "Subscription")
                        .WithMany("UserSubscription")
                        .HasForeignKey("SubscriptionId");

                    b.HasOne("Bitstream.Data.DataContext.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Bitstream.Data.DataContext.Entities.Variable", b =>
                {
                    b.HasOne("Bitstream.Data.DataContext.Entities.Customer", "Customer")
                        .WithMany("Variables")
                        .HasForeignKey("CustomerId");
                });

            modelBuilder.Entity("Bitstream.Data.DataContext.Entities.VirtualMachine", b =>
                {
                    b.HasOne("Bitstream.Data.DataContext.Entities.Base")
                        .WithMany("VirtualMachines")
                        .HasForeignKey("BaseId");

                    b.HasOne("Bitstream.Data.DataContext.Entities.Disk", "SystemDisk")
                        .WithMany()
                        .HasForeignKey("SystemDiskId");
                });
#pragma warning restore 612, 618
        }
    }
}
