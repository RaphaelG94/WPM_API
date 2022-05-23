using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Reflection;
using System.Text;
using WPM_API.Azure;
using WPM_API.Code.Infrastructure;
using WPM_API.Code.Infrastructure.LogOn;
using WPM_API.Common;
using WPM_API.Data.Models;
using WPM_API.Models;
using WPM_API.Options;
using AZURE = WPM_API.Azure.Models;
using DATA = WPM_API.Data.DataContext.Entities;

namespace WPM_API.Controllers.Base
{
    /// <summary>
    /// Manage Base in Context from a Customer
    /// </summary>
    [Route("customers/{customerId}/bases")]
    public class BaseController : BasisController
    {
        public BaseController(AppSettings appSettings, ConnectionStrings connectionStrings, OrderEmailOptions orderEmailOptions, AgentEmailOptions agentEmailOptions, SendMailCreds sendMailCreds, SiteOptions siteOptions, ILogonManager logonManager) : base(appSettings, connectionStrings, orderEmailOptions, agentEmailOptions, sendMailCreds, siteOptions, logonManager)
        {
        }

        /// <summary>
        /// Retrieve all bases without details.
        /// </summary>
        /// <param name="customerId">Id of the Customer</param>
        /// <returns>[BaseRef]</returns>
        [HttpGet]
        [Authorize(Policy = Constants.Roles.Customer)]
        public IActionResult GetBases([FromRoute] string customerId)
        {
            List<BaseViewModel> bases = new List<BaseViewModel>();
            DATA.CloudEntryPoint creds = GetCEP(customerId);
            if (creds != null)
            {
                List<DATA.Base> dbEntries = UnitOfWork.Bases.GetAll(BaseIncludes.GetAllIncludes()).Where(x => x.CredentialsId == creds.Id).ToList();
                bases = Mapper.Map<List<DATA.Base>, List<BaseViewModel>>(dbEntries);
            }

            // Serialize and return the response
            var json = JsonConvert.SerializeObject(bases, serializerSettings);
            return new OkObjectResult(json);
        }

        [HttpDelete]
        [Route("{baseId}")]
        [Authorize(Policy = Constants.Policies.Customer)]
        public async Task<IActionResult> DeleteBase([FromRoute] string baseId, [FromRoute] string customerId)
        {
            try
            {
                using (var unitOfWork = CreateUnitOfWork())
                {
                    DATA.Customer customer = unitOfWork.Customers.GetOrNull(customerId);
                    if (customer == null)
                    {
                        return BadRequest("ERROR: The customer does not exist");
                    }

                    DATA.Base toDelete = unitOfWork.Bases.GetOrNull(baseId, BaseIncludes.GetAllIncludes());

                    // Check for existence
                    if (toDelete == null)
                    {
                        return BadRequest("ERROR: The base does not exist");
                    }

                    // Delete resource group in Azrue
                    DATA.CloudEntryPoint cep = GetCEP(customerId);
                    if (cep == null)
                    {
                        return BadRequest("ERROR: No Cloud Entry Point is set yet");
                    }

                    AzureCommunicationService azure = new AzureCommunicationService(cep.TenantId, cep.ClientId, cep.ClientSecret);
                    var resGrpExists = azure.ResourceGroupService().GetRessourceGroupByName(toDelete.ResourceGroup.Name, toDelete.Subscription.AzureId);
                    if (resGrpExists)
                    {
                        var success = await azure.ResourceGroupService().DeleteRessourceGroupAsync(toDelete.Subscription.AzureId, toDelete.ResourceGroup.Name);
                    }

                    // Delete all relations of the base
                    unitOfWork.Subscriptions.MarkForDelete(toDelete.Subscription, GetCurrentUser().Id);
                    unitOfWork.ResourceGroups.MarkForDelete(toDelete.ResourceGroup, GetCurrentUser().Id);
                    unitOfWork.StorageAccounts.MarkForDelete(toDelete.StorageAccount, GetCurrentUser().Id);
                    unitOfWork.VirtualNetworks.MarkForDelete(toDelete.VirtualNetwork, GetCurrentUser().Id);
                    foreach (DATA.VirtualMachine vm in toDelete.VirtualMachines)
                    {
                        unitOfWork.VirtualMachines.MarkForDelete(vm, GetCurrentUser().Id);
                    }
                    foreach (DATA.Client client in toDelete.Clients)
                    {
                        client.Base = null;
                        unitOfWork.Clients.MarkForUpdate(client, GetCurrentUser().Id);
                    }
                    unitOfWork.Bases.MarkForDelete(toDelete, GetCurrentUser().Id);
                    unitOfWork.SaveChanges();
                    return Ok();
                }
            }
            catch (Exception e)
            {
                return BadRequest("ERROR: " + e.Message);
            }
        }

