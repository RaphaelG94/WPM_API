using WPM_API.Azure.Core;
using Microsoft.Azure.Management.ResourceManager;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;
using Microsoft.Rest;
using Microsoft.Rest.Azure.Authentication;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace WPM_API.Azure
{
    public class AzureCommunicationService
    {
        private ServiceClientCredentials _credentials = null;
        private readonly AzureCredentials _azureCredentials = null;
        private readonly string tenantId;
        private readonly string clientId;
        private readonly string clientSecret;

        public AzureCommunicationService(string tenantId, string clientId, string clientSecret)
        {
            this.tenantId = tenantId;
            this.clientId = clientId;
            this.clientSecret = clientSecret;
            _azureCredentials = SdkContext.AzureCredentialsFactory.FromServicePrincipal(clientId, clientSecret, tenantId, AzureEnvironment.AzureGlobalCloud);
        }

        private ResourceGroupService _resourceGroupService = null;
        public ResourceGroupService ResourceGroupService()
        {
            SetCredentialsAsync().Wait();
            if (_resourceGroupService == null)
            {
                _resourceGroupService = new ResourceGroupService(_credentials);
            }
            return _resourceGroupService;
        }

        private StorageService _storageService = null;
        public StorageService StorageService()
        {
            SetCredentialsAsync().Wait();
            if (_storageService == null)
            {
                _storageService = new StorageService(_azureCredentials);
            }
            return _storageService;
        }

        private SubscriptionService _subscriptionService = null;
        public SubscriptionService SubscriptionService()
        {
            SetCredentialsAsync().Wait();
            _subscriptionService = new SubscriptionService(_credentials);

            return _subscriptionService;
        }

        private VirtualMachineService _virtualMachineService = null;
        public VirtualMachineService VirtualMachineService()
        {
            SetCredentialsAsync().Wait();
            if (_virtualMachineService == null)
            {
                _virtualMachineService = new VirtualMachineService(_azureCredentials);
            }
            return _virtualMachineService;
        }

        private VirtualNetworkService _virtualNetworkService = null;
        public VirtualNetworkService VirtualNetworkService()
        {
            SetCredentialsAsync().Wait();
            if (_virtualNetworkService == null)
            {
                _virtualNetworkService = new VirtualNetworkService(_azureCredentials);
            }
            return _virtualNetworkService;
        }

        private VPNService _vpnService = null;
        public VPNService VPNService()
        {
            SetCredentialsAsync().Wait();
            if (_vpnService == null)
            {
                _vpnService = new VPNService(_azureCredentials);
            }
            return _vpnService;
        }

        private async Task<ServiceClientCredentials> GetCredentialsAsync()
        {
            if (_credentials == null)
            {
                _credentials = await ApplicationTokenProvider.LoginSilentAsync(tenantId, clientId, clientSecret);
            }
            return _credentials;
        }

        private async Task SetCredentialsAsync()
        {
            if (_credentials == null)
            {
                _credentials = await GetCredentialsAsync();
            }
            if (_azureCredentials.DefaultSubscriptionId == null)
            {
                // Set default Subscription
                using (var subscriptionClient = new Microsoft.Azure.Management.ResourceManager.SubscriptionClient(_credentials))
                {
                    string subscriptionId = subscriptionClient.Subscriptions.List().FirstOrDefault().SubscriptionId;
                    _azureCredentials.WithDefaultSubscription(subscriptionId);
                }
            }
        }

        ///********************************** SUBSCRIPTIONS *****************************************/
        //public async Task<List<Subscription>> GetSubscriptionsAsync()
        //{
        //    SubscriptionService service = new SubscriptionService(await GetCredentialsAsync());
        //    return await service.GetSubscriptions();
        //}

        //public async Task<List<Subscription>> GetSubscriptionsAsync(List<string> subscriptionIds)
        //{
        //    SubscriptionService service = new SubscriptionService(await GetCredentialsAsync());
        //    return service.GetSubscriptions(subscriptionIds);
        //}

        //public async Task<Subscription> GetSubscriptionAsync(string subscriptionId)
        //{
        //    SubscriptionService service = new SubscriptionService(await GetCredentialsAsync());
        //    return await service.GetSubscription(subscriptionId);
        //}

        ///********************************** RessourceGroups *****************************************/

        //public async Task<List<ResourceGroup>> GetResourceGroupsAsync(string subscriptionId)
        //{
        //    ResourceGroupService service = new ResourceGroupService(await GetCredentialsAsync());
        //    return service.GetRessourceGroups(subscriptionId);
        //}

        //public async Task<ResourceGroup> AddResourceGroupAsync(string subscriptionId, ResourceGroup ressourceGroup)
        //{
        //    ResourceGroupService service = new ResourceGroupService(await GetCredentialsAsync());
        //    return await service.AddRessourceGroup(subscriptionId, ressourceGroup);
        //}

        //public async Task<AzureOperationResponse> DeleteResourceGroupAsync(string subscriptionId, string ressourceGroupName)
        //{
        //    ResourceGroupService service = new ResourceGroupService(await GetCredentialsAsync());
        //    return await service.DeleteRessourceGroupAsync(subscriptionId, ressourceGroupName);
        //}

        ///********************************** Locations ***********************************************/

        //public async Task<List<Location>> GetLocations()
        //{
        //    SubscriptionService service = new SubscriptionService(await GetCredentialsAsync());
        //    return await service.GetLocations();
        //}

        ///********************************** Virtual Networks ***********************************************/

        //public async Task<List<VirtualNetwork>> GetVirtualNetworksAsync(string subscriptionId, string ressourceGroup)
        //{
        //    VirtualNetworkService service = new VirtualNetworkService(_azureCredentials);
        //    return await service.GetVirtualNetworksAsync(subscriptionId, ressourceGroup);
        //}

        //// TODO DNS-Server
        //public async Task<VirtualNetworkViewModel> AddOrModifyVirtualNetworkAsync(string subscriptionId, string resourceGroupName, VirtualNetworkAddOrEditViewModel virtualNetworkModel)
        //{
        //    VirtualNetworkService service = new VirtualNetworkService(_azureCredentials);
        //    var vn = await service.AddOrModifyVirtualNetworkAsync(subscriptionId, resourceGroupName, virtualNetworkModel);
        //    VirtualNetworkViewModel virtualNetwork = new VirtualNetworkViewModel
        //    {
        //        AddressRange = vn.AddressSpaces[0],
        //        //virtualNetwork.Dns = vn.DnsServerIps[0];
        //        Id = vn.Id,
        //        Name = vn.Name,
        //        ResourceGroup = vn.ResourceGroupName,
        //        Subnets = new List<SubnetViewModel>()
        //    };
        //    foreach (Microsoft.Azure.Management.Network.Fluent.ISubnet s in vn.Subnets.Values)
        //    {
        //        SubnetViewModel subnet = new SubnetViewModel
        //        {
        //            AddressRange = s.AddressPrefix,
        //            Name = s.Name
        //        };
        //        virtualNetwork.Subnets.Add(subnet);
        //    }
        //    return virtualNetwork;
        //}

        //public void DeleteVirtualNetwork(string subscriptionId, string virtualNetworkId)
        //{
        //    VirtualNetworkService service = new VirtualNetworkService(_azureCredentials);
        //    service.DeleteVirtualNetwork(subscriptionId, virtualNetworkId);
        //}

        ///********************************** Storage-Accounts ***********************************************/

        //public async Task<StorageAccount> GetStorageAccountAsync(string subscriptionId, string ressourceGroup, string storageAccountId)
        //{
        //    StorageService service = new StorageService(_azureCredentials);
        //    return (await service.GetStorageAccounts(subscriptionId, ressourceGroup)).Find(x => x.Id == storageAccountId);
        //}

        //public async Task<List<StorageAccount>> GetStorageAccountsAsync(string subscriptionId, string ressourceGroup)
        //{
        //    StorageService service = new StorageService(_azureCredentials);
        //    return await service.GetStorageAccounts(subscriptionId, ressourceGroup);
        //}

        //public async Task<StorageAccountViewModel> AddStorageAccountAsync(string subscriptionId, string ressourceGroupName, string storageAccountName, string storageAccountType)
        //{
        //    StorageService service = new StorageService(_azureCredentials);
        //    var result = await service.AddStorageAccountAsync(subscriptionId, ressourceGroupName, storageAccountName, storageAccountType);
        //    return new StorageAccountViewModel
        //    {
        //        Id = result.Id,
        //        Name = result.Name,
        //        ResourceGroup = result.ResourceGroupName,
        //        Type = (StorageType)result.Sku.Name
        //    };
        //}

        //public async Task<StorageAccountViewModel> EditStorageAccountAsync(string subscriptionId, string storageAccountId, string storageAccountType)
        //{
        //    StorageService service = new StorageService(_azureCredentials);
        //    var result = await service.EditStorageAccountAsync(subscriptionId, storageAccountId, storageAccountType);
        //    return new StorageAccountViewModel
        //    {
        //        Id = result.Id,
        //        Name = result.Name,
        //        ResourceGroup = result.ResourceGroupName,
        //        Type = (StorageType)result.Sku.Name
        //    };
        //}

        //public void DeleteStorageAccount(string storageAccountId)
        //{
        //    StorageService service = new StorageService(_azureCredentials);
        //    service.DeleteStorageAccountAsync(storageAccountId);
        //}

        //public Task<Microsoft.Azure.Management.Storage.Fluent.CheckNameAvailabilityResult> StorageAccountExistsAsync(string name)
        //{
        //    StorageService service = new StorageService(_azureCredentials);
        //    return service.StorageAccountExists(name);
        //}

        ///********************************** VPN ***********************************************/

        //public async Task<List<VPN>> GetVPNsAsync(string subscriptionId, string resourceGroupName)
        //{
        //    VPNService service = new VPNService(_azureCredentials);
        //    return await service.GetVPNsAsync(subscriptionId, resourceGroupName);
        //}

        //public async Task<VPN> GetVPNAsync(string subscriptionId, string resourceGroupName, string id)
        //{
        //    VPNService service = new VPNService(_azureCredentials);
        //    return await service.GetVPNAsync(subscriptionId, resourceGroupName, id);
        //}

        //public async Task<VPN> AddVPNAsync(string subscriptionId, string resourceGroupName, VpnAdd vpn)
        //{
        //    VPNService service = new VPNService(_azureCredentials);
        //    return await service.AddVPN(subscriptionId, resourceGroupName, vpn);
        //}

        //public async Task<VPN> EditVPNAsync(string subscriptionId, string resourceGroupName, VpnEdit vpn)
        //{
        //    VPNService service = new VPNService(_azureCredentials);
        //    return await service.EditVPN(subscriptionId, resourceGroupName, vpn);
        //}

        //public void DeleteVPNAsync(string subscriptionId, string resourceGroupName, string vpnId)
        //{
        //    VPNService service = new VPNService(_azureCredentials);
        //    service.DeleteVPN(subscriptionId, resourceGroupName, vpnId);
        //}

        ///********************************** Virtual Machines ***********************************************/

        //public async Task<List<VirtualMachineModel>> GetVirtualMachinesAsync(string subscriptionId)
        //{
        //    VirtualMachineService service = new VirtualMachineService(_azureCredentials);
        //    return await service.GetVirtualMachinesAsync(subscriptionId);
        //}

        //public Task<VirtualMachineModel> GetVirtualMachineAsync(string subscriptionId, string resourceGroupName, string id)
        //{
        //    VirtualMachineService service = new VirtualMachineService(_azureCredentials);
        //    return service.GetVirtualMachineAsync(subscriptionId, resourceGroupName, id);
        //}

        //public async Task<VirtualMachineModel> AddVirtualMachineAsync(VirtualMachineAddModel vm)
        //{
        //    VirtualMachineService service = new VirtualMachineService(_azureCredentials);
        //    return await service.AddVirtualMachinesAsync(vm);
        //}

        //public async Task<VirtualMachineModel> EditVirtualMachineAsync(VirtualMachineEditModel vm)
        //{
        //    VirtualMachineService service = new VirtualMachineService(_azureCredentials);
        //    return await service.EditVirtualMachinesAsync(vm);
        //}

        //public void DeleteVirtualMachineAsync(string subscriptionId, string resourceGroupName, string id)
        //{
        //    VirtualMachineService service = new VirtualMachineService(_azureCredentials);
        //    service.DeleteVirtualMachine(subscriptionId, id);
        //}

        //public async Task ExecuteVirtualMachineScriptAsync(VirtualMachineRefModel virtualMachineRefModel, string scriptName, string arguments)
        //{
        //    VirtualMachineService service = new VirtualMachineService(_azureCredentials);
        //    await service.ExecuteVirtualMachineScriptAsync(virtualMachineRefModel, scriptName, arguments);
        //}


        //public async Task AddInstShareAsync(VirtualMachineRefModel virtualMachineRefModel)
        //{
        //    VirtualMachineService service = new VirtualMachineService(_azureCredentials);
        //    await service.AddInstShare(virtualMachineRefModel);
        //}

        ///************************ Images **************************************************/
        //public List<string> GetImages()
        //{
        //    VirtualMachineService service = new VirtualMachineService(_azureCredentials);
        //    return service.GetAvailableSku();
        //}
    }
}
