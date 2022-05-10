using WPM_API.Data.DataContext.Entities;
using WPM_API.Data.DataContext.Entities.AssetMgmt;
using WPM_API.Data.DataContext.Entities.SmartDeploy;
using WPM_API.Data.DataContext.Entities.Storages;
using WPM_API.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Simple.OData.Client;
using Microsoft.Extensions.Configuration;

namespace  WPM_API.Data.DataContext
{
    public class DBData : DbContext
    {
        protected readonly IConfiguration Configuration;

        public DBData(DbContextOptions<DBData> options)
            : base(options)
        {
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder options)
        //{
        //    // connect to mysql with connection string from app settings
        //    var connectionString = Configuration.GetConnectionString("ConnectionStrings.Bitstream");
        //    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {            
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {                
                entity.SetTableName(entity.DisplayName());
            }
            modelBuilder.AddEntityConfigurationsFromAssembly(GetType().Assembly);

            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<ImageStream> ImageStreams { get; set; }
        public virtual DbSet<CustomerImageStream> CustomerImageStreams { get; set; }
        public virtual DbSet<RevisionMessage> RevisionMessages { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<UserForgotPassword> UserForgotPasswords { get; set; }
        public virtual DbSet<Attachment> Attachments { get; set; }
        public virtual DbSet<Scheduler> Schedulers { get; set; }
        public virtual DbSet<NotificationEmail> NotificationEmails { get; set; }
        public virtual DbSet<NotificationEmailAttachment> NotificationEmailAttachments { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<Subscription> Subscriptions { get; set; }
        public virtual DbSet<Systemhouse> Systemhouse { get; set; }
        public virtual DbSet<Base> Base { get; set; }
        public virtual DbSet<Domain> Domain { get; set; }
        public virtual DbSet<Script> Scripts { get; set; }
        public virtual DbSet<ExecutionLog> Logs { get; set; }
        public virtual DbSet<ErrorLog> ErrorLogs { get; set; }
        public virtual DbSet<Software> Software { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<ShopItem> ShopItems { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<ChangeLog> ChangeLogs { get; set; }
        public virtual DbSet<DomainUser> DomainUsers { get; set; }
        public virtual DbSet<NetworkConfiguration> NetworkConfigurations { get; set; }
        public virtual DbSet<Hardware> Hardwares { get; set; }
        public virtual DbSet<Bios> BiosSet { get; set; }
        public virtual DbSet<OS> OSSet { get; set; }
        public virtual DbSet<ClientProperty> ClientProperties { get; set; }
        public virtual DbSet<MacAddress> MacAddressSet { get; set; }
        public virtual DbSet<HDDPartition> HDDPartitions { get; set; }
        public virtual DbSet<Purchase> ClientPurchases { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<Token> Tokens { get; set; }
        public virtual DbSet<AzureBlobStorage> AzureBlobStorages { get; set; }
        public virtual DbSet<ClientParameter> ClientParameters { get; set; }
        public virtual DbSet<Certification> Certifications { get; set; }
        public virtual DbSet<Workflow> Workflows { get; set; }
        public virtual DbSet<DomainRegistrationTemp> DomainRegistrations { get; set; }
        public virtual DbSet<Inventory> Inventory { get; set; }
        public virtual DbSet<PreinstalledSoftware> PreinstalledSoftwares { get; set; }
        public virtual DbSet<WMIInvenotryCmds> WMIInvenotryCmds { get; set; }
        public virtual DbSet<HardwareModel> HardwareModels { get; set; }
        public virtual DbSet<Driver> Drivers { get; set; }
        public virtual DbSet<BIOSModel> BIOSModels { get; set; }
        public virtual DbSet<OSModel> OSModels { get; set; }
        public virtual DbSet<AssetModel> AssetModels { get; set; }
        public virtual DbSet<AssetType> AssetTypes { get; set; }
        public virtual DbSet<AssetClass> AssetClasses { get; set; }
        public virtual DbSet<VendorModel> VendorModels { get; set; }
        public virtual DbSet<Architecture> Architectures { get; set; }
        public virtual DbSet<OsVersionName> OsVersionNames { get; set; }
        public virtual DbSet<Win10Version> Win10Versions { get; set; }
        public virtual DbSet<StorageEntryPoint> StorageEntryPoints { get; set; }
        public virtual DbSet<SoftwareStream> SoftwareStreams { get; set; }
        public virtual DbSet<CustomerSoftware> CustomerSoftwares { get; set; }
        public virtual DbSet<CustomerSoftwareStream> CustomerSoftwareStreams { get; set; }
        public virtual DbSet<ReleasePlan> ReleasePlans { get; set; }
        public virtual DbSet<Image> Images { get; set; }
        public virtual DbSet<CustomerImage> CustomerImages { get; set; }
        public virtual DbSet<DriverShopItem> DriverShopItems { get; set; }
        public virtual DbSet<CustomerHardwareModel> CustomerHardwareModels { get; set; }
        public virtual DbSet<BaseStatus> BaseStatuses { get; set; }
        public virtual DbSet<ImagesSystemhouse> ImagesSystemhouses { get; set; }
        public virtual DbSet<ImagesCustomer> ImagesCustomers { get; set; }
        public virtual DbSet<AdminDeviceOption> AdminDeviceOptions { get; set; }
        public virtual DbSet<CustomerDriver> CustomerDrivers { get; set; }
    }
}
