using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WPM_API.Azure;
using WPM_API.Code.Infrastructure;
using WPM_API.Code.Infrastructure.LogOn;
using WPM_API.Common;
using WPM_API.Data.DataContext.Entities;
using WPM_API.Data.DataContext.Projections.Users;
using WPM_API.Options;

namespace WPM_API.Controllers
{
    [Route("azure/locations")]
    public class LocationsController : BasisController
    {
        public LocationsController(AppSettings appSettings, ConnectionStrings connectionStrings, OrderEmailOptions orderEmailOptions, AgentEmailOptions agentEmailOptions, SendMailCreds sendMailCreds, SiteOptions siteOptions, ILogonManager logonManager) : base(appSettings, connectionStrings, orderEmailOptions, agentEmailOptions, sendMailCreds, siteOptions, logonManager)
        {
        }

        /// <summary>
        /// Retrieve all Azure locations (aka regions).
        /// </summary>
        /// <returns>[Location]</returns>
        [HttpGet]
        [Authorize(Policy = Constants.Roles.Customer)]
        public async Task<IActionResult> GetLocations()
        {
            AccountProjection user = GetCurrentUser();

            CloudEntryPoint credentials = null;
            if (string.IsNullOrEmpty(user.CustomerId))
            {
                // Wenn Admin oder Systemhouse
                credentials = new CloudEntryPoint()
                {
                    ClientId = appSettings.ClientId,
                    ClientSecret = appSettings.ClientSecret,
                    TenantId = appSettings.TenantId
                };
            }
            else
            {
                credentials = GetCEP(null);
            }
            if (credentials == null)
            {
                return BadRequest("AzureCredentials not found.");
            }

            AzureCommunicationService azure = new AzureCommunicationService(credentials.TenantId, credentials.ClientId, credentials.ClientSecret);

            var json = JsonConvert.SerializeObject(await (azure.SubscriptionService()).GetLocations(), serializerSettings);
            return new OkObjectResult(json);
        }
    }
}