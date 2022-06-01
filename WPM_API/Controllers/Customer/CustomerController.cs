using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using WPM_API.Azure;
using WPM_API.Code.Infrastructure;
using WPM_API.Code.Infrastructure.LogOn;
using WPM_API.Common;
using WPM_API.Data.DataContext.Entities;
using WPM_API.Data.DataContext.Entities.Storages;
using WPM_API.Data.Models;
using WPM_API.Models;
using WPM_API.Options;

namespace WPM_API.Controllers
{
    [Route("customers")]
    public class CustomerController : BasisController
    {
        public CustomerController(AppSettings appSettings, ConnectionStrings connectionStrings, OrderEmailOptions orderEmailOptions, AgentEmailOptions agentEmailOptions, SendMailCreds sendMailCreds, SiteOptions siteOptions, ILogonManager logonManager) : base(appSettings, connectionStrings, orderEmailOptions, agentEmailOptions, sendMailCreds, siteOptions, logonManager)
        {
        }

        /// <summary>
        /// Retrieve all customers.
        /// </summary>
        /// <returns>[Customer]</returns>
        [HttpGet]
        [Authorize(Policy = Constants.Roles.Systemhouse)]
        public IActionResult GetCustomers()
        {
            try
            {
                List<CustomerViewModel> result = new List<CustomerViewModel>();
                List<WPM_API.Data.DataContext.Entities.Customer> customers = new List<WPM_API.Data.DataContext.Entities.Customer>();

                if (CurrentUserIsInRole(Constants.Roles.Admin))
                {
                    // Admin: Darf alle sehen
                    customers = UnitOfWork.Customers.GetAll(CustomerIncludes.Systemhouse, CustomerIncludes.Banner, CustomerIncludes.IconRight, CustomerIncludes.IconLeft).ToList();
                }
                else if (CurrentUserIsInRole(Constants.Roles.Systemhouse))
                {
                    string systemhouseId = (GetCurrentUser()).SystemhouseId;
                    // Systemhouse_Manager: Darf alle eigenen sehen
                    customers = UnitOfWork.Customers.GetAll(CustomerIncludes.Systemhouse, CustomerIncludes.Banner, CustomerIncludes.IconRight, CustomerIncludes.IconLeft)
                                .Where(x => x.SystemhouseId == systemhouseId).ToList();
                }

                result = Mapper.Map<List<WPM_API.Data.DataContext.Entities.Customer>, List<CustomerViewModel>>(customers);

                var json = JsonConvert.SerializeObject(result, serializerSettings);
                return new OkObjectResult(json);
            }
            catch (Exception e)
            {
                return BadRequest("ERROR: " + e.Message);
            }
        }

        [HttpGet]
        [Authorize(Policy = Constants.Policies.Customer)]
        [Route("getGeneralSettings/{customerId}")]
        public IActionResult GetGeneralSettings([FromRoute] string customerId)
        {
            Customer customer = UnitOfWork.Customers.Get(customerId);
            customer.AutoRegisterPassword = DecryptString(customer.AutoRegisterPassword);

            var json = JsonConvert.SerializeObject(customer, serializerSettings);
            return Ok(json);
        }

        [HttpPost]
        [Authorize(Policy = Constants.Policies.Customer)]
        [Route("setGeneralSettings/{customerId}")]
        public IActionResult SetGeneralSettings([FromBody] GeneralSettingsModel data, [FromRoute] string customerId)
        {
            using (var unitOfWork = CreateUnitOfWork())
            {
                Customer customer = unitOfWork.Customers.Get(customerId);
                customer.AutoRegisterPassword = EncryptString(data.AutoRegisterPassword);
                customer.AutoRegisterClients = data.AutoRegisterClients;

                unitOfWork.Customers.MarkForUpdate(customer, GetCurrentUser().Id);
                unitOfWork.SaveChanges();
                return Ok();
            }
        }

        [HttpPost]
        [Authorize(Policy = Constants.Policies.Systemhouse)]
        [Route("getCustomersClients")]
        public IActionResult GetCustomersClients([FromBody] List<string> customers)
        {
            using (var unitOfWork = CreateUnitOfWork())
            {
                List<WPM_API.Data.DataContext.Entities.Client> result = new List<WPM_API.Data.DataContext.Entities.Client>();
                foreach (string id in customers)
                {
                    result.AddRange(unitOfWork.Clients.GetAll().Where(x => x.CustomerId == id).ToList());
                }

                var json = JsonConvert.SerializeObject(Mapper.Map<List<ClientViewModel>>(result), serializerSettings);
                return Ok(json);
            }
        }

        /// <summary>
        /// Get a single customer
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        [Route("{customerId}")]
        [HttpGet]
        [Authorize(Policy = Constants.Policies.Customer)]
        public IActionResult GetCustomer([FromRoute] string customerId)
        {
            CustomerViewModel result = new CustomerViewModel();
            WPM_API.Data.DataContext.Entities.Customer customer = UnitOfWork.Customers.Get(customerId, CustomerIncludes.GetAllIncludes());
            var currentUser = GetCurrentUser();
            if (!CurrentUserIsInRole(Constants.Roles.Admin))
            {
                if (currentUser.SystemhouseId != customer.SystemhouseId)
                {
                    return BadRequest("Systemhouse not authorized.");
                }
            }

            result = Mapper.Map<WPM_API.Data.DataContext.Entities.Customer, CustomerViewModel>(customer);

            var json = JsonConvert.SerializeObject(result, serializerSettings);
            return new OkObjectResult(json);
        }

