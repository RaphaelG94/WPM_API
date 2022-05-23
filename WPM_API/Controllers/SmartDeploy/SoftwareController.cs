using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net;
using System.Text;
using WPM_API.Azure;
using WPM_API.Code.Infrastructure;
using WPM_API.Code.Infrastructure.LogOn;
using WPM_API.Common;
using WPM_API.Data.DataContext.Entities;
using WPM_API.Data.DataContext.Entities.SmartDeploy;
using WPM_API.Data.DataContext.Entities.Storages;
using WPM_API.Data.Models;
using WPM_API.Options;
using WPM_API.TransferModels;
using WPM_API.TransferModels.SmartDeploy;
using static WPM_API.FileRepository.FileRepository;
using File = WPM_API.Data.DataContext.Entities.File;
using Task = WPM_API.Data.DataContext.Entities.Task;

namespace WPM_API.Controllers
{
    [Authorize(Policy = Constants.Roles.Systemhouse)]
    [Route("/software")]
    public class SoftwareController : BasisController
    {
        public SoftwareController(AppSettings appSettings, ConnectionStrings connectionStrings, OrderEmailOptions orderEmailOptions, AgentEmailOptions agentEmailOptions, SendMailCreds sendMailCreds, SiteOptions siteOptions, ILogonManager logonManager) : base(appSettings, connectionStrings, orderEmailOptions, agentEmailOptions, sendMailCreds, siteOptions, logonManager)
        {
        }

        [HttpPost]
        [Route("icon/upload")]
        public async Task<IActionResult> UploadIconAsync(Microsoft.AspNetCore.Http.IFormFile file)
        {
            FileRepository.FileRepository software = new FileRepository.FileRepository(connectionStrings.FileRepository, appSettings.FileRepositoryFolder);
            string id = await software.UploadFile(file.OpenReadStream());
            var json = JsonConvert.SerializeObject(new { Id = id }, serializerSettings);
            return new OkObjectResult(json);
        }

        [HttpPost]
        [Route("importSoftware")]
        public IActionResult ImportSoftware(Microsoft.AspNetCore.Http.IFormFile file)
        {
            using (StreamReader sr = new StreamReader(file.OpenReadStream()))
            {
                string json = sr.ReadToEnd();
                Software temp = JsonConvert.DeserializeObject<Software>(json);
                SoftwareViewModel result = Mapper.Map<SoftwareViewModel>(temp);
                result.Icon = null;
                result.Id = "";
                result.RuleDetection.VersionNr = "";
                result.RuleDetection.Data = null;
                result.TaskInstall.Files = new List<FileRef>();
                result.TaskInstall.Executable = "";
                result.RevisionNumber = temp.RevisionNumber;
                return Ok(JsonConvert.SerializeObject(temp, serializerSettings));
            }
        }

        [HttpPost]
        [Route("iconAndBanner/upload")]
        [Authorize(Policy = Constants.Policies.Systemhouse)]
        public async Task<IActionResult> UploadIconAndBannerAsync(Microsoft.AspNetCore.Http.IFormFile file)
        {
            FileRepository.FileRepository software = new FileRepository.FileRepository(connectionStrings.FileRepository, appSettings.IconsAndBanners);
            string id = await software.UploadFile(file.OpenReadStream());
            var json = JsonConvert.SerializeObject(new { Id = id }, serializerSettings);
            return new OkObjectResult(json);
        }

        [HttpGet]
        [Route("icon/get/{softwareId}")]
        public IActionResult GetSoftwareIconFileRef([FromRoute] string softwareId)
        {
            Software dbSoftware = UnitOfWork.Software.GetOrNull(softwareId);

            // Check if software exists
            if (dbSoftware == null)
            {
                return BadRequest("ERROR: The software does not exist");
            }

            var json = String.Empty;

            // TODO: Get icon from stream
            // Check if Icon is null
            // if (dbSoftware.Icon == null)
            // {
            //  json = JsonConvert.SerializeObject(new FileRef());
            // }

            // Get FileRef and return it
            //json = JsonConvert.SerializeObject(Mapper.Map<FileRef>(dbSoftware.Icon), serializerSettings);

            return Ok();
        }

        [HttpGet]
        [Route("export/{fileId}")]
        public IActionResult ExportSoftware([FromRoute] string fileId)
        {
            try
            {
                Software toExport = UnitOfWork.Software.Get(fileId, SoftwareIncludes.GetAllIncludes());
                toExport.RuleDetection.VersionNr = "";
                toExport.RuleDetection.Data = null;
                toExport.TaskInstall.Files = null;
                toExport.TaskInstall.ExecutionFileId = "";
                toExport.TaskInstall.ExecutionFile = null;
                var json = JsonConvert.SerializeObject(
                    toExport,
                    new JsonSerializerSettings
                    {
                        Formatting = Formatting.Indented,
                        ContractResolver = new CamelCasePropertyNamesContractResolver(),
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });
                return File(Encoding.UTF8.GetBytes(json), "application/json", toExport.Name + ".json");
            }
            catch (Exception e)
            {
                return BadRequest("ERROR: " + e.Message);
            }
        }

        /// <summary>
        /// Retrieve all Software.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetSoftware()
        {
            var softwareList = new List<Software>();
            using (var unitOfWork = CreateUnitOfWork())
            {
                // SoftwareIncludes.GetAllIncludes()
                softwareList = unitOfWork.Software.GetAll().ToList();
            }
            List<SoftwareViewModel> result = Mapper.Map<List<SoftwareViewModel>>(softwareList);

            var json = JsonConvert.SerializeObject(result, serializerSettings);
            return new OkObjectResult(json);
        }

        [HttpGet]
        [Authorize(Policy = Constants.Policies.Systemhouse)]
        [Route("getWithIncludes/{softwareId}")]
        public IActionResult GetSoftwarePackage([FromRoute] string softwareId)
        {
            Software toLoad = UnitOfWork.Software.GetOrNull(softwareId, SoftwareIncludes.GetAllIncludes());
            if (toLoad == null)
            {
                return BadRequest("Error: The software package does not exist!");
            }
            SoftwareViewModel result = Mapper.Map<SoftwareViewModel>(toLoad);

            var json = JsonConvert.SerializeObject(result, serializerSettings);
            return new OkObjectResult(json);
        }

