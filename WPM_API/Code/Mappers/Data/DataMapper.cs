using System.Linq;
using WPM_API.Data.DataContext;
using WPM_API.Data.DataContext.Entities;
using WPM_API.Data.DataContext.Entities.AssetMgmt;
using WPM_API.Data.DataContext.Entities.SmartDeploy;
using WPM_API.Data.DataContext.Entities.Storages;
using WPM_API.Data.Models;
using WPM_API.TransferModels;
using WPM_API.TransferModels.SmartDeploy;
using WPM_API.Controllers;
using WPM_API.Controllers.Storages;
using WPM_API.Models;
using WPM_API.Models.AssetMgmt;
using WPM_API.Models.Release_Mgmt;
using static WPM_API.Controllers.Base.ImageStreamController;
using static WPM_API.Controllers.CustomerImageController;
using static WPM_API.Controllers.ReleasePlanController;
using AZURE = WPM_API.Azure.Models;
using File = WPM_API.Data.DataContext.Entities.File;
using Task = WPM_API.Data.DataContext.Entities.Task;

namespace WPM_API.Code.Mappers.Data
{
    public class DataMapper : MapperBase
    {
        public DataMapper()
        {
            CreateMap<Driver, CustomerDriver>()
                .Ignore(x => x.CustomerId)
                .Ignore(x => x.DriverId)
                .ReverseMap();
            CreateMap<AdminDeviceOption, AdminDeviceOptionViewModel>().Ignore(x => x.Content).ReverseMap();
            CreateMap<CustomerHardwareModel, CustomerHardwareModelViewModel>().ReverseMap();
            CreateMap<Image, ShopItemViewModel>()
                .Ignore(x => x.Description)
                .Ignore(x => x.Drivers)
                .Ignore(x => x.DriverShopItems)
                .Ignore(x => x.Images)
                .Ignore(x => x.DescriptionShort)
                .Ignore(x => x.Price)
                .Ignore(x => x.ManagedServiceLifecyclePrice)
                .Ignore(x => x.ManagedServicePrice)
                .Ignore(x => x.Categories)
                .Ignore(x => x.bruttoManagedServiceLifecyclePrice)
                .Ignore(x => x.bruttoManagedServicePrice)
                .Ignore(x => x.bruttoPrice)
                .ReverseMap();
            CreateMap<Image, ImageViewModel>()
                .ForMember(
                    dest => dest.Systemhouses,
                    opt => opt.MapFrom(x => x.Systemhouses.Select(y => y.SystemhouseId).ToArray()))
               .ForMember(
                   dest => dest.Customers,
                   opt => opt.MapFrom(x => x.Customers.Select(y => y.CustomerId).ToArray()))
               /*
                .ForMember(
                    dest => dest.Clients,
                    opt => opt.MapFrom(x => x.Clients.Select(y => y.ClientId).ToArray()))
               */
                .ReverseMap();
            CreateMap<Image, CustomerImage>()
                .Ignore(x => x.CustomerId)
                .Ignore(x => x.ImageId)
                .Ignore(x => x.CustomerImageStreamId)
                .ReverseMap();
            CreateMap<CustomerImage, CustomerImageViewModel>().ReverseMap();
            CreateMap<ReleasePlan, ReleasePlanViewModel>().ReverseMap();
            CreateMap<SoftwareStream, CustomerSoftwareStream>()
                .Ignore(x => x.Priority)
                .Ignore(x => x.IsEnterpriseStream)
                .Ignore(x => x.CustomerId)
                .Ignore(x => x.SoftwareStreamId)
                .Ignore(x => x.ReleasePlan)
                .Ignore(x => x.ReleasePlanId)
                .Ignore(x => x.ApplicationOwner)
                .Ignore(x => x.ApplicationOwnerId)
                .Ignore(x => x.ClientOrServer)
                .ReverseMap();
            CreateMap<ImageStream, CustomerImageStream>()
                .Ignore(x => x.ImageStreamId)
                .Ignore(x => x.CustomerId)
                /*
                .Ignore(x => x.KeyboardLayoutLinux)
                .Ignore(x => x.KeyboardLayoutWindows)
                .Ignore(x => x.TimeZoneLinux)
                .Ignore(x => x.TimeZoneWindows)
                .Ignore(x => x.UsernameLinux)
                .Ignore(x => x.UserPasswordLinux)
                .Ignore(x => x.AdminPasswordLinux)
                .Ignore(x => x.PartitionEncryptionPassLinux)
                */
                .Ignore(x => x.LocalSettingLinux)                
                .Ignore(x => x.ProductKey)
                .ReverseMap();
            CreateMap<SoftwareClientViewModel, CustomerSoftware>()
                .Ignore(x => x.CustomerStatus)
                .Ignore(x => x.CustomerSoftwareStreamId)
                .Ignore(x => x.SoftwareId)
                .Ignore(x => x.CreatedByUserId)
                .Ignore(x => x.CreatedDate)
                .Ignore(x => x.DeletedByUserId)
                .Ignore(x => x.DeletedDate)
                .Ignore(x => x.UpdatedByUserId)
                .Ignore(x => x.UpdatedDate)
                .ReverseMap();
            CreateMap<Software, CustomerSoftware>()
                .Ignore(x => x.CustomerSoftwareStreamId)
                .Ignore(x => x.SoftwareId)
                .Ignore(x => x.CustomerStatus)
                .ReverseMap();
            CreateMap<ShopItemCategory, ShopItemCategoryViewModel>().ReverseMap();
            CreateMap<SoftwareStream, SoftwareStreamViewModel>().ReverseMap();
            CreateMap<ImageStream, ImageStreamViewModel>().ReverseMap();
            CreateMap<StorageEntryPoint, StorageEntryPointViewModel>().ReverseMap();
            CreateMap<CloudEntryPoint, AzureCredentialViewModel>().ReverseMap();
            CreateMap<Customer, CustomerViewModel>()
                .ForMember(
                    dest => dest.LtSASWrite,
                    opt => opt.MapFrom(src => src.LtSASWríte)
                )
                .ReverseMap();
            CreateMap<User, UserViewModel>()
                .Map(m => m.Roles, d => string.Join(", ", d.UserRoles.Select(c => c.Role.Name)))
                .Map(m => m.Name, m => m.UserName).Map(m => m.Admin, m => m.Admin);
            //CreateMap<Systemhouse, SystemhouseViewModel>().ReverseMap();
            CreateMap<Systemhouse, SystemhouseRefViewModel>().ReverseMap();
            CreateMap<Customer, CustomerNameViewModel>()
                .ForMember(
                    dest => dest.LtSASWrite,
                    opt => opt.MapFrom(src => src.LtSASWríte)
                )
                .Ignore(dest => dest.Exists)
                .ReverseMap();

            /** Domain **/
            CreateMap<Domain, DomainNameViewModel>();
            CreateMap<Domain, DomainRefViewModel>().ForMember(
                dest => dest.Domain,
                opt => opt.MapFrom(x => x)
            );

            CreateMap<DomainAddViewModel, Domain>().ForMember(
                dest => dest.Id,
                opt =>
                {
                    opt.Ignore();
                    opt.UseDestinationValue();
                }
            ).Ignore(
                dest => dest.Customer
            ).Ignore(
                dest => dest.CustomerId
            ).Ignore(
                dest => dest.DeletedDate
            ).Ignore(
                dest => dest.CreatedDate
            ).Ignore(
                dest => dest.UpdatedByUserId
            ).Ignore(
                dest => dest.UpdatedDate
            ).Ignore(
                dest => dest.DeletedByUserId
            ).Ignore(
                dest => dest.CreatedByUserId
            ).ForMember(
                dest => dest.Name,
                opt => opt.MapFrom(src => src.Domain.Name)
            ).ForMember(
                dest => dest.Tld,
                opt => opt.MapFrom(src => src.Domain.Tld)
            ).Ignore(
                dest => dest.Status
            ).Ignore(
                dest => dest.Base
            ).Ignore(
                dest => dest.BaseId
            ).Ignore(
                dest => dest.ExecutionVMId
            ).Ignore(
                dest => dest.Servers
            ).Ignore(
                dest => dest.DNS
            ).Ignore(
                dest => dest.Office365ConfigurationXML
            );

            CreateMap<DomainViewModel, Domain>().ForMember(
                dest => dest.Id,
                opt =>
                {
                    opt.Ignore();
                    opt.UseDestinationValue();
                }
            ).ForMember(
                dest => dest.CreatedDate,
                opt => opt.MapFrom(src => src.CreationDate)
            ).Ignore(
                dest => dest.Customer
            ).Ignore(
                dest => dest.CustomerId
            ).Ignore(
                dest => dest.DeletedDate
            ).Ignore(
                dest => dest.UpdatedByUserId
            ).Ignore(
                dest => dest.UpdatedDate
            ).Ignore(
                dest => dest.DeletedByUserId
            ).Ignore(
                dest => dest.CreatedByUserId
            ).ForMember(
                dest => dest.Name,
                opt => opt.MapFrom(src => src.Domain.Name)
            ).ForMember(
                dest => dest.Tld,
                opt => opt.MapFrom(src => src.Domain.Tld)
            ).Ignore(
                dest => dest.Base
            ).Ignore(
                dest => dest.BaseId
            ).Ignore(
                dest => dest.ExecutionVMId
            ).Ignore(
                dest => dest.Servers
            ).Ignore(
                dest => dest.DNS
            ).Ignore(
                dest => dest.Office365ConfigurationXML
            ).ReverseMap();


            CreateMap<GroupPolicyObject, GroupPolicyObjectViewModel>().ForMember(
                dest => dest.Id,
                opt =>
                {
                    opt.Ignore();
                    opt.UseDestinationValue();
                }
            ).Ignore(
                dest => dest.Lockscreen
            ).Ignore(
                dest => dest.Wallpaper
            ).ReverseMap();
            CreateMap<DomainUser, DomainUserViewModel>().ReverseMap(
            ).Ignore(dest => dest.Id
            ).Ignore(dest => dest.CreatedByUserId
            ).Ignore(dest => dest.CreatedDate);
            CreateMap<DomainUserViewModel, DomainUser>().ForMember(
                dest => dest.Id,
                opt =>
                {
                    opt.Ignore();
                    opt.UseDestinationValue();
                }
            ).Ignore(
                dest => dest.DeletedDate
            ).Ignore(
                dest => dest.UpdatedByUserId
            ).Ignore(
                dest => dest.UpdatedDate
            ).Ignore(
                dest => dest.DeletedByUserId
            ).Ignore(
                dest => dest.CreatedDate
            ).Ignore(
                dest => dest.Domain
            ).Ignore(
                dest => dest.DomainId
            ).Ignore(
                dest => dest.CreatedByUserId
            );

            CreateMap<Default, AdvancedProperty>()
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Value))
                .Ignore(x => x.CategoryId)
                .Ignore(x => x.Base)
                .Ignore(x => x.BaseId)
                .Ignore(x => x.Id)
                .Ignore(x => x.isEditable)
                .Ignore(x => x.Category)
                .Ignore(x => x.CreatedByUserId)
                .Ignore(x => x.CreatedDate)
                .Ignore(x => x.UpdatedByUserId)
                .Ignore(x => x.UpdatedDate)
                .Ignore(x => x.DeletedByUserId)
                .Ignore(x => x.DeletedDate)
                .ReverseMap();
            CreateMap<AdvancedProperty, AdvancedPropertyViewModel>().ReverseMap();
            //            CreateMap<AdvancedProperty, ClientProperty>().ForMember(
            //                dest => dest.Name,
            //                opt => opt.MapFrom(src => src.Name)
            //            ).ForMember(
            //                dest => dest.ParameterName,
            //                opt => opt.MapFrom(src => src.Value))
            //                .Ignore(dest => dest.Id)
            //                .Ignore(dest => dest.Category)
            //                .Ignore(dest => dest.Command)
            //                .Ignore(dest => dest.CategoryId)
            //                .ReverseMap();