        [HttpGet]
        [Route("getWinPESAS/{customerId}")]
        [Authorize(Policy = Constants.Policies.Customer)]
        public IActionResult getWinPESAS([FromRoute] string customerId)
        {
            WPM_API.Data.DataContext.Entities.Customer customer = UnitOfWork.Customers.GetOrNull(customerId);
            if (customer == null)
            {
                return BadRequest("ERROR: The customer does not exist");
            }
            CustomerNameViewModel result = Mapper.Map<CustomerNameViewModel>(customer);
            if (result.WinPEDownloadLink == null || result.WinPEDownloadLink.Length == 0)
            {
                result.WinPEDownloadLink = "https://bitstream.blob.core.windows.net/download-repository/BitStream/WinPEv2004.06_BitStream.iso?sv=2019-10-10&st=2020-11-01T00%3A00%3A00Z&se=2022-03-31T00%3A00%3A00Z&sr=b&sp=r&sig=SYt4urPVWsiBDM%2BiXjlhLqLz7LMQoiQ2uYQJhzFAP1g%3D";
            }

            var json = JsonConvert.SerializeObject(result, serializerSettings);

            return Ok(json);
        }

        [HttpGet]
        [Route("iconOrBanner/{fileId}")]
        public async Task<IActionResult> DownloadIconOrBanner([FromRoute] string fileId)
        {
            try
            {
                FileRepository.FileRepository software = new FileRepository.FileRepository(connectionStrings.FileRepository, appSettings.IconsAndBanners);
                var file = UnitOfWork.Files.Get(fileId);
                string fileName = "";
                if (await software.FindFileAsync(fileId))
                {
                    fileName = fileId;
                }
                else if (await software.FindFileAsync(file.Guid))
                {
                    fileName = file.Guid;
                }
                if (fileName != "")
                {
                    var blob = software.GetBlobFile(fileName);
                    var ms = new MemoryStream();
                    await blob.DownloadToAsync(ms);
                    ms.Seek(0, SeekOrigin.Begin);
                    return File(ms, System.Net.Mime.MediaTypeNames.Application.Octet, file.Name);
                }
                else
                {
                    return BadRequest("ERROR: The file does not exist anymore. Please upload a new icon or banner");
                }
            }
            catch (Exception e)
            {
                return BadRequest("ERROR: The file does not exist anymore. Please upload a new icon or banner");
            }
        }

        [HttpDelete]
        [Route("iconOrBanner/{fileId}")]
        public async Task<IActionResult> DeleteIconOrBanner([FromRoute] string fileId)
        {
            try
            {
                FileRepository.FileRepository iconsAndBanners = new FileRepository.FileRepository(connectionStrings.FileRepository, appSettings.IconsAndBanners);
                var exists = await iconsAndBanners.FindFileAsync(fileId);
                if (exists)
                {
                    var deleteSuccess = await iconsAndBanners.DeleteFile(fileId);
                    if (deleteSuccess)
                    {
                        return Ok();
                    }
                    else
                    {
                        return BadRequest("ERROR: The file could not be deleted");
                    }
                }
                else
                {
                    return Ok();
                }
            }
            catch (Exception e)
            {
                return BadRequest("ERROR: " + e.Message);
            }
        }

        [HttpPost]
        [Route("unattend/upload/{customerId}")]
        public async Task<IActionResult> UploadUnattendFile(Microsoft.AspNetCore.Http.IFormFile file, [FromRoute] string customerId)
        {
            using (var unitOfWork = CreateUnitOfWork())
            {
                try
                {
                    // Get needed data for file transfer to Azure
                    WPM_API.Data.DataContext.Entities.Customer customer = unitOfWork.Customers.GetAll("StorageEntryPoints", "OwnUnattendFile").Where(x => x.Id == customerId).FirstOrDefault();
                    if (customer == null)
                    {
                        return BadRequest("ERROR: The customer does not exist");
                    }
                    CloudEntryPoint cep = GetCEP(customerId);
                    StorageEntryPoint csdp = customer.StorageEntryPoints.Find(x => x.IsCSDP == true);
                    if (csdp == null)
                    {
                        return BadRequest("ERROR: There is not Storage Entry Point set yet");
                    }

                    // Set up Azure connection
                    AzureCommunicationService azureCustomer;
                    if (csdp.Managed)
                    {
                        azureCustomer = new AzureCommunicationService(appSettings.DevelopmentTenantId, appSettings.DevelopmentClientId, appSettings.DevelopmentClientSecret);
                    }
                    else
                    {
                        if (cep == null)
                        {
                            return BadRequest("ERROR: The Cloud Entry Point is not set or does not exist anymore");
                        }

                        azureCustomer = new AzureCommunicationService(cep.TenantId, cep.ClientId, cep.ClientSecret);

                    }
                    string connectionString = azureCustomer.StorageService().GetStorageAccConnectionString(csdp.SubscriptionId, csdp.ResourceGrpName, csdp.StorageAccount);

                    CloudStorageAccount customerStorageAccount = CloudStorageAccount.Parse(connectionString);
                    CloudBlobClient customerClient = customerStorageAccount.CreateCloudBlobClient();
                    CloudBlobContainer csdpContainer = customerClient.GetContainerReference(csdp.BlobContainerName);

                    var sas = csdpContainer.GetSharedAccessSignature(new SharedAccessBlobPolicy()
                    {
                        SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(15),
                        Permissions = SharedAccessBlobPermissions.Write | SharedAccessBlobPermissions.Read | SharedAccessBlobPermissions.List
                    });

                    CloudBlockBlob blob = csdpContainer.GetBlockBlobReference("Customer_Repository/Win10_20H2_DE_unattend_v02.xml");
                    blob.Properties.ContentType = file.ContentType;
                    await blob.UploadFromStreamAsync(file.OpenReadStream());
                    return Ok();
                }
                catch (Exception e)
                {
                    return BadRequest("ERROR: " + e.Message);
                }
            }
        }