        /// <summary>
        /// Create new Software.
        /// </summary>
        /// <param name="software">New Software</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddSoftware([FromBody] SoftwareAddViewModel software)
        {
            Software newSoftware = Mapper.Map<Software>(software);
            newSoftware.RuleApplicability.VersionNr = software.Version;
            newSoftware.RevisionNumber = Guid.NewGuid().ToString();
            newSoftware.Systemhouses = new List<SoftwaresSystemhouse>();
            newSoftware.Customers = new List<SoftwaresCustomer>();
            newSoftware.Clients = new List<SoftwaresClient>();
            newSoftware.DisplayRevisionNumber = 1;
            newSoftware.AllWin10Versions = software.AllWin10Versions;
            newSoftware.AllWin11Versions = software.AllWin11Versions;
            using (var unitOfWork = CreateUnitOfWork())
            {
                // Mapping architecture data
                for (int i = 0; i < newSoftware.RuleApplicability.Architecture.Count; i++)
                {
                    Architecture tempArchitecture = UnitOfWork.Architectures.CreateEmpty();
                    tempArchitecture.Version = software.RuleApplicability.Architecture[i];
                    newSoftware.RuleApplicability.Architecture[i] = tempArchitecture;
                    newSoftware.RuleApplicability.Architecture[i].Version = software.RuleApplicability.Architecture[i];
                }

                for (int i = 0; i < newSoftware.RuleDetection.Architecture.Count; i++)
                {
                    newSoftware.RuleDetection.Architecture[i].Version = software.RuleDetection.Architecture[i];
                }

                // Mapping osVersionName data
                if (software.RuleApplicability.OsVersionNames != null)
                {
                    for (int i = 0; i < newSoftware.RuleApplicability.OsVersionNames.Count; i++)
                    {
                        newSoftware.RuleApplicability.OsVersionNames[i].Version = software.RuleApplicability.OsVersionNames[i];
                    }
                }

                // Mapping windows 10 version names data
                if (software.RuleApplicability.Win10Versions != null)
                {
                    for (int i = 0; i < newSoftware.RuleApplicability.Win10Versions.Count; i++)
                    {
                        newSoftware.RuleApplicability.Win10Versions[i].Version = software.RuleApplicability.Win10Versions[i];
                    }
                }

                // Mapping windows 11 version names
                if (software.RuleApplicability.Win11Versions != null)
                {
                    for (int i = 0; i < newSoftware.RuleApplicability.Win11Versions.Count; i++)
                    {
                        newSoftware.RuleApplicability.Win11Versions[i].Version = software.RuleApplicability.Win11Versions[i];
                    }
                }

                if (newSoftware.TaskInstall != null)
                {
                    // Set the ID as GUID and delete ID for automatically ID from EF
                    newSoftware.TaskInstall.Files.ForEach(x => { x.Guid = x.Id; x.Id = null; });
                }
                if (newSoftware.TaskUpdate != null)
                {
                    // Set the ID as GUID and delete ID for automatically ID from EF
                    newSoftware.TaskUpdate.Files.ForEach(x => { x.Guid = x.Id; x.Id = null; });
                }
                if (newSoftware.TaskUninstall != null)
                {
                    // Set the ID as GUID and delete ID for automatically ID from EF
                    newSoftware.TaskUninstall.Files.ForEach(x => { x.Guid = x.Id; x.Id = null; });
                }
                /*
                if (software.Icon != null && software.Icon.Id != "" && software.Icon.Name != "")
                {
                    File icon = UnitOfWork.Files.CreateEmpty();
                    icon.Name = software.Icon.Name;
                    icon.Guid = software.Icon.Id;
                    newSoftware.Icon = icon;
                } else
                {
                newSoftware.Icon = null;
                }
                */
                if (newSoftware.RuleDetection.Path != null)
                {
                    newSoftware.TaskInstall.ExePath = newSoftware.RuleDetection.Path;
                }
                unitOfWork.Software.MarkForInsert(newSoftware, GetCurrentUser().Id);
                unitOfWork.SaveChanges();

                if (software.TaskInstall != null)
                {
                    newSoftware.TaskInstall.ExecutionFile = unitOfWork.Files.GetAll().First(x => x.Guid.Equals(software.TaskInstall.Executable));
                }
                if (software.TaskUpdate != null)
                {
                    newSoftware.TaskUpdate.ExecutionFile = unitOfWork.Files.GetAll().First(x => x.Guid.Equals(software.TaskUpdate.Executable));
                }
                if (software.TaskUninstall != null)
                {
                    newSoftware.TaskUninstall.ExecutionFile = unitOfWork.Files.GetAll().First(x => x.Guid.Equals(software.TaskUninstall.Executable));
                }
                unitOfWork.SaveChanges();

                if (software.Systemhouses.Count > 0)
                {
                    newSoftware.Systemhouses = new List<SoftwaresSystemhouse>();
                    foreach (string id in software.Systemhouses)
                    {
                        var systemhouse = unitOfWork.Systemhouses.Get(id);
                        SoftwaresSystemhouse temp = new SoftwaresSystemhouse();
                        temp.SystemhouseId = systemhouse.Id;
                        temp.SoftwareId = newSoftware.Id;
                        newSoftware.Systemhouses.Add(temp);
                    }

                    if (software.Customers.Count > 0)
                    {
                        foreach (string id in software.Customers)
                        {
                            var customer = unitOfWork.Customers.Get(id);
                            SoftwaresCustomer temp = new SoftwaresCustomer();
                            temp.SoftwareId = newSoftware.Id;
                            temp.CustomerId = customer.Id;
                            newSoftware.Customers.Add(temp);
                        }

                        if (software.Clients.Count > 0)
                        {
                            foreach (string id in software.Clients)
                            {
                                var client = unitOfWork.Clients.Get(id);
                                SoftwaresClient temp = new SoftwaresClient();
                                temp.SoftwareId = newSoftware.Id;
                                temp.ClientId = client.Id;
                                newSoftware.Clients.Add(temp);
                            }
                        }
                    }
                }
                SoftwareStream stream = unitOfWork.SoftwareStreams.GetOrNull(software.StreamId, "StreamMembers");
                if (stream == null)
                {
                    return BadRequest("ERROR: The software could not be added to the stream! The stream does not exist");
                }
                if (stream.StreamMembers == null)
                {
                    stream.StreamMembers = new List<Software>();
                }

                stream.StreamMembers.Add(newSoftware);
                unitOfWork.SaveChanges();

                // Serialize and return result
                newSoftware = UnitOfWork.Software.Get(newSoftware.Id, SoftwareIncludes.GetAllIncludes());
                SoftwareViewModel result = Mapper.Map<SoftwareViewModel>(newSoftware);

                var json = JsonConvert.SerializeObject(result, serializerSettings);
                return new OkObjectResult(json);
            }
        }

        [HttpGet]
        [Route("revisionMessage/{softwareId}")]
        public IActionResult GetRevisionMessages([FromRoute] string softwareId)
        {
            List<RevisionMessage> revisionMessages = UnitOfWork.RevisionMessages.GetAll().Where(x => x.SoftwareId == softwareId).ToList();

            var json = JsonConvert.SerializeObject(revisionMessages, serializerSettings);
            return Ok(json);
        }

        [HttpPost]
        [Route("revisionMessage")]
        public IActionResult AddRevisionMessage([FromBody] RevisionMessageViewModel data)
        {
            using (var unitOfWork = CreateUnitOfWork())
            {
                Software sw = unitOfWork.Software.GetOrNull(data.EntityId);
                if (sw == null)
                {
                    return BadRequest("ERROR: The software does not exist");
                }
                RevisionMessage newMessage = unitOfWork.RevisionMessages.CreateEmpty();
                newMessage.DisplayRevisionNumber = sw.DisplayRevisionNumber;
                newMessage.SoftwareId = sw.Id;
                newMessage.Message = data.Value;
                newMessage.CreatedByUserId = GetCurrentUser().Id;
                newMessage.CreatedDate = DateTime.Now;

                unitOfWork.SaveChanges();

                return Ok();
            }
        }

