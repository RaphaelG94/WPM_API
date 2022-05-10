using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using WPM_API.Data.DataContext.Entities;
using WPM_API.Data.DataContext.Projections.Users;
using WPM_API.Common;
using WPM_API.Azure;

namespace WPM_API.Controllers
{
    [Route("images")]
    public class ImagesController : BasisController
    {
        /// <summary>
        /// Retrieve all available images.
        /// </summary>
        /// <returns>[Image]</returns>
        [HttpGet]
        [Authorize(Policy = Constants.Roles.Customer)]
        public IActionResult GetImages()
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

            var json = JsonConvert.SerializeObject(azure.VirtualMachineService().GetImages(), _serializerSettings);
            return new OkObjectResult(json);
        }
    }
}