using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Linq.Dynamic.Core;
using WPM_API.Azure;
using WPM_API.Code.Infrastructure;
using WPM_API.Code.Infrastructure.LogOn;
using WPM_API.Common;
using WPM_API.Data.Models;
using WPM_API.Models;
using WPM_API.Options;
using AZURE = WPM_API.Azure.Models;
using DATA = WPM_API.Data.DataContext.Entities;

namespace WPM_API.Controllers.VirtualMachines
{
    [Route("virtual-machines")]
    public class VirtualMachineController : BasisController
    {
        public VirtualMachineController(AppSettings appSettings, ConnectionStrings connectionStrings, OrderEmailOptions orderEmailOptions, AgentEmailOptions agentEmailOptions, SendMailCreds sendMailCreds, SiteOptions siteOptions, ILogonManager logonManager) : base(appSettings, connectionStrings, orderEmailOptions, agentEmailOptions, sendMailCreds, siteOptions, logonManager)
        {
        }

        [HttpPost]
        [Authorize(Policy = Constants.Policies.Customer)]
        public async Task<IActionResult> CreateVirtualMachineAsync([FromBody] VirtualMachineAddViewModel vmData)
        {
            // Fetch BASE and check for existence
            DATA.Base fetchedBase = UnitOfWork.Bases.GetOrNull(vmData.BaseId, BaseIncludes.GetAllIncludes());
            DATA.Customer customer = UnitOfWork.Customers.Get(vmData.CustomerId);
            if (fetchedBase == null)
            {
                return BadRequest("The BASE does not exist");
            }

            // Load subnet
            DATA.Subnet subnet = UnitOfWork.Subnets.Get(vmData.Subnet);

            // Create VM for DB
            DATA.VirtualMachine vm = UnitOfWork.VirtualMachines.CreateEmpty();
            vm.CreatedByUserId = GetCurrentUser().Id;
            vm.CreatedDate = DateTime.Now;
            vm = Mapper.Map<DATA.VirtualMachine>(vmData);
            vm.CurrentCustomerId = vmData.CustomerId;
            if (vmData.Admin == null || vmData.Admin.Username == null)
            {
                vm.AdminUserName = "Bitstream";
            }
            if (vmData.Admin == null || vmData.Admin.Password == null)
            {
                vm.AdminUserPassword = "BitStream2000!";
            }
            else
            {
                vm.AdminUserPassword = vmData.Admin.Password;
            }

            vm.BaseId = vmData.BaseId;
            vm.Base = fetchedBase;
            vm.Disks = new List<DATA.Disk>();
            vm.Disks.Add(new DATA.Disk() { DiskType = DATA.DiskType.SystemDisk, Name = vmData.SystemDisk.Name, SizeInGb = vmData.SystemDisk.SizeInGb.GetValueOrDefault() });
            vm.Disks.Add(new DATA.Disk() { DiskType = DATA.DiskType.AdditionalDisk, Name = vmData.AdditionalDisk.Name, SizeInGb = vmData.AdditionalDisk.SizeInGb.GetValueOrDefault() });
            vm.Status = "Starting creation of VM";
            vm.Subnet = subnet.Name;
            vm.Location = fetchedBase.ResourceGroup.Location;
            vm.SubscriptionName = fetchedBase.Subscription.Name;
            vm.SubscriptionId = fetchedBase.Subscription.AzureId;
            fetchedBase.VirtualMachines.Add(vm);
            UnitOfWork.SaveChanges();


            // Setup connection to Azure
            DATA.CloudEntryPoint credentials = GetCEP(vmData.CustomerId);
            AzureCommunicationService azure = new AzureCommunicationService(credentials.TenantId, credentials.ClientId, credentials.ClientSecret);

            try
            {
                // Get Azure ResourceGroup
                var resourceGrp = await azure.ResourceGroupService().GetResourceGroup(fetchedBase.ResourceGroup.Name, fetchedBase.Subscription.AzureId);


                // Create VM in Azure
                vm.Status = "Creating VM in Azure";
                using (var unitOfWork = CreateUnitOfWork())
                {
                    unitOfWork.VirtualMachines.MarkForUpdate(vm, GetCurrentUser().Id);
                    unitOfWork.SaveChanges();
                }
                string vmAzureId = await AddVirtualMachineAsync(azure, vm, vmData.BaseId, vmData.CustomerId, resourceGrp, fetchedBase.VirtualNetwork.AzureId, fetchedBase.StorageAccount.AzureId, fetchedBase.Subscription.AzureId);
                vm.AzureId = vmAzureId;
                vm.Status = "VM created";
                UnitOfWork.VirtualMachines.MarkForUpdate(vm, GetCurrentUser().Id);
                UnitOfWork.Bases.MarkForUpdate(fetchedBase, GetCurrentUser().Id);
                UnitOfWork.SaveChanges();


                /*
                // Install SmartDeploy
                AZURE.VirtualMachineRefModel virtualMachineRefModel = new AZURE.VirtualMachineRefModel() { SubscriptionId = fetchedBase.Subscription.AzureId, VmId = vmAzureId };
                vm.Status = "Installing SmartDeploy";

                using (var unitOfWork = CreateUnitOfWork())
                {
                    string token = unitOfWork.Token.GenerateToken(vmData.CustomerId, DateTime.Now.AddHours(4));
                    unitOfWork.VirtualMachines.MarkForUpdate(vm, GetCurrentUser().Id);
                    unitOfWork.SaveChanges();

                    // Create Storage
                    ResourcesRepository fileRepo = new ResourcesRepository(connectionStrings.FileRepository, appSettings.TempFolder);
                    var tempStorageCredentials = new AZURE.StorageCredentialModel()
                    {
                        ScriptAzureStoragePath = appSettings.AzureStoragePath + appSettings.TempFolder + "/",
                        ScriptStorageAccountKey = appSettings.StorageAccountKey,
                        ScriptStorageAccountName = appSettings.StorageAccountName
                    };
                    var assembly = Assembly.Load("WPM_API");
                    var scriptcontent = assembly.GetManifestResourceStream("WPM_API.Resources.ScriptFolder.SmartDeployNoTouchInstaller.ps1");
                    string scriptname = Guid.NewGuid().ToString().Replace("-", "") + ".ps1";
                    await fileRepo.UploadFile(scriptname, scriptcontent);
                    var url = "https://bitstreamrelease.azurewebsites.net/";

                    // Use the token to execute installer
                    await azure.VirtualMachineService().ExecuteVirtualMachineScriptAsync(tempStorageCredentials, virtualMachineRefModel, scriptname, customer.Name + " " + vmData.Name + " "  + url + " " + "");
                    await fileRepo.DeleteFile(scriptname);
                
                    */
                vm.Status = "VM ready";
                using (var unitOfWork = CreateUnitOfWork())
                {
                    unitOfWork.VirtualMachines.MarkForUpdate(vm, GetCurrentUser().Id);
                    unitOfWork.SaveChanges();
                }
                // Serialize and return result
                var json = JsonConvert.SerializeObject(Mapper.Map<DATA.VirtualMachine, VirtualMachineViewModel>(vm), serializerSettings);
                return new OkObjectResult(json);
            }
            catch (Exception e)
            {
                vm.Status = "ERROR: " + e.Message;
                using (var unitOfWork = CreateUnitOfWork())
                {
                    unitOfWork.VirtualMachines.MarkForUpdate(vm, GetCurrentUser().Id);
                    unitOfWork.SaveChanges();
                }
                return BadRequest("The virtual machine could not be created. " + e.Message + " " + e.InnerException.Message);
            }

        }