        /// <summary>
        /// Create new base.
        /// </summary>
        /// <param name="customerId">Id of the Customer</param>
        /// <param name="baseAdd">New Base</param>
        /// <returns>Base</returns>
        [HttpPost]
        [Authorize(Policy = Constants.Roles.Customer)]
        public async Task<IActionResult> AddBase([FromRoute] string customerId, [FromBody] BaseAddViewModel baseAdd)
        {
            using (var unitOfWork = CreateUnitOfWork())
            {
                BaseViewModel baseModel = new BaseViewModel();
                DATA.Base newEntry = Mapper.Map<BaseAddViewModel, DATA.Base>(baseAdd);
                newEntry.BaseStatus = new DATA.BaseStatus();
                newEntry.BaseStatus.Status = "Creating";
                DATA.Customer customer = unitOfWork.Customers.Get(customerId);
                try
                {
                    DATA.CloudEntryPoint credentials = GetCEP(customerId);
                    if (credentials == null)
                    {
                        return BadRequest("ERROR: The cloud entry point does not exist or is not valid!");
                    }
                    AzureCommunicationService azure = new AzureCommunicationService(credentials.TenantId, credentials.ClientId, credentials.ClientSecret);

                    // Get Subscription for SubscriptionName
                    var subscription = (azure.SubscriptionService()).GetSubscription(newEntry.Subscription.AzureId);

                    newEntry.Subscription.Id = null; // HOTFIX!! Todo: Automapper ignores Subscription.Id && VirtualMachine
                    for (int i = 0; i < newEntry.VirtualNetwork.Subnets.Count; i++)
                    {
                        newEntry.VirtualNetwork.Subnets[i].Number = i + 1;
                    }

                    newEntry.VirtualMachines = new List<DATA.VirtualMachine>();

                    // Add SubscriptionName
                    newEntry.Subscription.Name = (await subscription).DisplayName;

                    // Add Properties
                    var defaults = UnitOfWork.Customers.Get(customerId, "Defaults").Defaults.FindAll(x => x.Category.Equals("base-prop"));
                    newEntry.Properties = Mapper.Map<List<DATA.AdvancedProperty>>(defaults);
                    newEntry.Customer = customer;
                    newEntry.CredentialsId = credentials.Id;

                    // Create worker data

                    unitOfWork.Bases.MarkForInsert(newEntry, GetCurrentUser().Id);
                    unitOfWork.SaveChanges();
                }
                catch (Exception e)
                {
                    return BadRequest("ERROR: " + e.Message);
                }

                // Create Azure instances of Base
                CreateResourceGrp(customer, newEntry.Id, baseAdd.VirtualNetwork);

                // Serialize and return the response
                baseModel = Mapper.Map<DATA.Base, BaseViewModel>(newEntry);
                var json = JsonConvert.SerializeObject(baseModel, serializerSettings);
                return new OkObjectResult(json);
            }
        }

        private async Task CreateResourceGrp(DATA.Customer customer, string baseId, VirtualNetworkAddViewModel VirtualNetwork)
        {
            using (var unitOfWork = CreateUnitOfWork())
            {
                DATA.Base toEdit = unitOfWork.Bases.Get(baseId, BaseIncludes.GetAllIncludes());
                try
                {
                    toEdit.BaseStatus.ResourceGroupStatus = "Creating";
                    unitOfWork.Bases.MarkForUpdate(toEdit, GetCurrentUser().Id);
                    unitOfWork.SaveChanges();
                    DATA.CloudEntryPoint credentials = GetCEP(customer.Id);
                    AzureCommunicationService azure = new AzureCommunicationService(credentials.TenantId, credentials.ClientId, credentials.ClientSecret);
                    string subscriptionId = toEdit.Subscription.AzureId;
                    var resourceGroup = await azure.ResourceGroupService().AddResourceGroup(subscriptionId, new Microsoft.Azure.Management.ResourceManager.Models.ResourceGroup(toEdit.ResourceGroup.Location, null, toEdit.ResourceGroup.Name));
                    string resourceGroupName = toEdit.ResourceGroup.Name;
                    toEdit.ResourceGroup.AzureSubscriptionId = subscriptionId;
                    toEdit.ResourceGroup.CustomerId = customer.Id;
                    toEdit.BaseStatus.ResourceGroupStatus = "Created";
                    unitOfWork.Bases.MarkForUpdate(toEdit);
                    unitOfWork.SaveChanges();

                    CreateVirtualNetwork(customer, baseId, VirtualNetwork);
                }
                catch (Exception e)
                {
                    toEdit.Status = "ERROR: " + e.Message;
                    toEdit.BaseStatus.ResourceGroupStatus = "Error";
                    toEdit.BaseStatus.Status = "Error";
                    toEdit.BaseStatus.ErrorMessage += e.Message + "\n";
                    unitOfWork.Bases.MarkForUpdate(toEdit);
                    unitOfWork.SaveChanges();

                    // return BadRequest("ERROR: " + e.Message);
                }
            }
        }