        [HttpPost]
        [Route("oemPartition/upload/{customerId}")]
        public async Task<IActionResult> UploadOEMPartitionFile(Microsoft.AspNetCore.Http.IFormFile file, [FromRoute] string customerId)
        {
            using (var unitOfWork = CreateUnitOfWork())
            {
                try
                {
                    // Get needed data for file transfer to Azure
                    WPM_API.Data.DataContext.Entities.Customer customer = unitOfWork.Customers.GetAll("StorageEntryPoints").Where(x => x.Id == customerId).FirstOrDefault();
                    if (customer == null)
                    {
                        return BadRequest("ERROR: The customer does not exist");
                    }
                    CloudEntryPoint cep = GetCEP(customerId);
                    StorageEntryPoint csdp = customer.StorageEntryPoints.Find(x => x.IsCSDP == true);
                    if (csdp == null)
                    {
                        return BadRequest("ERROR: There is not Storage Entry Point set yet");
                    }

                    // Set up Azure connection
                    AzureCommunicationService azureCustomer;
                    if (csdp.Managed)
                    {
                        azureCustomer = new AzureCommunicationService(appSettings.DevelopmentTenantId, appSettings.DevelopmentClientId, appSettings.DevelopmentClientSecret);
                    }
                    else
                    {
                        if (cep == null)
                        {
                            return BadRequest("ERROR: The Cloud Entry Point is not set or does not exist anymore");
                        }

                        azureCustomer = new AzureCommunicationService(cep.TenantId, cep.ClientId, cep.ClientSecret);

                    }
                    string connectionString = azureCustomer.StorageService().GetStorageAccConnectionString(csdp.SubscriptionId, csdp.ResourceGrpName, csdp.StorageAccount);

                    CloudStorageAccount customerStorageAccount = CloudStorageAccount.Parse(connectionString);
                    CloudBlobClient customerClient = customerStorageAccount.CreateCloudBlobClient();
                    CloudBlobContainer csdpContainer = customerClient.GetContainerReference(csdp.BlobContainerName);

                    var sas = csdpContainer.GetSharedAccessSignature(new SharedAccessBlobPolicy()
                    {
                        SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(15),
                        Permissions = SharedAccessBlobPermissions.Write | SharedAccessBlobPermissions.Read | SharedAccessBlobPermissions.List
                    });

                    CloudBlockBlob blob = csdpContainer.GetBlockBlobReference("Customer_Repository/Partition-Layout-Windows-UEFI_v02.txt");
                    blob.Properties.ContentType = file.ContentType;
                    await blob.UploadFromStreamAsync(file.OpenReadStream());
                    return Ok();
                }
                catch (Exception e)
                {
                    return BadRequest("ERROR: " + e.Message);
                }
            }
        }

        [HttpPost]
        [Route("deskBackground/upload/{customerId}")]
        public async Task<IActionResult> UploadDeskBackground(Microsoft.AspNetCore.Http.IFormFile file, [FromRoute] string customerId)
        {
            using (var unitOfWork = CreateUnitOfWork())
            {
                try
                {
                    // Get needed data for file transfer to Azure
                    WPM_API.Data.DataContext.Entities.Customer customer = unitOfWork.Customers.GetAll("StorageEntryPoints").Where(x => x.Id == customerId).FirstOrDefault();
                    if (customer == null)
                    {
                        return BadRequest("ERROR: The customer does not exist");
                    }
                    CloudEntryPoint cep = GetCEP(customerId);
                    StorageEntryPoint csdp = customer.StorageEntryPoints.Find(x => x.IsCSDP == true);
                    if (csdp == null)
                    {
                        return BadRequest("ERROR: There is not Storage Entry Point set yet");
                    }

                    // Set up Azure connection
                    AzureCommunicationService azureCustomer;
                    if (csdp.Managed)
                    {
                        azureCustomer = new AzureCommunicationService(appSettings.DevelopmentTenantId, appSettings.DevelopmentClientId, appSettings.DevelopmentClientSecret);
                    }
                    else
                    {
                        if (cep == null)
                        {
                            return BadRequest("ERROR: The Cloud Entry Point is not set or does not exist anymore");
                        }

                        azureCustomer = new AzureCommunicationService(cep.TenantId, cep.ClientId, cep.ClientSecret);

                    }
                    string connectionString = azureCustomer.StorageService().GetStorageAccConnectionString(csdp.SubscriptionId, csdp.ResourceGrpName, csdp.StorageAccount);

                    CloudStorageAccount customerStorageAccount = CloudStorageAccount.Parse(connectionString);
                    CloudBlobClient customerClient = customerStorageAccount.CreateCloudBlobClient();
                    CloudBlobContainer csdpContainer = customerClient.GetContainerReference(csdp.BlobContainerName);

                    var sas = csdpContainer.GetSharedAccessSignature(new SharedAccessBlobPolicy()
                    {
                        SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(15),
                        Permissions = SharedAccessBlobPermissions.Write | SharedAccessBlobPermissions.Read | SharedAccessBlobPermissions.List
                    });

                    // Check directory for existing .deskthemepack files and delete them
                    CloudBlobDirectory directory = csdpContainer.GetDirectoryReference("Software_Repository/Enterprise_Class/LaF-Package");
                    var fileList = await directory.ListBlobsSegmentedAsync(null);

                    for (int i = 0; i < fileList.Results.Count(); i++)
                    {
                        var element = fileList.Results.ElementAt(i);
                        if (element.GetType().Name == "CloudBlockBlob")
                        {
                            CloudBlockBlob tempBlob = (CloudBlockBlob)element;
                            if (tempBlob.Name.Contains(".deskthemepack"))
                            {
                                // Delete blob
                                await tempBlob.DeleteAsync();
                            }
                            // Check if the file has to be deleted
                        }
                    }

                    // Upload new .deskthemepack file
                    CloudBlockBlob blob = csdpContainer.GetBlockBlobReference("Software_Repository/Enterprise_Class/LaF-Package/" + file.FileName);
                    blob.Properties.ContentType = file.ContentType;
                    await blob.UploadFromStreamAsync(file.OpenReadStream());
                    return Ok();
                }
                catch (Exception e)
                {
                    return BadRequest("ERROR: " + e.Message);
                }
            }
        }