        private async Task<string> AddVirtualMachineAsync(AzureCommunicationService azure, DATA.VirtualMachine vm, string baseId, string customerId, Microsoft.Azure.Management.ResourceManager.Models.ResourceGroup resourceGroup, string virtualNetworkId, string storageAccountId, string subscriptionId)
        {
            var vmvm = Mapper.Map<VirtualMachineAddViewModel>(vm);
            var vmModel = Mapper.Map<AZURE.VirtualMachineAddModel>(vmvm);
            vmModel.CustomerId = customerId;
            vmModel.Location = resourceGroup.Location;
            vmModel.Network = new AZURE.VirtualMachineNetworkAdd(virtualNetworkId, vm.Subnet)
            {
                VirtualNetworkId = virtualNetworkId
            };
            vmModel.ResourceGroupName = resourceGroup.Name;
            vmModel.StorageAccountId = storageAccountId;
            vmModel.SubscriptionId = subscriptionId;
            vmModel.Admin.Password = vm.AdminUserPassword;
            vmModel.Admin.Username = vm.AdminUserName;
            vmModel.AdditionalDisk = new AZURE.DiskAdd(vm.Disks.Where(x => x.DiskType.Equals(DATA.DiskType.AdditionalDisk)).First().Name, vm.Disks.Where(x => x.DiskType.Equals(DATA.DiskType.AdditionalDisk)).First().SizeInGb);

            AZURE.VirtualMachineModel azureVM = await azure.VirtualMachineService().AddVirtualMachinesAsync(vmModel);
            string azureid = string.Empty;
            using (var unitOfWork = CreateUnitOfWork())
            {
                var dbBase = unitOfWork.Bases.Get(baseId, "VirtualMachines");

                var dbVm = dbBase.VirtualMachines.Where(x => x.Id == vm.Id).First();
                azureid = dbVm.AzureId = azureVM.Id;
                dbVm.LocalIp = azureVM.LocalIp;
                dbVm.Location = vmModel.Location;
                unitOfWork.Bases.MarkForUpdate(dbBase, GetCurrentUser().Id);
                unitOfWork.SaveChanges();

            }

            return azureid;
        }