            CreateMap<Base, BaseRefViewModel>().ReverseMap();
            CreateMap<Base, BaseViewModel>().Ignore(x => x.CreationUser)
                .ForMember(dest => dest.CreationDate, opt => opt.MapFrom(src => src.CreatedDate))
                .ForMember(dest => dest.AdvancedProperties, opt => opt.MapFrom(src => src.Properties))
                .ReverseMap();
            CreateMap<Base, BaseAddViewModel>()
                .ForMember(
                dest => dest.VirtualMachine,
                opt => opt.MapFrom(src => src.VirtualMachines.FirstOrDefault())
            ).ForMember(
                dest => dest.SubscriptionId,
                opt => opt.MapFrom(src => src.Subscription.AzureId)
            ).ReverseMap();
            CreateMap<BaseAddViewModel, Subscription>()
                .Ignore(x => x.CreatedByUserId)
                .Ignore(x => x.CreatedDate)
                .Ignore(x => x.UpdatedByUserId)
                .Ignore(x => x.UpdatedDate)
                .Ignore(x => x.DeletedByUserId)
                .Ignore(x => x.DeletedDate)
                .ForMember(
                dest => dest.AzureId,
                opt => opt.MapFrom(src => src.SubscriptionId)
            ).Ignore(x => x.Id).Ignore(x => x.UserSubscription).ReverseMap();
            CreateMap<Parameter, ParameterViewModel>(
                    ).Ignore(
                        dest => dest.DisplayName
                    ).Ignore(
                        dest => dest.Reference
                    ).Ignore(
                        dest => dest.Origin
                    ).ReverseMap();
            CreateMap<SubscriptionViewModel, Subscription>()
                .Ignore(x => x.CreatedByUserId)
                .Ignore(x => x.CreatedDate)
                .Ignore(x => x.UpdatedByUserId)
                .Ignore(x => x.UpdatedDate)
                .Ignore(x => x.DeletedByUserId)
                .Ignore(x => x.DeletedDate)
                .ForMember(dest => dest.AzureId,
                    opt => opt.MapFrom(src => src.Id)).Ignore(x => x.UserSubscription).Ignore(x => x.Id).ReverseMap();
            CreateMap<ResourceGroup, ResourceGroupViewModel>().ReverseMap();
            CreateMap<StorageAccount, StorageAccountViewModel>()
                .ForMember(
                    dest => dest.ResourceGroupId,
                    opt => opt.MapFrom(src => src.ResourceGroupId)
                    )
                .ReverseMap();
            CreateMap<VirtualNetwork, VirtualNetworkViewModel>().ReverseMap();
            CreateMap<VirtualNetwork, VirtualNetworkAddViewModel>().ReverseMap();
            CreateMap<VpnViewModel, Vpn>()
                .Ignore(x => x.CreatedByUserId)
                .Ignore(x => x.CreatedDate)
                .Ignore(x => x.UpdatedByUserId)
                .Ignore(x => x.UpdatedDate)
                .Ignore(x => x.DeletedByUserId)
                .Ignore(x => x.DeletedDate)
                .ForMember(
                    dest => dest.LocalAddressRange,
                    opt => opt.MapFrom(src => src.Local.AddressRange)
                ).ForMember(
                    dest => dest.LocalPublicIp,
                    opt => opt.MapFrom(src => src.Local.PublicIp)
                ).ForMember(
                    dest => dest.VirtualNetwork,
                    opt => opt.MapFrom(src => src.Virtual.VirtualNetwork)
                ).ForMember(
                    dest => dest.VirtualPublicIp,
                    opt => opt.MapFrom(src => src.Virtual.PublicIp)
                ).Ignore(dest => dest.Id).Ignore(x => x.SharedKey)
                .ReverseMap();
            CreateMap<VpnAddViewModel, Vpn>()
                .Ignore(x => x.CreatedByUserId)
                .Ignore(x => x.CreatedDate)
                .Ignore(x => x.UpdatedByUserId)
                .Ignore(x => x.UpdatedDate)
                .Ignore(x => x.DeletedByUserId)
                .Ignore(x => x.DeletedDate)
                .ForMember(
                    dest => dest.LocalAddressRange,
                    opt => opt.MapFrom(src => src.Local.AddressRange)
                ).ForMember(
                    dest => dest.LocalPublicIp,
                    opt => opt.MapFrom(src => src.Local.PublicIp)
                ).Ignore(dest => dest.Id)
                // VirtualNetwork wird erst noch erstellt.
                .Ignore(dest => dest.VirtualNetwork)
                .Ignore(dest => dest.VirtualPublicIp)
                .Ignore(x => x.SharedKey)
                .ReverseMap();
            CreateMap<VirtualMachine, VirtualMachineViewModel>()
                .ForMember(
                    dest => dest.SystemDisk,
                    opt => opt.MapFrom(src => src.Disks.FirstOrDefault(x => x.DiskType == DiskType.SystemDisk))
                ).ForMember(
                    dest => dest.AdditionalDisk,
                    opt => opt.MapFrom(src =>
                        src.Disks.Where(x => x.DiskType == DiskType.AdditionalDisk).FirstOrDefault())
                ).ReverseMap();
            CreateMap<VirtualMachine, VirtualMachineAddViewModel>()
                .ForMember(
                    dest => dest.SystemDisk,
                    opt => opt.MapFrom(src => src.Disks.FirstOrDefault(x => x.DiskType == DiskType.SystemDisk))
                ).ForMember(
                    dest => dest.AdditionalDisk,
                    opt => opt.MapFrom(src =>
                        src.Disks.Where(x => x.DiskType == DiskType.AdditionalDisk).FirstOrDefault())
                )
                .Ignore(x => x.BaseId)
                .Ignore(x => x.CustomerId)
                .Ignore(x => x.Admin)
                .Ignore(x => x.CurrentCustomerId)
                .ReverseMap();
            CreateMap<VirtualMachine, AdminCredentials>().ForMember(
                dest => dest.Username,
                opt => opt.MapFrom(src => src.AdminUserName)).ForMember(
                dest => dest.Password,
                opt => opt.MapFrom(src => src.AdminUserPassword)).ReverseMap();
            CreateMap<AZURE.VirtualMachineModel, VirtualMachineAddViewModel>()
                .Ignore(x => x.BaseId)
                .Ignore(x => x.Subnet)
                .Ignore(x => x.CurrentCustomerId)
                .ReverseMap();
            CreateMap<Subnet, SubnetViewModel>().ReverseMap();
            CreateMap<Disk, DiskViewModel>().ReverseMap();
            /********************* Scripts **********************/
            CreateMap<Script, ScriptViewModel>().ReverseMap().Ignore(x => x.Type);
            CreateMap<Script, ScriptAddViewModel>()
                .Ignore(x => x.BitstreamScript)
                .Ignore(dest => dest.Content).Ignore(x => x.Attachments).ReverseMap().Ignore(x => x.Type);
            CreateMap<ScriptVersion, ScriptEditViewModel>()
                .Ignore(dest => dest.Content)
                .Ignore(x => x.PEOnly)
                .Ignore(dest => dest.Description)
                .Ignore(dest => dest.BitstreamScript)
                .Ignore(x => x.OSType)
                .ReverseMap();
            CreateMap<ScriptVersion, ScriptVersionViewModel>().ReverseMap();
            CreateMap<ScriptVersion, ScriptVersionContentViewModel>().Ignore(dest => dest.Content).ReverseMap();
            CreateMap<ScriptVersion, ScriptVersionAddViewModel>().Ignore(dest => dest.Content).Ignore(x => x.PEOnly).ReverseMap();