        private async Task CreateVirtualNetwork(DATA.Customer customer, string baseId, VirtualNetworkAddViewModel VirtualNetwork)
        {
            using (var unitOfWork = CreateUnitOfWork())
            {
                DATA.Base toEdit = unitOfWork.Bases.Get(baseId, BaseIncludes.GetAllIncludes());
                try
                {
                    toEdit.BaseStatus.VirtualNetworkStatus = "Creating";
                    unitOfWork.Bases.MarkForUpdate(toEdit);
                    unitOfWork.SaveChanges();
                    DATA.CloudEntryPoint credentials = GetCEP(customer.Id);
                    AzureCommunicationService azure = new AzureCommunicationService(credentials.TenantId, credentials.ClientId, credentials.ClientSecret);
                    if (String.IsNullOrEmpty(toEdit.VirtualNetwork.Dns))
                    {
                        toEdit.VirtualNetwork.Dns = "Azure DNS";
                    }
                    var vnModel = new AZURE.VirtualNetworkAddOrEditViewModel();
                    vnModel.AddressRange = VirtualNetwork.AddressRange;
                    vnModel.Name = VirtualNetwork.Name;
                    // Mapper.Map<List<AZURE.SubnetViewModel>>(VirtualNetwork.Subnets);
                    vnModel.Subnets = new List<AZURE.SubnetViewModel>();
                    foreach (SubnetViewModel subnet in VirtualNetwork.Subnets)
                    {
                        vnModel.Subnets.Add(new AZURE.SubnetViewModel() { Name = subnet.Name, AddressRange = subnet.AddressRange });
                    }
                    var virtualNetwork = await azure.VirtualNetworkService().AddOrModifyVirtualNetworkAsync(toEdit.Subscription.AzureId, toEdit.ResourceGroup.Name, vnModel, toEdit.ResourceGroup.Location);
                    toEdit.VirtualNetwork.AzureId = virtualNetwork.Id;
                    toEdit.BaseStatus.VirtualNetworkStatus = "Created";
                    unitOfWork.Bases.MarkForUpdate(toEdit);
                    unitOfWork.SaveChanges();

                    CreateStorageAccount(customer, baseId);
                }
                catch (Exception e)
                {
                    toEdit.Status = "ERROR: " + e.Message;
                    toEdit.BaseStatus.VirtualNetworkStatus = "Error";
                    toEdit.BaseStatus.Status = "Error";
                    toEdit.BaseStatus.ErrorMessage += e.Message + "\n";
                    unitOfWork.Bases.MarkForUpdate(toEdit);
                    unitOfWork.SaveChanges();

                }
            }
        }

        private async Task CreateStorageAccount(DATA.Customer customer, string baseId)
        {
            using (var unitOfWork = CreateUnitOfWork())
            {
                DATA.Base toEdit = unitOfWork.Bases.Get(baseId, BaseIncludes.GetAllIncludes());
                try
                {
                    toEdit.BaseStatus.StorageAccountStatus = "Creating";
                    unitOfWork.Bases.MarkForUpdate(toEdit);
                    unitOfWork.SaveChanges();
                    DATA.CloudEntryPoint credentials = GetCEP(customer.Id);
                    AzureCommunicationService azure = new AzureCommunicationService(credentials.TenantId, credentials.ClientId, credentials.ClientSecret);
                    string storageAccountId = String.Empty;

                    var checkStorageAccount = await azure.StorageService().CheckNameAvailabilityAsync(toEdit.StorageAccount.Name);
                    if (checkStorageAccount.IsAvailable == true)
                    {
                        toEdit.Status = "Adding Storage Account.";
                        unitOfWork.Bases.MarkForUpdate(toEdit);
                        unitOfWork.SaveChanges();
                        var storageAccountAzure = await azure.StorageService()
                            .AddStorageAccountAsync(toEdit.Subscription.AzureId, toEdit.ResourceGroup.Name, toEdit.StorageAccount.Name, toEdit.StorageAccount.Type, toEdit.ResourceGroup.Location, "Storage V2");
                        storageAccountId = storageAccountAzure.Id;
                        toEdit.StorageAccount.AzureId = storageAccountId;
                        toEdit.StorageAccount.CustomerId = customer.Id;
                        toEdit.StorageAccount.ResourceGroupId = toEdit.ResourceGroup.Id;
                        toEdit.BaseStatus.StorageAccountStatus = "Created";
                        unitOfWork.Bases.MarkForUpdate(toEdit);
                        unitOfWork.SaveChanges();

                        CreateVPN(customer, baseId);
                    }
                    else
                    {
                        try
                        {
                            storageAccountId = (await azure.StorageService().GetStorageAccounts(toEdit.Subscription.AzureId, toEdit.ResourceGroup.Name)).First(x => x.Name == toEdit.StorageAccount.Name).Id;
                        }
                        catch (Exception e)
                        {
                            // Error
                            toEdit.Status = "Error: Storage Account already taken.";
                            toEdit.BaseStatus.StorageAccountStatus = "Error";
                            toEdit.BaseStatus.ErrorMessage += e.Message + "\n";
                            unitOfWork.Bases.MarkForUpdate(toEdit);
                            unitOfWork.SaveChanges();
                        }
                    }

                }
                catch (Exception e)
                {
                    toEdit.Status = "ERROR: " + e.Message;
                    toEdit.BaseStatus.StorageAccountStatus = "Error";
                    toEdit.BaseStatus.Status = "Error";
                    toEdit.BaseStatus.ErrorMessage += e.Message + "\n";
                    UnitOfWork.Bases.MarkForUpdate(toEdit);
                    UnitOfWork.SaveChanges();

                }
            }
        }

