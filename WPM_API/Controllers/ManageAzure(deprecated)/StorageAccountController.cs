using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using  WPM_API.Azure;
using Newtonsoft.Json;
using WPM_API.Models;
using Azure = Microsoft.Azure.Management.Storage.Models;
using WPM_API.Common;
using WPM_API.Data.DataContext.Entities;

namespace WPM_API.Controllers
{
    [Route("storage-accounts")]
    public class StorageAccountController : BasisController
    {
        public StorageAccountController()
        {
        }

        //[HttpGet]
        //[Authorize(Policy = Constants.Roles.Customer)]
        //public async Task<IActionResult> GetStorageAccount([FromBody] StorageAccountRefViewModel storageAccountRef)
        //{
        //    AzureCredentials credentials = GetCurrentAzureCredentials(storageAccountRef.CustomerId);
        //    AzureCommunicationService azure = new AzureCommunicationService(credentials.TenantId, credentials.ClientId, credentials.ClientSecret);

        //    var sa = await azure.GetStorageAccountAsync(storageAccountRef.SubscriptionId, storageAccountRef.ResourceGroupName, storageAccountRef.Id);

        //    StorageAccountViewModel storageAccountViewModel = Mapper.Map<Azure.StorageAccount, StorageAccountViewModel>(sa);

        //    var json = JsonConvert.SerializeObject(storageAccountViewModel, _serializerSettings);
        //    return new OkObjectResult(json);
        //}

        //[HttpPost]
        //[Authorize(Policy = Constants.Roles.Customer)]
        //public async Task<IActionResult> AddStorageAccount([FromBody] StorageAccountAddViewModel storageAccountAdd)
        //{
        //    AzureCredentials credentials = GetCurrentAzureCredentials(storageAccountAdd.CustomerId);
        //    AzureCommunicationService azure = new AzureCommunicationService(credentials.TenantId, credentials.ClientId, credentials.ClientSecret);

        //    var azureStorageAccount = azure.AddStorageAccountAsync(storageAccountAdd.SubscriptionId, storageAccountAdd.ResourceGroupName, storageAccountAdd.Name, Enum.GetName(typeof(StorageType), storageAccountAdd.Type));
        //    StorageAccountViewModel storageAccountViewModel = Mapper.Map<AzureCommunication.Models.StorageAccountViewModel, StorageAccountViewModel>(await azureStorageAccount);
        //    // Serialize and return the response
        //    var json = JsonConvert.SerializeObject(storageAccountViewModel, _serializerSettings);
        //    return new OkObjectResult(json);
        //}

        //[HttpPut]
        //[Authorize(Policy = Constants.Roles.Customer)]
        //public async Task<IActionResult> EditStorageAccount([FromBody] StorageAccountEditViewModel storageAccountEdit)
        //{
        //    AzureCredentials credentials = GetCurrentAzureCredentials(storageAccountEdit.CustomerId);
        //    AzureCommunicationService azure = new AzureCommunicationService(credentials.TenantId, credentials.ClientId, credentials.ClientSecret);

        //    var azureStorageAccount = azure.EditStorageAccountAsync(storageAccountEdit.SubscriptionId, storageAccountEdit.Id, Enum.GetName(typeof(StorageType), storageAccountEdit.Type));
        //    StorageAccountViewModel storageAccountViewModel = Mapper.Map<AzureCommunication.Models.StorageAccountViewModel, StorageAccountViewModel>(await azureStorageAccount);
        //    // Serialize and return the response
        //    var json = JsonConvert.SerializeObject(storageAccountViewModel, _serializerSettings);
        //    return new OkObjectResult(json);
        //}

        //[HttpDelete]
        //[Authorize(Policy = Constants.Roles.Customer)]
        //public IActionResult DeleteStorageAccounts([FromBody] StorageAccountRefViewModel storageAccountRef)
        //{
        //    AzureCredentials credentials = GetCurrentAzureCredentials(storageAccountRef.CustomerId);
        //    AzureCommunicationService azure = new AzureCommunicationService(credentials.TenantId, credentials.ClientId, credentials.ClientSecret);

        //    azure.DeleteStorageAccount(storageAccountRef.Id);
        //    return NoContent();
        //}
    }
}