            /********************* Software **********************/
            // NUR beim Hinzufügen ist die ID die Guid
            CreateMap<Models.FileRefModel, File>().ForMember(
                    dest => dest.Guid,
                    opt => {
                        //opt.Condition((src, dest) => !dest.Id.Equals(src.Id));
                        opt.MapFrom(src => src.Id);
                    })
                .Ignore(x => x.Id).Ignore(x => x.TaskId).Ignore(x => x.Task);
            CreateMap<TransferModels.FileRef, File>().ForMember(
                    dest => dest.Guid,
                    opt => opt.MapFrom(src => src.Id))
                .Ignore(x => x.Id).Ignore(x => x.TaskId).Ignore(x => x.Task);
            CreateMap<Rule, RuleViewModel>().ForMember(
                dest => dest.Type,
                opt => opt.MapFrom(x => x.Type.Name))
                .ForMember(d => d.Architecture, o => o.MapFrom(s => s.Architecture.Select(c => c.Version).ToArray()))
                .ForMember(d => d.Win10Versions, o => o.MapFrom(s => s.Win10Versions.Select(c => c.Version).ToArray()))
                .ForMember(d => d.Win11Versions, o => o.MapFrom(s => s.Win11Versions.Select(c => c.Version).ToArray()))
                .ForMember(d => d.OsVersionNames, o => o.MapFrom(s => s.OsVersionNames.Select(c => c.Version).ToArray()))
                .ReverseMap();
            CreateMap<Rule, RuleAddViewModel>().ForMember(
                dest => dest.Type,
                opt => opt.MapFrom(x => x.Type.Name))
                .ForMember(d => d.Architecture, o => o.MapFrom(s => s.Architecture.Select(c => c.Version).ToArray()))
                .ForMember(d => d.Win10Versions, o => o.MapFrom(s => s.Win10Versions.Select(c => c.Version).ToArray()))
                .ForMember(d => d.OsVersionNames, o => o.MapFrom(s => s.OsVersionNames.Select(c => c.Version).ToArray()))
                .ReverseMap();
            CreateMap<Task, TaskViewModel>().ForMember(
                    dest => dest.Executable,
                    opt => opt.MapFrom(x => x.ExecutionFileId))
                .ReverseMap().Ignore(x => x.ExecutionFile).Ignore(x => x.ExecutionFileId);
            CreateMap<Task, TaskAddViewModel>().ForMember(
                    dest => dest.Executable,
                    opt => opt.MapFrom(x => x.ExecutionFileId))
                .ReverseMap().Ignore(x => x.ExecutionFile).Ignore(x => x.ExecutionFileId);
            CreateMap<Software, SoftwareViewModel>()
               .ForMember(
                    dest => dest.Systemhouses,
                    opt => opt.MapFrom(x => x.Systemhouses.Select(y => y.SystemhouseId).ToArray()))
               .ForMember(
                   dest => dest.Customers,
                   opt => opt.MapFrom(x => x.Customers.Select(y => y.CustomerId).ToArray()))
               .ForMember(
                    dest => dest.Clients,
                    opt => opt.MapFrom(x => x.Clients.Select(y => y.ClientId).ToArray()))
               .Ignore(x => x.StreamId)
               .Ignore(x => x.CustomerStatus)
               .Ignore(x => x.Icon)
                .ReverseMap();
            CreateMap<Software, SoftwareAddViewModel>()
                .Ignore(x => x.Icon)
                .Ignore(x => x.RuleApplicabilityId)
                .Ignore(x => x.RuleDetectionId)
                .Ignore(x => x.StreamId)
                .ForMember(
                    dest => dest.Systemhouses,
                    opt => opt.MapFrom(x => x.Systemhouses.Select(y => y.SystemhouseId).ToArray()))
               .ForMember(
                   dest => dest.Customers,
                   opt => opt.MapFrom(x => x.Customers.Select(y => y.CustomerId).ToArray()))
                .ForMember(
                    dest => dest.Clients,
                    opt => opt.MapFrom(x => x.Clients.Select(y => y.ClientId).ToArray()))
                .ReverseMap();
            // Map all, but no Tasks, because the frontend doesnt know anything about Guid.
            CreateMap<Software, SoftwareEditViewModel>()
                .Ignore(x => x.CustomerStatus)
                .Ignore(x => x.Icon)
                .Ignore(x => x.RuleApplicabilityId)
                .Ignore(x => x.RuleDetectionId)
                .Ignore(x => x.StreamId)
                .ForMember(
                    dest => dest.Systemhouses,
                    opt => opt.MapFrom(x => x.Systemhouses.Select(y => y.SystemhouseId).ToArray()))
               .ForMember(
                   dest => dest.Customers,
                   opt => opt.MapFrom(x => x.Customers.Select(y => y.CustomerId).ToArray()))
               .ForMember(
                    dest => dest.Clients,
                    opt => opt.MapFrom(x => x.Clients.Select(y => y.ClientId).ToArray()))
                .ReverseMap().ForMember(
                    dest => dest.TaskInstall,
                    opt => opt.UseDestinationValue())
                .ForMember(
                    dest => dest.TaskUninstall,
                    opt => opt.UseDestinationValue())
                .ForMember(
                    dest => dest.TaskUpdate,
                    opt => opt.UseDestinationValue());
            CreateMap<Software, SoftwareAssignViewModel>()
                .Ignore(x => x.Required)
                .ReverseMap();
            CreateMap<CustomerSoftware, SoftwareAssignViewModel>()
                .Ignore(x => x.Required)
                .ReverseMap();

