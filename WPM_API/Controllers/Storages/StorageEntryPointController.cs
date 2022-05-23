using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WPM_API.Azure;
using WPM_API.Code.Infrastructure;
using WPM_API.Code.Infrastructure.LogOn;
using WPM_API.Common;
using WPM_API.Data.DataContext.Entities;
using WPM_API.Data.DataContext.Entities.Storages;
using WPM_API.Data.Models;
using WPM_API.Options;

namespace WPM_API.Controllers.Storages
{
    [Route("sep")]
    public class StorageEntryPointController : BasisController
    {
        public StorageEntryPointController(AppSettings appSettings, ConnectionStrings connectionStrings, OrderEmailOptions orderEmailOptions, AgentEmailOptions agentEmailOptions, SendMailCreds sendMailCreds, SiteOptions siteOptions, ILogonManager logonManager) : base(appSettings, connectionStrings, orderEmailOptions, agentEmailOptions, sendMailCreds, siteOptions, logonManager)
        {
        }

        [HttpGet]
        [Route("{customerId}")]
        [Authorize(Policy = Constants.Policies.Customer)]
        public IActionResult GetCustomersSEPs([FromRoute] string customerId)
        {
            WPM_API.Data.DataContext.Entities.Customer customer = UnitOfWork.Customers.GetOrNull(customerId, "StorageEntryPoints");
            if (customer == null)
            {
                return BadRequest("ERROR: The customer does not exist");
            }
            if (customer.StorageEntryPoints == null)
            {
                customer.StorageEntryPoints = new List<StorageEntryPoint>();
            }

            var json = JsonConvert.SerializeObject(Mapper.Map<List<StorageEntryPointViewModel>>(customer.StorageEntryPoints), serializerSettings);
            return new OkObjectResult(json);
        }