        /// <summary>
        /// Change an existing Software.
        /// </summary>
        /// <param name="softwareId">Id of Software to change</param>
        /// <param name="softwareEdit">New values</param>
        /// <returns></returns>
        [HttpPut]
        [Route("{softwareId}")]
        public IActionResult UpdatesSoftware([FromRoute] string softwareId, [FromBody] SoftwareEditViewModel softwareEdit)
        {
            using (var unitOfWork = CreateUnitOfWork())
            {
                Software dbSoftware = unitOfWork.Software.Get(softwareId, SoftwareIncludes.GetAllIncludes());
                //unitOfWork.Software.MarkForUpdate(dbSoftware, GetCurrentUser().Id);
                dbSoftware.TaskInstall.CommandLine = softwareEdit.TaskInstall.Commandline;
                dbSoftware.DisplayRevisionNumber++;
                if (dbSoftware == null)
                {
                    return new NotFoundResult();
                }

                // Map all, but no Tasks, because the frontend doesnt know anything about Guid.
                // Automapper destroys EF Tracking
                //Mapper.Map<SoftwareEditViewModel, Software>(softwareEdit, dbSoftware);
                dbSoftware.Id = softwareId;
                dbSoftware.Name = softwareEdit.Name;
                dbSoftware.Version = softwareEdit.Version;
                dbSoftware.Status = softwareEdit.Status;
                dbSoftware.Type = softwareEdit.Type;
                dbSoftware.InstallationTime = softwareEdit.InstallationTime;
                dbSoftware.PackageSize = softwareEdit.PackageSize;
                dbSoftware.Prerequisites = softwareEdit.Prerequisites;
                dbSoftware.VendorReleaseDate = softwareEdit.VendorReleaseDate;
                dbSoftware.CompliancyRule = softwareEdit.CompliancyRule;
                dbSoftware.Checksum = softwareEdit.Checksum;
                dbSoftware.RunningContext = softwareEdit.RunningContext;
                dbSoftware.DedicatedDownloadLink = softwareEdit.DedicatedDownloadLink;
                dbSoftware.RevisionNumber = Guid.NewGuid().ToString();
                dbSoftware.RuleApplicability.VersionNr = softwareEdit.Version;
                dbSoftware.AllWin10Versions = softwareEdit.AllWin10Versions;
                dbSoftware.AllWin11Versions = softwareEdit.AllWin11Versions;

                // Set previous and following softwares
                if (softwareEdit.PrevSoftwareId != "" && softwareEdit.PrevSoftwareId != null)
                {
                    Software previousSW = unitOfWork.Software.GetOrNull(softwareEdit.PrevSoftwareId);
                    if (previousSW == null)
                    {
                        return BadRequest("ERROR: The previous software version does not exist");
                    }
                    dbSoftware.PrevSoftwareId = softwareEdit.PrevSoftwareId;
                }

                if (softwareEdit.NextSoftwareId != "" && softwareEdit.NextSoftwareId != null)
                {
                    Software nextSW = unitOfWork.Software.GetOrNull(softwareEdit.NextSoftwareId);
                    if (nextSW == null)
                    {
                        return BadRequest("ERROR: The next software version does not exist");
                    }
                    dbSoftware.NextSoftwareId = softwareEdit.NextSoftwareId;
                }

                if (softwareEdit.MinimalSoftwareId != "" && softwareEdit.MinimalSoftwareId != null)
                {
                    Software minimalSW = unitOfWork.Software.GetOrNull(softwareEdit.MinimalSoftwareId);
                    if (minimalSW == null)
                    {
                        return BadRequest("ERROR: The minimal software version does not exist");
                    }
                    dbSoftware.MinimalSoftwareId = softwareEdit.MinimalSoftwareId;
                }

                var userId = GetCurrentUser().Id;
                // Delete existing Syshouses, customers and clients
                foreach (SoftwaresSystemhouse sys in dbSoftware.Systemhouses)
                {
                    unitOfWork.SoftwaresSystemhouses.MarkForDelete(sys, userId);
                }
                foreach (SoftwaresCustomer cus in dbSoftware.Customers)
                {
                    unitOfWork.SoftwaresCustomers.MarkForDelete(cus, userId);
                }
                foreach (SoftwaresClient cl in dbSoftware.Clients)
                {
                    unitOfWork.SoftwaresClients.MarkForDelete(cl, userId);
                }
                unitOfWork.SaveChanges();

                if (softwareEdit.Systemhouses.Count > 0)
                {
                    dbSoftware.Systemhouses = new List<SoftwaresSystemhouse>();
                    foreach (string id in softwareEdit.Systemhouses)
                    {
                        var systemhouse = unitOfWork.Systemhouses.Get(id);
                        SoftwaresSystemhouse temp = new SoftwaresSystemhouse();
                        temp.SystemhouseId = systemhouse.Id;
                        temp.SoftwareId = dbSoftware.Id;
                        dbSoftware.Systemhouses.Add(temp);
                    }

                    if (softwareEdit.Customers.Count > 0)
                    {
                        foreach (string id in softwareEdit.Customers)
                        {
                            var customer = unitOfWork.Customers.Get(id);
                            SoftwaresCustomer temp = new SoftwaresCustomer();
                            temp.SoftwareId = dbSoftware.Id;
                            temp.CustomerId = customer.Id;
                            dbSoftware.Customers.Add(temp);
                        }

                        if (softwareEdit.Clients.Count > 0)
                        {
                            foreach (string id in softwareEdit.Clients)
                            {
                                var client = unitOfWork.Clients.Get(id);
                                SoftwaresClient temp = new SoftwaresClient();
                                temp.SoftwareId = dbSoftware.Id;
                                temp.ClientId = client.Id;
                                dbSoftware.Clients.Add(temp);
                            }
                        }
                    }
                }

                // Create new Syshouses, customers and clients
                foreach (SoftwaresSystemhouse toDelete in dbSoftware.Systemhouses)
                {
                    unitOfWork.SoftwaresSystemhouses.MarkForDelete(toDelete, GetCurrentUser().Id);
                }
                foreach (SoftwaresCustomer toDelete in dbSoftware.Customers)
                {
                    unitOfWork.SoftwaresCustomers.MarkForDelete(toDelete, GetCurrentUser().Id);
                }
                foreach (SoftwaresClient toDelete in dbSoftware.Clients)
                {
                    unitOfWork.SoftwaresClients.MarkForDelete(toDelete, GetCurrentUser().Id);
                }
                unitOfWork.SaveChanges();
                dbSoftware.Systemhouses = new List<SoftwaresSystemhouse>();
                dbSoftware.Customers = new List<SoftwaresCustomer>();
                dbSoftware.Clients = new List<SoftwaresClient>();
                if (softwareEdit.Systemhouses.Count > 0)
                {
                    dbSoftware.Systemhouses = new List<SoftwaresSystemhouse>();
                    foreach (string id in softwareEdit.Systemhouses)
                    {
                        var systemhouse = unitOfWork.Systemhouses.Get(id);
                        SoftwaresSystemhouse temp = new SoftwaresSystemhouse();
                        temp.SystemhouseId = systemhouse.Id;
                        temp.SoftwareId = dbSoftware.Id;
                        dbSoftware.Systemhouses.Add(temp);
                    }

                    if (softwareEdit.Customers.Count > 0)
                    {
                        foreach (string id in softwareEdit.Customers)
                        {
                            var customer = unitOfWork.Customers.Get(id);
                            SoftwaresCustomer temp = new SoftwaresCustomer();
                            temp.SoftwareId = dbSoftware.Id;
                            temp.CustomerId = customer.Id;
                            dbSoftware.Customers.Add(temp);
                        }

                        if (softwareEdit.Clients.Count > 0)
                        {
                            foreach (string id in softwareEdit.Clients)
                            {
                                var client = unitOfWork.Clients.Get(id);
                                SoftwaresClient temp = new SoftwaresClient();
                                temp.SoftwareId = dbSoftware.Id;
                                temp.ClientId = client.Id;
                                dbSoftware.Clients.Add(temp);
                            }
                        }
                    }
                }

                unitOfWork.SaveChanges();

                // Update the rules
                Rule applicabilityRule = unitOfWork.Rules.Get(softwareEdit.RuleApplicabilityId, "Win11Versions");
                Rule detectionRule = unitOfWork.Rules.Get(softwareEdit.RuleDetectionId);

                // Edit detection rule
                foreach (Architecture toDelete in detectionRule.Architecture)
                {
                    unitOfWork.Architectures.MarkForDelete(toDelete, GetCurrentUser().Id);
                }
                detectionRule.Architecture = new List<Architecture>();
                for (int i = 0; i < softwareEdit.RuleDetection.Architecture.Count; i++)
                {
                    detectionRule.Architecture.Add(new Architecture() { Version = softwareEdit.RuleDetection.Architecture[i] });
                }

                detectionRule.CheckVersionNr = softwareEdit.RuleDetection.CheckVersionNr;
                detectionRule.Data = Mapper.Map<File>(softwareEdit.RuleDetection.Data);
                detectionRule.Name = softwareEdit.RuleDetection.Name;
                detectionRule.Path = softwareEdit.RuleDetection.Path;
                detectionRule.Successon = softwareEdit.RuleDetection.Successon;
                detectionRule.Type.Name = softwareEdit.RuleDetection.Type;
                detectionRule.UpdatedDate = DateTime.Now;
                detectionRule.VersionNr = softwareEdit.RuleDetection.VersionNr;
                unitOfWork.Rules.MarkForUpdate(detectionRule);

                dbSoftware.RuleDetection = detectionRule;
                dbSoftware.TaskInstall.CheckVersionNr = dbSoftware.RuleDetection.CheckVersionNr;
                dbSoftware.TaskInstall.VersionNr = dbSoftware.RuleDetection.VersionNr;
                if (dbSoftware.RuleDetection.Path != null)
                {
                    dbSoftware.TaskInstall.ExePath = dbSoftware.RuleDetection.Path;
                }

                // Edit applicability rule
                applicabilityRule.OsType = softwareEdit.RuleApplicability.OsType;
                foreach (Architecture toDelete in applicabilityRule.Architecture)
                {
                    unitOfWork.Architectures.MarkForDelete(toDelete, GetCurrentUser().Id);
                }
                applicabilityRule.Architecture = new List<Architecture>();
                for (int i = 0; i < softwareEdit.RuleApplicability.Architecture.Count; i++)
                {
                    applicabilityRule.Architecture.Add(new Architecture() { Version = softwareEdit.RuleApplicability.Architecture[i] });
                }

                foreach (OsVersionName toDelete in applicabilityRule.OsVersionNames)
                {
                    unitOfWork.OsVersionNames.MarkForDelete(toDelete, GetCurrentUser().Id);
                }
                applicabilityRule.OsVersionNames = new List<OsVersionName>();

                for (int i = 0; i < softwareEdit.RuleApplicability.OsVersionNames.Count; i++)
                {
                    applicabilityRule.OsVersionNames.Add(new OsVersionName() { Version = softwareEdit.RuleApplicability.OsVersionNames[i] });
                }

                foreach (Win10Version toDelete in applicabilityRule.Win10Versions)
                {
                    unitOfWork.Win10Versions.MarkForDelete(toDelete, GetCurrentUser().Id);
                }
                applicabilityRule.Win10Versions = new List<Win10Version>();
                for (int i = 0; i < softwareEdit.RuleApplicability.Win10Versions.Count; i++)
                {
                    applicabilityRule.Win10Versions.Add(new Win10Version() { Version = softwareEdit.RuleApplicability.Win10Versions[i] });
                }

                foreach (Win11Version toDelete in applicabilityRule.Win11Versions)
                {
                    unitOfWork.Win11Versions.MarkForDelete(toDelete, GetCurrentUser().Id);
                }
                for (int i = 0; i < softwareEdit.RuleApplicability.Win11Versions.Count; i++)
                {
                    applicabilityRule.Win11Versions.Add(new Win11Version() { Version = softwareEdit.RuleApplicability.Win11Versions[i] });
                }

                applicabilityRule.CheckVersionNr = softwareEdit.RuleDetection.CheckVersionNr;
                applicabilityRule.Data = Mapper.Map<File>(softwareEdit.RuleDetection.Data);
                applicabilityRule.Name = softwareEdit.RuleDetection.Name;
                applicabilityRule.Successon = softwareEdit.RuleDetection.Successon;
                applicabilityRule.Type.Name = softwareEdit.RuleDetection.Type;
                applicabilityRule.UpdatedDate = DateTime.Now;
                applicabilityRule.VersionNr = softwareEdit.RuleDetection.VersionNr;
                unitOfWork.Rules.MarkForUpdate(applicabilityRule);

                dbSoftware.RuleApplicability = applicabilityRule;

                unitOfWork.SaveChanges();

                if (dbSoftware.TaskInstall != null)
                {
                    dbSoftware.TaskInstall.Files = new List<File>();
                    dbSoftware.TaskInstall.UseShellExecute = softwareEdit.TaskInstall.UseShellExecute;
                    dbSoftware.TaskInstall.RunningContext = softwareEdit.RunningContext;
                    dbSoftware.TaskInstall.InstallationType = softwareEdit.TaskInstall.InstallationType;
                    dbSoftware.TaskInstall.RestartRequired = softwareEdit.TaskInstall.RestartRequired;
                    foreach (FileRef tempFileView in softwareEdit.TaskInstall.Files)
                    {
                        File tempFile = unitOfWork.Files.GetOrNull(tempFileView.Id);
                        if (tempFile != null)
                        {
                            dbSoftware.TaskInstall.Files.Add(tempFile);
                        }
                        else
                        {
                            tempFile = new File();
                            // tempFile.Id = softwareEdit.TaskInstall.Executable;
                            tempFile.Guid = tempFileView.Id;
                            tempFile.Name = tempFileView.Name;
                            dbSoftware.TaskInstall.Files.Add(tempFile);
                            unitOfWork.Files.MarkForInsert(tempFile);
                            unitOfWork.SaveChanges();
                        }
                    }
                    File exe = dbSoftware.TaskInstall.Files.Find(x => x.Guid == softwareEdit.TaskInstall.Executable);
                    if (exe == null)
                    {
                        exe = dbSoftware.TaskInstall.Files.Find(x => x.Id == softwareEdit.TaskInstall.Executable);
                    }
                    if (exe == null)
                    {
                        return BadRequest("ERROR: The executable File was not found");
                    }

                    dbSoftware.TaskInstall.ExecutionFileId = exe.Id;
                    // unitOfWork.Software.MarkForUpdate(dbSoftware, GetCurrentUser().Id);
                    unitOfWork.SaveChanges();
                }


                List<string> ids = new List<string>();

                // Check if files were deleted
                if (dbSoftware.TaskUninstall != null)
                {
                    ids = new List<string>();
                    dbSoftware.TaskUninstall.Files.ForEach(x =>
                    {
                        if (!softwareEdit.TaskUninstall.Files.Exists(y => y.Id == x.Id))
                        {
                            ids.Add(x.Id);
                        }
                    });
                    dbSoftware.TaskUninstall.Files.RemoveAll(x => ids.Contains(x.Id));
                }
                // Check if new files were added
                if (softwareEdit.TaskUninstall != null)
                {
                    softwareEdit.TaskUninstall.Files.ForEach(x =>
                    {
                        // When this id not exists, then it is a new file and the id is the GUID.
                        if (dbSoftware.TaskUninstall == null)
                        {
                            dbSoftware.TaskUninstall = Mapper.Map<Task>(softwareEdit.TaskUninstall);
                            dbSoftware.TaskUninstall.Files = new List<File>(); // new files where added in next step (conflict with guid and id)
                        }
                        if (!dbSoftware.TaskUninstall.Files.Exists(y => y.Id == x.Id))
                        {
                            dbSoftware.TaskUninstall.Files.Add(new File() { Guid = x.Id, Name = x.Name });
                        }
                    });
                }


                // Check if files were deleted
                if (dbSoftware.TaskUpdate != null)
                {
                    ids = new List<string>();
                    dbSoftware.TaskUpdate.Files.ForEach(x =>
                    {
                        if (!softwareEdit.TaskUpdate.Files.Exists(y => y.Id == x.Id))
                        {
                            ids.Add(x.Id);
                        }
                    });
                    dbSoftware.TaskUpdate.Files.RemoveAll(x => ids.Contains(x.Id));
                }
                // Check if new files were added
                if (softwareEdit.TaskUpdate != null)
                {
                    softwareEdit.TaskUpdate.Files.ForEach(x =>
                    {
                        // When this id not exists, then it is a new file and the id is the GUID.
                        if (dbSoftware.TaskUpdate == null)
                        {
                            dbSoftware.TaskUpdate = Mapper.Map<Task>(softwareEdit.TaskUpdate);
                            dbSoftware.TaskUpdate.Files = new List<File>(); // new files where added in next step (conflict with guid and id)
                        }
                        if (!dbSoftware.TaskUpdate.Files.Exists(y => y.Id == x.Id))
                        {
                            dbSoftware.TaskUpdate.Files.Add(new File() { Guid = x.Id, Name = x.Name });
                        }
                    });
                }
                unitOfWork.SaveChanges();

                // set new executable
                dbSoftware = unitOfWork.Software.Get(dbSoftware.Id, SoftwareIncludes.GetAllIncludes());

                if (softwareEdit.TaskUpdate != null && softwareEdit.TaskUpdate.Id != null)
                {
                    // new file
                    var file = unitOfWork.Files.GetByGuid(softwareEdit.TaskUpdate.Executable);
                    if (file == null)
                    {
                        // old file
                        file = unitOfWork.Files.Get(softwareEdit.TaskUpdate.Executable);
                    }
                    dbSoftware.TaskUpdate.ExecutionFile = file;
                }
                if (softwareEdit.TaskUninstall != null && softwareEdit.TaskUninstall.Id != null)
                {
                    // new file
                    var file = unitOfWork.Files.GetByGuid(softwareEdit.TaskUninstall.Executable);
                    if (file == null)
                    {
                        // old file
                        file = unitOfWork.Files.Get(softwareEdit.TaskUninstall.Executable);
                    }
                    dbSoftware.TaskUninstall.ExecutionFile = file;
                }
                unitOfWork.SaveChanges();
                // Software was changed and is returned.
                dbSoftware = UnitOfWork.Software.Get(dbSoftware.Id, SoftwareIncludes.GetAllIncludes());
                SoftwareViewModel result = Mapper.Map<SoftwareViewModel>(dbSoftware);
                var json = JsonConvert.SerializeObject(result, serializerSettings);

                List<CustomerSoftware> customerSoftwares = unitOfWork.CustomerSoftwares.GetAll().Where(x => x.SoftwareId == dbSoftware.Id).ToList();
                foreach (CustomerSoftware cs in customerSoftwares)
                {
                    cs.AllWin10Versions = dbSoftware.AllWin10Versions;
                    cs.AllWin11Versions = dbSoftware.AllWin11Versions;
                    unitOfWork.CustomerSoftwares.MarkForUpdate(cs);
                }

                System.Threading.Tasks.Task.Run(() => UpdateCustomerSWPackages(dbSoftware, appSettings, connectionStrings, GetCurrentUser().Id));

                return new OkObjectResult(json);
            }
        }

