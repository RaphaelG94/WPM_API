using System;
using System.Threading.Tasks;
using WPM_API.Controllers;
using WPM_API.Models;
using  WPM_API.Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Management.ResourceManager.Models;
using AZURE =  WPM_API.Azure.Models;
using Newtonsoft.Json;
using System.Linq;
using WPM_API.Common;
using WPM_API.Data.DataContext.Entities;
using WPM_API.Common.Logs;

[Route("domains")]
public class DomainController : BasisController
{
    public DomainController()
    {

    }

    //[HttpPost]
    //[Authorize(Policy = Constants.Roles.Customer)]
    //public async Task<IActionResult> AddDomain([FromBody] DomainAddViewModel domainViewModel)
    //{
    //    string customerId = domainViewModel.CustomerId;
    //    string subscriptionId;
    //    string resourceGroupName;
    //    string virtualNetworkId;
    //    string storageAccountId;

    //    AzureCredentials credentials = GetCurrentAzureCredentials(customerId);
    //    AzureCommunicationService azure = new AzureCommunicationService(credentials.TenantId, credentials.ClientId, credentials.ClientSecret);

    //    Domain domain = Mapper.Map<DomainAddViewModel, Domain>(domainViewModel);
    //    try
    //    {
    //        domain.Customer = UnitOfWork.Customers.Get(customerId);
    //        domain.Status = "Creating";
    //        UnitOfWork.Domains.MarkForInsert(domain);
    //        UnitOfWork.SaveChanges();
    //    }
    //    catch (Exception ex)
    //    {
    //        LogHolder.MainLog.Error(ex);
    //        return BadRequest(ex.Message);
    //    }

    //    // Create Admin-User
    //    domainViewModel.VirtualMachine.Admin = new AdminCredentials("bitstream", "Windows2000!");

    //    // Get subscription
    //    var subscription = await azure.GetSubscriptionAsync(domainViewModel.SubscriptionId);
    //    if (subscription == null)
    //    {
    //        return BadRequest("Subscription not found.");
    //    }
    //    subscriptionId = subscription.SubscriptionId;
    //    // Create or get ResourceGroup
    //    var resourceGroup = await azure.AddResourceGroupAsync(subscriptionId, new ResourceGroup(domainViewModel.ResourceGroup.Location, null, domainViewModel.ResourceGroup.ResourceGroupName));
    //    resourceGroupName = resourceGroup.Name;
    //    domain.Status = "Creating: ResourceGroup added.";
    //    UnitOfWork.SaveChanges();
    //    // Create Virtual Network
    //    var virtualNetwork = azure.AddOrModifyVirtualNetworkAsync(subscriptionId, resourceGroupName, Mapper.Map<AZURE.VirtualNetworkAddOrEditViewModel>(domainViewModel.VirtualNetwork));
    //    // Create Storage Account
    //    var checkStorageAccount = await azure.StorageAccountExistsAsync(domainViewModel.StorageAccount.Name);
    //    if (checkStorageAccount.IsAvailalbe == true)
    //    {
    //        storageAccountId = (await azure.AddStorageAccountAsync(subscriptionId, resourceGroupName, domainViewModel.StorageAccount.Name, Enum.GetName(typeof(StorageType), domainViewModel.StorageAccount.Type))).Id;
    //        domain.Status = "Creating: StorageAccount added.";
    //        UnitOfWork.SaveChanges();
    //    }
    //    else
    //    {
    //        var storageAccount = (await azure.GetStorageAccountsAsync(subscriptionId, resourceGroupName)).Where(x => x.Name == domainViewModel.StorageAccount.Name).First();
    //        if (storageAccount == null)
    //        {
    //            return BadRequest(checkStorageAccount.Message);
    //        }
    //        else
    //        {
    //            storageAccountId = storageAccount.Id;
    //        }
    //    }

    //    // Create VPN Connections
    //    virtualNetworkId = (await virtualNetwork).Id;
    //    domain.Status = "Creating: Virtual Network added.";
    //    UnitOfWork.SaveChanges();
    //    var vpnModel = Mapper.Map<AZURE.VpnAdd>(domainViewModel.Vpn);
    //    vpnModel.CustomerId = customerId;
    //    vpnModel.ResourceGroupName = resourceGroupName;
    //    vpnModel.SubscriptionId = subscriptionId;
    //    vpnModel.VirtualNetworkId = virtualNetworkId;
    //    var vpn = azure.AddVPNAsync(subscriptionId, resourceGroupName, vpnModel);

    //    // Create VirtualMachines
    //    var vmModel = Mapper.Map<AZURE.VirtualMachineAddModel>(domainViewModel.VirtualMachine);
    //    vmModel.CustomerId = customerId;
    //    vmModel.Location = resourceGroup.Location;
    //    vmModel.Network = new AZURE.VirtualMachineNetworkAdd(virtualNetworkId, domainViewModel.VirtualMachine.Subnet)
    //    {
    //        VirtualNetworkId = virtualNetworkId
    //    };
    //    vmModel.ResourceGroupName = resourceGroupName;
    //    vmModel.StorageAccountId = storageAccountId;
    //    vmModel.SubscriptionId = subscriptionId;

