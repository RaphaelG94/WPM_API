using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Linq.Dynamic.Core;
using WPM_API.Azure;
using WPM_API.Code.Infrastructure;
using WPM_API.Code.Infrastructure.LogOn;
using WPM_API.Common;
using WPM_API.Data.DataContext.Entities;
using WPM_API.Models;
using WPM_API.Options;

namespace WPM_API.Controllers.Base
{
    [Route("/storage-accounts")]
    public class StorageAccountController : BasisController
    {
        public StorageAccountController(AppSettings appSettings, ConnectionStrings connectionStrings, OrderEmailOptions orderEmailOptions, AgentEmailOptions agentEmailOptions, SendMailCreds sendMailCreds, SiteOptions siteOptions, ILogonManager logonManager) : base(appSettings, connectionStrings, orderEmailOptions, agentEmailOptions, sendMailCreds, siteOptions, logonManager)
        {
        }

        /// <summary>
        /// Create a new Azure storage Account and save it in database
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = Constants.Policies.Customer)]
        public async Task<IActionResult> CreateStorageAccountAsync([FromBody] StorageAccountAddViewModel payload)
        {
            using (var unitOfWork = CreateUnitOfWork())
            {
                try
                {
                    // Get resource group
                    ResourceGroup resourceGroup = unitOfWork.ResourceGroups.Get(payload.ResourceGroupId);
                    CloudEntryPoint creds = GetCEP(payload.CustomerId);
                    AzureCommunicationService azure = new AzureCommunicationService(creds.TenantId, creds.ClientId, creds.ClientSecret);
                    var azureStorageAccount = await azure.StorageService().AddStorageAccountAsync(payload.SubscriptionId, resourceGroup.Name, payload.Name, payload.Type, resourceGroup.Location, "Storage V2");

                    // Save storage account in db
                    StorageAccount newAccount = unitOfWork.StorageAccounts.CreateEmpty();
                    newAccount.AzureId = azureStorageAccount.Id;
                    newAccount.CustomerId = payload.CustomerId;
                    newAccount.Name = payload.Name;
                    newAccount.ResourceGroupId = payload.ResourceGroupId;
                    newAccount.Type = payload.Type;
                    unitOfWork.SaveChanges();

                    // Create response
                    var json = JsonConvert.SerializeObject(Mapper.Map<StorageAccountViewModel>(newAccount), serializerSettings);
                    return new OkObjectResult(json);

                }
                catch (Exception e)
                {
                    return new BadRequestObjectResult("There was an error creating the storage account. " + e.Message);
                }

            }
        }

        /// <summary>
        /// Validates the storage-accountname in azure
        /// </summary>
        /// <param name="customerId">Id from the Customer</param>
        /// <param name="storageAccountName">Name from the storage-account</param>
        /// <returns>200 if the name can is free and can be used. 400 with error-message if not</returns>
        [HttpPost]
        [Authorize(Policy = Constants.Roles.Customer)]
        [Route("{customerId}/validateName")]
        public async Task<IActionResult> ValidateStorageAccountName([FromBody] StorageAccountNameViewModel storageAccountName, [FromRoute] string customerId)
        {
            AzureCommunicationService azure;
            if (storageAccountName.Managed == null || storageAccountName.Managed != "Managed")
            {
                CloudEntryPoint cep = GetCEP(customerId);
                if (cep == null)
                {
                    return BadRequest("ERROR: The Cloud Entry Point was not set yet");
                }
                azure = new AzureCommunicationService(cep.TenantId, cep.ClientId, cep.ClientSecret);
            }
            else
            {
                // TODO: Check system
                azure = new AzureCommunicationService(appSettings.DevelopmentTenantId, appSettings.DevelopmentClientId, appSettings.DevelopmentClientSecret);
            }


            var result = await azure.StorageService().CheckNameAvailabilityAsync(storageAccountName.Name);

            if (result.IsAvailable.GetValueOrDefault())
            {
                return new OkResult();
            }
            else
            {
                return new BadRequestObjectResult(result.Message);
            }
        }

        /// <summary>
        ///  Get all storage accounts 
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns>List<StorageAccount></returns>
        [HttpGet]
        [Authorize(Policy = Constants.Policies.Customer)]
        [Route("{customerId}")]
        public IActionResult GetCustomersStorageAccounts([FromRoute] string customerId)
        {
            // Fetch storage accounts
            List<StorageAccount> storageAccounts = UnitOfWork.StorageAccounts.GetAll().Where(x => x.CustomerId == customerId).ToList<StorageAccount>();

            // Serialize and return result
            var json = JsonConvert.SerializeObject(Mapper.Map<List<StorageAccount>, List<StorageAccountViewModel>>(storageAccounts), serializerSettings);
            return new OkObjectResult(json);
        }

        [HttpGet]
        [Authorize(Policy = Constants.Policies.Customer)]
        [Route("{storageAccountId}")]
        public async Task<IActionResult> GetStorageAccount([FromRoute] string subscriptionId, [FromRoute] string storageAccountId)
        {
            // Create communication service with Azure
            AzureCommunicationService azure = new AzureCommunicationService(appSettings.TenantId, appSettings.ClientId, appSettings.ClientSecret);

            var storageAccount = await azure.StorageService().GetStorageAccountAsync(storageAccountId);
            return NoContent();

        }
    }
}