using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using WPM_API.Data.DataContext;
using WPM_API.Common;

namespace WPM_API.Data.ProjectMigration.Migrations
{
    [DbContext(typeof(DBData))]
    [Migration("20170712140933_00_Initial")]
    partial class _00_Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
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
                        .ValueGeneratedOnAdd()
                        .HasColumnName("PK_AzureCredentials");

                    b.Property<string>("ClientId");

                    b.Property<string>("ClientSecret");

                    b.Property<string>("Customer_Id")
                        .HasColumnName("FK_Customer");

                    b.Property<string>("TenantId");

                    b.HasKey("Id");

                    b.HasIndex("Customer_Id")
                        .IsUnique();

                    b.ToTable("AzureCredentials");
                });

            modelBuilder.Entity("Bitstream.Data.DataContext.Entities.Customer", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("PK_Customer");

                    b.Property<string>("Name");

                    b.Property<string>("Systemhouse_Id")
                        .HasColumnName("FK_Systemhouse");

                    b.HasKey("Id");

                    b.HasIndex("Systemhouse_Id");

                    b.ToTable("Customer");
                });

            modelBuilder.Entity("Bitstream.Data.DataContext.Entities.Domain", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("PK_Domain");

                    b.Property<string>("Customer_Id")
                        .HasColumnName("FK_Customer");

                    b.Property<string>("GpoId");

                    b.Property<string>("Name");

                    b.Property<string>("Status");

                    b.Property<string>("Tld");

                    b.HasKey("Id");

                    b.HasIndex("Customer_Id");

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

                    b.Property<string>("SchedulerId")
                        .HasColumnName("FK_Scheduler");

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

                    b.Property<string>("AttachmentId")
                        .HasColumnName("FK_Attachment");

                    b.Property<string>("NotificationEmailId")
                        .HasColumnName("FK_Email");

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

            modelBuilder.Entity("Bitstream.Data.DataContext.Entities.Subscription", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("PK_Subscription");

                    b.Property<string>("Customer_Id")
                        .HasColumnName("FK_Customer");

                    b.Property<string>("SubscriptionId");

                    b.HasKey("Id");

                    b.HasIndex("Customer_Id");

                    b.ToTable("Subscription");
                });

            modelBuilder.Entity("Bitstream.Data.DataContext.Entities.Systemhouse", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("PK_Systemhouse");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Systemhouse");
                });

            modelBuilder.Entity("Bitstream.Data.DataContext.Entities.User", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("PK_User");

                    b.Property<bool>("Active");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("CustomerId")
                        .HasColumnName("FK_Customer");

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
                        .IsRequired()
                        .HasColumnName("FK_Systemhouse");

                    b.Property<string>("UpdatedByUserId");

                    b.Property<DateTime>("UpdatedDate");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.HasIndex("DeletedByUserId");

                    b.HasIndex("SystemhouseId");

                    b.HasIndex("UpdatedByUserId");

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

                    b.Property<string>("UserId")
                        .HasColumnName("FK_User");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserForgotPassword");
                });

            modelBuilder.Entity("Bitstream.Data.DataContext.Entities.UserRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("PK_UserRole");

                    b.Property<string>("RoleId")
                        .HasColumnName("FK_Role");

                    b.Property<string>("UserId")
                        .HasColumnName("FK_User");

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
                        .HasForeignKey("Bitstream.Data.DataContext.Entities.AzureCredentials", "Customer_Id");
                });

            modelBuilder.Entity("Bitstream.Data.DataContext.Entities.Customer", b =>
                {
                    b.HasOne("Bitstream.Data.DataContext.Entities.Systemhouse", "Systemhouse")
                        .WithMany("Customer")
                        .HasForeignKey("Systemhouse_Id");
                });

            modelBuilder.Entity("Bitstream.Data.DataContext.Entities.Domain", b =>
                {
                    b.HasOne("Bitstream.Data.DataContext.Entities.Customer", "Customer")
                        .WithMany()
                        .HasForeignKey("Customer_Id");

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

            modelBuilder.Entity("Bitstream.Data.DataContext.Entities.Subscription", b =>
                {
                    b.HasOne("Bitstream.Data.DataContext.Entities.Customer", "Customer")
                        .WithMany("Subscriptions")
                        .HasForeignKey("Customer_Id");
                });

            modelBuilder.Entity("Bitstream.Data.DataContext.Entities.User", b =>
                {
                    b.HasOne("Bitstream.Data.DataContext.Entities.Customer", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId");

                    b.HasOne("Bitstream.Data.DataContext.Entities.User", "DeletedByUser")
                        .WithMany("DeletedUsers")
                        .HasForeignKey("DeletedByUserId");

                    b.HasOne("Bitstream.Data.DataContext.Entities.Systemhouse", "Systemhouse")
                        .WithMany()
                        .HasForeignKey("SystemhouseId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Bitstream.Data.DataContext.Entities.User", "UpdatedByUser")
                        .WithMany("UpdatedUsers")
                        .HasForeignKey("UpdatedByUserId");
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
        }
    }
}
