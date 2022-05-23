using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WPM_API.Code;
using WPM_API.Code.Infrastructure;
using WPM_API.Code.Infrastructure.LogOn;
using WPM_API.Common;
using WPM_API.FileRepository;
using WPM_API.Models;
using WPM_API.Options;
using AZURE = WPM_API.Azure.Models;
using DATA = WPM_API.Data.DataContext.Entities;

namespace WPM_API.Controllers
{
    [Route("customers/{customerId}/bases/{baseId}/domains")]
    public class DomainController : BasisController
    {
        public DomainController(AppSettings appSettings, ConnectionStrings connectionStrings, OrderEmailOptions orderEmailOptions, AgentEmailOptions agentEmailOptions, SendMailCreds sendMailCreds, SiteOptions siteOptions, ILogonManager logonManager) : base(appSettings, connectionStrings, orderEmailOptions, agentEmailOptions, sendMailCreds, siteOptions, logonManager)
        {
        }

        /// <summary>
        /// Executable Scripts for Creating a Domain
        /// </summary>
        private enum Scripts
        {
            JoinDomain,
            Master,
            CreateOU,
            ImportUser,
            ConfigureDNS,
            ConfigureGPO,
            CreateAD,
            AddAzureCSEScript
        }


        /// <summary>
        /// Retrieve all domains without details.
        /// </summary>
        /// <param name="customerId">Id of the Customer</param>
        /// <param name="baseId">Id of the Base</param>
        /// <returns>[DomainRef]</returns>
        [HttpGet]
        [Authorize(Policy = Constants.Roles.Customer)]
        public IActionResult GetDomains([FromRoute] string customerId, [FromRoute] string baseId)
        {
            var dbDomains = UnitOfWork.Domains.GetAll("Gpo", "DomainUsers", "OrganizationalUnits")
                .Where(x => x.CustomerId.Equals(customerId) && x.BaseId.Equals(baseId)).ToList();
            List<DomainRefViewModel> domains = Mapper.Map<List<DomainRefViewModel>>(dbDomains);

            // Serialize and return the response
            var json = JsonConvert.SerializeObject(domains, serializerSettings);
            return new OkObjectResult(json);
        }

        /// <summary>
        /// Create new domain. Script execution on domain creation is commented out for the time being.
        /// </summary>
        /// <param name="customerId">Id of the Customer</param>
        /// <param name="baseId">Id of the Base</param>
        /// <param name="domainAdd">New Domain</param>
        /// <returns>Domain</returns>
        [HttpPost]
        [Authorize(Policy = Constants.Roles.Customer)]
        public async Task<IActionResult> AddDomain([FromRoute] string customerId, [FromRoute] string baseId, [FromBody] DomainAddViewModel domainAdd)
        {
            // Persist Files
            /*
                        FileRef userCSV = await PersistFileAsync(domainAdd.DomainUserCSV);
                        FileRef gpoWallpaper = await PersistFileAsync(domainAdd.Gpo.Wallpaper);
                        FileRef gpoLockscreen = await PersistFileAsync(domainAdd.Gpo.Lockscreen);
            */

            DomainViewModel domain = new DomainViewModel();
            // General
            DATA.Domain newDomain = Mapper.Map<DATA.Domain>(domainAdd);
            newDomain.BaseId = baseId;
            newDomain.CustomerId = customerId;

            // Create AdvancedProperty for BASE
            DATA.Base fetchedBase = null;
            using (var unitOfWork = CreateUnitOfWork())
            {
                fetchedBase = unitOfWork.Bases.Get(baseId, "Properties");
                DATA.AdvancedProperty netBiosName = fetchedBase.Properties.Where(x => x.Name == "$NetBIOSName").First();
                DATA.AdvancedProperty tld = fetchedBase.Properties.Where(x => x.Name == "$TLD").First();
                netBiosName.Value = domainAdd.Domain.Name;
                tld.Value = domainAdd.Domain.Tld;
                unitOfWork.SaveChanges();
            }

            // User
            /*
                        if (userCSV != null)
                        {
                            newDomain.DomainUserCSV = new DATA.File() { Guid = userCSV.Id, Name = userCSV.Name };
                        }
                        else
                        {
                            newDomain.DomainUserCSV = null;
                        }
            */

            // DNS
            /*
                        newDomain.DNS = new List<DATA.DNS>();
                        if (domainAdd.DNS.Forwarders != null)
                        {
                            foreach (var dnsAddForwarder in domainAdd.DNS.Forwarders)
                            {
                                newDomain.DNS.Add(new DATA.DNS() { Forwarder = dnsAddForwarder });
                            }
                        }
            */

            // OU
            /*
                        List<DATA.OrganizationalUnit> ouStructure = Mapper.Map<List<DATA.OrganizationalUnit>>(domainAdd.OrganizationalUnits.OULevels);
                        newDomain.OrganizationalUnits = Mapper.Map<List<DATA.OrganizationalUnit>>(GenerateOURecursive(ouStructure, domainAdd.OrganizationalUnits.OUBaseLevels));
            */

            // GPO
            /*
                        if (domainAdd.Gpo.Settings == null)
                        {
                            domainAdd.Gpo.Settings = GPOSettings.GetStandard();
                        }
                        newDomain.Gpo.Settings = domainAdd.Gpo.Settings;
                        if (gpoWallpaper != null)
                        {
                            newDomain.Gpo.Wallpaper = new DATA.File() { Guid = gpoWallpaper.Id, Name = gpoWallpaper.Name };
                        }
                        else
                        {
                            newDomain.Gpo.Wallpaper = null;
                        }
                        if (gpoLockscreen != null)
                        {
                            newDomain.Gpo.Lockscreen = new DATA.File() { Guid = gpoLockscreen.Id, Name = gpoLockscreen.Name };
                        }
                        else
                        {
                            newDomain.Gpo.Lockscreen = null;
                        }
            */

            // Save Domain
            using (var unitOfWork = CreateUnitOfWork())
            {
                unitOfWork.Domains.MarkForInsert(newDomain, GetCurrentUser().Id);
                unitOfWork.SaveChanges();
                /*
                                if (newDomain.DomainUserCSV != null)
                                {
                                    await SaveDomainUsers(customerId, baseId, newDomain);
                                }
                */
            }
            SetDomainStatus(newDomain.Id, "All information has been saved.", false);
            try
            {
                //await CreateDomainAsync(customerId, baseId, newDomain.Id);
            }
            catch (Exception)
            {
                return BadRequest("Domain could not be created.");
            }

            // Serialize and return the response
            var json = JsonConvert.SerializeObject(GetDomainDetails(newDomain.Id), serializerSettings);
            return new OkObjectResult(json);
        }

