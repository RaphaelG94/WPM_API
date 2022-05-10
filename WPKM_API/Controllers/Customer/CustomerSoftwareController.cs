using WPM_API.Common;
using WPM_API.Data.DataContext.Entities;
using WPM_API.Data.DataContext.Entities.SmartDeploy;
using WPM_API.Data.DataContext.Entities.Storages;
using WPM_API.Data.Models;
using WPM_API.TransferModels.SmartDeploy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WPM_API.Azure;
using static WPM_API.FileRepository.FileRepository;

namespace WPM_API.Controllers
{
    [Route("customerSoftware/{customerId}")]
    [Authorize(Policy = Constants.Policies.Customer)]
    public class CustomerSoftwareController : BasisController
    {
        [HttpGet]
        public IActionResult GetCustomersSoftware([FromRoute] string customerId)
        {
            using (var unitOfWork = CreateUnitOfWork())
            {
                List<CustomerSoftware> result = new List<CustomerSoftware>();
                List<CustomerSoftwareStream> customerSoftwareStreams = unitOfWork.CustomerSoftwareStreamss.GetAll("StreamMembers").Where(x => x.CustomerId == customerId).ToList();
                foreach (CustomerSoftwareStream stream in customerSoftwareStreams)
                {
                    result.AddRange(stream.StreamMembers);
                }

                var json = Newtonsoft.Json.JsonConvert.SerializeObject(result, _serializerSettings);
                return Ok(json);
            }
        }

        [HttpPost]
        [Route("setPriorities")]
        public IActionResult SetPriorities([FromBody] List<CustomerSWViewModel> payload)
        {
            using (var unitOfWork = CreateUnitOfWork())
            {
                foreach (CustomerSWViewModel cs in payload)
                {
                    CustomerSoftwareStream toEdit = unitOfWork.CustomerSoftwareStreamss.Get(cs.Id);
                    toEdit.Priority = payload.IndexOf(cs) + 1;
                    unitOfWork.CustomerSoftwareStreamss.MarkForUpdate(toEdit, GetCurrentUser().Id);
                    unitOfWork.SaveChanges();
                }
                return Ok();
            }
        }

        [HttpDelete]
        [Route("{customerSoftwareId}")]
        public async Task<IActionResult> DeleteCustomerSoftware([FromRoute] string customerSoftwareId)
        {
            try
            {
                using (var unitOfWork = CreateUnitOfWork())
                {
                    CustomerSoftware toDelete = unitOfWork.CustomerSoftwares.GetOrNull(customerSoftwareId, "TaskInstall.Files");
                    if (toDelete == null)
                    {
                        return BadRequest("ERROR: The software package does not exist");
                    }

                    CustomerSoftwareStream stream = unitOfWork.CustomerSoftwareStreamss.GetOrNull(toDelete.CustomerSoftwareStreamId, "StreamMembers");

                    if (stream == null)
                    {
                        return BadRequest("ERROR: The customer software stream does not exist");
                    }

                    WPM_API.Data.DataContext.Entities.Customer customer = unitOfWork.Customers.GetOrNull(stream.CustomerId, "StorageEntryPoints");

                    if (customer == null)
                    {
                        return BadRequest("ERROR: The customer does not exist");
                    }

                    CloudEntryPoint cep = GetCEP(stream.CustomerId);

                    StorageEntryPoint csdp = customer.StorageEntryPoints.Find(x => x.IsCSDP == true);

                    if (cep == null && !csdp.Managed)
                    {
                        return BadRequest("ERROR: The cloud entry points are not set");
                    }

                    AzureCommunicationService azureCustomer;
                    if (csdp.Managed)
                    {
                        azureCustomer = new AzureCommunicationService(_appSettings.DevelopmentTenantId, _appSettings.DevelopmentClientId, _appSettings.DevelopmentClientSecret);
                    }
                    else
                    {
                        azureCustomer = new AzureCommunicationService(cep.TenantId, cep.ClientId, cep.ClientSecret);
                    }

                    string connectionString = azureCustomer.StorageService().GetStorageAccConnectionString(csdp.SubscriptionId, csdp.ResourceGrpName, csdp.StorageAccount);
                    FileRepository.FileRepository csdpCustomer = new FileRepository.FileRepository(connectionString, "csdp/filerepository");
                    string error = String.Empty;

                    foreach (Data.DataContext.Entities.File file in toDelete.TaskInstall.Files)
                    {
                        if (file.Guid == null)
                        {
                            return BadRequest("ERROR: The file has no Guid for Azure");
                        }
                        bool result = await csdpCustomer.DeleteFile(file.Guid);
                    }

                    if (error.Length != 0)
                    {
                        return BadRequest("ERROR: Deleting files from Azure did not succeed: \n" + error);
                    }

                    unitOfWork.CustomerSoftwares.MarkForDelete(toDelete, GetCurrentUser().Id);
                    unitOfWork.SaveChanges();
                    stream.StreamMembers.RemoveAll(x => x.Id == toDelete.Id);
                    if (stream.StreamMembers.Count == 0)
                    {
                        unitOfWork.CustomerSoftwareStreamss.MarkForDelete(stream, GetCurrentUser().Id);
                    }
                    else
                    {
                        unitOfWork.CustomerSoftwareStreamss.MarkForUpdate(stream, GetCurrentUser().Id);
                    }
                    unitOfWork.SaveChanges();

                    var json = JsonConvert.SerializeObject(stream, _serializerSettings);
                    return Ok(json);
                }
            }
            catch (Exception e)
            {
                return BadRequest("ERROR: " + e.Message);
            }
        }