        // AutoUpdate for CustomerSoftware
        private async void UpdateCustomerSWPackages(Software dbSoftware, AppSettings appSettings, ConnectionStrings connectionStrings, string userId)
        {
            // TODO: Update Customer Software Packages
            using (var unitOfWork = CreateUnitOfWork())
            {
                List<CustomerSoftwareStream> customerSoftwareStreams = unitOfWork.CustomerSoftwareStreamss.GetAll("StreamMembers")
                    .Where(x => x.UpdateSettings == "auto" && x.SoftwareStreamId == dbSoftware.SoftwareStreamId).ToList();
                foreach (CustomerSoftwareStream stream in customerSoftwareStreams)
                {
                    CustomerSoftware customerSoftware = stream.StreamMembers.Find(x => x.SoftwareId == dbSoftware.Id);
                    if (customerSoftware != null)
                    {
                        // Shop files
                        WPM_API.Data.DataContext.Entities.Customer customer = unitOfWork.Customers.GetOrNull(stream.CustomerId, CustomerIncludes.GetAllIncludes());
                        var cep = GetCEP(stream.CustomerId);
                        StorageEntryPoint csdp = customer.StorageEntryPoints.Find(x => x.IsCSDP == true);
                        FileRepository.FileRepository csdpBitstreamRepo = new FileRepository.FileRepository(connectionStrings.FileRepository, appSettings.FileRepositoryFolder);
                        AzureCommunicationService azureCustomer;
                        if (!csdp.Managed)
                        {
                            azureCustomer = new AzureCommunicationService(cep.TenantId, cep.ClientId, cep.ClientSecret);
                        }
                        else
                        {
                            // TODO: Check for system; fix for live system
                            azureCustomer = new AzureCommunicationService(appSettings.DevelopmentTenantId, appSettings.DevelopmentClientId, appSettings.DevelopmentClientSecret);
                        }

                        List<FileAndSAS> sasKeys = new List<FileAndSAS>();
                        // Get files from BitStream storage account
                        foreach (Data.DataContext.Entities.File file in dbSoftware.TaskInstall.Files)
                        {
                            if (file.Guid != null)
                            {
                                FileAndSAS sasKey = await csdpBitstreamRepo.GetSASFile(file.Name, file.Guid);
                                if (sasKey != null)
                                {
                                    sasKeys.Add(sasKey);
                                }

                            }
                        }

                        List<Microsoft.Azure.Management.Storage.Models.StorageAccount> storageAccounts = await azureCustomer.StorageService().GetStorageAccounts(csdp.SubscriptionId, csdp.ResourceGrpName);
                        Microsoft.Azure.Management.Storage.Models.StorageAccount storageAccountCustomer = storageAccounts.Find(x => x.Name == csdp.StorageAccount);

                        string connectionString = azureCustomer.StorageService().GetStorageAccConnectionString(csdp.SubscriptionId, csdp.ResourceGrpName, csdp.StorageAccount);
                        FileRepository.FileRepository csdpCustomer = new FileRepository.FileRepository(connectionString, "csdp");
                        foreach (FileAndSAS sasKey in sasKeys)
                        {
                            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(sasKey.SasUri);
                            request.Method = "GET";
                            WebResponse response = request.GetResponse();
                            await csdpCustomer.UploadFile(response.GetResponseStream(), "filerepository/" + sasKey.FileName);
                        }

                        // update customer sw package data
                        customerSoftware = MapCustomerSoftware(customerSoftware, dbSoftware);
                        unitOfWork.CustomerSoftwares.MarkForUpdate(customerSoftware, userId);
                        unitOfWork.SaveChanges();
                    }
                }

                unitOfWork.SaveChanges();
            }
        }