        /// <summary>
        /// Retrieve a single domain.
        /// </summary>
        /// <param name="customerId">Id of the Customer</param>
        /// <param name="baseId">Id of the Base</param>
        /// <param name="domainId">Id of the Domain</param>
        /// <returns>Domain</returns>
        [HttpGet]
        [Authorize(Policy = Constants.Roles.Customer)]
        [Route("{domainId}")]
        public IActionResult GetDomainDetail([FromRoute] string customerId, [FromRoute] string baseId, [FromRoute] string domainId)
        {
            DomainViewModel domain = new DomainViewModel();
            var dbDomain = UnitOfWork.Domains.Get(domainId, "Gpo", "DomainUsers", "OrganizationalUnits");
            if (!(dbDomain.BaseId.Equals(baseId) && dbDomain.CustomerId.Equals(customerId)))
            {
                return new BadRequestObjectResult("Domain is in the wrong Base or from the wrong Customer");
            }

            // Serialize and return the response
            var json = JsonConvert.SerializeObject(GetDomainDetails(domainId), serializerSettings);
            return new OkObjectResult(json);
        }


        private DomainViewModel GetDomainDetails(string domainId)
        {
            var dbDomain = UnitOfWork.Domains.Get(domainId, "Gpo", "DomainUsers", "OrganizationalUnits");
            DomainViewModel domainViewModel = Mapper.Map<DomainViewModel>(dbDomain);
            domainViewModel.CreationUser = UnitOfWork.Users.GetAccountById(dbDomain.CreatedByUserId).UserName;
            return domainViewModel;
        }

        /// <summary>
        /// Retrieve parameter of a particular script version.
        /// </summary>
        /// <param name="customerId">Id of the Customer</param>
        /// <param name="baseId">Id of the Base</param>
        /// <param name="domainId">Id of the Domain</param>
        /// <param name="scriptVersionId">Id of the ScriptVersion</param>
        /// <returns>[Parameter]</returns>
        [HttpGet]
        [Authorize(Policy = Constants.Roles.Customer)]
        [Route("{domainId}/script/{scriptVersionId}")]
        public async Task<IActionResult> GetScriptParameters([FromRoute] string customerId, [FromRoute] string baseId, [FromRoute] string domainId, [FromRoute] string scriptVersionId)
        {
            using (var unitOfWork = CreateUnitOfWork())
            {
                List<ParameterViewModel> parameters = new List<ParameterViewModel>();
                // Get Version from DB
                DATA.Script deviceOption = null;
                List<DATA.Script> deviceOptions = unitOfWork.Scripts.GetAll("Versions").ToList();

                DATA.ScriptVersion version = unitOfWork.Scripts.GetAll().SelectMany(x => x.Versions)
                    .First(y => y.Id == scriptVersionId);
                foreach (DATA.Script temp in deviceOptions)
                {
                    if (temp.Versions.Contains(version))
                    {
                        deviceOption = temp;
                        break;
                    }
                }
                // Get Script from FileRepository
                FileRepository.FileRepository repository =
                    new FileRepository.FileRepository(connectionStrings.FileRepository,
                        appSettings.FileRepositoryFolder);
                string script = string.Empty;
                try
                {
                    BlobDownloadResult dlResult = await repository.GetBlobFile(version.ContentUrl).DownloadContentAsync();
                    script = dlResult.Content.ToString();
                }
                catch (Exception)
                {
                    return new NotFoundObjectResult("Script in script-repository not found.");
                }

                // Get Parameter from Script
                parameters = ScriptHelper.GetParametersFromScript(script, deviceOption.OSType);
                var varList = unitOfWork.Bases.Get(baseId, "Properties").Properties;
                foreach (ParameterViewModel p in parameters)
                {
                    var variable = varList.Find(x => x.Name.Equals(p.Key));
                    p.Value = variable != null ? variable.Value : string.Empty;
                }

                // Prefill Parameter from Created Objects
                parameters = PrefillReferences(parameters, baseId, domainId);

                // Serialize and return the response
                var json = JsonConvert.SerializeObject(parameters, serializerSettings);
                return new OkObjectResult(json);
            }
        }