        [HttpPost]
        public IActionResult EditCustomerSoftware([FromBody] CustomerSoftware data)
        {
            using (var unitOfWork = CreateUnitOfWork())
            {
                CustomerSoftware toEdit = unitOfWork.CustomerSoftwares.Get(data.Id);
                toEdit.CustomerStatus = data.CustomerStatus;

                // unitOfWork.CustomerSoftwares.MarkForUpdate(toEdit, GetCurrentUser().Id);
                unitOfWork.SaveChanges();

                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(toEdit, _serializerSettings));
            }
        }

        [HttpGet]
        [Route("customerStreams")]
        public IActionResult GetCustomersSoftwareStreams([FromRoute] string customerId)
        {
            using (var unitOfWork = CreateUnitOfWork())
            {
                List<CustomerSoftwareStream> streams = unitOfWork.CustomerSoftwareStreamss.GetAll("StreamMembers", "ApplicationOwner").Where(x => x.CustomerId == customerId && x.DeletedDate == null).ToList();
                foreach (CustomerSoftwareStream stream in streams)
                {
                    SoftwareStream origin = unitOfWork.SoftwareStreams.Get(stream.SoftwareStreamId, "Icon");
                    stream.Icon = origin.Icon;
                }

                var json = JsonConvert.SerializeObject(streams, _serializerSettings);
                return Ok(json);
            }
        }

        [HttpDelete]
        [Route("customerStreams/{customerStreamId}")]
        public async Task<IActionResult> DeleteCustomerStream([FromRoute] string customerStreamId)
        {
            try
            {
                using (var unitOfWork = CreateUnitOfWork())
                {
                    CustomerSoftwareStream toDelete = unitOfWork.CustomerSoftwareStreamss.GetOrNull(customerStreamId, "StreamMembers", "StreamMembers.TaskInstall", "StreamMembers.TaskInstall.Files");


                    WPM_API.Data.DataContext.Entities.Customer customer = unitOfWork.Customers.GetOrNull(toDelete.CustomerId, "StorageEntryPoints");

                    if (customer == null)
                    {
                        return BadRequest("ERROR: The customer does not exist");
                    }

                    AzureCommunicationService azureCustomer;
                    StorageEntryPoint csdp = customer.StorageEntryPoints.Find(x => x.IsCSDP == true);
                    if (!csdp.Managed)
                    {
                        CloudEntryPoint cep = GetCEP(toDelete.CustomerId);

                        if (cep == null)
                        {
                            return BadRequest("ERROR: The cloud entry points are not set");
                        }
                        azureCustomer = new AzureCommunicationService(cep.TenantId, cep.ClientId, cep.ClientSecret);
                    }
                    else
                    {
                        // TODO: Check system; fix for live system
                        azureCustomer = new AzureCommunicationService(_appSettings.DevelopmentTenantId, _appSettings.DevelopmentClientId, _appSettings.DevelopmentClientSecret);
                    }
                    string connectionString = azureCustomer.StorageService().GetStorageAccConnectionString(csdp.SubscriptionId, csdp.ResourceGrpName, csdp.StorageAccount);
                    FileRepository.FileRepository csdpCustomer = new FileRepository.FileRepository(connectionString, "csdp/filerepository");
                    string error = String.Empty;

                    foreach (CustomerSoftware sw in toDelete.StreamMembers)
                    {
                        foreach (Data.DataContext.Entities.File file in sw.TaskInstall.Files)
                        {
                            if (file.Guid == null)
                            {
                                return BadRequest("ERROR: The file has no Guid for Azure");
                            }
                            bool result = await csdpCustomer.DeleteFile(file.Guid);
                        }
                        unitOfWork.CustomerSoftwares.MarkForDelete(sw, GetCurrentUser().Id);
                    }

                    if (error.Length != 0)
                    {
                        return BadRequest("ERROR: Deleting files from Azure did not succeed: \n" + error);
                    }

                    toDelete.StreamMembers = null;
                    unitOfWork.CustomerSoftwareStreamss.MarkForDelete(toDelete, GetCurrentUser().Id);
                    unitOfWork.SaveChanges();

                    var json = JsonConvert.SerializeObject(toDelete, _serializerSettings);
                    return Ok(json);
                }
            }
            catch (Exception e)
            {
                return BadRequest("ERROR: " + e.Message);
            }
        }