    //    // Create VM with first Script as CSE
    //    AZURE.VirtualMachineModel vm = new AZURE.VirtualMachineModel();
    //    try
    //    {
    //        vm = await azure.AddVirtualMachineAsync(vmModel, Scripts.ConfigureAD + ".ps1", GetArguments(Scripts.ConfigureAD, domainViewModel));
    //        domain.Status = "Creating: AD added.";
    //        UnitOfWork.SaveChanges();
    //    }
    //    catch (Exception ex)
    //    {
    //        LogHolder.MainLog.Error(ex);
    //        domain.Status = "Error: Add AD Failure: " + ex.Message;
    //        UnitOfWork.SaveChanges();
    //        return BadRequest(ex.Message);
    //    }

    //    // After restart - run second script
    //    try
    //    {
    //        // Mount InstShare:
    //        await azure.AddInstShareAsync(Mapper.Map<AZURE.VirtualMachineRefModel>(vm));
    //        domain.Status = "Creating: InstShare Mounted.";
    //        UnitOfWork.SaveChanges();
    //        await azure.ExecuteVirtualMachineScriptAsync(Mapper.Map<AZURE.VirtualMachineRefModel>(vm), Scripts.Configure_DNS_CentralStore_DHCP + ".ps1", GetArguments(Scripts.Configure_DNS_CentralStore_DHCP, domainViewModel));
    //        domain.Status = "Creating: Configure_DNS_CentralStore_DHCP added.";
    //        UnitOfWork.SaveChanges();
    //        await azure.ExecuteVirtualMachineScriptAsync(Mapper.Map<AZURE.VirtualMachineRefModel>(vm), Scripts.GPO + ".ps1", GetArguments(Scripts.GPO, domainViewModel));
    //        domain.Status = "Creating: GPO added.";
    //        UnitOfWork.SaveChanges();
    //        await azure.ExecuteVirtualMachineScriptAsync(Mapper.Map<AZURE.VirtualMachineRefModel>(vm), Scripts.OU_Structure + ".ps1", GetArguments(Scripts.OU_Structure, domainViewModel));
    //        domain.Status = "Creating: OU_Structure added.";
    //        UnitOfWork.SaveChanges();
    //        await azure.ExecuteVirtualMachineScriptAsync(Mapper.Map<AZURE.VirtualMachineRefModel>(vm), Scripts.SetUser + ".ps1", GetArguments(Scripts.SetUser, domainViewModel));
    //        domain.Status = "Creating: SetUser added.";
    //        UnitOfWork.SaveChanges();
    //        await azure.ExecuteVirtualMachineScriptAsync(Mapper.Map<AZURE.VirtualMachineRefModel>(vm), Scripts.WDS + ".ps1", GetArguments(Scripts.WDS, domainViewModel));
    //        domain.Status = "Creating: WDS added.";
    //        UnitOfWork.SaveChanges();
    //    }
    //    catch (Exception ex)
    //    {
    //        LogHolder.MainLog.Error(ex);
    //        domain.Status += "Error: " + ex.Message;
    //        UnitOfWork.SaveChanges();
    //        return BadRequest(ex.Message);
    //    }

    //    // After Creation: Save Domain Object status
    //    domain.Status = "Done";
    //    UnitOfWork.SaveChanges();

    //    var json = JsonConvert.SerializeObject(domain, _serializerSettings);
    //    return new OkObjectResult(json);
    //}

    //private string GetArguments(Scripts script, DomainAddViewModel domainViewModel)
    //{
    //    //PasswordGenerator pwg = new PasswordGenerator(true, true, true, true, 6, 8);

    //    #region default-values
    //    string SMADMINPW = domainViewModel.VirtualMachine.Admin.Password ?? "Windows2000!";
    //    string FORRESTMODE = "Win2012R2";
    //    string DOMAINMODE = "Win2012R2";
    //    string DATABASEPATH = "C:\\NTDS";
    //    string SYSVOLPATH = "C:\\SYSVOL";