        /*
        private CustomerSoftware MapCustomerSoftware (CustomerSoftware customerSoftware, Software dbSoftware)
        {
            customerSoftware.Checksum = dbSoftware.Checksum;
            customerSoftware.CompliancyRule = dbSoftware.CompliancyRule;
            customerSoftware.DedicatedDownloadLink = dbSoftware.DedicatedDownloadLink;
            customerSoftware.DisplayRevisionNumber = dbSoftware.DisplayRevisionNumber;
            customerSoftware.InstallationTime = dbSoftware.InstallationTime;
            customerSoftware.MinimalSoftwareId = dbSoftware.MinimalSoftwareId;
            customerSoftware.Name = dbSoftware.Name;
            customerSoftware.NextSoftwareId = dbSoftware.NextSoftwareId;
            customerSoftware.PackageSize = dbSoftware.PackageSize;
            customerSoftware.Prerequisites = dbSoftware.Prerequisites;
            customerSoftware.PrevSoftwareId = dbSoftware.PrevSoftwareId;
            customerSoftware.RevisionNumber = dbSoftware.RevisionNumber;
            customerSoftware.RunningContext = dbSoftware.RunningContext;
            customerSoftware.Status = dbSoftware.Status;
            customerSoftware.Type = dbSoftware.Type;
            customerSoftware.VendorReleaseDate = dbSoftware.VendorReleaseDate;
            customerSoftware.Version = dbSoftware.Version;
            return customerSoftware;

        }
        */