        [HttpGet]
        [Route("revisionMessages/{customerSoftwareId}")]
        public IActionResult GetRevisionMessages([FromRoute] string customerSoftwareId)
        {
            CustomerSoftware customerSoftware = UnitOfWork.CustomerSoftwares.GetOrNull(customerSoftwareId);

            if (customerSoftware == null)
            {
                return BadRequest("ERROR: The customer software does not exist");
            }

            Software sw = UnitOfWork.Software.GetOrNull(customerSoftware.SoftwareId);

            if (sw == null)
            {
                return BadRequest("ERROR: The software does not exist");
            }

            List<RevisionMessage> revisionMessages = UnitOfWork.RevisionMessages.GetAll().Where(x => x.SoftwareId == sw.Id).ToList();

            var json = JsonConvert.SerializeObject(revisionMessages, _serializerSettings);
            return Ok(json);
        }

        [HttpPost]
        [Route("edit/customerSoftwareStream/{streamId}")]
        public IActionResult EditCustomerSoftwareStream([FromRoute] string streamId, [FromBody] PersonAndReleasePlanViewModel data)
        {
            CustomerSoftwareStream stream = UnitOfWork.CustomerSoftwareStreamss.GetOrNull(streamId, "StreamMembers", "ApplicationOwner");
            if (stream == null)
            {
                return BadRequest("ERROR: The customer software stream does not exist");
            }
            stream.IsEnterpriseStream = data.IsEnterpriseStream;
            if (!stream.IsEnterpriseStream)
            {
                stream.Priority = 0;
            }
            stream.UpdateSettings = data.UpdateSettings;
            stream.Priority = data.Priority;
            stream.ClientOrServer = data.ClientOrServer;
            Person person = UnitOfWork.Persons.GetOrNull(data.ApplicationOwnerId);
            ReleasePlan releasePlan = UnitOfWork.ReleasePlans.GetOrNull(data.ReleasePlanId);

            if (releasePlan != null)
            {
                stream.ReleasePlan = releasePlan;
            }
            if (person != null)
            {
                stream.ApplicationOwner = person;
            }
            UnitOfWork.CustomerSoftwareStreamss.MarkForUpdate(stream, GetCurrentUser().Id);
            UnitOfWork.SaveChanges();

            SoftwareStream origin = UnitOfWork.SoftwareStreams.Get(stream.SoftwareStreamId, "Icon");
            stream.Icon = origin.Icon;

            var json = JsonConvert.SerializeObject(stream, _serializerSettings);
            return Ok(json);
        }