            /******************** Organisational Units **************************/
            CreateMap<OULevelAddViewModel, OrganizationalUnit>()
                .Ignore(x => x.Id)
                .Ignore(x => x.IsLeaf)
                .ForMember(
                    dest => dest.Children,
                    opt => opt.MapFrom(x => x.Children))
                .AfterMap((s, x) => x.IsLeaf = false);
            CreateMap<OUBaseLevelAddViewModel, OrganizationalUnit>()
                .Ignore(x => x.Id)
                .Ignore(x => x.IsLeaf)
                .Ignore(x => x.Children)
                .Ignore(x => x.Description)
                .AfterMap((s, x) => x.IsLeaf = true);
            CreateMap<OrganizationalUnit, OUViewModel>().ForPath(
                    d => d.OULevels, o => o.MapFrom(s => !s.IsLeaf))
                .ForPath(
                    d => d.OUBaseLevels, o => o.MapFrom(s => s.IsLeaf));
            CreateMap<OrganizationalUnit, OUScriptViewModel>();
            CreateMap<OUScriptViewModel, OrganizationalUnit>().ForMember(
                dest => dest.Id,
                opt =>
                {
                    opt.Ignore();
                    opt.UseDestinationValue();
                }).ReverseMap();

            /******************** Client ***************************************/
            CreateMap<Client, ClientViewModel>().ReverseMap();
            CreateMap<Client, WPM_API.Models.ClientAddViewModelWeb>()
                .Ignore(x => x.CustomerName)
                .Ignore(x => x.MacAddresses)
                .ReverseMap();
            CreateMap<Software, SoftwareClientViewModel>()
                .Ignore(x => x.CustomerStatus)
                .Ignore(x => x.Icon)
                .Ignore(x => x.Required)
                .Ignore(x => x.StreamId)
                .ForMember(
                    dest => dest.Systemhouses,
                    opt => opt.MapFrom(x => x.Systemhouses.Select(y => y.SystemhouseId).ToArray()))
               .ForMember(
                   dest => dest.Customers,
                   opt => opt.MapFrom(x => x.Customers.Select(y => y.CustomerId).ToArray()))
               .ForMember(
                    dest => dest.Clients,
                    opt => opt.MapFrom(x => x.Clients.Select(y => y.ClientId).ToArray()))
                .ReverseMap();
            CreateMap<ClientSoftware, SoftwareClientViewModel>()
                .Ignore(x => x.CustomerStatus)
                .Ignore(x => x.DedicatedDownloadLink)
                .Ignore(x => x.NextSoftwareId)
                .Ignore(x => x.MinimalSoftwareId)
                .Ignore(x => x.PrevSoftwareId) 
                .Ignore(x => x.PublishInShop)
                .Ignore(x => x.StreamId)
                .Ignore(x => x.SoftwareStreamId)
                .Ignore(x => x.Icon)
                .Ignore(x => x.Systemhouses)
                .Ignore(x => x.Customers)
                .Ignore(x => x.Clients)
                .Ignore(x => x.DisplayRevisionNumber)
                .ForMember(
                    dest => dest.Id,
                    opt => opt.MapFrom(src => src.CustomerSoftware.Id))
                .ForMember(
                    dest => dest.Name,
                    opt => opt.MapFrom(src => src.CustomerSoftware.Name))
                .ForMember(
                    dest => dest.Required,
                    opt => opt.MapFrom(src => src.Install))
                .ForMember(
                    dest => dest.Version,
                    opt => opt.MapFrom(src => src.CustomerSoftware.Version))
                .ForMember(
                    dest => dest.RuleApplicability,
                    opt => opt.MapFrom(src => src.CustomerSoftware.RuleApplicability))
                .ForMember(
                    dest => dest.RuleDetection,
                    opt => opt.MapFrom(src => src.CustomerSoftware.RuleDetection))
                .ForMember(
                    dest => dest.TaskInstall,
                    opt => opt.MapFrom(src => src.CustomerSoftware.TaskInstall))
                .ForMember(
                    dest => dest.TaskUninstall,
                    opt => opt.MapFrom(src => src.CustomerSoftware.TaskUninstall))
                .ForMember(
                    dest => dest.TaskUpdate,
                    opt => opt.MapFrom(src => src.CustomerSoftware.TaskUpdate))
                .ForMember(dest => dest.InstallationTime,
                    opt => opt.MapFrom(src => src.CustomerSoftware.InstallationTime))
                .ForMember(dest => dest.PackageSize,
                    opt => opt.MapFrom(src => src.CustomerSoftware.PackageSize))
                .ForMember(dest => dest.Prerequisites,
                    opt => opt.MapFrom(src => src.CustomerSoftware.Prerequisites))
                .ForMember(dest => dest.VendorReleaseDate,
                    opt => opt.MapFrom(src => src.CustomerSoftware.VendorReleaseDate))
                .ForMember(dest => dest.CompliancyRule,
                    opt => opt.MapFrom(src => src.CustomerSoftware.CompliancyRule))
                .ForMember(dest => dest.Checksum,
                    opt => opt.MapFrom(src => src.CustomerSoftware.Checksum))
                .Ignore(x => x.RuleDetectionId)
                .Ignore(x => x.RuleApplicabilityId)
                .Ignore(x => x.Status)
                .Ignore(x => x.Type);
            CreateMap<BiosViewModels, Bios>().ReverseMap();
            CreateMap<OsViewModels, OS>().ReverseMap();
            CreateMap<HardwareViewModels, Hardware>().ReverseMap();
            CreateMap<HDDPartitionViewModels, HDDPartition>().ReverseMap();
            CreateMap<NetworkConfigurationViewModels, NetworkConfiguration>().ReverseMap();
            CreateMap<PurchaseViewModels, Purchase>().ReverseMap();
            CreateMap<MacAddressViewModels, MacAddress>()
                .Ignore(x => x.Client)
                .Ignore(x => x.CreatedByUserId)
                .Ignore(x => x.CreatedDate)
                .Ignore(x => x.DeletedByUserId)
                .Ignore(x => x.DeletedDate)
                .Ignore(x => x.UpdatedByUserId)
                .Ignore(x => x.UpdatedDate)
                .ReverseMap();
            //  CreateMap<CategoryViewModels, ClientDatasheetCategories>().ReverseMap();
            //CreateMap<PropertyViewModels, ClientProperties>().ReverseMap();
            /******************** Category ***************************************/
            CreateMap<ShopItemCategory, CategoryViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Category.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Category.Name));
            CreateMap<Category, CategoryViewModel>().ReverseMap();
            CreateMap<CategoryAddViewModel, Category>().ReverseMap();
            CreateMap<Category, CategoryAddViewModel>().ReverseMap().Ignore(x => x.Id);
            CreateMap<Category, CategoryRefViewModel>().ReverseMap().Ignore(x => x.Name);


            /***************** Shop **************************************/
            CreateMap<ShopItem, ShopItemViewModel>()
                .ForMember(
                    dest => dest.Id,
                    opt =>
                    {
                        opt.Ignore();
                        opt.UseDestinationValue();
                    }).ForMember(
                    dest => dest.Categories,
                    opt => opt.MapFrom(src => src.Categories))
                .ReverseMap();
            CreateMap<ShopItem, ShopItemAddViewModel>().ReverseMap();
            CreateMap<ShopItemAddViewModel, ShopItem>()
                .Ignore(x => x.DriverShopItems)
                .ForMember(
                dest => dest.Id,
                opt =>
                {
                    opt.Ignore();
                    opt.UseDestinationValue();
                }).Ignore(
                dest => dest.CreatedDate
            ).Ignore(
                dest => dest.DeletedDate
            ).Ignore(
                dest => dest.UpdatedByUserId
            ).Ignore(
                dest => dest.UpdatedDate
            ).Ignore(
                dest => dest.DeletedByUserId
            ).Ignore(
                dest => dest.CreatedByUserId
            ).ForPath(dest => dest.Categories, opt => opt.MapFrom(src => src.Categories))
            .ForPath(dest => dest.ManagedServicePrice, opt => opt.MapFrom(src => src.ManagedServicePrice)
            ).ReverseMap();
            CreateMap<ShopItem, ShopItemEditViewModel>().Ignore(x => x.Drivers).ReverseMap();
            CreateMap<ShopItemViewModel, ShopItem>().ForMember(
                dest => dest.Id,
                opt =>
                {
                    opt.Ignore();
                    opt.UseDestinationValue();
                }
            ).Ignore(
                dest => dest.CreatedDate
            ).Ignore(
                dest => dest.DeletedDate
            ).Ignore(
                dest => dest.UpdatedByUserId
            ).Ignore(
                dest => dest.UpdatedDate
            ).Ignore(
                dest => dest.DeletedByUserId
            ).Ignore(
                dest => dest.CreatedByUserId
            ).ForMember(
                dest => dest.Categories,
                opt => opt.MapFrom(src => src.Categories)).ReverseMap();

            CreateMap<ShopItemViewModel, OrderShopItem>()
                .ForMember(dest => dest.ShopItem, opt => opt.MapFrom(src => src))
                .ForPath(dest => dest.ShopItem.Id, opt => opt.MapFrom(src => src.Id))
                .ForPath(dest => dest.ShopItem.Name, opt => opt.MapFrom(src => src.Name))
                .ForPath(dest => dest.ShopItem.Price, opt => opt.MapFrom(src => src.Price))
                .ForPath(dest => dest.ShopItem.ManagedServicePrice, opt => opt.MapFrom(src => src.ManagedServicePrice))
                .ForPath(dest => dest.ShopItem.Images, opt => opt.MapFrom(src => src.Images))
                .ForPath(dest => dest.ShopItem.Description, opt => opt.MapFrom(src => src.Description))
                .ForPath(dest => dest.ShopItem.DescriptionShort, opt => opt.MapFrom(src => src.DescriptionShort))
                .Ignore(x => x.ShopItemId).Ignore(x => x.Order).Ignore(x => x.OrderId).ReverseMap();

            /******************** Order ***************************************/
            CreateMap<Order, OrderViewModel>().ForMember(
                dest => dest.ShopItems,
                opt => opt.MapFrom(src => src.ShopItems)).ReverseMap();


            /******************** DeviceProperties *****************************/
            CreateMap<ClientProperty, DevicePropertyAddViewModel>().ReverseMap();
            CreateMap<ClientProperty, DevicePropertyViewModel>().ReverseMap();

            CreateMap<Workflow, WorkflowViewModels>().ReverseMap();
            CreateMap<Certification, CertificationViewModels>().ReverseMap();
            CreateMap<DomainRegistrationTemp, DomainRegistrationTempViewModel>().ReverseMap();
            CreateMap<Models.PreinstalledSoftwareViewModel, PreinstalledSoftware>().ReverseMap();
            CreateMap<WMIInvenotryCmds, WMIInventoryCmdViewModel>().ReverseMap();
            CreateMap<InventoryCSV, Inventory>()
                .Ignore(x => x.Client)
                .Ignore(x => x.CreatedByUserId)
                .Ignore(x => x.CreatedDate)
                .Ignore(x => x.UpdatedDate)
                .Ignore(x => x.DeletedByUserId)
                .Ignore(x => x.DeletedDate)
                .Ignore(x => x.UpdatedByUserId)
                .ReverseMap();
            CreateMap<Driver, DriverViewModel>()
                .ReverseMap();
            CreateMap<BIOSModel, BIOSModelViewModel>().ReverseMap();
            CreateMap<OSModel, OSModelViewModel>()
                .Ignore(x => x.HardwareModels)
                .Ignore(x => x.ValidModels)
                .Ignore(x => x.Content)
                .ReverseMap();
            CreateMap<HardwareModel, HardwareModelViewModel>().ReverseMap();
            CreateMap<AssetModel, AssetModelViewModel>().ReverseMap();
            CreateMap<AssetType, AssetTypeViewModel>().ReverseMap();
            CreateMap<AssetClass, AssetClassViewModel>().ReverseMap();
            CreateMap<VendorModel, VendorModelViewModel>().ReverseMap();
            CreateMap<RulesViewModel, Rule>()
                .Ignore(x => x.CreatedByUserId)
                .Ignore(x => x.CreatedDate)
                .Ignore(x => x.DeletedByUserId)
                .Ignore(x => x.DeletedDate)
                .Ignore(x => x.UpdatedByUserId)
                .Ignore(x => x.UpdatedDate)
                .ReverseMap();
            CreateMap<ActivityLog, ActivityLogViewModel>().ReverseMap();
        }
    }
}