        [HttpPost]
        [Route("oemLogo/upload/{customerId}")]
        public async Task<IActionResult> UploadOEMLogoFile(Microsoft.AspNetCore.Http.IFormFile file, [FromRoute] string customerId)
        {
            using (var unitOfWork = CreateUnitOfWork())
            {
                try
                {
                    // Get needed data for file transfer to Azure
                    WPM_API.Data.DataContext.Entities.Customer customer = unitOfWork.Customers.GetAll("StorageEntryPoints").Where(x => x.Id == customerId).FirstOrDefault();
                    if (customer == null)
                    {
                        return BadRequest("ERROR: The customer does not exist");
                    }
                    CloudEntryPoint cep = GetCEP(customerId);
                    StorageEntryPoint csdp = customer.StorageEntryPoints.Find(x => x.IsCSDP == true);
                    if (csdp == null)
                    {
                        return BadRequest("ERROR: There is not Storage Entry Point set yet");
                    }

                    // Set up Azure connection
                    AzureCommunicationService azureCustomer;
                    if (csdp.Managed)
                    {
                        azureCustomer = new AzureCommunicationService(appSettings.DevelopmentTenantId, appSettings.DevelopmentClientId, appSettings.DevelopmentClientSecret);
                    }
                    else
                    {
                        if (cep == null)
                        {
                            return BadRequest("ERROR: The Cloud Entry Point is not set or does not exist anymore");
                        }

                        azureCustomer = new AzureCommunicationService(cep.TenantId, cep.ClientId, cep.ClientSecret);

                    }
                    string connectionString = azureCustomer.StorageService().GetStorageAccConnectionString(csdp.SubscriptionId, csdp.ResourceGrpName, csdp.StorageAccount);

                    CloudStorageAccount customerStorageAccount = CloudStorageAccount.Parse(connectionString);
                    CloudBlobClient customerClient = customerStorageAccount.CreateCloudBlobClient();
                    CloudBlobContainer csdpContainer = customerClient.GetContainerReference(csdp.BlobContainerName);

                    var sas = csdpContainer.GetSharedAccessSignature(new SharedAccessBlobPolicy()
                    {
                        SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(15),
                        Permissions = SharedAccessBlobPermissions.Write | SharedAccessBlobPermissions.Read | SharedAccessBlobPermissions.List
                    });

                    CloudBlockBlob blob = csdpContainer.GetBlockBlobReference("Customer_Repository/oemlogo.bmp");
                    blob.Properties.ContentType = file.ContentType;
                    await blob.UploadFromStreamAsync(file.OpenReadStream());
                    return Ok();
                }
                catch (Exception e)
                {
                    return BadRequest("ERROR: " + e.Message);
                }
            }
        }

        /// <summary>
        /// Check if a customer with a certain name is already in db.
        /// </summary>
        /// <param name="customerData"></param>
        /// <returns>CustomerName</returns>
        [HttpPost]
        [Authorize(Policy = Constants.Policies.Systemhouse)]
        public IActionResult CheckCustomerExistsByName([FromBody] CustomerNameViewModel customerData)
        {
            using (var unitOfWork = CreateUnitOfWork())
            {
                Customer customer = unitOfWork.Customers.GetAll()
                .Where(x => string.Compare(x.Name, customerData.Name) == 0)
                .SingleOrDefault();
                if (customer == null)
                {
                    customerData.Exists = false;
                }
                else
                {
                    customerData.Exists = true;
                }

                var json = JsonConvert.SerializeObject(customerData);
                return new OkObjectResult(json);
            }
        }