        // TODO: Test it
        [HttpGet]
        [Route("newSoftware")]
        public IActionResult GetNewSoftware([FromRoute] string customerId)
        {
            try
            {
                List<Software> result = new List<Software>();
                // Get all software streams 
                using (var unitOfWork = CreateUnitOfWork())
                {
                    Customer customer = unitOfWork.Customers.GetOrNull(customerId);
                    if (customer == null)
                    {
                        return BadRequest("ERROR: The customer does not exist");
                    }
                    List<CustomerSoftwareStream> customerStreams = unitOfWork.CustomerSoftwareStreamss.GetAll("StreamMembers")
                            .Where(x => x.CustomerId == customerId).ToList();

                    // Get latest version of customer used software package
                    string latestCustomerVersion;
                    foreach (CustomerSoftwareStream stream in customerStreams)
                    {
                        latestCustomerVersion = String.Empty;
                        foreach (CustomerSoftware swpackage in stream.StreamMembers)
                        {
                            if (latestCustomerVersion == String.Empty)
                            {
                                latestCustomerVersion = swpackage.Version;
                            }
                            else
                            {
                                if (IsLaterVersion(latestCustomerVersion, swpackage.Version))
                                {
                                    latestCustomerVersion = swpackage.Version;
                                }
                            }
                        }

                        // Compare to admin stream latest version
                        SoftwareStream adminStream = unitOfWork.SoftwareStreams.GetOrNull(stream.SoftwareStreamId, "StreamMembers", "StreamMembers.Customers");
                        if (adminStream != null)
                        {
                            string latestAdminSWVersion = String.Empty;
                            foreach (Software software in adminStream.StreamMembers)
                            {
                                bool skip = false;
                                if (software.Customers != null && software.Customers.Count != 0)
                                {
                                    if (software.Customers.Find(x => x.CustomerId == customerId) == null)
                                    {
                                        skip = true;
                                    }
                                }
                                if (!skip)
                                {
                                    if (software.PublishInShop)
                                    {
                                        if (software.DeletedDate == null)
                                        {
                                            if (latestAdminSWVersion == String.Empty)
                                            {
                                                latestAdminSWVersion = software.Version;
                                            }
                                            else
                                            {
                                                if (IsLaterVersion(latestAdminSWVersion, software.Version))
                                                {
                                                    latestAdminSWVersion = software.Version;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            if (IsLaterVersion(latestCustomerVersion, latestAdminSWVersion))
                            {
                                result.Add(adminStream.StreamMembers.Find(x => x.Version == latestAdminSWVersion));
                            }
                        }
                    }
                    // Return result
                    foreach (Software sw in result)
                    {
                        sw.Customers = null;
                    }
                    var json = JsonConvert.SerializeObject(result, _serializerSettings);
                    return Ok(json);
                }
            }
            catch (Exception e)
            {
                var st = new StackTrace(e, true);
                // Get the top stack frame
                var frame = st.GetFrame(0);
                // Get the line number from the stack frame
                var line = frame.GetFileLineNumber();
                return BadRequest("ERROR: " + e.Message + " line: " + line);
            }
        }

        [HttpGet]
        [Route("checkRevisionNumbers")]
        public IActionResult GetRevisionSoftware([FromRoute] string customerId)
        {
            List<Software> result = new List<Software>();
            // Get all software streams 
            using (var unitOfWork = CreateUnitOfWork())
            {
                Customer customer = unitOfWork.Customers.GetOrNull(customerId);
                if (customer == null)
                {
                    return BadRequest("ERROR: The customer does not exist");
                }
                List<CustomerSoftwareStream> customerStreams = unitOfWork.CustomerSoftwareStreamss.GetAll("StreamMembers")
                        .Where(x => x.CustomerId == customerId).ToList();
                foreach (CustomerSoftwareStream stream in customerStreams)
                {
                    foreach (CustomerSoftware csw in stream.StreamMembers)
                    {
                        Software sw = unitOfWork.Software.GetOrNull(csw.SoftwareId);
                        if (sw != null)
                        {
                            string guid = Guid.NewGuid().ToString();
                            if (sw.RevisionNumber == null)
                            {
                                sw.RevisionNumber = guid;
                                csw.RevisionNumber = guid;
                                unitOfWork.Software.MarkForUpdate(sw, GetCurrentUser().Id);
                                unitOfWork.CustomerSoftwares.MarkForUpdate(csw, GetCurrentUser().Id);
                                unitOfWork.SaveChanges();
                            }
                            else
                            {
                                if (csw.RevisionNumber != sw.RevisionNumber)
                                {
                                    result.Add(sw);
                                }
                            }
                        }
                    }
                }
            }
            var json = JsonConvert.SerializeObject(result, _serializerSettings);
            return Ok(json);
        }


        /**
         * Shop a new software packafe for a software stream already used by the specific customer.
         **/
        [HttpPost]
        [Route("shopLatestVersion/{softwareId}")]
        public async Task<IActionResult> ShopLatestVersion([FromRoute] string customerId, [FromRoute] string softwareId)
        {
            try
            {
                // TODO: Shop new sw Package and add it to customer sw stream
                using (var unitOfWork = CreateUnitOfWork())
                {
                    WPM_API.Data.DataContext.Entities.Customer customer = unitOfWork.Customers.GetOrNull(customerId, CustomerIncludes.GetAllIncludes());
                    var cep = GetCEP(customerId);
                    Software software = unitOfWork.Software.GetOrNull(softwareId, SoftwareIncludes.GetAllIncludes());
                    if (software == null)
                    {
                        return BadRequest("ERROR: The software does not exist: " + softwareId);
                    }
                    CustomerSoftwareStream customerStream
                        = unitOfWork.CustomerSoftwareStreamss.GetAll("StreamMembers")
                        .Where(x => x.SoftwareStreamId == software.SoftwareStreamId && x.CustomerId == customerId)
                        .FirstOrDefault();
                    StorageEntryPoint csdp = null;
                    if (customer == null)
                    {
                        return BadRequest("ERROR: The customer does not exist");
                    }
                    csdp = customer.StorageEntryPoints.Find(x => x.IsCSDP == true);
                    if (cep == null && !csdp.Managed)
                    {
                        return BadRequest("ERROR: The csdp Cloud Entry Point is not set");
                    }
                    if (csdp == null)
                    {
                        return BadRequest("ERROR: The csdp storage entry point does not exist or has not been created yet");
                    }
                    List<FileAndSAS> sasKeys = new List<FileAndSAS>();

                    // Create needed Azrue connections
                    if (_appSettings == null || _connectionStrings == null)
                    {
                        return BadRequest("ERROR: Cannot fetch Bitstream Azure connection from config files");
                    }
                    FileRepository.FileRepository csdpBitstreamRepo = new FileRepository.FileRepository(_connectionStrings.FileRepository, _appSettings.FileRepositoryFolder);
                    // TODO: upload Files from admin sw
                    AzureCommunicationService azureCustomer;
                    if (!csdp.Managed)
                    {
                        azureCustomer = new AzureCommunicationService(cep.TenantId, cep.ClientId, cep.ClientSecret);
                    }
                    else
                    {
                        // TODO: Check for system; fix for live system
                        azureCustomer = new AzureCommunicationService(_appSettings.DevelopmentTenantId, _appSettings.DevelopmentClientId, _appSettings.DevelopmentClientSecret);
                    }
                    foreach (Data.DataContext.Entities.File file in software.TaskInstall.Files)
                    {
                        if (file.Guid == null)
                        {
                            return BadRequest("ERROR: The file has no Guid for Azure");
                        }
                        FileAndSAS sasKey = await csdpBitstreamRepo.GetSASFile(file.Name, file.Guid);
                        if (sasKey == null)
                        {
                            return BadRequest("ERROR: The sas key for " + file.Name + " is null");
                        }
                        sasKeys.Add(sasKey);
                    }

                    List<Microsoft.Azure.Management.Storage.Models.StorageAccount> storageAccounts = await azureCustomer.StorageService().GetStorageAccounts(csdp.SubscriptionId, csdp.ResourceGrpName);
                    Microsoft.Azure.Management.Storage.Models.StorageAccount storageAccountCustomer = storageAccounts.Find(x => x.Name == csdp.StorageAccount);
                    if (storageAccountCustomer == null)
                    {
                        return BadRequest("ERROR: Your csdp in Azrue has been deleted. Please restore the storage account");
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
                    // Create new CustomerSoftware
                    CustomerSoftware newCustomerSoftware = Mapper.Map<CustomerSoftware>(software);
                    newCustomerSoftware.Id = null;
                    newCustomerSoftware.SoftwareId = softwareId;
                    newCustomerSoftware.CustomerStatus = "Test";
                    newCustomerSoftware.CustomerSoftwareStreamId = customerStream.Id;
                    customerStream.StreamMembers.Add(newCustomerSoftware);
                    unitOfWork.CustomerSoftwares.MarkForInsert(newCustomerSoftware, GetCurrentUser().Id);
                    unitOfWork.CustomerSoftwareStreamss.MarkForUpdate(customerStream, GetCurrentUser().Id);
                    unitOfWork.SaveChanges();

                    SoftwareViewModel result = Mapper.Map<SoftwareViewModel>(software);
                    var json = JsonConvert.SerializeObject(result, _serializerSettings);
                    return Ok(json);
                }
            }
            catch (Exception e)
            {
                return BadRequest("ERROR: " + e.Message);
            }
        }

        [HttpPost]
        [Route("repairSoftware/{softwareId}")]
        public async Task<IActionResult> RepairSoftwarePackage([FromRoute] string customerId, [FromRoute] string softwareId)
        {
            using (var unitOfWork = CreateUnitOfWork())
            {
                // TODO: update customer software package with data and update/delete files in Azure
                try
                {
                    WPM_API.Data.DataContext.Entities.Customer customer = unitOfWork.Customers.GetOrNull(customerId, CustomerIncludes.GetAllIncludes());
                    var cep = GetCEP(customerId);
                    Software software = unitOfWork.Software.GetOrNull(softwareId, SoftwareIncludes.GetAllIncludes());
                    CustomerSoftwareStream customerStream = unitOfWork.CustomerSoftwareStreamss.GetAll("StreamMembers")
                        .Where(x => x.SoftwareStreamId == software.SoftwareStreamId && x.CustomerId == customer.Id).FirstOrDefault();
                    if (customerStream == null)
                    {
                        return BadRequest("ERROR: The customer stream does not exist!");
                    }
                    CustomerSoftware customerSoftware = unitOfWork.CustomerSoftwares.GetAll()
                        .Where(x => x.SoftwareId == softwareId && x.CustomerSoftwareStreamId == customerStream.Id)
                        .FirstOrDefault();
                    StorageEntryPoint csdp = null;
                    if (customer == null)
                    {
                        return BadRequest("ERROR: The customer does not exist");
                    }
                    csdp = customer.StorageEntryPoints.Find(x => x.IsCSDP == true);
                    if (cep == null && !csdp.Managed)
                    {
                        return BadRequest("ERROR: The csdp Cloud Entry Point is not set");
                    }
                    if (software == null)
                    {
                        return BadRequest("ERROR: The software does not exist");
                    }
                    if (customerSoftware == null)
                    {
                        return BadRequest("ERROD: The customer software does not exist");
                    }
                    if (csdp == null)
                    {
                        return BadRequest("ERROR: The csdp storage entry point does not exist or has not been created yet");
                    }
                    List<FileAndSAS> sasKeys = new List<FileAndSAS>();

                    // Create needed Azrue connections
                    if (_appSettings == null || _connectionStrings == null)
                    {
                        return BadRequest("ERROR: Cannot fetch Bitstream Azure connection from config files");
                    }
                    FileRepository.FileRepository csdpBitstreamRepo = new FileRepository.FileRepository(_connectionStrings.FileRepository, _appSettings.FileRepositoryFolder);
                    // TODO: upload Files from admin sw
                    AzureCommunicationService azureCustomer;
                    if (!csdp.Managed)
                    {
                        azureCustomer = new AzureCommunicationService(cep.TenantId, cep.ClientId, cep.ClientSecret);
                    }
                    else
                    {
                        // TODO: Check for system; fix for live system
                        azureCustomer = new AzureCommunicationService(_appSettings.DevelopmentTenantId, _appSettings.DevelopmentClientId, _appSettings.DevelopmentClientSecret);
                    }
                    foreach (Data.DataContext.Entities.File file in software.TaskInstall.Files)
                    {
                        if (file.Guid == null)
                        {
                            return BadRequest("ERROR: The file has no Guid for Azure");
                        }
                        FileAndSAS sasKey = await csdpBitstreamRepo.GetSASFile(file.Name, file.Guid);
                        if (sasKey == null)
                        {
                            return BadRequest("ERROR: The sas key for " + file.Name + " is null");
                        }
                        sasKeys.Add(sasKey);
                    }

                    List<Microsoft.Azure.Management.Storage.Models.StorageAccount> storageAccounts = await azureCustomer.StorageService().GetStorageAccounts(csdp.SubscriptionId, csdp.ResourceGrpName);
                    Microsoft.Azure.Management.Storage.Models.StorageAccount storageAccountCustomer = storageAccounts.Find(x => x.Name == csdp.StorageAccount);
                    if (storageAccountCustomer == null)
                    {
                        return BadRequest("ERROR: Your csdp in Azrue has been deleted. Please restore the storage account");
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
                    customerSoftware.DisplayRevisionNumber = software.DisplayRevisionNumber;
                    if (software.RevisionNumber != null)
                    {
                        customerSoftware.RevisionNumber = software.RevisionNumber;
                        unitOfWork.CustomerSoftwareStreamss.MarkForUpdate(customerStream, GetCurrentUser().Id);
                        unitOfWork.CustomerSoftwares.MarkForUpdate(customerSoftware, GetCurrentUser().Id);
                        unitOfWork.SaveChanges();
                    }
                    else
                    {
                        string revisionNr = new Guid().ToString();
                        software.RevisionNumber = revisionNr;
                        customerSoftware.RevisionNumber = revisionNr;
                        unitOfWork.Software.MarkForUpdate(software, GetCurrentUser().Id);
                        unitOfWork.CustomerSoftwareStreamss.MarkForUpdate(customerStream, GetCurrentUser().Id);
                        unitOfWork.CustomerSoftwares.MarkForUpdate(customerSoftware, GetCurrentUser().Id);
                        unitOfWork.SaveChanges();
                    }
                    SoftwareViewModel result = Mapper.Map<SoftwareViewModel>(software);
                    var json = JsonConvert.SerializeObject(result, _serializerSettings);
                    return Ok(json);
                }
                catch (Exception e)
                {
                    return BadRequest("ERROR: " + e.Message);
                }
            }
        }

        [HttpPut]
        [Route("activateLatestSoftwares")]
        public IActionResult ActivateLatestSWVersionForStreams([FromRoute] string customerId)
        {
            using (var unitOfWork = CreateUnitOfWork())
            {
                List<CustomerSoftwareStream> streams = unitOfWork.CustomerSoftwareStreamss.GetAll("StreamMembers").Where(x => x.CustomerId == customerId).ToList();
                foreach (CustomerSoftwareStream stream in streams)
                {
                    var streamMembers = stream.StreamMembers.OrderByDescending(x => x.Version).ToList();
                    CustomerSoftware toActivate = streamMembers.First();
                    toActivate.CustomerStatus = "Active";

                    unitOfWork.CustomerSoftwares.MarkForUpdate(toActivate, GetCurrentUser().Id);
                    unitOfWork.SaveChanges();
                }
                return Ok();
            }
        }

        [HttpPost]
        [Route("setApplicationOwners")]
        public IActionResult SetApplicationOwner([FromBody] SetAppOwnerModel data)
        {
            using (var unitOfWork = CreateUnitOfWork())
            {
                foreach (CustomerSWViewModel customerSWStream in data.SelectedSoftwareStreams)
                {
                    CustomerSoftwareStream stream = unitOfWork.CustomerSoftwareStreamss.Get(customerSWStream.Id);
                    stream.ApplicationOwnerId = data.ApplicationOwnerId;

                    unitOfWork.CustomerSoftwareStreamss.MarkForUpdate(stream, GetCurrentUser().Id);
                    unitOfWork.SaveChanges();
                }
                return Ok();
            }
        }

        private bool IsLaterVersion(string currentVersion, string toCompare)
        {
            try
            {
                System.Version currentPackageVersion = System.Version.Parse(currentVersion);
                System.Version toCompareVersion = System.Version.Parse(toCompare);
                int compareValue = toCompareVersion.CompareTo(currentPackageVersion);
                if (compareValue == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                return false;
            }

            /*
            string[] currentVersionArray = currentVersion.Split(".");
            string[] toCompareArray = toCompare.Split(".");
            if (toCompareArray.Length != currentVersionArray.Length)
            {
                return false;
            }
            for (int i = 0; i < toCompareArray.Length; i++)
            {
                if (int.Parse(toCompareArray[i]) > int.Parse(currentVersionArray[i]))
                {
                    return true;
                }
                else if (int.Parse(toCompareArray[i]) < int.Parse(currentVersionArray[i]))
                {
                    return false;
                }
            }
            return false;
            */
        }

        public class PersonAndReleasePlanViewModel
        {
            public string ApplicationOwnerId { get; set; }
            public string ReleasePlanId { get; set; }
            public bool IsEnterpriseStream { get; set; }
            public string UpdateSettings { get; set; }
            public int Priority { get; set; }
            public string ClientOrServer { get; set; }
        }

        public class CustomerSWViewModel
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string UpdateSettings { get; set; }
            public string ApplicationOwnerId { get; set; }
        }

        public class SetAppOwnerModel
        {
            public List<CustomerSWViewModel> SelectedSoftwareStreams { get; set; }
            public string ApplicationOwnerId { get; set; }
        }
    }
}