        [HttpPost]
        [Route("addSEP/{customerId}")]
        [Authorize(Policy = Constants.Policies.Customer)]
        public async Task<IActionResult> AddSEP([FromBody] StorageEntryPointViewModel data, [FromRoute] string customerId)
        {
            using (var unitOfWork = CreateUnitOfWork())
            {
                // Declare variables
                StorageEntryPoint newSEP = Mapper.Map<StorageEntryPoint>(data);
                CloudEntryPoint cep;
                AzureCommunicationService azure;
                string connectionString;
                try
                {
                    if (!newSEP.Managed)
                    {
                        cep = GetCEP(customerId);
                        if (cep == null)
                        {
                            return BadRequest("ERROR: No Cloud Entry Point is set yet");
                        }
                        azure = new AzureCommunicationService(cep.TenantId, cep.ClientId, cep.ClientSecret);
                    }
                    else
                    {
                        azure = new AzureCommunicationService(appSettings.DevelopmentTenantId, appSettings.DevelopmentClientId, appSettings.DevelopmentClientSecret);
                    }

                    newSEP.Status = "Starting creating Storage Entry Point";

                    // Checking subscription

                    var subscription = await azure.SubscriptionService().GetSubscription(data.SubscriptionId);
                    if (subscription == null)
                    {
                        newSEP.Status = "ERROR: The subscription does not exist";
                        return BadRequest("ERROR: The subscription does not exist");
                    }

                    // Creating resource group
                    newSEP.Status = "Creating Resource Group";
                    var resourceGroup = await azure.ResourceGroupService().AddResourceGroup(data.SubscriptionId, new Microsoft.Azure.Management.ResourceManager.Models.ResourceGroup(data.Location, null, data.ResourceGrpName));

                    // Create storage account
                    var checkStorageAccount = await azure.StorageService().CheckNameAvailabilityAsync(data.StorageAccount);
                    if (checkStorageAccount.IsAvailable != true)
                    {
                        newSEP.Status = "ERROR: The storage account name is already taken";
                        return BadRequest("ERROR: The storage account name is already taken");
                    }
                    var storageAccountAzure = await azure.StorageService().AddStorageAccountAsync(data.SubscriptionId, data.ResourceGrpName, data.StorageAccount, data.StorageAccountType, data.Location, data.Kind);


                    // Create Blob Container
                    connectionString = azure.StorageService().GetStorageAccConnectionString(data.SubscriptionId, data.ResourceGrpName, data.StorageAccount);
                    var container = await azure.StorageService().CreateBlobContainer(connectionString, data.BlobContainerName);
                    newSEP.Url = "https://" + newSEP.StorageAccount + ".blob.core.windows.net/" + newSEP.BlobContainerName;
                }
                catch (Exception e)
                {
                    newSEP.Status = "ERROR: Could not create StorageEntryPoint. " + e.Message;
                    return BadRequest("ERROR: Could not create StorageEntryPoint. " + e.Message);
                }

                try
                {
                    // Save new SEP
                    if (!newSEP.Status.Contains("ERROR"))
                    {
                        newSEP.Status = "SEP successfully created";
                    }
                    WPM_API.Data.DataContext.Entities.Customer customer = unitOfWork.Customers.GetOrNull(customerId, "StorageEntryPoints", "Parameters");

                    DateTime expireDate = DateTime.UtcNow.AddMonths(12);
                    string sasKeyRead = azure.StorageService().CreateSASKey(connectionString, data.BlobContainerName, "read", expireDate);
                    string sasKeyWrite = azure.StorageService().CreateSASKey(connectionString, data.BlobContainerName, "write", expireDate);

                    customer.LtSASRead = sasKeyRead;
                    customer.LtSASWríte = sasKeyWrite;
                    customer.LtSASExpireDate = expireDate;

                    // Update customer parameters
                    var parameterRead = customer.Parameters.Find(x => x.Key == "$LtSASread");
                    if (parameterRead == null)
                    {
                        parameterRead = new Parameter()
                        {
                            Key = "$LtSASread",
                            Value = sasKeyRead,
                            IsEditable = false
                        };
                        customer.Parameters.Add(parameterRead);
                    }
                    else
                    {
                        parameterRead.Value = sasKeyRead;
                        parameterRead.IsEditable = false;
                    }

                    var parameterWrite = customer.Parameters.Find(x => x.Key == "$LtSASwrite");
                    if (parameterWrite == null)
                    {
                        parameterWrite = new Parameter()
                        {
                            Key = "$LtSASwrite",
                            Value = sasKeyWrite,
                            IsEditable = false
                        };
                        customer.Parameters.Add(parameterWrite);
                    }
                    else
                    {
                        parameterWrite.Value = sasKeyWrite;
                        parameterWrite.IsEditable = false;
                    }

                    // Update parameters for azure connection
                    Parameter azureBlobRoot = customer.Parameters.Find(x => x.Key == "$AzureBlobRoot");
                    Parameter csdpContainer = customer.Parameters.Find(x => x.Key == "$CSDPcontainer");
                    if (azureBlobRoot == null)
                    {
                        azureBlobRoot = new Parameter()
                        {
                            IsEditable = false,
                            Key = "$AzureBlobRoot",
                            Value = "https://" + newSEP.StorageAccount + ".blob.core.windows.net"
                        };
                        customer.Parameters.Add(azureBlobRoot);
                    }
                    else
                    {
                        azureBlobRoot.IsEditable = false;
                        azureBlobRoot.Value = "https://" + newSEP.StorageAccount + ".blob.core.windows.net";
                    }

                    if (csdpContainer == null)
                    {
                        csdpContainer = new Parameter()
                        {
                            IsEditable = false,
                            Key = "$CSDPcontainer",
                            Value = newSEP.BlobContainerName
                        };
                        customer.Parameters.Add(csdpContainer);
                    }
                    else
                    {
                        csdpContainer.IsEditable = false;
                        csdpContainer.Value = newSEP.BlobContainerName;
                    }

                    if (customer.StorageEntryPoints == null)
                    {
                        customer.StorageEntryPoints = new List<StorageEntryPoint>();
                    }
                    unitOfWork.StorageEntryPoints.MarkForInsert(newSEP, GetCurrentUser().Id);
                    customer.StorageEntryPoints.Add(newSEP);
                    unitOfWork.Customers.MarkForUpdate(customer, GetCurrentUser().Id);
                    unitOfWork.SaveChanges();

                    var json = JsonConvert.SerializeObject(Mapper.Map<StorageEntryPointViewModel>(newSEP), serializerSettings);
                    return new OkObjectResult(json);
                }
                catch (Exception e)
                {
                    return BadRequest("ERROR: Could not save the Storage Entry Point! " + e.Message);
                }
            }
        }

