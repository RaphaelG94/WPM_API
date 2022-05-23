using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WPM_API.Azure;
using WPM_API.Azure.Core;
using WPM_API.Code.Infrastructure;
using WPM_API.Code.Infrastructure.LogOn;
using WPM_API.Common;
using WPM_API.Data.DataContext.Entities;
using WPM_API.Models;
using WPM_API.Options;
using AZURE = Microsoft.Azure.Management.ResourceManager.Models;

namespace WPM_API.Controllers.Base
{
    [Route("customers/{customerId}")]
    public class SubscriptionController : BasisController
    {
        public SubscriptionController(AppSettings appSettings, ConnectionStrings connectionStrings, OrderEmailOptions orderEmailOptions, AgentEmailOptions agentEmailOptions, SendMailCreds sendMailCreds, SiteOptions siteOptions, ILogonManager logonManager) : base(appSettings, connectionStrings, orderEmailOptions, agentEmailOptions, sendMailCreds, siteOptions, logonManager)
        {
        }

        /// <summary>
        /// Retrieve all subscriptions of the customer.
        /// </summary>
        /// <param name="customerId">Id from the Customer</param>
        /// <returns>SubscriptionRef</returns>
        [HttpGet]
        [Authorize(Policy = Constants.Roles.Customer)]
        [Route("subscriptions")]
        public async Task<IActionResult> GetSubscription([FromRoute] string customerId)
        {
            if (CurrentUserIsInRole(Constants.Roles.Admin))
            {
                // No Errors
            }
            else if (CurrentUserIsInRole(Constants.Roles.Systemhouse))
            {
                Systemhouse systemhouse = UnitOfWork.Systemhouses.Get(GetCurrentUser().SystemhouseId, "Customer");
                // is customerId in my systemhouse?
                if (UnitOfWork.Systemhouses.Get(GetCurrentUser().SystemhouseId, "Customer").Customer.Where(x => x.Id == customerId).Count() == 0)
                {
                    return new ForbidResult();
                }

            }
            else if (CurrentUserIsInRole(Constants.Roles.Customer) && (GetCurrentUser()).CustomerId != customerId)
            {
                return new ForbidResult();
            }

            AzureCommunicationService azure;
            List<SubscriptionViewModel> response = new List<SubscriptionViewModel>();

            CloudEntryPoint credentials = GetCEP(customerId);
            if (credentials == null)
            {
                return BadRequest("ERROR: The CEP does not exist or is not valid!");
            }
            azure = new AzureCommunicationService(credentials.TenantId, credentials.ClientId, credentials.ClientSecret);
            SubscriptionService subService = azure.SubscriptionService();

            List<AZURE.Subscription> subscriptions = await (azure.SubscriptionService()).GetSubscriptions();
            foreach (AZURE.Subscription s in subscriptions)
            {
                SubscriptionViewModel subscriptionViewModel = new SubscriptionViewModel()
                {
                    Id = s.SubscriptionId,
                    Name = s.DisplayName
                };
                response.Add(subscriptionViewModel);
            }

            // Serialize and return the response
            var json = JsonConvert.SerializeObject(response, serializerSettings);
            return new OkObjectResult(json);
        }

        [HttpGet]
        [Authorize(Policy = Constants.Roles.Customer)]
        [Route("subscriptions/bitstream")]
        public async Task<IActionResult> GetBitstreamSubscriptions()
        {
            AzureCommunicationService azure;
            List<SubscriptionViewModel> response = new List<SubscriptionViewModel>();

            // TODO: Fix for live system
            azure = new AzureCommunicationService(appSettings.DevelopmentTenantId, appSettings.DevelopmentClientId, appSettings.DevelopmentClientSecret);
            SubscriptionService subService = azure.SubscriptionService();

            List<AZURE.Subscription> subscriptions = await (azure.SubscriptionService()).GetSubscriptions();
            foreach (AZURE.Subscription s in subscriptions)
            {
                SubscriptionViewModel subscriptionViewModel = new SubscriptionViewModel()
                {
                    Id = s.SubscriptionId,
                    Name = s.DisplayName
                };
                response.Add(subscriptionViewModel);
            }

            // Serialize and return the response
            var json = JsonConvert.SerializeObject(response, serializerSettings);
            return new OkObjectResult(json);
        }
    }
}