    //    string AdminPW = domainViewModel.VirtualMachine.Admin.Password ?? "Windows2000!";
    //    string DSRMpw = "Windows2000!"; //pwg.Dequeue();
    //    string InstShareSource = "B:\\";
    //    string ADServerIPAddress = "172.18.0.11";
    //    string ADServerIPLenth = "16";
    //    string DHCPDNSDynamicUpdateAccount = "dnsupdater";
    //    string DHCPDNSDynamicUpdateAccountPW = "93cslo(h1f%9hF3lMk7!4";
    //    string DHCP_Scope_Name = "LAN01";
    //    string DHCP_ScopeIP = "172.18.0.0";
    //    string DHCP_DNS_IP = "172.18.0.11";
    //    string DHCP_NTP_IP = "172.18.0.11";
    //    string DHCPScope_Start = "172.18.0.1";
    //    string DHCPScope_End = "172.18.254.254";
    //    string DHCPScope_SNM = "255.255.0.0";
    //    string DHCPExclusion_Start = "172.18.0.1";
    //    string DHCPExclusion_End = "172.18.0.100";
    //    string DHCP_Router_IP = "172.18.0.1";
    //    string JoinWDSAdmin = "WDSjoinAccount";
    //    string JoinWDSAdminPW = "m2!9Ji3f/2aePQx8l";
    //    string WDSRemoteInstallPath = "C:\\RemoteInstall";
    //    string Manufacturer = "Yourdomain GmbH";
    //    string SupportHours = "06:30 - 20:45";
    //    string SupportPhone = "+49 123 12345-789";
    //    string SupportURL = "http://www.yourdomain.net";
    //    string RegisteredOrganization = "Endkunde GmbH";
    //    string RegisteredOwner = "Endkunde GmbH";
    //    string HomePage = "http://www.test.de";
    //    #endregion

    //    switch (script)
    //    {
    //        case Scripts.ConfigureAD:
    //            return "-smadminpw \"" + SMADMINPW
    //                + "\" -ForrestMode \"" + FORRESTMODE
    //                + "\" -DomainMode \"" + DOMAINMODE
    //                + "\" -DomainName \"" + domainViewModel.Domain.Name + "." + domainViewModel.Domain.Tld
    //                + "\" -NetBIOSName \"" + domainViewModel.Domain.Name + "\"  -LDAPDomain \"DC=" + domainViewModel.Domain.Name + ",DC=local"
    //                + "\" -DatabasePath \"" + DATABASEPATH
    //                + "\" -SysvolPath \"" + SYSVOLPATH + "\"";
    //        case Scripts.SetIP_AD_DNS:
    //            // 001_setIP_AD_DNS
    //            return "-ADServerIPAddress \"" + ADServerIPAddress
    //                + "\" -ADServerIPLenth \"" + ADServerIPLenth
    //                + "\" -Domain \"" + domainViewModel.Domain.Name
    //                + "\" -TLD \"" + domainViewModel.Domain.Tld
    //                + "\" -AdminPW \"" + AdminPW
    //                + "\" -DSRMpw \"" + DSRMpw;
    //        case Scripts.Configure_DNS_CentralStore_DHCP:
    //            return "-Domain " + domainViewModel.Domain.Name
    //                + " -TLD " + domainViewModel.Domain.Tld
    //                + " -DHCPDNSDynamicUpdateAccount " + DHCPDNSDynamicUpdateAccount
    //                + " -DHCPDNSDynamicUpdateAccountPW " + DHCPDNSDynamicUpdateAccountPW
    //                + " -InstShareSource " + InstShareSource
    //                + " -DHCP_Scope_Name " + DHCP_Scope_Name
    //                + " -DHCP_ScopeIP " + DHCP_ScopeIP
    //                + " -DHCP_DNS_IP " + DHCP_DNS_IP
    //                + " -DHCP_NTP_IP " + DHCP_NTP_IP
    //                + " -DHCPScope_Start " + DHCPScope_Start
    //                + " -DHCPScope_End " + DHCPScope_End
    //                + " -DHCPScope_SNM " + DHCPScope_SNM
    //                + " -DHCPExclusion_Start " + DHCPExclusion_Start
    //                + " -DHCPExclusion_End " + DHCPExclusion_End
    //                + " -DHCP_Router_IP " + DHCP_Router_IP;
    //        case Scripts.GPO:
    //        case Scripts.OU_Structure:
    //        case Scripts.SetUser:
    //            return "-Domain " + domainViewModel.Domain.Name
    //                + " -TLD " + domainViewModel.Domain.Tld;
    //        case Scripts.WDS:
    //            return "-Domain " + domainViewModel.Domain.Name
    //               + " -TLD " + domainViewModel.Domain.Tld
    //               + " -JoinWDSAdmin " + JoinWDSAdmin
    //               + " -JoinWDSAdminPW " + JoinWDSAdminPW
    //               + " -WDSRemoteInstallPath " + WDSRemoteInstallPath
    //               + " -Manufacturer " + Manufacturer
    //               + " -SupportHours " + SupportHours
    //               + " -SupportPhone " + SupportPhone
    //               + " -SupportURL " + SupportURL
    //               + " -RegisteredOrganization " + RegisteredOrganization
    //               + " -RegisteredOwner " + RegisteredOwner
    //               + " -HomePage " + HomePage;
    //        default:
    //            return "";
    //    }
    //}

    //enum Scripts
    //{
    //    ConfigureAD,
    //    SetIP_AD_DNS,
    //    Configure_DNS_CentralStore_DHCP,
    //    GPO,
    //    OU_Structure,
    //    SetUser,
    //    WDS,
    //}
}