        private async Task CreateVPN(DATA.Customer customer, string baseId)
        {
            using (var unitOfWork = CreateUnitOfWork())
            {
                DATA.Base toEdit = unitOfWork.Bases.Get(baseId, BaseIncludes.GetAllIncludes());
                try
                {
                    toEdit.BaseStatus.VPNStatus = "Creating";
                    unitOfWork.Bases.MarkForUpdate(toEdit);
                    unitOfWork.SaveChanges();
                    DATA.CloudEntryPoint credentials = GetCEP(customer.Id);
                    AzureCommunicationService azure = new AzureCommunicationService(credentials.TenantId, credentials.ClientId, credentials.ClientSecret);

                    await AddVpnAsync(azure, toEdit.Id, customer.Id, toEdit.Subscription.AzureId, toEdit.ResourceGroup.Name, toEdit.VirtualNetwork.AzureId);

                }
                catch (Exception e)
                {
                    toEdit.Status = "ERROR: " + e.Message;
                    if (toEdit.BaseStatus.VPNStatus != "Created")
                    {
                        toEdit.BaseStatus.VPNStatus = "Error";
                    }
                    toEdit.BaseStatus.Status = "Error";
                    toEdit.BaseStatus.ErrorMessage += e.Message + "\n";
                    unitOfWork.Bases.MarkForUpdate(toEdit);
                    unitOfWork.SaveChanges();

                }
            }
        }

        [HttpGet]
        [Route("baseStatus/{baseId}")]
        public IActionResult GetBaseStatus([FromRoute] string baseId)
        {
            DATA.Base toFetch = UnitOfWork.Bases.GetOrNull(baseId, "BaseStatus");

            if (toFetch == null)
            {
                return BadRequest("ERROR: The base does not exist");
            }

            BaseStatusViewModel result = new BaseStatusViewModel();
            result.Id = toFetch.BaseStatus.Id;
            result.ResourceGroupStatus = toFetch.BaseStatus.ResourceGroupStatus;
            result.Status = toFetch.BaseStatus.Status;
            result.StorageAccountStatus = toFetch.BaseStatus.StorageAccountStatus;
            result.VirtualNetworkStatus = toFetch.BaseStatus.VirtualNetworkStatus;
            result.VPNStatus = toFetch.BaseStatus.VPNStatus;
            result.ErrorMessage = toFetch.BaseStatus.ErrorMessage;

            var json = JsonConvert.SerializeObject(result, serializerSettings);
            return Ok(json);
        }

        [HttpPost]
        [Route("addParams")]
        public IActionResult AddParamBase([FromBody] BaseAddViewModel baseAdd)
        {
            DATA.Base toEdit = UnitOfWork.Bases.Get(baseAdd.Id, BaseIncludes.GetAllIncludes());
            var FixedParams = GenerateFixedBaseParams(toEdit);
            foreach (var param in FixedParams)
            {
                param.BaseId = toEdit.Id;
                param.Base = toEdit;
                toEdit.Properties.Add(param);
            }

            UnitOfWork.Bases.MarkForUpdate(toEdit);
            UnitOfWork.SaveChanges();
            return Ok();
        }

        /// <summary>
        /// Retrieve a single bases.
        /// </summary>
        /// <param name="customerId">Id of the Customer</param>
        /// <param name="baseId">Details from Base</param>
        /// <returns>Base</returns>
        [HttpGet]
        [Authorize(Policy = Constants.Roles.Customer)]
        [Route("{baseId}")]
        public IActionResult GetBaseDetail([FromRoute] string customerId, [FromRoute] string baseId)
        {
            DATA.Base dbBase = UnitOfWork.Bases.Get(baseId, BaseIncludes.GetAllIncludes());
            BaseViewModel baseViewModel = Mapper.Map<BaseViewModel>(dbBase);
            // Dürfte nicht passieren!
            if (dbBase.CreatedByUserId == null)
            {
                return new BadRequestObjectResult("Error: No CreationUser found.");
            }

            baseViewModel.CreationUser = UnitOfWork.Users.GetAccountById(dbBase.CreatedByUserId).UserName;

            // Serialize and return the response
            var json = JsonConvert.SerializeObject(baseViewModel, serializerSettings);
            return new OkObjectResult(json);
        }

        /// <summary>
        /// Get overview of the VPN config files for the firewall.
        /// </summary>
        /// <param name="customerId">Id of the Customer</param>
        /// <param name="baseId">Details from Base</param>
        /// <returns>text/plain</returns>
        [HttpGet]
        [Authorize(Policy = Constants.Roles.Customer)]
        [Route("{baseId}/vpnconfig")]
        public IActionResult GetVpnConfigs([FromRoute] string customerId, [FromRoute] string baseId)
        {
            List<FirewallConfigViewModel> fwConfigs = new List<FirewallConfigViewModel>();
            fwConfigs.Add(new FirewallConfigViewModel() { Id = "1", FirmwareVersion = "5.04", Model = "Fortigate60E" });
            fwConfigs.Add(new FirewallConfigViewModel() { Id = "2", FirmwareVersion = "10.30", Model = "Lancom 1781AW" });
            fwConfigs.Add(new FirewallConfigViewModel() { Id = "3", FirmwareVersion = "5.04", Model = "Fortigate60E PXEPi" });
            // Serialize and return the response
            var json = JsonConvert.SerializeObject(fwConfigs, serializerSettings);
            return new OkObjectResult(json);
        }