        [HttpDelete]
        [Route("deleteSEP/{customerId}/{sepId}")]
        [Authorize(Policy = Constants.Policies.Customer)]
        public async Task<IActionResult> DeleteSEP([FromRoute] string customerId, [FromRoute] string sepId)
        {
            using (var unitOfWork = CreateUnitOfWork())
            {
                try
                {
                    WPM_API.Data.DataContext.Entities.Customer customer = unitOfWork.Customers.GetOrNull(customerId, "StorageEntryPoints", "Parameters");
                    if (customer == null)
                    {
                        return BadRequest("ERROR: The customer does not exist");
                    }

                    StorageEntryPoint toDelete = unitOfWork.StorageEntryPoints.GetOrNull(sepId);
                    if (toDelete == null)
                    {
                        return BadRequest("ERROR: The storage entry point does not exist");
                    }

                    AzureCommunicationService azure;
                    if (!toDelete.Managed)
                    {
                        CloudEntryPoint cep = GetCEP(customerId);
                        if (cep == null)
                        {
                            return BadRequest("ERROR: No Cloud Entry Point is set yet");
                        }
                        azure = new AzureCommunicationService(cep.TenantId, cep.ClientId, cep.ClientSecret);
                    }
                    else
                    {
                        azure = new AzureCommunicationService(appSettings.DevelopmentTenantId, appSettings.DevelopmentClientId, appSettings.DevelopmentClientSecret);
                    }

                    // Delete $LtSASwrite and $LtSASread if the SEP isCSDP
                    if (toDelete.IsCSDP)
                    {
                        var parameterRead = customer.Parameters.Find(x => x.Key == "$LtSASread");
                        if (parameterRead != null)
                        {
                            customer.Parameters.Remove(parameterRead);
                            unitOfWork.Customers.MarkForUpdate(customer);
                            unitOfWork.Parameters.MarkForDelete(parameterRead);
                            unitOfWork.SaveChanges();
                        }

                        var parameterWrite = customer.Parameters.Find(x => x.Key == "$LtSASwrite");
                        if (parameterWrite != null)
                        {
                            customer.Parameters.Remove(parameterWrite);
                            unitOfWork.Customers.MarkForUpdate(customer);
                            unitOfWork.Parameters.MarkForDelete(parameterWrite);
                            unitOfWork.SaveChanges();
                        }
                    }

                    var exists = azure.ResourceGroupService().GetRessourceGroupByName(toDelete.ResourceGrpName, toDelete.SubscriptionId);
                    if (exists)
                    {
                        var success = await azure.ResourceGroupService().DeleteRessourceGroupAsync(toDelete.SubscriptionId, toDelete.ResourceGrpName);
                        if (success.Response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            customer.StorageEntryPoints.Remove(toDelete);
                            unitOfWork.StorageEntryPoints.MarkForDelete(toDelete, GetCurrentUser().Id);
                            unitOfWork.SaveChanges();
                            var json = JsonConvert.SerializeObject(Mapper.Map<StorageEntryPointViewModel>(toDelete), serializerSettings);
                            return new OkObjectResult(json);
                        }
                        else
                        {
                            return BadRequest("ERROR: Could not delete storage entry point in Azure! " + success.Response.Content);
                        }
                    }
                    else
                    {
                        customer.StorageEntryPoints.Remove(toDelete);
                        unitOfWork.StorageEntryPoints.MarkForDelete(toDelete, GetCurrentUser().Id);
                        unitOfWork.SaveChanges();
                        var json = JsonConvert.SerializeObject(Mapper.Map<StorageEntryPointViewModel>(toDelete), serializerSettings);
                        return new OkObjectResult(json);
                    }
                }
                catch (Exception e)
                {
                    return BadRequest("ERROR: Could not delete storage entry point. " + e.Message);
                }
            }
        }