        private List<ParameterViewModel> PrefillReferences(List<ParameterViewModel> parameters, string baseId, string domainId)
        {
            string key;
            key = "SubscriptionName";
            if (parameters.Exists(x => x.Key.Equals(key)))
            {
                ParameterViewModel p = parameters.Find(x => x.Key.Equals(key));
                p.Value = UnitOfWork.Bases.Get(baseId, "Subscription").Subscription.Name;
            }

            key = "BaseName";
            if (parameters.Exists(x => x.Key.Equals(key)))
            {
                ParameterViewModel p = parameters.Find(x => x.Key.Equals(key));
                p.Value = UnitOfWork.Bases.Get(baseId).Name;
            }

            key = "ResourceGroupName";
            if (parameters.Exists(x => x.Key.Equals(key)))
            {
                ParameterViewModel p = parameters.Find(x => x.Key.Equals(key));
                p.Value = UnitOfWork.Bases.Get(baseId, "ResourceGroup").ResourceGroup.Name;
            }

            key = "Location";
            if (parameters.Exists(x => x.Key.Equals(key)))
            {
                ParameterViewModel p = parameters.Find(x => x.Key.Equals(key));
                p.Value = UnitOfWork.Bases.Get(baseId, "ResourceGroup").ResourceGroup.Location;
            }

            key = "Subnet01prefix";
            if (parameters.Exists(x => x.Key.Equals(key)))
            {
                ParameterViewModel p = parameters.Find(x => x.Key.Equals(key));
                p.Value = UnitOfWork.Bases.Get(baseId, "VirtualNetwork", "VirtualNetwork.Subnets").VirtualNetwork
                    .Subnets.FirstOrDefault(x => x.Number.Equals(1)).AddressRange;
            }

            key = "Subnet01name";
            if (parameters.Exists(x => x.Key.Equals(key)))
            {
                ParameterViewModel p = parameters.Find(x => x.Key.Equals(key));
                p.Value = UnitOfWork.Bases.Get(baseId, "VirtualNetwork", "VirtualNetwork.Subnets").VirtualNetwork
                    .Subnets.FirstOrDefault(x => x.Number.Equals(1)).Name;
            }

            key = "Subnet02prefix";
            if (parameters.Exists(x => x.Key.Equals(key)))
            {
                ParameterViewModel p = parameters.Find(x => x.Key.Equals(key));
                p.Value = UnitOfWork.Bases.Get(baseId, "VirtualNetwork", "VirtualNetwork.Subnets").VirtualNetwork
                    .Subnets.FirstOrDefault(x => x.Name.Equals("gatewaysubnet")).AddressRange;
            }

            key = "VnetName";
            if (parameters.Exists(x => x.Key.Equals(key)))
            {
                ParameterViewModel p = parameters.Find(x => x.Key.Equals(key));
                p.Value = UnitOfWork.Bases.Get(baseId, "VirtualNetwork").VirtualNetwork.Name;
            }

            key = "VnetPrefix";
            if (parameters.Exists(x => x.Key.Equals(key)))
            {
                ParameterViewModel p = parameters.Find(x => x.Key.Equals(key));
                p.Value = UnitOfWork.Bases.Get(baseId, "VirtualNetwork").VirtualNetwork.AddressRange;
            }

            key = "DNSserverIP";
            if (parameters.Exists(x => x.Key.Equals(key)))
            {
                ParameterViewModel p = parameters.Find(x => x.Key.Equals(key));
                var vm = UnitOfWork.Domains.Get(domainId, "Servers", "Servers.VirtualMachine").Servers
                    .Find(x => x.Type.Equals(DATA.ServerType.ADController));
                if (vm != null)
                {
                    p.Value = vm.VirtualMachine.LocalIp;
                }
            }

            key = "StorageAccountName";
            if (parameters.Exists(x => x.Key.Equals(key)))
            {
                ParameterViewModel p = parameters.Find(x => x.Key.Equals(key));
                p.Value = UnitOfWork.Bases.Get(baseId, "StorageAccount").StorageAccount.Name;
            }

            key = "StorageType";
            if (parameters.Exists(x => x.Key.Equals(key)))
            {
                ParameterViewModel p = parameters.Find(x => x.Key.Equals(key));
                p.Value = UnitOfWork.Bases.Get(baseId, "StorageAccount").StorageAccount.Type;
            }

            key = "ADserverName";
            if (parameters.Exists(x => x.Key.Equals(key)))
            {
                ParameterViewModel p = parameters.Find(x => x.Key.Equals(key));
                var vm = UnitOfWork.Domains.Get(domainId, "Servers", "Servers.VirtualMachine").Servers
                    .Find(x => x.Type.Equals(DATA.ServerType.ADController));
                if (vm != null)
                {
                    p.Value = vm.VirtualMachine.Name;
                }
            }

            key = "ADSubnet";
            if (parameters.Exists(x => x.Key.Equals(key)))
            {
                ParameterViewModel p = parameters.Find(x => x.Key.Equals(key));
                var vm = UnitOfWork.Domains.Get(domainId, "Servers", "Servers.VirtualMachine").Servers
                    .Find(x => x.Type.Equals(DATA.ServerType.ADController));
                if (vm != null)
                {
                    p.Value = vm.VirtualMachine.Subnet;
                }
            }

            key = "ADServerSize";
            if (parameters.Exists(x => x.Key.Equals(key)))
            {
                ParameterViewModel p = parameters.Find(x => x.Key.Equals(key));
                var vm = UnitOfWork.Domains.Get(domainId, "Servers", "Servers.VirtualMachine").Servers
                    .Find(x => x.Type.Equals(DATA.ServerType.ADController));
                if (vm != null)
                {
                    p.Value = vm.VirtualMachine.Type;
                }
            }

            key = "ADsystemDiskName";
            if (parameters.Exists(x => x.Key.Equals(key)))
            {
                ParameterViewModel p = parameters.Find(x => x.Key.Equals(key));
                var vm = UnitOfWork.Domains.Get(domainId, "Servers", "Servers.VirtualMachine").Servers
                    .Find(x => x.Type.Equals(DATA.ServerType.ADController));
                if (vm != null)
                {
                    p.Value = vm.VirtualMachine.Disks.FirstOrDefault(x => x.DiskType == DATA.DiskType.SystemDisk).Name;
                }
            }

            key = "ADSystemDiskSize";
            if (parameters.Exists(x => x.Key.Equals(key)))
            {
                ParameterViewModel p = parameters.Find(x => x.Key.Equals(key));
                var vm = UnitOfWork.Domains.Get(domainId, "Servers", "Servers.VirtualMachine").Servers
                    .Find(x => x.Type.Equals(DATA.ServerType.ADController));
                if (vm != null)
                {
                    p.Value = vm.VirtualMachine.Disks.FirstOrDefault(x => x.DiskType == DATA.DiskType.SystemDisk)
                        .SizeInGb.ToString();
                }
            }

            key = "ADdataDiskName";
            if (parameters.Exists(x => x.Key.Equals(key)))
            {
                ParameterViewModel p = parameters.Find(x => x.Key.Equals(key));
                var vm = UnitOfWork.Domains
                    .Get(domainId, "Servers", "Servers.VirtualMachine", "Servers.VirtualMachine.Disks").Servers
                    .Find(x => x.Type.Equals(DATA.ServerType.ADController));
                if (vm != null)
                {
                    p.Value = vm.VirtualMachine.Disks.Where(x => x.DiskType == DATA.DiskType.AdditionalDisk)
                        .FirstOrDefault().Name;
                }
            }

            key = "ADDataDiskSize";
            if (parameters.Exists(x => x.Key.Equals(key)))
            {
                ParameterViewModel p = parameters.Find(x => x.Key.Equals(key));
                var vm = UnitOfWork.Domains
                    .Get(domainId, "Servers", "Servers.VirtualMachine", "Servers.VirtualMachine.Disks").Servers
                    .Find(x => x.Type.Equals(DATA.ServerType.ADController));
                if (vm != null)
                {
                    p.Value = vm.VirtualMachine.Disks.Where(x => x.DiskType == DATA.DiskType.AdditionalDisk)
                        .FirstOrDefault().SizeInGb.ToString();
                }
            }

            key = "ADadminName";
            if (parameters.Exists(x => x.Key.Equals(key)))
            {
                ParameterViewModel p = parameters.Find(x => x.Key.Equals(key));
                var vm = UnitOfWork.Domains.Get(domainId, "Servers", "Servers.VirtualMachine").Servers
                    .Find(x => x.Type.Equals(DATA.ServerType.ADController));
                if (vm != null)
                {
                    p.Value = vm.VirtualMachine.AdminUserName;
                }
            }

            key = "ADadminPW";
            if (parameters.Exists(x => x.Key.Equals(key)))
            {
                ParameterViewModel p = parameters.Find(x => x.Key.Equals(key));
                var vm = UnitOfWork.Domains.Get(domainId, "Servers", "Servers.VirtualMachine").Servers
                    .Find(x => x.Type.Equals(DATA.ServerType.ADController));
                if (vm != null)
                {
                    p.Value = vm.VirtualMachine.AdminUserPassword;
                }
            }

            key = "ADskuName";
            if (parameters.Exists(x => x.Key.Equals(key)))
            {
                ParameterViewModel p = parameters.Find(x => x.Key.Equals(key));
                var vm = UnitOfWork.Domains.Get(domainId, "Servers", "Servers.VirtualMachine").Servers
                    .Find(x => x.Type.Equals(DATA.ServerType.ADController));
                if (vm != null)
                {
                    p.Value = vm.VirtualMachine.OperatingSystem;
                }
            }

            key = "ADServerIPAddress";
            if (parameters.Exists(x => x.Key.Equals(key)))
            {
                ParameterViewModel p = parameters.Find(x => x.Key.Equals(key));
                var vm = UnitOfWork.Domains.Get(domainId, "Servers", "Servers.VirtualMachine").Servers
                    .Find(x => x.Type.Equals(DATA.ServerType.ADController));
                if (vm != null)
                {
                    p.Value = vm.VirtualMachine.LocalIp;
                }
            }

            key = "VpnName";
            if (parameters.Exists(x => x.Key.Equals(key)))
            {
                ParameterViewModel p = parameters.Find(x => x.Key.Equals(key));
                p.Value = UnitOfWork.Bases.Get(baseId, "Vpn").Vpn.Name;
            }

            key = "GatewayIpAddress";
            if (parameters.Exists(x => x.Key.Equals(key)))
            {
                ParameterViewModel p = parameters.Find(x => x.Key.Equals(key));
                p.Value = UnitOfWork.Bases.Get(baseId, "Vpn").Vpn.LocalPublicIp;
            }

            key = "LocalAdressPrefix";
            if (parameters.Exists(x => x.Key.Equals(key)))
            {
                ParameterViewModel p = parameters.Find(x => x.Key.Equals(key));
                p.Value = UnitOfWork.Bases.Get(baseId, "Vpn").Vpn.LocalAddressRange;
            }

            key = "gwpip";
            if (parameters.Exists(x => x.Key.Equals(key)))
            {
                ParameterViewModel p = parameters.Find(x => x.Key.Equals(key));
                p.Value = UnitOfWork.Bases.Get(baseId, "Vpn").Vpn.VirtualPublicIp;
            }

            key = "SharedKey";
            if (parameters.Exists(x => x.Key.Equals(key)))
            {
                ParameterViewModel p = parameters.Find(x => x.Key.Equals(key));
                p.Value = UnitOfWork.Bases.Get(baseId, "Vpn").Vpn.SharedKey;
            }

            key = "NetBIOSName";
            if (parameters.Exists(x => x.Key.Equals(key)))
            {
                ParameterViewModel p = parameters.Find(x => x.Key.Equals(key));
                p.Value = UnitOfWork.Domains.Get(domainId).Name;
            }

            key = "TLD";
            if (parameters.Exists(x => x.Key.Equals(key)))
            {
                ParameterViewModel p = parameters.Find(x => x.Key.Equals(key));
                p.Value = UnitOfWork.Domains.Get(domainId).Tld;
            }

            return parameters;
        }