        [Route("add-parameter/check-unique")]
        [HttpPost]
        [Authorize(Policy = Constants.Policies.Customer)]
        public bool CheckParameterNameUnique([FromBody] StorageAccountNameViewModel name)
        {
            ClientParameter param = UnitOfWork.ClientParameters.GetAll().Where(x => x.ParameterName == name.Name).FirstOrDefault();
            if (param != null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Update existing customer.
        /// </summary>
        /// <param name="customerId">Id of the Customer</param>
        /// <param name="customerEdit">Modified Customer</param>
        /// <returns>Customer</returns>
        [HttpPut]
        [Authorize(Policy = Constants.Roles.Systemhouse)]
        [Route("{customerId}")]
        public async Task<IActionResult> UpdateCustomerAsync([FromRoute] string customerId, [FromBody] CustomerEditViewModel customerEdit)
        {
            using (var unitOfWork = CreateUnitOfWork())
            {
                WPM_API.Data.DataContext.Entities.Customer customer = new WPM_API.Data.DataContext.Entities.Customer();

                if (CurrentUserIsInRole(Constants.Roles.Admin))
                {
                    // Admin: Darf alle sehen
                    customer = unitOfWork.Customers.Get(customerId, CustomerIncludes.GetAllIncludes());
                    WPM_API.Data.DataContext.Entities.Customer checkName = unitOfWork.Customers.GetAll().Where(x => x.Name == customerEdit.Name).FirstOrDefault();
                    if (checkName != null)
                    {
                        if (checkName.Id != customer.Id)
                        {
                            return BadRequest("ERROR: There already exists a customer with the chosen Name! Please choose another name");
                        }
                    }
                    if (customer == null)
                    {
                        return NotFound("Customer not found.");
                    }
                    else
                    {
                        customer.Name = customerEdit.Name;
                        customer.SystemhouseId = customerEdit.SystemhouseId;
                        customer.WinPEDownloadLink = customerEdit.WinPEDownloadLink;

                        Parameter parameter = customer.Parameters.Find(x => x.Key == "$CustomerName");
                        if (parameter == null)
                        {
                            parameter = new Parameter();
                            parameter.Key = "$CustomerName";
                            parameter.Value = customer.Name;
                            parameter.IsEditable = false;
                            customer.Parameters.Add(parameter);
                            unitOfWork.Customers.MarkForUpdate(customer, GetCurrentUser().Id);

                        }
                        else
                        {
                            parameter.Value = customer.Name;
                            unitOfWork.Parameters.MarkForUpdate(parameter, GetCurrentUser().Id);
                        }

                        try
                        {
                            unitOfWork.SaveChanges();
                        }
                        catch (Exception e)
                        {
                            return BadRequest("Customer could not be changed.");
                        }
                    }
                }
                else if (CurrentUserIsInRole(Constants.Roles.Systemhouse))
                {
                    string systemhouseId = (GetCurrentUser()).SystemhouseId;
                    // Systemhouse_Manager: Darf alle eigenen sehen
                    customer = unitOfWork.Customers.Get(customerId, CustomerIncludes.GetAllIncludes());
                    if (customer == null)
                    {
                        return NotFound("Customer not found.");
                    }
                    else if (customer.SystemhouseId != systemhouseId)
                    {
                        return BadRequest("Systemhouse-manager can modify only their customers.");
                    }
                    else
                    {
                        customer.Name = customerEdit.Name;
                        customer.SystemhouseId = customerEdit.SystemhouseId;
                        Parameter parameter = customer.Parameters.Find(x => x.Key == "$CustomerName");
                        parameter.Value = customer.Name;
                        unitOfWork.Parameters.MarkForUpdate(parameter);

                        try
                        {
                            unitOfWork.SaveChanges();
                        }
                        catch (Exception)
                        {
                            return BadRequest("Customer could not be changed.");
                        }
                    }
                }

                // Load Customer again
                WPM_API.Data.DataContext.Entities.Customer result = unitOfWork.Customers.Get(customerId, CustomerIncludes.GetAllIncludes());

                // Customer was changed and is returned.
                var json = JsonConvert.SerializeObject(Mapper.Map<WPM_API.Data.DataContext.Entities.Customer, CustomerViewModel>(result), serializerSettings);
                return new OkObjectResult(json);
            }
        }


        /// <summary>
        /// Add or Edit a custom Corporate Parameter.
        /// </summary>
        /// <param name="customerId">Id of the Customer</param>
        /// <returns>text/plain</returns>
        [HttpPost]
        [Authorize(Policy = Constants.Roles.Customer)]
        [Route("{customerId}/parameters")]
        public IActionResult AddOrEditParameter([FromRoute] string customerId, [FromBody] ParameterViewModel parameterView)
        {
            using (var unitOfWork = CreateUnitOfWork())
            {
                WPM_API.Data.DataContext.Entities.Customer customer = unitOfWork.Customers.Get(customerId, CustomerIncludes.Parameters);
                Parameter parameter = customer.Parameters.Find(x => x.Id.Equals(parameterView.Id));
                if (parameter == null)
                {
                    parameter = new Parameter() { Key = parameterView.Key, IsEditable = true };
                    customer.Parameters.Add(parameter);
                }
                else
                {
                    parameter.Key = parameterView.Key;
                }

                parameter.Value = parameterView.Value;
                unitOfWork.SaveChanges();

                var json = JsonConvert.SerializeObject(Mapper.Map<ParameterViewModel>(parameter), serializerSettings);
                return new OkObjectResult(json);
            }
        }

        [HttpDelete]
        [Route("deleteParameter/{customerId}/{parameterId}")]
        [Authorize(Policy = Constants.Policies.Customer)]
        public IActionResult DeleteCustomerParameter([FromRoute] string customerId, [FromRoute] string parameterId)
        {
            using (var unitOfWork = CreateUnitOfWork())
            {
                WPM_API.Data.DataContext.Entities.Customer customer = unitOfWork.Customers.GetOrNull(customerId, "Parameters");
                if (customer == null)
                {
                    return BadRequest("ERROR: Customer does not exist");
                }
                Parameter toDelete = unitOfWork.Parameters.GetOrNull(parameterId);
                if (toDelete == null)
                {
                    return BadRequest("ERROR: The parameter does not exist");
                }
                customer.Parameters.Remove(toDelete);
                unitOfWork.Parameters.MarkForDelete(toDelete, GetCurrentUser().Id);
                unitOfWork.SaveChanges();
                return Ok();
            }
        }

        /// <summary>
        /// Update general data of a customer
        /// </summary>
        /// <param name="mainCompData"></param>
        /// <returns>AddCompany</returns>
        [HttpPut]
        [Authorize(Policy = Constants.Policies.Customer)]
        [Route("{customerId}/general")]
        public IActionResult AddOrUpdateMainCompany([FromBody] UpdateMainCompanyViewModel mainCompData)
        {
            WPM_API.Data.DataContext.Entities.Customer customer = UnitOfWork.Customers.GetAll("MainCompany").Where(x => x.Id == mainCompData.CustomerId).SingleOrDefault();
            Company mainCompany = customer.MainCompany;
            if (mainCompany == null)
            {
                mainCompany = UnitOfWork.Companies.CreateEmpty();
                mainCompany.CreatedByUserId = GetCurrentUser().Id;
                mainCompany.CreatedDate = DateTime.Now;
            }
            mainCompany.CorporateName = mainCompData.CorporateName;
            mainCompany.Description = mainCompData.Description;
            mainCompany.FormOfOrganization = mainCompData.FormOfOrganization;
            mainCompany.LinkWebsite = mainCompData.LinkWebsite;
            mainCompany.Type = mainCompData.Type;
            mainCompany.ExpertId = mainCompData.ExpertId;
            mainCompany.HeadquarterId = mainCompData.HeadquarterId;
            mainCompany.CustomerId = mainCompData.CustomerId;
            customer.MainCompanyId = mainCompany.Id;
            customer.UpdatedByUserId = GetCurrentUser().Id;
            customer.UpdatedDate = DateTime.Now;

            Person expert = UnitOfWork.Persons.GetOrNull(mainCompData.ExpertId);
            Location headquarter = UnitOfWork.Locations.GetOrNull(mainCompData.HeadquarterId);
            if (expert == null)
            {
                return BadRequest("The expert does not exist!");
            }
            if (headquarter == null)
            {
                return BadRequest("The headquarter does not exist!");
            }
            expert.CompanyId = mainCompany.Id;
            headquarter.CompanyId = mainCompany.Id;

            try
            {
                UnitOfWork.SaveChanges();
            }
            catch (Exception e)
            {
                return BadRequest("The general data could not be updated. " + e.Message);
            }

            mainCompany = UnitOfWork.Companies.GetOrNull(mainCompany.Id, "Expert", "Headquarter");

            var json = JsonConvert.SerializeObject(Mapper.Map<Company, CompanyOverviewViewModel>(mainCompany), serializerSettings);
            return new OkObjectResult(json);
        }

        /// <summary>
        /// Delete customer.
        /// </summary>
        /// <param name="customerId">Id of the Customer</param>
        /// <returns></returns>
        [HttpDelete]
        [Authorize(Policy = Constants.Roles.Systemhouse)]
        [Route("{customerId}")]
        public async Task<IActionResult> DeleteCustomer([FromRoute] string customerId)
        {
            WPM_API.Data.DataContext.Entities.Customer customer;

            if (CurrentUserIsInRole(Constants.Roles.Admin))
            {
                // Admin: Darf alle loeschen
                customer = UnitOfWork.Customers.Get(customerId, "StorageEntryPoints");
                if (customer == null)
                {
                    return NotFound("Customer not found.");
                }
                else
                {
                    await DeleteSEPs(customer);
                    UnitOfWork.Customers.MarkForDelete(customer, GetCurrentUser().Id);
                    try
                    {
                        UnitOfWork.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        return BadRequest("Customer could not be deleted. " + ex.Message);
                    }
                }
            }
            else if (CurrentUserIsInRole(Constants.Roles.Systemhouse))
            {
                string systemhouseId = (GetCurrentUser()).SystemhouseId;
                // Systemhouse_Manager: Darf alle eigenen sehen
                customer = UnitOfWork.Customers.Get(customerId);
                if (customer == null)
                {
                    return NotFound("Customer not found.");
                }
                else if (customer.SystemhouseId != systemhouseId)
                {
                    return BadRequest("Systemhouse-manager can delete only their customers.");
                }
                else
                {
                    await DeleteSEPs(customer);
                    UnitOfWork.Customers.MarkForDelete(customer, GetCurrentUser().Id);
                    try
                    {
                        UnitOfWork.SaveChanges();
                    }
                    catch (Exception)
                    {
                        return BadRequest("Customer could not be deleted.");
                    }
                }
            }

            // Customer was deleted.
            return NoContent();
        }

        private async System.Threading.Tasks.Task DeleteSEPs(WPM_API.Data.DataContext.Entities.Customer customer)
        {
            try
            {
                CloudEntryPoint cep = GetCEP(customer.Id);
                if (cep != null)
                {
                    AzureCommunicationService azure = new AzureCommunicationService(cep.TenantId, cep.ClientId, cep.ClientSecret);
                    // TODO: Delete SEPs
                    foreach (WPM_API.Data.DataContext.Entities.Storages.StorageEntryPoint sep in customer.StorageEntryPoints)
                    {
                        var exists = azure.ResourceGroupService().GetRessourceGroupByName(sep.ResourceGrpName, sep.SubscriptionId);
                        if (exists)
                        {
                            var success = await azure.ResourceGroupService().DeleteRessourceGroupAsync(sep.SubscriptionId, sep.ResourceGrpName);
                            if (success.Response.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                customer.StorageEntryPoints.Remove(sep);
                                UnitOfWork.StorageEntryPoints.MarkForDelete(sep, GetCurrentUser().Id);
                                UnitOfWork.SaveChanges();
                            }
                        }
                        else
                        {
                            customer.StorageEntryPoints.Remove(sep);
                            UnitOfWork.StorageEntryPoints.MarkForDelete(sep, GetCurrentUser().Id);
                            UnitOfWork.SaveChanges();
                        }
                    }
                }
            }
            catch (Exception e)
            {

            }
        }

        [HttpPost]
        [Authorize(Policy = Constants.Policies.Systemhouse)]
        [Route("{customerId}")]
        public IActionResult CreateAndSetMainCompany([FromBody] CompanyViewModel mainCompanyData, [FromRoute] string customerId)
        {
            // Load customer and check for existence
            WPM_API.Data.DataContext.Entities.Customer customer = UnitOfWork.Customers.Get(customerId);
            if (customer == null)
            {
                return BadRequest("The customer does not exist");
            }
            // Create new company and set values
            Company mainCompany = UnitOfWork.Companies.CreateEmpty();
            mainCompany.CorporateName = mainCompanyData.CorporateName;
            mainCompany.Description = mainCompanyData.Description;
            mainCompany.FormOfOrganization = mainCompanyData.FormOfOrganization;
            mainCompany.LinkWebsite = mainCompanyData.LinkWebsite;
            mainCompany.Type = mainCompanyData.Type;
            mainCompany.UpdatedByUserId = LoggedUser.Id;
            mainCompany.UpdatedDate = DateTime.Now;

            // Add mainCompany to customer
            mainCompany.CustomerId = customerId;
            customer.MainCompanyId = mainCompany.Id;
            customer.Companies.Add(mainCompany);

            // Finish DB transaction
            try
            {
                UnitOfWork.SaveChanges();
            }
            catch (Exception exc)
            {
                return BadRequest("MainCompany could not be created. " + exc.Message);
            }

            // Serialize result and return it
            var json = JsonConvert.SerializeObject(Mapper.Map<Company, CompanyViewModel>(mainCompany), serializerSettings);
            return new OkObjectResult(json);
        }

        /// <summary>
        /// Retrieve the main company of a customer with it's expert person data.
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns>Company</returns>
        [HttpGet]
        [Authorize(Policy = Constants.Policies.Customer)]
        [Route("mainCompany/{customerId}")]
        public IActionResult GetMainCompanyHeadquarterExpertData([FromRoute] string customerId)
        {
            WPM_API.Data.DataContext.Entities.Customer customer = null;
            // Load customer
            try
            {
                customer = UnitOfWork.Customers.GetAll(CustomerIncludes.MainCompanyExpert, CustomerIncludes.Headquarter).Where(x => x.Id == customerId).Single();
            }
            catch (Exception exc)
            {
                return BadRequest("The customer could not be loaded. " + exc.Message);
            }

            // Check if customer exists
            if (customer == null)
            {
                return BadRequest("The customer does not exist");
            }
            Company mainCompany = customer.MainCompany;

            // Serialize & return result
            var json = JsonConvert.SerializeObject(Mapper.Map<Company, CompanyViewModel>(mainCompany), serializerSettings);
            return new OkObjectResult(json);
        }

        /// <summary>
        /// Get all persons of a customer.
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns>List<PersonViewModel></returns>
        [HttpGet]
        [Authorize(Policy = Constants.Policies.Customer)]
        [Route("{customerId}/persons")]
        public IActionResult GetAllPersons([FromRoute] string customerId)
        {
            // Fetch data
            List<Person> customersPersons = UnitOfWork.Persons.GetAll(PersonIncludes.Company).Where(x => x.CustomerId == customerId).ToList();


            // Serialize & return the data
            var json = JsonConvert.SerializeObject(Mapper.Map<List<Person>, List<PersonViewModel>>(customersPersons), serializerSettings);
            return new OkObjectResult(json);
        }

        /// <summary>
        /// Get all companies of a customer
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns>List<CompanyViewModel></returns>
        [HttpGet]
        [Authorize(Policy = Constants.Policies.Customer)]
        [Route("{customerId}/companies")]
        public IActionResult GetAllCompanies([FromRoute] string customerId)
        {
            // Fetch data
            List<Company> customersCompanies = UnitOfWork.Companies.GetAll().Where(x => x.CustomerId == customerId).ToList();

            // Serialize & return the result
            var json = JsonConvert.SerializeObject(Mapper.Map<List<Company>, List<CompanyViewModel>>(customersCompanies), serializerSettings);
            return new OkObjectResult(json);
        }

        /// <summary>
        /// Get all locations of a customer
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns>List<LocationViewModel></returns>
        [HttpGet]
        [Authorize(Policy = Constants.Policies.Customer)]
        [Route("{customerId}/locations")]
        public IActionResult GetAllLocations([FromRoute] string customerId)
        {
            // Get db entries
            List<Location> customersLocations = UnitOfWork.Locations.GetAll("Customer").Where(x => x.CustomerId == customerId).ToList();

            // Serialize & return result
            var json = JsonConvert.SerializeObject(Mapper.Map<List<Location>, List<LocationViewModel>>(customersLocations), serializerSettings);
            return new OkObjectResult(json);
        }

        [HttpPost]
        [Route("customerSettings")]
        public async Task<IActionResult> SetSDSettings([FromBody] CustomerRefViewModel customerData)
        {
            using (var unitOfWork = CreateUnitOfWork())
            {
                WPM_API.Data.DataContext.Entities.Customer customer = unitOfWork.Customers.Get(customerData.Id, CustomerIncludes.GetAllIncludes());
                // Settings
                customer.Email = customerData.Email;
                customer.Phone = customerData.Phone;
                customer.OpeningTimes = customerData.OpeningTimes;
                customer.CmdBtn1 = customerData.CmdBtn1;
                customer.CmdBtn2 = customerData.CmdBtn2;
                customer.CmdBtn3 = customerData.CmdBtn3;
                customer.CmdBtn4 = customerData.CmdBtn4;
                customer.Btn1Label = customerData.Btn1Label;
                customer.Btn2Label = customerData.Btn2Label;
                customer.Btn3Label = customerData.Btn3Label;
                customer.Btn4Label = customerData.Btn4Label;
                customer.CsdpContainer = customerData.CsdpContainer;
                customer.CsdpRoot = customerData.CsdpRoot;
                customer.LtSASRead = customerData.LtSASRead;
                customer.LtSASWríte = customerData.LtSASWrite;
                customer.BannerLink = customerData.BannerLink;
                // Icons
                FileRepository.FileRepository iconsAndBanners = new FileRepository.FileRepository(connectionStrings.FileRepository, appSettings.IconsAndBanners);
                // Check icon right for update
                if (customerData.IconRight != null)
                {
                    if (customer.IconRight != null)
                    {
                        if (customer.IconRight.Id != customerData.IconRight.Id)
                        {
                            /*
                            // Delete old right icon
                            var guidIconRightDelete = "";
                            if (customer.IconRight != null && customer.IconRight.Guid != "")
                            {
                                guidIconRightDelete = customer.IconRight.Guid;
                                await iconsAndBanners.DeleteFile(guidIconRightDelete);
                            }
                            */
                            // Create new right icon
                            Data.DataContext.Entities.File newIconRight = UnitOfWork.Files.CreateEmpty();
                            newIconRight.Name = customerData.IconRight.Name;
                            newIconRight.Guid = customerData.IconRight.Id;
                            unitOfWork.Files.MarkForInsert(newIconRight);
                            unitOfWork.SaveChanges();
                            customer.IconRight = newIconRight;
                        }
                    }
                    else
                    {
                        if (customerData.IconRight != null)
                        {
                            // Create new right icon
                            Data.DataContext.Entities.File newIconRight = UnitOfWork.Files.CreateEmpty();
                            newIconRight.Name = customerData.IconRight.Name;
                            newIconRight.Guid = customerData.IconRight.Id;
                            unitOfWork.Files.MarkForInsert(newIconRight);
                            unitOfWork.SaveChanges();
                            customer.IconRight = newIconRight;
                        }
                    }
                }
                else
                {
                    customer.IconRight = null;
                }

                if (customerData.IconLeft != null)
                {

                    // Check icon left for update
                    if (customer.IconLeft != null)
                    {
                        if (customer.IconLeft.Id != customerData.IconLeft.Id)
                        {
                            /*
                            // Delete old left icon 
                            var guidIconLeftDelete = "";
                            if (customer.IconLeft != null && customer.IconLeft.Guid != "")
                            {
                                guidIconLeftDelete = customer.IconLeft.Guid;
                                await iconsAndBanners.DeleteFile(guidIconLeftDelete);
                            }
                            */
                            // Create new left icon
                            Data.DataContext.Entities.File newIconLeft = UnitOfWork.Files.CreateEmpty();
                            newIconLeft.Name = customerData.IconLeft.Name;
                            newIconLeft.Guid = customerData.IconLeft.Id;
                            unitOfWork.Files.MarkForInsert(newIconLeft);
                            unitOfWork.SaveChanges();
                            customer.IconLeft = newIconLeft;
                        }
                    }
                    else
                    {
                        if (customerData.IconLeft != null)
                        {
                            // Create new left icon
                            Data.DataContext.Entities.File newIconLeft = UnitOfWork.Files.CreateEmpty();
                            newIconLeft.Name = customerData.IconLeft.Name;
                            newIconLeft.Guid = customerData.IconLeft.Id;
                            unitOfWork.Files.MarkForInsert(newIconLeft);
                            unitOfWork.SaveChanges();
                            customer.IconLeft = newIconLeft;
                        }
                    }
                }
                else
                {
                    customer.IconLeft = null;
                }

                // Check banner for update
                if (customerData.Banner != null)
                {
                    if (customer.Banner != null)
                    {
                        if (customer.Banner.Id != customerData.Banner.Id)
                        {
                            /*
                            // Delete old banner
                            var guidBannerDelete = "";
                            if (customer.Banner != null && customer.Banner.Guid != "")
                            {
                                guidBannerDelete = customer.Banner.Guid;
                                await iconsAndBanners.DeleteFile(guidBannerDelete);
                            }
                            */
                            // Create new banner
                            Data.DataContext.Entities.File banner = UnitOfWork.Files.CreateEmpty();
                            banner.Name = customerData.Banner.Name;
                            banner.Guid = customerData.Banner.Id;
                            unitOfWork.Files.MarkForInsert(banner);
                            unitOfWork.SaveChanges();
                            customer.Banner = banner;
                        }
                    }
                    else
                    {
                        if (customerData.Banner != null)
                        {
                            // Create new banner
                            Data.DataContext.Entities.File banner = UnitOfWork.Files.CreateEmpty();
                            banner.Name = customerData.Banner.Name;
                            banner.Guid = customerData.Banner.Id;
                            unitOfWork.Files.MarkForInsert(banner);
                            unitOfWork.SaveChanges();
                            customer.Banner = banner;
                        }
                    }
                }
                else
                {
                    customer.Banner = null;
                }
                unitOfWork.Customers.MarkForUpdate(customer, GetCurrentUser().Id);
                unitOfWork.SaveChanges();

                var json = JsonConvert.SerializeObject(Mapper.Map<WPM_API.Data.DataContext.Entities.Customer, CustomerViewModel>(customer), serializerSettings);
                return new OkObjectResult(json);
            }
        }

        [HttpGet]
        [Route("getRegisterClientSAS")]
        public IActionResult GetRegisterClientSAS()
        {
            // https://bitstream.blob.core.windows.net/download-repository/BitStream/WPM_API.AutoRegisterClient.exe

            // Connect to Azure
            CloudStorageAccount storage = CloudStorageAccount.Parse(appSettings.LiveSystemConnectionString);
            CloudBlobClient customerClient = storage.CreateCloudBlobClient();
            CloudBlobContainer csdpContainer = customerClient.GetContainerReference("download-repository");
            CloudBlockBlob exe = csdpContainer.GetBlockBlobReference("BitStream/WPM_API.AutoRegisterClient.exe");
            string sas = exe.GetSharedAccessSignature(new SharedAccessBlobPolicy()
            {
                SharedAccessExpiryTime = DateTime.UtcNow.AddHours(2),
                Permissions = SharedAccessBlobPermissions.Write | SharedAccessBlobPermissions.Read | SharedAccessBlobPermissions.List
            });
            return Ok(exe.Uri + sas);
        }

        [HttpGet]
        [Route("getOfficeConfig/{customerId}")]
        [Authorize(Policy = Constants.Policies.Customer)]
        public IActionResult GetOfficeConfig([FromRoute] string customerId)
        {
            Customer customer = UnitOfWork.Customers.GetOrNull(customerId);
            if (customer == null)
            {
                return BadRequest("ERROR: The customer does not exist");
            }

            return Ok(new CustomerConfigModel { OfficeConfig = customer.OfficeConfig, UseCustomConfig = customer.UseCustomConfig });
        }

        [HttpPost]
        [Route("setOfficeConfig/{customerId}")]
        [Authorize(Policy = Constants.Policies.Customer)]
        public IActionResult SetOfficeConfig([FromRoute] string customerId, [FromBody] CustomerConfigModel data)
        {
            Customer customer = UnitOfWork.Customers.GetOrNull(customerId);
            if (customer == null)
            {
                return BadRequest("ERROR: The customer does not exist");
            }

            customer.OfficeConfig = data.OfficeConfig;
            customer.UseCustomConfig = data.UseCustomConfig;

            UnitOfWork.Customers.MarkForUpdate(customer, GetCurrentUser().Id);
            UnitOfWork.SaveChanges();

            return Ok();
        }

        [HttpGet]
        [Route("getSEPs/{customerId}")]
        public IActionResult GetCustomersSEPs([FromRoute] string customerId)
        {
            using (var unitOfWork = CreateUnitOfWork())
            {
                Customer customer = unitOfWork.Customers.GetAll("StorageEntryPoints").Where(x => x.Id == customerId).FirstOrDefault();
                if (customer == null)
                {
                    return BadRequest("ERROR: The customer does not exist");
                }

                List<StorageEntryPoint> result = new List<StorageEntryPoint>();
                if (customer.StorageEntryPoints != null)
                {
                    result = customer.StorageEntryPoints;
                }

                var json = JsonConvert.SerializeObject(result);
                return Ok(json);
            }
        }

        public class CustomerConfigModel
        {
            public string OfficeConfig { get; set; }
            public bool UseCustomConfig { get; set; }
        }

        public class GeneralSettingsModel
        {
            public string AutoRegisterPassword { get; set; }
            public bool AutoRegisterClients { get; set; }
        }
    }
}