        [Route("{customerId}")]
        [HttpGet]
        [Authorize(Policy = Constants.Policies.Customer)]
        public IActionResult GetCustomersVMs([FromRoute] string customerId)
        {
            List<DATA.VirtualMachine> vms = new List<DATA.VirtualMachine>();
            vms = UnitOfWork.VirtualMachines.GetAll().Where(x => x.CurrentCustomerId == customerId).ToList();
            var json = JsonConvert.SerializeObject(Mapper.Map<List<DATA.VirtualMachine>, List<VirtualMachineViewModel>>(vms), serializerSettings);
            return new OkObjectResult(json);
        }

        [Route("{customerId}/{baseId}/{diskName}")]
        [HttpGet]
        [Authorize(Policy = Constants.Policies.Customer)]
        public async Task<IActionResult> CheckDiskNamesAsync([FromRoute] string customerId, [FromRoute] string baseId, [FromRoute] string diskName)
        {
            DATA.Base fetchedBase = UnitOfWork.Bases.Get(baseId, "Subscription");
            string subscriptionAzureId = fetchedBase.Subscription.AzureId;
            DATA.CloudEntryPoint creds = GetCEP(customerId);
            AzureCommunicationService azure = new AzureCommunicationService(creds.TenantId, creds.ClientId, creds.ClientSecret);
            List<AZURE.VirtualMachineModel> vms = await azure.VirtualMachineService().GetVirtualMachinesAsync(subscriptionAzureId);

            foreach (AZURE.VirtualMachineModel vm in vms)
            {
                if (vm.SystemDisk.Name == diskName)
                {
                    return BadRequest("There already exists a disk named '" + diskName + "'");
                }
            }

            return new OkResult();
        }

        [HttpGet]
        [Route("{customerId}/{baseId}/{vmName}/checkName")]
        public async Task<IActionResult> CheckVMName([FromRoute] string customerId, [FromRoute] string baseId, [FromRoute] string vmName)
        {
            DATA.Base fetchedBase = UnitOfWork.Bases.GetOrNull(baseId, "Subscription");
            if (fetchedBase == null)
            {
                return BadRequest("The base does not exist!");
            }
            string subscriptionAzureId = fetchedBase.Subscription.AzureId;
            DATA.CloudEntryPoint creds = GetCEP(customerId);
            AzureCommunicationService azure = new AzureCommunicationService(creds.TenantId, creds.ClientId, creds.ClientSecret);
            List<AZURE.VirtualMachineModel> vms = await azure.VirtualMachineService().GetVirtualMachinesAsync(subscriptionAzureId);

            foreach (AZURE.VirtualMachineModel vm in vms)
            {
                if (vm.Name == vmName)
                {
                    return BadRequest("There already exists a VM with the name " + vmName);
                }
            }

            return new OkResult();
        }
    }
}