        /// <summary>
        /// Execute a script in a particular domain. 
        /// </summary>
        /// <param name="customerId">Id of the Customer</param>
        /// <param name="baseId">Id of the Base</param>
        /// <param name="domainId">Id of the Domain</param>
        /// <param name="scriptVersionId">Id of the ScriptVersion</param>
        /// <param name="parameter">The Parameter for execution</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = Constants.Roles.Customer)]
        [Route("{domainId}/script/{scriptVersionId}")]
        public async Task<IActionResult> ExecuteScriptAsync([FromRoute] string baseId, [FromRoute] string domainId, [FromRoute] string scriptVersionId, [FromBody] List<ParameterViewModel> parameter)
        {
            // TODO: Fix code if used... (commented sections)
            try
            {
                var dbBase = UnitOfWork.Bases.Get(baseId, "Credentials", "VirtualMachines", "Subscription");
                DATA.ScriptVersion version = UnitOfWork.Scripts.GetAll().SelectMany(x => x.Versions)
                    .First(y => y.Id == scriptVersionId);
                // AzureCommunicationService azure = new AzureCommunicationService(dbBase.Credentials.TenantId,
                // dbBase.Credentials.ClientId, dbBase.Credentials.ClientSecret);
                FileRepository.FileRepository repository =
                    new FileRepository.FileRepository(connectionStrings.FileRepository,
                        appSettings.FileRepositoryFolder);
                BlobDownloadResult dwResult = await repository.GetBlobFile(version.ContentUrl).DownloadContentAsync();
                var script = dwResult.Content.ToString();

                DATA.Domain dbDomain = UnitOfWork.Domains.Get(domainId);
                // Wait for finish creation, max 2 Hour after starting creating the base
                DateTime end = dbBase.CreatedDate.AddHours(2);
                while (!dbBase.Status.Equals("Base successfully created.") && DateTime.Compare(DateTime.Now, end) < 0)
                {
                    Thread.Sleep(5000);
                    using (var unitOfWork = CreateUnitOfWork())
                    {
                        dbBase = unitOfWork.Bases.Get(baseId, "Credentials", "VirtualMachines", "Subscription");
                    }
                }

                // Get VM
                DATA.VirtualMachine vm = dbBase.VirtualMachines.First();
                string vmId = dbDomain.ExecutionVMId ?? vm.AzureId;
                AZURE.VirtualMachineRefModel vmRef = new AZURE.VirtualMachineRefModel
                {
                    SubscriptionId = dbBase.Subscription.AzureId,
                    VmId = vmId
                };

                DATA.ExecutionLog log = new DATA.ExecutionLog()
                {
                    ExecutionDate = DateTime.Now,
                    Script = script,
                    ScriptArguments = new List<DATA.Parameter>(),
                    ScriptVersionId = version.Id,
                    UserId = GetCurrentUser().Id,
                    VirtualMachineId = vm.Id,
                };

                // Format Parameter
                string arguments = string.Empty;
                foreach (ParameterViewModel p in parameter)
                {
                    arguments += " -" + p.Key + " \"" + p.Value + "\"";
                    log.ScriptArguments.Add(new DATA.Parameter() { Key = p.Key, Value = p.Value });
                }

                // Execute Script with Params
                // Copy File to Temp
                FileRepository.FileRepository scriptRepository = new FileRepository.FileRepository(connectionStrings.FileRepository, appSettings.FileRepositoryFolder);

                var credentialModel = new Azure.Models.StorageCredentialModel()
                {
                    ScriptAzureStoragePath = appSettings.AzureStoragePath + appSettings.FileRepositoryFolder + "/",
                    ScriptStorageAccountKey = appSettings.StorageAccountKey,
                    ScriptStorageAccountName = appSettings.StorageAccountName
                };

                using (var unitOfWork = CreateUnitOfWork())
                {
                    unitOfWork.Logs.MarkForInsert(log);
                    unitOfWork.SaveChanges();
                }

                // await azure.VirtualMachineService().ExecuteVirtualMachineScriptAsync(credentialModel, vmRef, version.ContentUrl, arguments);

                return new OkResult();
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }


        /// <summary>
        /// Create a domain asynchronously and 
        /// execute the finish step of the configuration wizard afterwards.
        ///
        /// Not used for the time being.
        /// </summary>
        /// <param name="baseId"></param>
        /// <param name="domainId"></param>
        /// <param name="scriptIds"></param>
        /// <returns></returns>
/*        private async Task CreateDomainAsync(string customerId, string baseId, string domainId)
        {
            var dbBase = UnitOfWork.Bases.Get(baseId, "Credentials", "VirtualMachines", "VirtualNetwork", "Subscription");
            AzureCommunicationService azure = new AzureCommunicationService(dbBase.Credentials.TenantId, dbBase.Credentials.ClientId, dbBase.Credentials.ClientSecret);
            DATA.Domain dbDomain = null;
            try
            {
                // Wait for finish creation, max 1 Hour
                SetDomainStatus(domainId, "Waiting for Base.", true);
                DateTime end = DateTime.Now.AddHours(1);
                while (!dbBase.Status.Equals("Base successfully created.") && DateTime.Compare(DateTime.Now, end) < 0)
                {
                    Thread.Sleep(5000);
                    using (var unitOfWork = CreateUnitOfWork())
                    {
                        dbBase = unitOfWork.Bases.Get(baseId, "Credentials", "VirtualMachines", "VirtualNetwork", "Subscription");
                    }
                }

                // Get VM
                var dbVM = dbBase.VirtualMachines.First();
                if (dbVM == null)
                {
                    throw new Exception("Create Domain: No Server available");
                }

                VirtualMachineRefModel vmRef = new VirtualMachineRefModel
                {
                    SubscriptionId = dbBase.Subscription.AzureId,
                    VmId = dbVM.AzureId
                };

                // Mark AD-Controller and restart
                using (var unitOfWork = CreateUnitOfWork())
                {
                    dbDomain = unitOfWork.Domains.Get(domainId);
                    dbDomain.Status = "Installing AD-Domain-Service";
                    if (dbDomain.Servers == null)
                    {
                        dbDomain.Servers = new List<DATA.Server>();
                    }

                    dbDomain.Servers.Add(new DATA.Server() { Type = DATA.ServerType.ADController, VirtualMachineId = dbVM.Id });
                    unitOfWork.Domains.MarkForUpdate(dbDomain, GetCurrentUser().Id);
                    unitOfWork.SaveChanges();
                }

                // Execute Domain-Skripts:
                // await azure.VirtualMachineService().ExecuteVirtualMachineScriptAsync(vmRef, Scripts.Master + ".ps1", GetArguments(Scripts.Master, dbDomain, dbVM));
                // await ExecuteFinishStep(customerId, baseId, domainId, domainAdd, azure, vmRef, dbVM);
                var assembly = Assembly.Load("WPM_API");
                var resourcePath = "WPM_API.Resources.ScriptFolder.";
                var fileId = string.Empty;
                var credentialModel = new StorageCredentialModel()
                {
                    ScriptAzureStoragePath = appSettings.AzureStoragePath + appSettings.TempFolder + "/",
                    ScriptStorageAccountKey = appSettings.StorageAccountKey,
                    ScriptStorageAccountName = appSettings.StorageAccountName
                };

                using (var unitOfWork = CreateUnitOfWork())
                {
                    dbDomain = unitOfWork.Domains.Get(domainId, DomainIncludes.GetAllIncludes());
                    dbDomain.OrganizationalUnits = unitOfWork.Domains.GetOrganisationalUnits(dbDomain.Id);
                }

                // Execute Scripts with Params
                TempRepository tempRepository = new TempRepository(connectionStrings.FileRepository, appSettings.TempFolder);
                DomainFileRepository domainRepository = new DomainFileRepository(connectionStrings.FileRepository, appSettings.DomainFileRepositoryFolder);

                //Create AD
                string scriptname = Guid.NewGuid().ToString().Replace("-", "") + ".ps1";
                await tempRepository.UploadFile(scriptname, assembly.GetManifestResourceStream(resourcePath + Scripts.CreateAD + ".ps1"));
                await azure.VirtualMachineService().ExecuteVirtualMachineScriptAsync(credentialModel, vmRef, scriptname, " -NetBIOSName \"" + dbDomain.Name + "\" -TLD \"" + dbDomain.Tld + "\"");
                await tempRepository.DeleteFile(scriptname);

                // Import Users
                if (dbDomain.DomainUserCSV != null)
                {
                    SetDomainStatus(domainId, "Import Users.", false);
                    // Upload Script
                    scriptname = Guid.NewGuid().ToString().Replace("-", "") + ".ps1";
                    await tempRepository.UploadFile(scriptname, assembly.GetManifestResourceStream(resourcePath + Scripts.ImportUser + ".ps1"));
                    // Upload Files
                    // Execute Scripts
                    await azure.VirtualMachineService().ExecuteVirtualMachineScriptAsync(credentialModel, vmRef, scriptname, "-Path \"" + fileId + "\"", fileId);
                    // Delete Files
                    await tempRepository.DeleteFile(scriptname);
                    await tempRepository.DeleteFile(fileId);
                }

                // Create OU
                SetDomainStatus(domainId, "Create OU.", false);
                var list = Mapper.Map<List<DATA.OrganizationalUnit>, List<OUScriptViewModel>>(dbDomain.OrganizationalUnits);
                string ouargs = " -NetBIOSName \"" + dbDomain.Name
                                          + "\" -TLD \"" + dbDomain.Tld
                                          + "\" -LocAdminPW \"" + dbVM.AdminUserPassword
                                          + "\" -JSON \"" +
                                          JsonConvert.SerializeObject(list).Replace("\n", "").Replace("\r", "").Replace("\"", "'").Replace("\\", "") + "\"";
                scriptname = Guid.NewGuid().ToString().Replace("-", "") + ".ps1";
                await tempRepository.UploadFile(scriptname, assembly.GetManifestResourceStream(resourcePath + Scripts.CreateOU + ".ps1"));
                await azure.VirtualMachineService().ExecuteVirtualMachineScriptAsync(credentialModel, vmRef, scriptname, ouargs);
                await tempRepository.DeleteFile(scriptname);

                // Configure DNS
                SetDomainStatus(domainId, "Configure DNS.", false);
                List<string> forwarder = new List<string>();
                dbDomain.DNS.ForEach(d => forwarder.Add(d.Forwarder));
                scriptname = Guid.NewGuid().ToString().Replace("-", "") + ".ps1";
                await tempRepository.UploadFile(scriptname, assembly.GetManifestResourceStream(resourcePath + Scripts.ConfigureDNS + ".ps1"));
                var jsonForwarders = JsonConvert.SerializeObject(forwarder).Replace("\n", "").Replace("\r", "").Replace("\"", "'").Replace("\\", "").Replace("[", "").Replace("]", "").Replace(" ", "").Replace("'", "");
                await azure.VirtualMachineService().ExecuteVirtualMachineScriptAsync(credentialModel, vmRef, scriptname, " -DNSForwarders " + jsonForwarders);
                await tempRepository.DeleteFile(scriptname);

                // Office365 excluded
                // await azure.VirtualMachineService().ExecuteVirtualMachineScriptAsync(vmRef, connectionString, "temp",.ExecuteVirtualMachineScriptAsync(virtualMachineRefModel, "ConfigureOffice365.ps1", "-Path " + domainAddViewModel.Office365ConfigurationXML.Id, new StorageFileModel() { Name = domainAddViewModel.Office365ConfigurationXML.Id, Path = AzureStoragePath });

                // Add CSE-Scripts
                SetDomainStatus(domainId, "Add CSE-Scripts.", false);
                string filename = Guid.NewGuid().ToString().Replace("-", "") + ".zip";
                await tempRepository.UploadFile(filename, assembly.GetManifestResourceStream(resourcePath + "BitStream_v1709.1806.14.zip"));
                scriptname = Guid.NewGuid().ToString().Replace("-", "") + ".ps1";
                await tempRepository.UploadFile(scriptname, assembly.GetManifestResourceStream(resourcePath + Scripts.AddAzureCSEScript + ".ps1"));
                await azure.VirtualMachineService().ExecuteVirtualMachineScriptAsync(credentialModel, vmRef, scriptname, "-CompanyIndex \"" + filename + "\"", filename);
                await tempRepository.DeleteFile(scriptname);
                await tempRepository.DeleteFile(filename);

                // Configure GPO
                if (dbDomain.Gpo.Wallpaper != null && dbDomain.Gpo.Lockscreen != null)
                {
                    SetDomainStatus(domainId, "Configure GPO-Settings.", false);
                    scriptname = Guid.NewGuid().ToString().Replace("-", "") + ".ps1";
                    await tempRepository.UploadFile(scriptname, assembly.GetManifestResourceStream(resourcePath + Scripts.ConfigureGPO + ".ps1"));
                    fileId = await domainRepository.MoveFileAsync(dbDomain.Gpo.Wallpaper.Guid, appSettings.TempFolder);
                    string fileId2 = await domainRepository.MoveFileAsync(dbDomain.Gpo.Lockscreen.Guid, appSettings.TempFolder);
                    await azure.VirtualMachineService().ExecuteVirtualMachineScriptAsync(credentialModel, vmRef, scriptname, "-NetBIOSName \"" + dbDomain.Name + "\" -TLD \"" + dbDomain.Tld + "\" -Settings \"" + dbDomain.Gpo.Settings.Replace("\"", "'") + "\" -Wallpaper \"" + fileId + "\" -Lockscreen \"" + fileId2 + "\"", fileId, fileId2);
                    await tempRepository.DeleteFile(scriptname);
                    await tempRepository.DeleteFile(fileId);
                    await tempRepository.DeleteFile(fileId2);
                }

                // Change DNS-Server
                SetDomainStatus(domainId, "Change DNS-Server IP", false);
                await azure.VirtualNetworkService().SetDnsServer(dbBase.Subscription.AzureId, dbBase.VirtualNetwork.AzureId, dbVM.LocalIp);

                using (var unitOfWork = CreateUnitOfWork())
                {
                    dbBase.VirtualNetwork.Dns = dbVM.LocalIp;
                    dbDomain = unitOfWork.Domains.Get(domainId);
                    dbDomain.Status = "Domain successfully created.";
                    unitOfWork.Domains.MarkForUpdate(dbDomain, GetCurrentUser().Id);
                    unitOfWork.Bases.MarkForUpdate(dbBase, GetCurrentUser().Id);
                    unitOfWork.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                SetDomainStatus(domainId, " Error: " + ex.Message, true);
            }
        }*/

        private void SetDomainStatus(string domainId, string status, bool append = false)
        {
            using (var unitOfWork = CreateUnitOfWork())
            {
                DATA.Domain dbDomain = unitOfWork.Domains.Get(domainId);
                if (append)
                {
                    dbDomain.Status = dbDomain.Status + "; " + status;
                }
                else
                {
                    dbDomain.Status = status;
                }
                unitOfWork.Domains.MarkForUpdate(dbDomain, GetCurrentUser().Id);
                unitOfWork.SaveChanges();
            }
        }

        /* NOT USED ATM
        private async Task CreateExecutingMachine(string baseId, string domainId)
        {
            DATA.Base dbBase;
            using (var unitOfWork = CreateUnitOfWork())
            {
                dbBase = unitOfWork.Bases.Get(baseId, "Credentials", "VirtualNetwork", "VirtualMachines",
                    "Subscription", "ResourceGroup", "StorageAccount");
            }

            AzureCommunicationService azure = new AzureCommunicationService(dbBase.Credentials.TenantId,
                dbBase.Credentials.ClientId, dbBase.Credentials.ClientSecret);

            VirtualMachineAddModel vm = new VirtualMachineAddModel();
            vm.Admin = new AZURE.AdminCredentials("Bitstream", "Windows2000!");
            vm.CustomerId = dbBase.Credentials.CustomerId;
            vm.Location = dbBase.ResourceGroup.Location;
            vm.Name = "DomainExecutionVM" + DateTime.Now.ToFileTimeUtc();
            vm.Network = new VirtualMachineNetworkAdd(dbBase.VirtualNetwork.AzureId, dbBase.VirtualMachines.First().Subnet);
            vm.OperatingSystem = dbBase.VirtualMachines.First().OperatingSystem;
            vm.ResourceGroupName = dbBase.ResourceGroup.Name;
            vm.StorageAccountId = dbBase.StorageAccount.AzureId;
            vm.SubscriptionId = dbBase.Subscription.AzureId;
            vm.Type = "Standard_A0";
            vm.SystemDisk = new DiskAdd("DomainExecutionHDD" + DateTime.Now.ToFileTimeUtc(), 150);

            // Create Machine
            try
            {
                VirtualMachineModel azureVM = await azure.VirtualMachineService().AddVirtualMachinesAsync(vm);

                using (var unitOfWork = CreateUnitOfWork())
                {
                    DATA.Domain dbDomain = unitOfWork.Domains.Get(domainId);
                    dbDomain = unitOfWork.Domains.Get(domainId);
                    dbDomain.ExecutionVMId = azureVM.Id;
                    unitOfWork.SaveChanges();
                    VirtualMachineRefModel vmRef = new VirtualMachineRefModel
                    {
                        SubscriptionId = dbBase.Subscription.AzureId,
                        VmId = azureVM.Id
                    };

                var credentialModel = new StorageCredentialModel()
                {
                    ScriptAzureStoragePath = appSettings.AzureStoragePath + appSettings.TempFolder + "/",
                    ScriptStorageAccountKey = appSettings.StorageAccountKey,
                    ScriptStorageAccountName = appSettings.StorageAccountName
                };
                    string connectionString = connectionStrings.FileRepository;
                    // Join Domain
                    await azure.VirtualMachineService().ExecuteVirtualMachineScriptAsync(vmRef, connectionString,
                        "temp", Scripts.JoinDomain + ".ps1", GetArguments(Scripts.JoinDomain, dbDomain,
                            dbBase.VirtualMachines.First(x => x.Id.Equals(vmRef.VmId))));
                }
            }
            catch (Exception ex)
            {
                using (var unitOfWork = CreateUnitOfWork())
                {
                    DATA.Domain dbDomain = unitOfWork.Domains.Get(domainId);
                    dbDomain = unitOfWork.Domains.Get(domainId);
                    dbDomain.ExecutionVMId += ex.Message;
                    unitOfWork.SaveChanges();
                }
            }
        }*/

        //private string GetArguments(Scripts script, DATA.Domain domain, DATA.VirtualMachine vm)
        //{
        //    #region default-values

        //    string NetBIOSName = domain.Name;
        //    string TLD = domain.Tld;
        //    string DomainMode = "WinThreshold";
        //    string ForrestMode = "WinThreshold";
        //    string DSRMpw = "NUEeQxAAOp8QoMr+";
        //    string DNSForwarder = "8.8.8.8";
        //    //string PrimaryHDD = "C:\\";
        //    string SecondaryHDD = "C:\\";
        //    string DatabasePath = SecondaryHDD + "NTDS";
        //    string SysvolPath = SecondaryHDD + "SYSVOL";
        //    string LogPath = SecondaryHDD + "NTDS";
        //    string LocAdminPW = "BitStream2000!";
        //    string CompanyIndex = "BitStream_v1709.1806.14";

        //    #endregion

        //    switch (script)
        //    {
        //        case Scripts.JoinDomain:
        //            return "-Domain \"" + domain.Name
        //                                + "\" -TLD \"" + domain.Tld
        //                                + "\" -NewName \"ExecutionServer"
        //                                + "\" -DCName \"srv1ad"
        //                                + "\" -OUPath \"OU = SERVER,OU = PAS1,OU = DE,OU = LDA,DC = " + domain.Name +
        //                                ",DC = " + domain.Tld
        //                                + "\" -JoinAccount \"" + domain.Name + "\\Bitstream"
        //                                + "\" -JoinPW \"" + "BitStream2000!" + "\"";
        //        case Scripts.Master:
        //            return " -NetBIOSName \"" + domain.Name
        //                                      + "\" -TLD \"" + domain.Tld
        //                                      + "\" -DomainMode \"" + DomainMode
        //                                      + "\" -ForrestMode \"" + ForrestMode
        //                                      + "\" -DSRMpw \"" + DSRMpw
        //                                      + "\" -DatabasePath \"" + DatabasePath
        //                                      + "\" -SysvolPath \"" + SysvolPath
        //                                      + "\" -LogPath \"" + LogPath
        //                                      + "\" -ADserverName \"" + vm.Name
        //                                      + "\" -DNSForwarder \"" + DNSForwarder + "\"";
        //        case Scripts.CreateOU:
        //            var list = Mapper.Map<List<DATA.OrganizationalUnit>, List<OUScriptViewModel>>(domain.OrganizationalUnits);
        //            return " -NetBIOSName \"" + domain.Name
        //                                      + "\" -TLD \"" + domain.Tld
        //                                      + "\" -LocAdminPW \"" + LocAdminPW
        //                                      + "\" -JSON \"" +
        //                                      JsonConvert.SerializeObject(list).Replace("\n", "").Replace("\r", "").Replace("\"", "'").Replace("\\", "") + "\"";
        //        case Scripts.ImportUser:
        //            return " -UserCSV \"" + domain.DomainUserCSV;
        //        case Scripts.CreateAD:
        //            return " -NetBIOSName \"" + domain.Name
        //                                 + "\" -TLD \"" + domain.Tld + "\"";
        //        default:
        //            return "";
        //    }
        //}

        /// <summary>
        /// Generate JSON ou tree structure 
        /// that is suitable for the powershell script
        /// and can be mapped to the db model.
        /// </summary>
        /// <param name="organizationalUnits"></param>
        /// <param name="BaseLevels"></param>
        /// <returns></returns>
/*        private List<OUScriptViewModel> GenerateOURecursive(List<DATA.OrganizationalUnit> organizationalUnits,
            List<OUBaseLevelAddViewModel> BaseLevels)
        {
            foreach (DATA.OrganizationalUnit unit in organizationalUnits)
            {
                if (unit.Children.Count == 0)
                {
                    unit.Children = new List<DATA.OrganizationalUnit>();
                    foreach (OUBaseLevelAddViewModel baseLevel in BaseLevels)
                    {
                        DATA.OrganizationalUnit leaf = new DATA.OrganizationalUnit()
                        { IsLeaf = true, Name = baseLevel.Name };
                        unit.Children.Add(leaf);
                    }
                }
                else
                {
                    GenerateOURecursive(unit.Children, BaseLevels);
                }
            }

            List<OUScriptViewModel> OUScriptView =
                Mapper.Map<List<DATA.OrganizationalUnit>, List<OUScriptViewModel>>(organizationalUnits);
            return OUScriptView;
        }*/


        [HttpPost]
        [Route("temp")]
        [Authorize(Policy = Constants.Roles.Customer)]
        public async Task<IActionResult> UploadTempFileAsync([FromForm] IFormFile file)
        {
            var newFile = UnitOfWork.Files.CreateEmpty();
            FileRepository.FileRepository temp =
                new FileRepository.FileRepository(connectionStrings.FileRepository, appSettings.FileRepositoryFolder);
            string id = await temp.UploadFile(file.OpenReadStream());
            newFile.Guid = id;
            newFile.Name = file.FileName;
            UnitOfWork.Files.MarkForInsert(newFile);
            UnitOfWork.SaveChanges();
            var json = JsonConvert.SerializeObject(new { Id = id, Name = file.FileName }, serializerSettings);
            return new OkObjectResult(json);
        }

        [HttpGet]
        [Route("csvtemplate")]
        [Authorize(Policy = Constants.Roles.Customer)]
        public async Task<IActionResult> DownloadADUserCSVTemplate()
        {
            FileRepository.FileRepository fileRepository =
                new FileRepository.FileRepository(connectionStrings.FileRepository, appSettings.ResourcesRepositoryFolder);
            ResourcesRepository resourcesRepository = new ResourcesRepository(connectionStrings.FileRepository, appSettings.ResourcesRepositoryFolder);
            var stream = await resourcesRepository.DownloadWithLocalStorageAsync("AD_User_template.csv", Path.Combine(Directory.GetCurrentDirectory(), "Resources", "Temp"));
            return new FileStreamResult(stream, System.Net.Mime.MediaTypeNames.Application.Octet) { FileDownloadName = "AD_User_template.csv" };
        }


        // Not used for the time being.
        /*        private void SaveDNSForwarders(string customerId, string baseId, string domainId, DNSAddViewModel dnsAdd)
                {
                    var dbDomain = UnitOfWork.Domains.Get(domainId, "DNS");
                    dbDomain.DNS = new List<DATA.DNS>();
                    if (dnsAdd.Forwarders != null)
                    {
                        foreach (var dnsAddForwarder in dnsAdd.Forwarders)
                        {
                            dbDomain.DNS.Add(new DATA.DNS() { Forwarder = dnsAddForwarder });
                        }
                    }

                    UnitOfWork.Domains.MarkForUpdate(dbDomain, GetCurrentUser().Id);
                    UnitOfWork.SaveChanges();
                }*/

        // Should be moved to Software, unused for the time being
        //private IActionResult SaveOffice365Configuration(string customerId, string baseId, string domainId,
        //    FileRef xmlFileRef)
        //{
        //    var dbDomain = UnitOfWork.Domains.Get(domainId);
        //    if (!(dbDomain.BaseId.Equals(baseId) && dbDomain.CustomerId.Equals(customerId)))
        //    {
        //        return new BadRequestObjectResult("Domain is in the wrong Base or from the wrong Customer");
        //    }

        //    var file = UnitOfWork.Files.Get(xmlFileRef.Id);
        //    dbDomain.Office365ConfigurationXML = file;
        //    UnitOfWork.Domains.MarkForUpdate(dbDomain, GetCurrentUser().Id);
        //    UnitOfWork.SaveChanges();

        //    // Serialize and return the response
        //    var json = JsonConvert.SerializeObject(dbDomain, serializerSettings);
        //    return new OkObjectResult(json);
        //}

        /*        private void SaveGPOSettings(string customerId, string baseId, string domainId,
                    GroupPolicyObjectViewModel gpoAdd)
                {
                    var dbDomain = UnitOfWork.Domains.Get(domainId, "Gpo");
                    dbDomain.Status = "Saving GPO Settings.";
                    var wallpaper = UnitOfWork.Files.GetByGuid(gpoAdd.Wallpaper.Id);
                    var lockscreen = UnitOfWork.Files.GetByGuid(gpoAdd.Lockscreen.Id);
                    dbDomain.Gpo.Wallpaper = wallpaper;
                    dbDomain.Gpo.Lockscreen = lockscreen;
                    dbDomain.Gpo.Settings = gpoAdd.Settings;
                    UnitOfWork.Domains.MarkForUpdate(dbDomain, GetCurrentUser().Id);
                    UnitOfWork.SaveChanges();
                }*      

                /*********************************** DomainUserController-Methods ******************************************/
        // Not used ATM
        /*
                public async Task<DATA.Domain> SaveDomainUsers(string customerId, string baseId, DATA.Domain domain)
                {
                    //DomainFileRepository domainFileRepository = new DomainFileRepository(connectionStrings.FileRepository, appSettings.DomainFileRepositoryFolder);
                    DomainFileRepository domainFileRepository = new DomainFileRepository(connectionStrings.FileRepository, appSettings.DomainFileRepositoryFolder);
                    CloudBlockBlob blob = domainFileRepository.GetBlobFile(domain.DomainUserCSV.Guid);

                    Stream stream = await blob.OpenReadAsync();
                    List<DATA.DomainUser> users = new List<DATA.DomainUser>();
                    using (var csvStream = new StreamReader(stream))
                    {
                        var csvReader = ConfigureCsvReader(csvStream, true);
                        List<DATA.DomainUser> domainUsers = null;
                        using (var unitOfWork = CreateUnitOfWork())
                        {
                            domainUsers = unitOfWork.DomainUser.GetAll().Where(x => x.DomainId == domain.Id).ToList();

                            while (csvReader.Read())
                            {
                                DomainUserViewModel domainUserRecord = csvReader.GetRecord<DomainUserViewModel>();
                                var oldUser = domainUsers.Find(x => x.SamAccountName == domainUserRecord.SamAccountName);
                                if (oldUser != null)
                                {
                                    Mapper.Map(domainUserRecord, oldUser);
                                    unitOfWork.DomainUser.MarkForUpdate(oldUser);
                                    users.Add(oldUser);
                                }
                                else
                                {
                                    DATA.DomainUser newUser = new DATA.DomainUser();
                                    //Mapper.Map(domainUserRecord, newUser);
                                    newUser.Name = domainUserRecord.Name;
                                    newUser.UserGivenName = domainUserRecord.UserGivenName;
                                    newUser.UserLastName = domainUserRecord.UserLastName;
                                    newUser.SamAccountName = domainUserRecord.SamAccountName;
                                    newUser.UserPrincipalName = domainUserRecord.UserPrincipalName;
                                    newUser.MemberOf = domainUserRecord.MemberOf;
                                    newUser.Description = domainUserRecord.Description;
                                    newUser.Displayname = domainUserRecord.Displayname;
                                    newUser.Workphone = domainUserRecord.Workphone;
                                    newUser.Email = domainUserRecord.Email;

                                    newUser.DomainId = domain.Id;
                                    users.Add(newUser);
                                    unitOfWork.DomainUser.MarkForInsert(newUser);
                                }
                            }
                            unitOfWork.SaveChanges();
                        }
                    }

                    // Return something so that this method's execution can be awaited.
                    return domain;
                }
        */

        /*        private CsvReader ConfigureCsvReader(StreamReader csvStream, bool hasHeader)
                {
                    CsvReader csvReader = new CsvReader(csvStream);
                    csvReader.Configuration.MissingFieldFound = null;
                    csvReader.Configuration.HasHeaderRecord = hasHeader;
                    csvReader.Configuration.IgnoreQuotes = true;
                    // Trim
                    csvReader.Configuration.PrepareHeaderForMatch = header => header?.Trim();

                    // Remove whitespace
                    csvReader.Configuration.PrepareHeaderForMatch = header => header.Replace(" ", string.Empty);

                    // Remove underscores
                    csvReader.Configuration.PrepareHeaderForMatch = header => header.Replace("_", string.Empty);

                    // Ignore case
                    csvReader.Configuration.PrepareHeaderForMatch = header => header.ToLower();
                    csvReader.Read();
                    if (hasHeader)
                    {
                        csvReader.ReadHeader();
                    }
                    return csvReader;
                }*/
    }
}