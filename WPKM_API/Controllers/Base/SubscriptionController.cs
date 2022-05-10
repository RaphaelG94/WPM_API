using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WPM_API.Common;
using WPM_API.Azure;
using WPM_API.Models;
using WPM_API.Data.DataContext.Entities;
using Newtonsoft.Json;
using AZURE = Microsoft.Azure.Management.ResourceManager.Models;
using System.Web;
using WPM_API.Azure.Core;

namespace WPM_API.Controllers.Base
{
    [Route("customers/{customerId}")]
    public class SubscriptionController : BasisController
    {

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
            var json = JsonConvert.SerializeObject(response, _serializerSettings);
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
            azure = new AzureCommunicationService(_appSettings.DevelopmentTenantId, _appSettings.DevelopmentClientId, _appSettings.DevelopmentClientSecret);
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
            var json = JsonConvert.SerializeObject(response, _serializerSettings);
            return new OkObjectResult(json);
        }
    }
}