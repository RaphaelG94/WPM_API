using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using WPM_API.Data.DataContext;
using WPM_API.Data.DataContext.Entities;
using WPM_API.Data.DataContext.Interfaces;
using WPM_API.Data.DataRepository;
using WPM_API.Data.DataRepository.Users;
using WPM_API.Data.Exceptions;
using WPM_API.Data.Transaction;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace  WPM_API.Data.Infrastructure
{
    public class UnitOfWork : IDisposable
    {
        private readonly DBData Context;
        private readonly DataContextProvider ContextProvider;
        private DbContextTransactionWrapper CurrentTransaction { get; set; }

        public UnitOfWork(DBData context)
        {
            Context = context;
            ContextProvider = new DataContextProvider(Context);
        }

        private IServiceScope CreatedInScope { get; set; }
        public static UnitOfWork CreateInScope(DBData context, IServiceScope createdInScope)
        {
            var uow = new UnitOfWork(context)
            {
                CreatedInScope = createdInScope
            };
            return uow;
        }

        #region repositories
        public CustomerImageStreamRepository CustomerImageStreams => GetRepository<CustomerImageStreamRepository>();
        public ImageStreamRepository ImageStreams => GetRepository<ImageStreamRepository>();
        public UserRepository Users => GetRepository<UserRepository>();
        public AttachmentRepository Attachments => GetRepository<AttachmentRepository>();
        public SchedulerRepository Schedulers => GetRepository<SchedulerRepository>();
        public CustomerRepository Customers => GetRepository<CustomerRepository>();
        public DomainRepository Domains => GetRepository<DomainRepository>();
        public BaseRepository Bases => GetRepository<BaseRepository>();
        public SubscriptionRepository Subscriptions => GetRepository<SubscriptionRepository>();
        public SystemhouseRepository Systemhouses => GetRepository<SystemhouseRepository>();
        public ScriptRepository Scripts => GetRepository<ScriptRepository>();
        public ScriptVersionRepository ScriptVersions => GetRepository<ScriptVersionRepository>();
        public LogRepository Logs => GetRepository<LogRepository>();
        public SoftwareRepository Software => GetRepository<SoftwareRepository>();
        public UnattendRepository Unattend => GetRepository<UnattendRepository>();
        public ClientRepository Clients => GetRepository<ClientRepository>();
        public ShopRepository Shop => GetRepository<ShopRepository>();
        public OrderRepository Order => GetRepository<OrderRepository>();
        public RuleRepository Rules => GetRepository<RuleRepository>();
        public TaskRepository Tasks => GetRepository<TaskRepository>();
        public TokenRepository Token => GetRepository<TokenRepository>();
        public StatusRepository Status => GetRepository<StatusRepository>();
        public CategoryRepository Categories => GetRepository<CategoryRepository>();
        public DomainUserRepository DomainUser => GetRepository<DomainUserRepository>();
        public ClientPropertyRepository ClientProperties => GetRepository<ClientPropertyRepository>();
        public FileRepository Files => GetRepository<FileRepository>();
        public LocationRepository Locations => GetRepository<LocationRepository>();
        public PersonRepository Persons => GetRepository<PersonRepository>();
        public CompanyRepository Companies => GetRepository<CompanyRepository>();
        public AzureBlobStorageRepository AzureBlobs => GetRepository<AzureBlobStorageRepository>();
        public StorageAccountRepository StorageAccounts => GetRepository<StorageAccountRepository>();
        public ResourceGroupRepository ResourceGroups => GetRepository<ResourceGroupRepository>();
        public ClientParameterRepository ClientParameters => GetRepository<ClientParameterRepository>();
        public VirtualMachinesRepository VirtualMachines => GetRepository<VirtualMachinesRepository>();
        public ErrorLogsRepository ErrorLogs => GetRepository<ErrorLogsRepository>();
        public SubnetRepository Subnets => GetRepository<SubnetRepository>();
        public WorkflowRepository Workflows => GetRepository<WorkflowRepository>();
        public CertificationRepository Certifications => GetRepository<CertificationRepository>();
        public DomainRegistrationTempRepository DomainRegistrations => GetRepository<DomainRegistrationTempRepository>();
        public InventoryRepository Inventories => GetRepository<InventoryRepository>();
        public PreinstalledSoftwareRepository PreinstalledSoftwareRepositories => GetRepository<PreinstalledSoftwareRepository>();
        public WMIInventoryCmdsRepository WMIInventoryCmds => GetRepository<WMIInventoryCmdsRepository>();
        public HardwareModelRepository HardwareModels => GetRepository<HardwareModelRepository>();
        public DriverRepository Drivers => GetRepository<DriverRepository>();
        public BIOSModelRepository BiosModels => GetRepository<BIOSModelRepository>();
        public OSModelRepository OSModels => GetRepository<OSModelRepository>();
        public AssetMgmtRepository AssetModels => GetRepository<AssetMgmtRepository>();
        public AssetTypeRepository AssetTypes => GetRepository<AssetTypeRepository>();
        public AssetClassesReposiotry AssetClasses => GetRepository<AssetClassesReposiotry>();
        public VendorModelRepository VendorModels => GetRepository<VendorModelRepository>();
        public ClientTaskRepository ClientTasks => GetRepository<ClientTaskRepository>();
        public ArchitectureRepository Architectures => GetRepository<ArchitectureRepository>();
        public OsVersionNamesRepository OsVersionNames => GetRepository<OsVersionNamesRepository>();
        public Win10VersionsRepositry Win10Versions => GetRepository<Win10VersionsRepositry>();
        public Win11VersionRepository Win11Versions => GetRepository<Win11VersionRepository>();        
        public SoftwaresClientRepository SoftwaresClients => GetRepository<SoftwaresClientRepository>();
        public SoftwaresCustomerRepository SoftwaresCustomers => GetRepository<SoftwaresCustomerRepository>();
        public SoftwaresSystemhouseRepository SoftwaresSystemhouses => GetRepository<SoftwaresSystemhouseRepository>();
        public ActivityLogRepository ActivityLogs => GetRepository<ActivityLogRepository>();
        public AdvancedPropertyRepository AdvancedProperties => GetRepository<AdvancedPropertyRepository>();
        public ParameterRepository Parameters => GetRepository<ParameterRepository>();
        public CloudEntryPointRepository CloudEntryPoints => GetRepository<CloudEntryPointRepository>();
        public StorageEntryPointRepository StorageEntryPoints => GetRepository<StorageEntryPointRepository>();
        public ClientOptionRepository ClientOptions => GetRepository<ClientOptionRepository>();
        public ExecutionLogRepository ExecutionLogs => GetRepository<ExecutionLogRepository>();
        public VirtualNetworkRepository VirtualNetworks => GetRepository<VirtualNetworkRepository>();
        public SoftwareStreamRepository SoftwareStreams => GetRepository<SoftwareStreamRepository>();
        public ShopItemCategoryRepositry ShopItemCategories => GetRepository<ShopItemCategoryRepositry>();
        public CustomerSoftwareStreamRepository CustomerSoftwareStreamss => GetRepository<CustomerSoftwareStreamRepository>();
        public CustomerSoftwareRepository CustomerSoftwares => GetRepository<CustomerSoftwareRepository>();
        public ReleasePlanRepository ReleasePlans => GetRepository<ReleasePlanRepository>();
        public ImageRepository Images => GetRepository<ImageRepository>();
        public CustomerImagesRepository CustomerImages => GetRepository<CustomerImagesRepository>();
        public DriverShopItemRepository DriverShopItems => GetRepository<DriverShopItemRepository>();
        public CustomerHardwareModelRepository CustomerHardwareModels => GetRepository<CustomerHardwareModelRepository>();
        public ClientClientPropertyRepository ClientClientProperties => GetRepository<ClientClientPropertyRepository>();
        public ClientSoftwareRepository ClientSoftwares => GetRepository<ClientSoftwareRepository>();
        public BaseStatusRepository BaseStatuses => GetRepository<BaseStatusRepository>();
        public MacAddressRepository MacAddresses => GetRepository<MacAddressRepository>();
        public RevisionMessageRepository RevisionMessages => GetRepository<RevisionMessageRepository>();
        public ImagesCustomersRepository ImagesCustomers => GetRepository<ImagesCustomersRepository>();
        public ImagesSystemhouseRepository ImagesSystemhouses => GetRepository<ImagesSystemhouseRepository>();
        public AdminDeviceOptionRepository AdminOptions => GetRepository<AdminDeviceOptionRepository>();
        public CustomerDriverRepository CustomerDrivers => GetRepository<CustomerDriverRepository>();
        #endregion

        public void SaveChanges()
        {
            try
            {
                // .Where(p => p.State == EntityState.Modified)
                var modifiedEntities = Context.ChangeTracker.Entries().Where(p => p.State != EntityState.Unchanged).ToList();
                var now = DateTime.UtcNow;

                foreach (var change in modifiedEntities)
                {
                    var entityName = change.Entity.GetType().Name;
                    var primaryKey = ((IEntity)change.Entity).Id;

                    foreach (var prop in change.OriginalValues.Properties)
                    {
                        var originalValue = change.Property(prop.Name).OriginalValue == null ? "null" : change.Property(prop.Name).OriginalValue.ToString();
                        //var originalValue = change.OriginalValues[prop].ToString();
                        var currentValue = change.Property(prop.Name).CurrentValue == null ? "null" : change.Property(prop.Name).CurrentValue.ToString(); ;
                        if (!originalValue.Equals(currentValue))
                        {
                            ChangeLog log = new ChangeLog()
                            {
                                EntityName = entityName,
                                PrimaryKeyValue = primaryKey.ToString(),
                                PropertyName = prop.Name,
                                OldValue = originalValue,
                                NewValue = currentValue,
                                DateChanged = now
                            };                            
                            Context.ChangeLogs.Add(log);
                        }
                    }
                }

                Context.SaveChanges();
                // new Thread(() => 
                // {
                /*
                List<ChangeLog> changeLogs = Context.Set<ChangeLog>().ToList();                                        
                foreach (ChangeLog cl in changeLogs)
                {
                    if ((cl.DateChanged - DateTime.Now).TotalDays <= -173)
                    {
                        Context.ChangeLogs.Remove(cl);
                        Context.SaveChanges();
                    }                    
                }     
                */
                // });
            }           
            catch (DbUpdateException ex)
            {
                //used to analyze exception reason
                throw new DbUpdateExceptionWrapper(ex);
            }
        }

        /// <summary>
        /// start new transaction or return NestedTransactionWrapper for already started transaction
        /// </summary>
        public ITransactionWrapper BeginTransaction()
        {
            if (CurrentTransaction != null && !CurrentTransaction.IsDisposed)
            {
                return new NestedTransactionWrapper(CurrentTransaction);
            }
            else
            {
                CurrentTransaction = new DbContextTransactionWrapper(Context);
                return CurrentTransaction;
            }
        }

        private T GetRepository<T>() where T : RepositoryBase
        {
            return ContextProvider.GetRepository<T>();
        }

        public void Dispose()
        {
            CreatedInScope?.Dispose();
            Context.Dispose();
        }
    }
}