        /// <summary>
        /// Delete an existing Software.
        /// </summary>
        /// <param name="softwareId">Id of the Software to delete</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{softwareId}")]
        public IActionResult DeleteSoftware([FromRoute] string softwareId)
        {
            Software dbSoftware = null;
            using (var unitOfWork = CreateUnitOfWork())
            {
                dbSoftware = unitOfWork.Software.Get(softwareId, "TaskInstall");
                if (dbSoftware == null)
                {
                    return new NotFoundResult();
                }

                List<WPM_API.Data.DataContext.Entities.ClientTask> tasks = unitOfWork.ClientTasks.GetAll().Where(x => x.TaskId == dbSoftware.TaskInstall.Id && x.Status != "executed").ToList();
                foreach (WPM_API.Data.DataContext.Entities.ClientTask task in tasks)
                {
                    unitOfWork.ClientTasks.MarkForDelete(task);
                }
                unitOfWork.Software.MarkForDelete(dbSoftware, GetCurrentUser().Id);
                unitOfWork.SaveChanges();
            }

            // Software was changed and is returned.
            return new StatusCodeResult(204);
        }
        [HttpGet]
        [Route("icon/{fileId}")]
        public async Task<IActionResult> DownloadFileAsync([FromRoute] string fileId)
        {
            // TODO: Get files from CSDP
            FileRepository.FileRepository software = new FileRepository.FileRepository(connectionStrings.FileRepository, appSettings.FileRepositoryFolder);
            var file = UnitOfWork.Files.Get(fileId);
            if (file.Guid != null)
            {
                var blob = software.GetBlobFile(file.Guid);
                var ms = new MemoryStream();
                await blob.DownloadToAsync(ms);
                ms.Seek(0, SeekOrigin.Begin);
                return File(ms, System.Net.Mime.MediaTypeNames.Application.Octet, file.Name);
            }
            else
            {
                return Ok();
            }
        }

        [HttpGet]
        [Route("icon/forShop/{softwareId}")]
        public async Task<IActionResult> DownloadSWIconAsync([FromRoute] string softwareId)
        {
            Software sw = UnitOfWork.Software.GetOrNull(softwareId);
            if (sw == null)
            {
                return BadRequest("ERROR: The software does not exist");
            }

            SoftwareStream swStream = UnitOfWork.SoftwareStreams.GetOrNull(sw.SoftwareStreamId, "Icon");
            if (swStream == null)
            {
                return BadRequest("ERROR: The software stream does not exist");
            }
            if (swStream.Icon != null)
            {
                FileRepository.FileRepository software = new FileRepository.FileRepository(connectionStrings.FileRepository, appSettings.FileRepositoryFolder);
                var file = UnitOfWork.Files.Get(swStream.Icon.Id);
                var blob = software.GetBlobFile(file.Guid);
                var ms = new MemoryStream();
                await blob.DownloadToAsync(ms);
                ms.Seek(0, SeekOrigin.Begin);
                return File(ms, System.Net.Mime.MediaTypeNames.Application.Octet, file.Name);
            }
            else
            {
                return Ok();
            }
        }

        [HttpGet]
        [Route("downloadFiles/{softwareId}")]
        public async Task<IActionResult> DownloadSoftwareFiles([FromRoute] string softwareId)
        {
            Software sw = UnitOfWork.Software.GetOrNull(softwareId, "TaskInstall", "TaskInstall.Files", "TaskInstall.ExecutionFile");
            FileRepository.FileRepository fileRepository = new FileRepository.FileRepository(connectionStrings.FileRepository, appSettings.FileRepositoryFolder);
            List<FileAndSAS> SASKeys = new List<FileAndSAS>();
            foreach (File file in sw.TaskInstall.Files)
            {
                FileAndSAS fileAndSAS = await fileRepository.GetSASFile(file.Name, file.Guid);
                fileAndSAS.FileName = file.Name;
                SASKeys.Add(fileAndSAS);
            }

            var json = JsonConvert.SerializeObject(SASKeys, serializerSettings);
            return Ok(json);
        }

        [HttpPut]
        [Route("publishInShop/{softwareId}")]
        public IActionResult PublishInShop([FromRoute] string softwareId)
        {
            using (var unitOfWork = CreateUnitOfWork())
            {
                Software sw = UnitOfWork.Software.GetOrNull(softwareId, SoftwareIncludes.GetAllIncludes());
                if (sw == null)
                {
                    return BadRequest("ERROR: The software package does not exist");
                }

                sw.PublishInShop = true;
                unitOfWork.Software.MarkForUpdate(sw, GetCurrentUser().Id);
                unitOfWork.SaveChanges();
                System.Threading.Tasks.Task.Run(() => AutoUpdateCustomerSWStreams(sw, appSettings, connectionStrings, GetCurrentUser().Id));
                var json = JsonConvert.SerializeObject(Mapper.Map<SoftwareViewModel>(sw), serializerSettings);
                return Ok(json);
            }
        }