        /// <summary>
        /// Get the specific VPN config file for the firewall.
        /// </summary>
        /// <param name="customerId">Id of the Customer</param>
        /// <param name="baseId">Details from Base</param>
        /// <param name="configId">Id of config-file</param>
        /// <returns>text/plain</returns>
        [HttpGet]
        [Authorize(Policy = Constants.Roles.Customer)]
        [Route("{baseId}/vpnconfig/{configId}")]
        public IActionResult GetVpnConfig([FromRoute] string customerId, [FromRoute] string baseId, [FromRoute] string configId)
        {
            try
            {
                Stream resource;
                string fileName = string.Empty;
                string config = string.Empty;
                var utf8WithoutBom = new UTF8Encoding(false);
                var assembly = typeof(Program).GetTypeInfo().Assembly;

                switch (configId)
                {
                    case "1":
                        resource = assembly.GetManifestResourceStream("WPM_API.Resources.Fortigate60E.conf");
                        fileName = "fgt_system.conf";
                        break;
                    case "3":
                        resource = assembly.GetManifestResourceStream("WPM_API.Resources.Fortigate60EPXE.conf");
                        fileName = "fgt_system.conf";
                        break;
                    default:
                        resource = assembly.GetManifestResourceStream("WPM_API.Resources.Lancom1781AW.lcf");
                        fileName = "Bitstream_Lancom1781.lcf";
                        break;
                }

                using (StreamReader reader = new StreamReader(resource, utf8WithoutBom))
                {
                    config = reader.ReadToEnd();
                }

                var vpn = UnitOfWork.Bases.Get(baseId, "Vpn").Vpn;
                IPNetwork localnetwork = IPNetwork.Parse(vpn.LocalAddressRange);
                if (vpn.VirtualNetwork == null)
                {
                    return BadRequest("Base not successfully created.");
                }

                IPNetwork virtualnetwork = IPNetwork.Parse(vpn.VirtualNetwork);

                // VPN
                if (string.IsNullOrEmpty(vpn.SharedKey))
                {
                    return BadRequest("No Shared Key found.");
                }

                config = config.Replace("$PSKSECRET", vpn.SharedKey);
                // LOCAL
                config = config.Replace("$DEFAULTGWMASKIP4", localnetwork.Netmask.MapToIPv4().ToString());
                config = config.Replace("$DEFAULTGATEWAY", localnetwork.FirstUsable.MapToIPv4().ToString());
                config = config.Replace("$LOCALNETMASK", localnetwork.Netmask.ToString());
                config = config.Replace("$LOCALMASKIP4", localnetwork.Netmask.MapToIPv4().ToString());
                config = config.Replace("$LOCALIP4", localnetwork.Network.MapToIPv4().ToString());
                config = config.Replace("$LOCALSTART", localnetwork.FirstUsable.MapToIPv4().ToString()); // Todo: *.100
                config = config.Replace("$LOCALEND", localnetwork.LastUsable.MapToIPv4().ToString()); // Todo: *.199
                // VIRTUAL
                config = config.Replace("$REMOTEGATEWAY", vpn.VirtualPublicIp);
                config = config.Replace("$VIRTUALNETMASK", virtualnetwork.Netmask.ToString());
                config = config.Replace("$VIRTUALMASKIP4", virtualnetwork.Netmask.MapToIPv4().ToString());
                config = config.Replace("$VIRTUALIP4", virtualnetwork.Network.MapToIPv4().ToString());


                // Response
                System.Net.Mime.ContentDisposition cd = new System.Net.Mime.ContentDisposition
                {
                    FileName = fileName,
                    Inline = false // false = prompt the user for downloading;  true = browser to try to show the file inline
                };
                Response.Headers.Add("Content-Disposition", cd.ToString());
                Response.Headers.Add("Access-Control-Expose-Headers", "Content-Disposition");

                return File(utf8WithoutBom.GetBytes(config), "text/plain", fileName);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Add or Modify a advanced Base Parameter.
        /// </summary>
        /// <param name="customerId">Id of the Customer</param>
        /// <param name="baseId">Details from Base</param>
        /// <returns>text/plain</returns>
        [HttpPost]
        [Authorize(Policy = Constants.Roles.Customer)]
        [Route("{baseId}/advanced")]
        public IActionResult AddOrEditParameter([FromRoute] string customerId, [FromRoute] string baseId, [FromBody] AdvancedPropertyAddViewModel advancedProperty)
        {
            DATA.Category none = GetCategory("None");
            using (var unitOfWork = CreateUnitOfWork())
            {
                var dbBase = unitOfWork.Bases.Get(baseId, "Properties");
                DATA.AdvancedProperty property = dbBase.Properties.Find(x => x.Name.Equals(advancedProperty.Name));
                if (property == null)
                {
                    property = new DATA.AdvancedProperty() { Name = advancedProperty.Name, isEditable = true };
                    dbBase.Properties.Add(property);
                }

                property.Value = advancedProperty.Value;
                unitOfWork.Bases.MarkForUpdate(dbBase, GetCurrentUser().Id);
                unitOfWork.SaveChanges();

                property.CategoryId = none.Id;
                unitOfWork.AdvancedProperties.MarkForUpdate(property);
                unitOfWork.SaveChanges();

                AdvancedPropertyViewModel result = Mapper.Map<AdvancedPropertyViewModel>(property);
                result.Category = new CategoryViewModel() { Name = property.Category.Name, Id = property.Category.Id };
                var json = JsonConvert.SerializeObject(result, serializerSettings);
                return new OkObjectResult(json);
            }
        }

        private async Task CreateBaseAsync(string customerId, string baseId)
        {
            DATA.Base baseAdd = UnitOfWork.Bases.Get(baseId, "VirtualMachines", "VirtualMachines.Disks", "Subscription", "ResourceGroup", "VirtualNetwork", "VirtualNetwork.Subnets", "StorageAccount", "Vpn");
            DATA.CloudEntryPoint credentials = GetCEP(customerId);
            AzureCommunicationService azure = new AzureCommunicationService(credentials.TenantId, credentials.ClientId, credentials.ClientSecret);
            try
            {
                using (var unitOfWork = CreateUnitOfWork())
                {
                    // Get subscription
                    string subscriptionId = baseAdd.Subscription.AzureId;
                    var subscription = (azure.SubscriptionService()).GetSubscription(subscriptionId);
                    if (subscription == null)
                    {
                        baseAdd.Status = "Error: Subscription not found.";
                        try
                        {
                            unitOfWork.Bases.MarkForUpdate(baseAdd, GetCurrentUser().Id);
                            unitOfWork.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            string message = ex.Message;
                        }

                        return;
                    }

                    // Create or get ResourceGroup
                    baseAdd.Status = "Adding ResourceGroup.";
                    unitOfWork.Bases.MarkForUpdate(baseAdd, GetCurrentUser().Id);
                    unitOfWork.SaveChanges();
                    var resourceGroup = await azure.ResourceGroupService().AddResourceGroup(subscriptionId, new Microsoft.Azure.Management.ResourceManager.Models.ResourceGroup(baseAdd.ResourceGroup.Location, null, baseAdd.ResourceGroup.Name));
                    string resourceGroupName = baseAdd.ResourceGroup.Name;
                    baseAdd.ResourceGroup.AzureSubscriptionId = subscriptionId;
                    baseAdd.ResourceGroup.CustomerId = customerId;

                    // Create Virtual Network
                    VirtualNetworkAddViewModel vnvm = Mapper.Map<VirtualNetworkAddViewModel>(baseAdd.VirtualNetwork);
                    if (String.IsNullOrEmpty(baseAdd.VirtualNetwork.Dns))
                    {
                        baseAdd.VirtualNetwork.Dns = "Azure DNS";
                    }

                    baseAdd.Status = "Adding Virtual Network.";
                    unitOfWork.Bases.MarkForUpdate(baseAdd, GetCurrentUser().Id);
                    unitOfWork.SaveChanges();
                    var virtualNetwork = azure.VirtualNetworkService().AddOrModifyVirtualNetworkAsync(subscriptionId, resourceGroupName, Mapper.Map<AZURE.VirtualNetworkAddOrEditViewModel>(vnvm), baseAdd.ResourceGroup.Location);

                    // Parallel execution
                    List<Task> awaitables = new List<Task>();

                    // Create Storage Account
                    string storageAccountId = String.Empty;

                    var checkStorageAccount = await azure.StorageService().CheckNameAvailabilityAsync(baseAdd.StorageAccount.Name);
                    if (checkStorageAccount.IsAvailable == true)
                    {
                        baseAdd.Status = "Adding Storage Account.";
                        unitOfWork.Bases.MarkForUpdate(baseAdd, GetCurrentUser().Id);
                        unitOfWork.SaveChanges();
                        var storageAccountAzure = await azure.StorageService().AddStorageAccountAsync(subscriptionId, resourceGroupName, baseAdd.StorageAccount.Name, baseAdd.StorageAccount.Type, baseAdd.ResourceGroup.Location, "Storage V2");
                        storageAccountId = storageAccountAzure.Id;
                        baseAdd.StorageAccount.AzureId = storageAccountId;
                        baseAdd.StorageAccount.CustomerId = customerId;
                        baseAdd.StorageAccount.ResourceGroupId = baseAdd.ResourceGroup.Id;
                        unitOfWork.Bases.MarkForUpdate(baseAdd, GetCurrentUser().Id);
                        unitOfWork.SaveChanges();
                    }
                    else
                    {
                        try
                        {
                            storageAccountId = (await azure.StorageService().GetStorageAccounts(subscriptionId, resourceGroupName)).First(x => x.Name == baseAdd.StorageAccount.Name).Id;
                        }
                        catch (Exception)
                        {
                            // Error
                            baseAdd.Status = "Error: Storage Account already taken.";
                            unitOfWork.Bases.MarkForUpdate(baseAdd, GetCurrentUser().Id);
                            unitOfWork.SaveChanges();
                            return;
                        }
                    }


                    string virtualNetworkId = (await virtualNetwork).Id;
                    baseAdd.VirtualNetwork.AzureId = virtualNetworkId;

                    // Create VPN Connections
                    baseAdd.Status = "Adding VPN.";
                    unitOfWork.Bases.MarkForUpdate(baseAdd, GetCurrentUser().Id);
                    unitOfWork.SaveChanges();

                    awaitables.Add(AddVpnAsync(azure, baseId, customerId, subscriptionId, resourceGroupName, virtualNetworkId));

                    FileRepository.FileRepository tempRepository = new FileRepository.FileRepository(connectionStrings.FileRepository, appSettings.FileRepositoryFolder);
                    Task.WaitAll(awaitables.ToArray());

                    // Fill StorageAccountId after awaitables have been executed.
                    var storageAccounts = await azure.StorageService().GetStorageAccounts(subscriptionId, resourceGroupName);
                    storageAccountId = storageAccounts.FirstOrDefault(x => x.Name == baseAdd.StorageAccount.Name).Id;
                    baseAdd.StorageAccount.AzureId = storageAccountId;
                    baseAdd.Status = "Base successfully created.";
                    unitOfWork.Bases.MarkForUpdate(baseAdd, GetCurrentUser().Id);
                    unitOfWork.SaveChanges();
                    var dbBase = UnitOfWork.Bases.Get(baseId, "Properties");
                    var FixedParams = GenerateFixedBaseParams(dbBase);
                    foreach (var param in FixedParams)
                    {
                        param.BaseId = dbBase.Id;
                        param.Base = dbBase;
                        dbBase.Properties.Add(param);
                    }

                    UnitOfWork.Bases.MarkForUpdate(dbBase);
                    UnitOfWork.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                using (var unitOfWork = CreateUnitOfWork())
                {
                    var dbBase = UnitOfWork.Bases.Get(baseId);
                    dbBase.Status = ex.Message;
                    UnitOfWork.Bases.MarkForUpdate(dbBase);
                    UnitOfWork.SaveChanges();
                }
            }

            return;
        }


        private async Task AddVpnAsync(AzureCommunicationService azure, string baseId, string customerId, string subscriptionId, string resourceGroupName, string virtualNetworkId)
        {
            AZURE.VPN vpn;
            using (var unitOfWork = CreateUnitOfWork())
            {
                var dbBase = unitOfWork.Bases.Get(baseId, "Vpn", "BaseStatus");

                AZURE.VpnAdd vpnModel = new AZURE.VpnAdd();
                vpnModel.CustomerId = customerId;
                vpnModel.ResourceGroupName = resourceGroupName;
                vpnModel.SubscriptionId = subscriptionId;
                vpnModel.VirtualNetworkId = virtualNetworkId;
                vpnModel.Name = dbBase.Vpn.Name;
                vpnModel.Local = new AZURE.VPNLocalProperties() { AddressRange = dbBase.Vpn.LocalAddressRange, PublicIp = dbBase.Vpn.LocalPublicIp };
                vpn = await (azure.VPNService()).AddVPN(subscriptionId, resourceGroupName, vpnModel);
                var updateBase = unitOfWork.Bases.Get(baseId, "Vpn");
                updateBase.Vpn.VirtualNetwork = vpn.Virtual.VirtualNetwork;
                updateBase.Vpn.VirtualPublicIp = vpn.Virtual.PublicIp;
                updateBase.Status += " VPN added.";
                updateBase.BaseStatus.VPNStatus = "Created";
                updateBase.BaseStatus.Status = "Created";
                updateBase.Vpn.SharedKey = vpn.SharedKey;
                unitOfWork.Bases.MarkForUpdate(updateBase);
                unitOfWork.SaveChanges();
            }
        }

        private DATA.Category GetCategory(string name)
        {
            DATA.Category category = UnitOfWork.Categories.GetAll().FirstOrDefault(x => x.Name == name && x.Type == DATA.CategoryType.AdvancedProperty);

            if (category == null)
            {
                category = new DATA.Category { Name = name, Type = DATA.CategoryType.AdvancedProperty };
            }
            return category;
        }

        private HashSet<DATA.AdvancedProperty> GenerateFixedBaseParams(DATA.Base b)
        {
            var FixedParams = new HashSet<DATA.AdvancedProperty>();
            // Parameters from BaseDetails are categorized via the wizard steps General -> Network -> VPN, None for all others
            DATA.Category none = GetCategory("None");
            DATA.Category general = GetCategory("General");
            DATA.Category network = GetCategory("Network");
            DATA.Category vpn = GetCategory("VPN");

            //FixedParams.Add(new DATA.AdvancedProperty { Name = "$DataDriveLetter", Value = null, isEditable = true, Category = none });
            //FixedParams.Add(new DATA.AdvancedProperty { Name = "$CSDPpath", Value = null, isEditable = true, Category = none });
            FixedParams.Add(new DATA.AdvancedProperty { Name = "$ESDPpath", Value = null, isEditable = true, Category = none });
            //FixedParams.Add(new DATA.AdvancedProperty { Name = "$LocAdminPW", Value = null, isEditable = true, Category = none });
            FixedParams.Add(new DATA.AdvancedProperty { Name = "$NetBIOSDomainName", Value = null, isEditable = true, Category = none });
            FixedParams.Add(new DATA.AdvancedProperty { Name = "$TLD", Value = null, isEditable = true, Category = none });
            FixedParams.Add(new DATA.AdvancedProperty { Name = "$ADDomainMode", Value = null, isEditable = true, Category = none });
            FixedParams.Add(new DATA.AdvancedProperty { Name = "$ADForrestMode", Value = null, isEditable = true, Category = none });
            FixedParams.Add(new DATA.AdvancedProperty { Name = "$DSRMpw", Value = null, isEditable = true, Category = none });
            //FixedParams.Add(new DATA.AdvancedProperty { Name = "$CSDPserverName", Value = null, isEditable = true, Category = none });
            //FixedParams.Add(new DATA.AdvancedProperty { Name = "$CSDPshareName", Value = null, isEditable = true, Category = none });
            FixedParams.Add(new DATA.AdvancedProperty { Name = "$DNSForwarder", Value = null, isEditable = true, Category = none });
            FixedParams.Add(new DATA.AdvancedProperty { Name = "$firstSiteName", Value = null, isEditable = true, Category = none });
            FixedParams.Add(new DATA.AdvancedProperty { Name = "$gatewayIPAddress", Value = null, isEditable = true, Category = none });
            FixedParams.Add(new DATA.AdvancedProperty { Name = "$ADjoinServer", Value = null, isEditable = true, Category = none });
            FixedParams.Add(new DATA.AdvancedProperty { Name = "$ADjoinAccount", Value = null, isEditable = true, Category = none });
            FixedParams.Add(new DATA.AdvancedProperty { Name = "$ADjoinAccountPW", Value = null, isEditable = true, Category = none });
            FixedParams.Add(new DATA.AdvancedProperty { Name = "$ADjoinOU", Value = null, isEditable = true, Category = none });

            // Base Details, isEditable false by default
            FixedParams.Add(new DATA.AdvancedProperty { Name = "$subscriptionName", Value = b.Subscription?.Name, Category = general });
            FixedParams.Add(new DATA.AdvancedProperty { Name = "$resourceGroupName", Value = b.ResourceGroup?.Name, Category = general });
            FixedParams.Add(new DATA.AdvancedProperty { Name = "$locationName", Value = b.ResourceGroup?.Location, Category = general });
            FixedParams.Add(new DATA.AdvancedProperty { Name = "$storageAccountName", Value = b.StorageAccount?.Name, Category = general });
            FixedParams.Add(new DATA.AdvancedProperty { Name = "$storageAccountType", Value = b.StorageAccount?.Type, Category = general });
            FixedParams.Add(new DATA.AdvancedProperty { Name = "$virtualNetworkName", Value = b.VirtualNetwork?.Name, Category = network });
            FixedParams.Add(new DATA.AdvancedProperty { Name = "$virtualAddressRange", Value = b.VirtualNetwork?.AddressRange, Category = network });

            // DNS will be changed when Branch Cache Server is installed but for now stays on Azure DNS in our database. -> No edit functionality implemented.
            //FixedParams.Add(new DATA.AdvancedProperty { Name = "$VirtualNetworkDNS", Value = b.VirtualNetwork?.Dns, Category = network });
            b.VirtualNetwork.Subnets.ForEach(s => FixedParams.Concat(new List<DATA.AdvancedProperty>
            {
                new DATA.AdvancedProperty {Name = "SubnetName-" + s?.Name, Value = s?.Name, Category = network},
                new DATA.AdvancedProperty {Name = "SubnetAddressRange-" + s?.Name, Value = s?.AddressRange, Category = network}
            }));
            FixedParams.Add(new DATA.AdvancedProperty { Name = "$VPNName", Value = b.Vpn?.Name, Category = vpn });
            FixedParams.Add(new DATA.AdvancedProperty { Name = "localAddressRange", Value = b.Vpn?.LocalAddressRange, Category = vpn });
            FixedParams.Add(new DATA.AdvancedProperty { Name = "$localPublicIP", Value = b.Vpn?.LocalPublicIp, Category = vpn });
            FixedParams.Add(new DATA.AdvancedProperty { Name = "$virtualPublicIP", Value = b.Vpn?.VirtualPublicIp, Category = vpn });

            // Read from azure gateway
            FixedParams.Add(new DATA.AdvancedProperty { Name = "$localNetworkGatewayName", Value = "TODO", Category = vpn });
            return FixedParams;
        }

        [HttpGet]
        [Route("{baseId}/match-params")]
        [Authorize(Policy = Constants.Policies.Customer)]
        public IActionResult GetBaseParamSelection([FromBody] ParameterNames parameters, [FromRoute] string baseId)
        {
            DATA.Base fetchedBase = UnitOfWork.Bases.Get(baseId, "Properties");
            ParameterKeyValueModel result = new ParameterKeyValueModel();
            foreach (DATA.AdvancedProperty advancedProp in fetchedBase.Properties)
            {
                if (parameters.ParamNames.Contains(advancedProp.Name))
                {
                    result.Parameters.Add(new ParameterModel(advancedProp.Name, advancedProp.Value));
                }
            }

            var json = JsonConvert.SerializeObject(result, serializerSettings);
            return new OkObjectResult(json);
        }
    }
}