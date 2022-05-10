using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using WPM_API.Data.DataContext.Projections.Users;
using WPM_API.Data.DataContext.Entities;
using WPM_API.Common;
using WPM_API.Azure;

namespace WPM_API.Controllers
{
    [Route("azure/locations")]
    public class LocationsController : BasisController
    {

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
                    ClientId = _appSettings.ClientId,
                    ClientSecret = _appSettings.ClientSecret,
                    TenantId = _appSettings.TenantId
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

            var json = JsonConvert.SerializeObject(await (azure.SubscriptionService()).GetLocations(), _serializerSettings);
            return new OkObjectResult(json);
        }
    }
}