        [HttpPost]
        [Route("createSAS/{customerId}")]
        [Authorize(Policy = Constants.Policies.Customer)]
        public IActionResult AddSASKey([FromRoute] string customerId, [FromBody] StorageEntryPointViewModel data)
        {
            try
            {
                WPM_API.Data.DataContext.Entities.Customer customer = UnitOfWork.Customers.GetOrNull(customerId, CustomerIncludes.Parameters);
                if (customer == null)
                {
                    return BadRequest("ERROR: The customer does not exist");
                }

                CloudEntryPoint cep = GetCEP(customerId);
                if (cep == null)
                {
                    return BadRequest("ERROR: The cloud entry point does not exist");
                }

                AzureCommunicationService azure = new AzureCommunicationService(cep.TenantId, cep.ClientId, cep.ClientSecret);
                string connectionString = azure.StorageService().GetStorageAccConnectionString(data.SubscriptionId, data.ResourceGrpName, data.StorageAccount);
                DateTime expireDate = DateTime.UtcNow.AddMonths(12);
                string sasKeyRead = azure.StorageService().CreateSASKey(connectionString, data.BlobContainerName, "read", expireDate);
                string sasKeyWrite = azure.StorageService().CreateSASKey(connectionString, data.BlobContainerName, "write", expireDate);

                // Set keys to customer 
                customer.LtSASRead = sasKeyRead;
                customer.LtSASWríte = sasKeyWrite;
                customer.LtSASExpireDate = expireDate;

                // Update customer parameters
                var parameterRead = customer.Parameters.Find(x => x.Key == "$LtSASread");
                if (parameterRead == null)
                {
                    parameterRead = new Parameter()
                    {
                        Key = "$LtSASread",
                        Value = sasKeyRead,
                        IsEditable = false
                    };
                    customer.Parameters.Add(parameterRead);
                }
                else
                {
                    parameterRead.Value = sasKeyRead;
                    parameterRead.IsEditable = false;
                }

                var parameterWrite = customer.Parameters.Find(x => x.Key == "$LtSASwrite");
                if (parameterWrite == null)
                {
                    parameterWrite = new Parameter()
                    {
                        Key = "$LtSASwrite",
                        Value = sasKeyWrite,
                        IsEditable = false
                    };
                    customer.Parameters.Add(parameterWrite);
                }
                else
                {
                    parameterWrite.Value = sasKeyWrite;
                    parameterWrite.IsEditable = false;
                }

                UnitOfWork.SaveChanges();
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest("ERROR: Setting the SAS keys failed! " + e.Message);
            }
        }
    }

    public class StorageEntryPointViewModel
    {
        public string Id { get; set; }
        public bool isCSDP { get; set; }
        public string ResourceGrpName { get; set; }
        public string Location { get; set; }
        public string StorageAccount { get; set; }
        public string Type { get; set; }
        public string StorageAccountType { get; set; }
        public string SubscriptionId { get; set; }
        public string Status { get; set; }
        public string BlobContainerName { get; set; }
        public string Url { get; set; }
        public string Kind { get; set; }
        public bool Managed { get; set; }
    }
}