        private async void AutoUpdateCustomerSWStreams(Software sw, AppSettings appSettings, ConnectionStrings connectionStrings, string userId)
        {
            // TODO: Log errors 
            try
            {
                using (var unitOfWork = CreateUnitOfWork())
                {
                    // Shop sw package for all customer sw streams with auto update enabled
                    List<CustomerSoftwareStream> customerSWStreams = unitOfWork.CustomerSoftwareStreamss.GetAll("StreamMembers")
                        .Where(x => x.UpdateSettings == "auto" && x.SoftwareStreamId == sw.SoftwareStreamId).ToList();
                    foreach (CustomerSoftwareStream customerStream in customerSWStreams)
                    {
                        if (customerStream.StreamMembers.Find(x => x.SoftwareId == sw.Id) == null)
                        {
                            string customerId = customerStream.CustomerId;
                            Customer customer = unitOfWork.Customers.GetOrNull(customerId, CustomerIncludes.GetAllIncludes());
                            var cep = GetCEP(customerId);
                            StorageEntryPoint csdp = null;
                            if (customer == null)
                            {
                                // return BadRequest("ERROR: The customer does not exist");
                            }
                            csdp = customer.StorageEntryPoints.Find(x => x.IsCSDP == true);
                            if (cep == null && csdp != null && !csdp.Managed)
                            {
                                // return BadRequest("ERROR: The csdp Cloud Entry Point is not set");
                            }
                            if (sw == null)
                            {
                                // return BadRequest("ERROR: The software does not exist");
                            }
                            if (csdp == null)
                            {
                                // return BadRequest("ERROR: The csdp storage entry point does not exist or has not been created yet");
                            }

                            List<FileAndSAS> sasKeys = new List<FileAndSAS>();

                            // Create needed Azrue connections
                            if (appSettings == null || connectionStrings == null)
                            {
                                // return BadRequest("ERROR: Cannot fetch Bitstream Azure connection from config files");
                            }
                            FileRepository.FileRepository csdpBitstreamRepo = new FileRepository.FileRepository(connectionStrings.FileRepository, appSettings.FileRepositoryFolder);
                            AzureCommunicationService azureCustomer;
                            if (!csdp.Managed)
                            {
                                azureCustomer = new AzureCommunicationService(cep.TenantId, cep.ClientId, cep.ClientSecret);
                            }
                            else
                            {
                                // TODO: Check for system; fix for live system
                                azureCustomer = new AzureCommunicationService(appSettings.DevelopmentTenantId, appSettings.DevelopmentClientId, appSettings.DevelopmentClientSecret);
                            }

                            // Get files from BitStream storage account
                            foreach (Data.DataContext.Entities.File file in sw.TaskInstall.Files)
                            {
                                if (file.Guid == null)
                                {
                                    // return BadRequest("ERROR: The file has no Guid for Azure");
                                }
                                FileAndSAS sasKey = await csdpBitstreamRepo.GetSASFile(file.Name, file.Guid);
                                if (sasKey == null)
                                {
                                    // return BadRequest("ERROR: The sas key for " + file.Name + " is null");
                                }
                                sasKeys.Add(sasKey);
                            }

                            List<Microsoft.Azure.Management.Storage.Models.StorageAccount> storageAccounts = await azureCustomer.StorageService().GetStorageAccounts(csdp.SubscriptionId, csdp.ResourceGrpName);
                            Microsoft.Azure.Management.Storage.Models.StorageAccount storageAccountCustomer = storageAccounts.Find(x => x.Name == csdp.StorageAccount);
                            if (storageAccountCustomer == null)
                            {
                                // return BadRequest("ERROR: Your csdp in Azrue has been deleted. Please restore the storage account");
                            }

                            string connectionString = azureCustomer.StorageService().GetStorageAccConnectionString(csdp.SubscriptionId, csdp.ResourceGrpName, csdp.StorageAccount);
                            FileRepository.FileRepository csdpCustomer = new FileRepository.FileRepository(connectionString, "csdp");
                            foreach (FileAndSAS sasKey in sasKeys)
                            {
                                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(sasKey.SasUri);
                                request.Method = "GET";
                                WebResponse response = request.GetResponse();
                                await csdpCustomer.UploadFile(response.GetResponseStream(), "filerepository/" + sasKey.FileName);
                            }

                            // TODO: Create CustomerSoftware for CustomerSoftwareStream
                            CustomerSoftware newSoftware = unitOfWork.CustomerSoftwares.CreateEmpty();
                            newSoftware = MapCustomerSoftware(newSoftware, sw);
                            newSoftware.SoftwareId = sw.Id;
                            newSoftware.CustomerStatus = "Test";
                            newSoftware.CustomerSoftwareStreamId = customerStream.Id;
                            unitOfWork.CustomerSoftwares.MarkForInsert(newSoftware, userId);
                            unitOfWork.SaveChanges();
                            customerStream.StreamMembers.Add(newSoftware);
                            unitOfWork.CustomerSoftwareStreamss.MarkForUpdate(customerStream, userId);
                            newSoftware.CustomerSoftwareStreamId = customerStream.Id;
                            newSoftware.TaskInstall = sw.TaskInstall;
                            newSoftware.TaskUninstall = sw.TaskUninstall;
                            newSoftware.TaskUpdate = sw.TaskUpdate;
                            unitOfWork.CustomerSoftwares.MarkForUpdate(newSoftware, userId);
                            unitOfWork.SaveChanges();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                // return false;
            }
        }

        private CustomerSoftware MapCustomerSoftware(CustomerSoftware result, Software sw)
        {
            result.AllWin10Versions = sw.AllWin10Versions;
            result.AllWin11Versions = sw.AllWin11Versions;
            result.Checksum = sw.Checksum;
            result.CompliancyRule = sw.CompliancyRule;
            result.CreatedByUserId = sw.CreatedByUserId;
            result.CreatedDate = sw.CreatedDate;
            result.DedicatedDownloadLink = sw.DedicatedDownloadLink;
            result.DisplayRevisionNumber = sw.DisplayRevisionNumber;
            result.InstallationTime = sw.InstallationTime;
            result.MinimalSoftwareId = sw.MinimalSoftwareId;
            result.Name = sw.Name;
            result.NextSoftwareId = sw.NextSoftwareId;
            result.PackageSize = sw.PackageSize;
            result.Prerequisites = sw.Prerequisites;
            result.PrevSoftwareId = sw.PrevSoftwareId;
            result.RevisionNumber = sw.RevisionNumber;
            result.RuleApplicability = sw.RuleApplicability;
            result.RuleDetection = sw.RuleDetection;
            result.RunningContext = sw.RunningContext;
            result.Status = sw.Status;
            result.Type = sw.Type;
            result.VendorReleaseDate = sw.VendorReleaseDate;
            result.Version = sw.Version;
            return result;
        }

        [HttpPut]
        [Route("retreatFromShop/{softwareId}")]
        public IActionResult RetreatFromShop([FromRoute] string softwareId)
        {
            Software sw = UnitOfWork.Software.GetOrNull(softwareId);
            if (sw == null)
            {
                return BadRequest("ERROR: The software package does not exist");
            }

            sw.PublishInShop = false;

            UnitOfWork.Software.MarkForUpdate(sw, GetCurrentUser().Id);
            UnitOfWork.SaveChanges();

            var json = JsonConvert.SerializeObject(Mapper.Map<SoftwareViewModel>(sw), serializerSettings);
            return Ok(json);
        }

        [HttpGet]
        [Route("prevSW/{streamId}/{version}")]
        public IActionResult LoadPreviousSoftware([FromRoute] string streamId, [FromRoute] string version)
        {
            using (var unitOfWork = CreateUnitOfWork())
            {
                List<Software> previousSoftwares = new List<Software>();
                SoftwareStream stream = unitOfWork.SoftwareStreams.GetOrNull(streamId, "StreamMembers", "StreamMembers.RuleDetection.Type");
                if (stream == null)
                {
                    return BadRequest("ERROR: The software stream does not exist");
                }
                foreach (Software sw in stream.StreamMembers)
                {
                    //if (sw.Id != softwareId)
                    //{
                    string[] currentVersion = sw.Version.Split(".");
                    string[] toCompare = version.Split(".");
                    if (currentVersion.Length != 0)
                    {
                        string[] compared = compareVersions(currentVersion, toCompare);
                        if (compared.Length != 0)
                        {
                            if (compared[0] == "smaller")
                            {
                                previousSoftwares.Add(sw);
                            }
                            else if (compared[0] == "bigger")
                            {
                                continue;
                            }
                            else
                            {
                                for (int i = 1; i <= compared.Length - 1; i++)
                                {
                                    if (compared[i] == "smaller")
                                    {
                                        previousSoftwares.Add(sw);
                                    }
                                    else if (compared[i] == "similar")
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                        // }
                    }
                }
                var json = JsonConvert.SerializeObject(Mapper.Map<List<SoftwareViewModel>>(previousSoftwares), serializerSettings);
                return Ok(json);
            }
        }

        [HttpGet]
        [Route("prevSW/{streamId}/{version}/{softwareId}")]
        public IActionResult LoadPreviousSoftware([FromRoute] string streamId, [FromRoute] string version, [FromRoute] string softwareId)
        {
            using (var unitOfWork = CreateUnitOfWork())
            {
                List<Software> previousSoftwares = new List<Software>();
                SoftwareStream stream = unitOfWork.SoftwareStreams.GetOrNull(streamId, "StreamMembers");
                if (stream == null)
                {
                    return BadRequest("ERROR: The software stream does not exist");
                }
                foreach (Software sw in stream.StreamMembers)
                {
                    if (sw.Id != softwareId)
                    {
                        string[] currentVersion = sw.Version.Split(".");
                        string[] toCompare = version.Split(".");
                        if (currentVersion.Length != 0)
                        {
                            string[] compared = compareVersions(currentVersion, toCompare);
                            if (compared.Length != 0)
                            {
                                if (compared[0] == "smaller")
                                {
                                    previousSoftwares.Add(sw);
                                }
                                else if (compared[0] == "bigger")
                                {
                                    continue;
                                }
                                else
                                {
                                    for (int i = 1; i <= compared.Length - 1; i++)
                                    {
                                        if (compared[i] == "smaller")
                                        {
                                            if (previousSoftwares.Find(x => x.Id == sw.Id) == null)
                                            {
                                                previousSoftwares.Add(sw);
                                            }
                                        }
                                        else if (compared[i] == "similar")
                                        {
                                            continue;
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                var json = JsonConvert.SerializeObject(Mapper.Map<List<SoftwareViewModel>>(previousSoftwares), serializerSettings);
                return Ok(json);
            }
        }

        private string[] compareVersions(string[] currentVersion, string[] toCompare)
        {
            string[] result = new string[currentVersion.Length];
            if (currentVersion.Length == toCompare.Length)
            {
                for (int i = 0; i < currentVersion.Length; i++)
                {
                    int version1 = int.Parse(currentVersion[i]);
                    int version2 = int.Parse(toCompare[i]);
                    if (version1 < version2)
                    {
                        result[i] = "smaller";
                    }
                    else if (version1 == version2)
                    {
                        result[i] = "similar";
                    }
                    else
                    {
                        result[i] = "bigger";
                    }
                }
            }
            return result;
        }

        [HttpGet]
        [Route("nextSW/{streamId}/{version}")]
        public IActionResult LoadNextSoftware([FromRoute] string streamId, [FromRoute] string version)
        {
            using (var unitOfWork = CreateUnitOfWork())
            {
                List<Software> nextSoftwares = new List<Software>();
                SoftwareStream stream = unitOfWork.SoftwareStreams.GetOrNull(streamId, "StreamMembers");
                if (stream == null)
                {
                    return BadRequest("ERROR: The software stream does not exist");
                }
                foreach (Software sw in stream.StreamMembers)
                {
                    string[] currentVersion = sw.Version.Split(".");
                    string[] toCompare = version.Split(".");
                    if (currentVersion.Length != 0)
                    {
                        string[] compared = compareVersions(currentVersion, toCompare);
                        if (compared.Length != 0)
                        {
                            if (compared[0] == "bigger")
                            {
                                nextSoftwares.Add(sw);
                            }
                            else if (compared[0] == "smaller")
                            {
                                continue;
                            }
                            else
                            {
                                for (int i = 1; i <= compared.Length - 1; i++)
                                {
                                    if (compared[i] == "bigger")
                                    {
                                        nextSoftwares.Add(sw);
                                    }
                                    else if (compared[i] == "similar")
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }

                var json = JsonConvert.SerializeObject(Mapper.Map<List<SoftwareViewModel>>(nextSoftwares), serializerSettings);
                return Ok(json);
            }
        }

        [HttpGet]
        [Route("nextSW/{streamId}/{version}/{softwareId}")]
        public IActionResult LoadNextSoftware([FromRoute] string streamId, [FromRoute] string version, [FromRoute] string softwareId)
        {
            using (var unitOfWork = CreateUnitOfWork())
            {
                List<Software> nextSoftwares = new List<Software>();
                SoftwareStream stream = unitOfWork.SoftwareStreams.GetOrNull(streamId, "StreamMembers");
                if (stream == null)
                {
                    return BadRequest("ERROR: The software stream does not exist");
                }
                foreach (Software sw in stream.StreamMembers)
                {
                    if (sw.Id != softwareId)
                    {
                        string[] currentVersion = sw.Version.Split(".");
                        string[] toCompare = version.Split(".");
                        if (currentVersion.Length != 0)
                        {
                            string[] compared = compareVersions(currentVersion, toCompare);
                            if (compared.Length != 0)
                            {
                                if (compared[0] == "bigger")
                                {
                                    nextSoftwares.Add(sw);
                                }
                                else if (compared[0] == "smaller")
                                {
                                    continue;
                                }
                                else
                                {
                                    for (int i = 1; i <= compared.Length - 1; i++)
                                    {
                                        if (compared[i] == "bigger")
                                        {
                                            nextSoftwares.Add(sw);
                                        }
                                        else if (compared[i] == "similar")
                                        {
                                            continue;
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                var json = JsonConvert.SerializeObject(Mapper.Map<List<SoftwareViewModel>>(nextSoftwares), serializerSettings);
                return Ok(json);
            }
        }

        [HttpPost]
        [Route("activate/{softwareId}")]
        public IActionResult SetSWActive([FromRoute] string softwareId)
        {
            using (var unitOfWork = CreateUnitOfWork())
            {
                Software software = unitOfWork.Software.Get(softwareId);
                software.Status = "Active";
                unitOfWork.SaveChanges();

                return Ok(JsonConvert.SerializeObject(software, serializerSettings));
            }
        }

        [HttpPost]
        [Route("outdate/{softwareId}")]
        public IActionResult SetSWOutdated([FromRoute] string softwareId)
        {
            using (var unitOfWork = CreateUnitOfWork())
            {
                Software software = unitOfWork.Software.Get(softwareId);
                software.Status = "Outdated";
                unitOfWork.SaveChanges();

                return Ok(JsonConvert.SerializeObject(software, serializerSettings));
            }
        }

        public class RevisionMessageViewModel
        {
            public string Value { get; set; }
            public string EntityId { get; set; }
        }
    }
}

