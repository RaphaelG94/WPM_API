using WPM_API.Models;
using azure = Microsoft.Azure.Management.ResourceManager.Models;
using azureStorage = Microsoft.Azure.Management.Storage.Models;
using azureNetwork = Microsoft.Azure.Management.Network.Models;

namespace WPM_API.Code.Mappers.Azure
{
    public class AzureMapper : MapperBase
    {
        public AzureMapper()
        {
            CreateMap<azure.ResourceGroup, ResourceGroupViewModel>()
                .Ignore(x => x.AzureSubscriptionId)
                .ReverseMap();
            CreateMap<azureStorage.StorageAccount, StorageAccountViewModel>().ForMember(
                    dest => dest.Type,
                    opt => opt.MapFrom(src => src.Sku.Name)
                )
                .Ignore(x => x.ResourceGroupId)
                .ReverseMap();

            CreateMap<azureNetwork.Subnet, SubnetViewModel>().ForMember(
                    dest => dest.AddressRange,
                    opt => opt.MapFrom(src => src.AddressPrefix)
                ).ReverseMap();

            CreateMap<azureNetwork.VirtualNetwork, VirtualNetworkViewModel>()
                .ForMember(
                    dest => dest.AddressRange,
                    opt => opt.MapFrom(src => src.AddressSpace.AddressPrefixes[0]))
                .ForMember(
                    dest => dest.Dns,
                    opt => opt.MapFrom(src => src.DhcpOptions.DnsServers.Count > 0 ? src.DhcpOptions.DnsServers[0] : ""))
                .ReverseMap(); ;

            /** Virtual Network **/
            CreateMap<VirtualNetworkViewModel, WPM_API.Azure.Models.VirtualNetworkViewModel>().Ignore(x => x.Id).Ignore(x => x.ResourceGroup)
                .ReverseMap();
            CreateMap<VirtualNetworkAddViewModel, WPM_API.Azure.Models.VirtualNetworkAddOrEditViewModel>().ReverseMap();
            CreateMap<SubnetViewModel, WPM_API.Azure.Models.SubnetViewModel>().ReverseMap();

            /** VPN **/
            CreateMap<WPM_API.Azure.Models.VPN, VpnViewModel>().ReverseMap();
            CreateMap<WPM_API.Azure.Models.VpnAdd, VpnViewModel>().ForMember(
                    dest => dest.Virtual,
                    opt => opt.Ignore()
                ).ReverseMap();
            CreateMap<WPM_API.Azure.Models.VPNLocalProperties, VpnLocalProperties>().ReverseMap();
            CreateMap<WPM_API.Azure.Models.VPNVirtualProperties, VpnVirtualProperties>().ReverseMap();

            /** Virtual Machine **/
            CreateMap<WPM_API.Azure.Models.VirtualMachineModel, VirtualMachineViewModel>().Ignore(x=> x.AzureId).Ignore(x => x.BaseName).Ignore(x => x.Status).ForMember(
                    dest => dest.AdminUserName,
                    opt => opt.MapFrom(src => src.Admin.Username)
                ).ReverseMap();
            CreateMap<AdminCredentials, WPM_API.Azure.Models.AdminCredentials>().ReverseMap();
            CreateMap<DiskViewModel, WPM_API.Azure.Models.Disk>().ReverseMap();
            CreateMap<DiskViewModel, WPM_API.Azure.Models.DiskAdd>().ReverseMap();
            CreateMap<VirtualMachineAddViewModel, WPM_API.Azure.Models.VirtualMachineAddModel>().Ignore(x => x.CustomerId)
                .Ignore(x => x.SubscriptionId)
                .Ignore(x => x.ResourceGroupName)
                .Ignore(x => x.StorageAccountId)
                .Ignore(x => x.Network)
                .ReverseMap();
            CreateMap<WPM_API.Azure.Models.VirtualMachineModel, WPM_API.Azure.Models.VirtualMachineRefModel>().ForMember(
                    dest => dest.VmId,
                    opt => opt.MapFrom(src => src.Id)
                );
        }
    }
}
