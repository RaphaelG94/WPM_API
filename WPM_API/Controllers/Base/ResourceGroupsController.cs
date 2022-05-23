using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WPM_API.Azure;
using WPM_API.Code.Infrastructure;
using WPM_API.Code.Infrastructure.LogOn;
using WPM_API.Common;
using WPM_API.Data.DataContext.Entities;
using WPM_API.Models;
using WPM_API.Options;
using AZURE = Microsoft.Azure.Management.ResourceManager.Models;

namespace WPM_API.Controllers.Base
{
    [Route("resource-groups")]
    public class ResourceGroupsController : BasisController
    {
        public ResourceGroupsController(AppSettings appSettings, ConnectionStrings connectionStrings, OrderEmailOptions orderEmailOptions, AgentEmailOptions agentEmailOptions, SendMailCreds sendMailCreds, SiteOptions siteOptions, ILogonManager logonManager) : base(appSettings, connectionStrings, orderEmailOptions, agentEmailOptions, sendMailCreds, siteOptions, logonManager)
        {
        }

        [HttpGet]
        [Authorize(Policy = Constants.Policies.Customer)]
        [Route("{customerId}")]
        public IActionResult GetCustomersResourceGroups([FromRoute] string customerId)
        {
            List<ResourceGroup> resourceGroups = UnitOfWork.ResourceGroups.GetAll().Where(x => x.CustomerId == customerId).ToList();

            // Serialize & return result
            var json = JsonConvert.SerializeObject(Mapper.Map<List<ResourceGroup>, List<ResourceGroupViewModel>>(resourceGroups), serializerSettings);
            return new OkObjectResult(json);
        }

        [HttpPost]
        [Authorize(Policy = Constants.Policies.Customer)]
        public async System.Threading.Tasks.Task<IActionResult> CreateResourceGroupAsync([FromBody] AddResourceGroupViewModel data)
        {
            var creds = GetCEP(data.CustomerId);
            AzureCommunicationService azure = new AzureCommunicationService(creds.TenantId, creds.ClientId, creds.ClientSecret);
            AZURE.Subscription subscription = await azure.SubscriptionService().GetSubscription(data.SubscriptionId);
            if (subscription == null)
            {
                return BadRequest("The subscription does not exist");
            }
            if (CheckResourceGroupExists(data))
            {
                return StatusCode(409, "The resource group already exists in the subscription " + subscription.DisplayName);
            }
            else
            {
                AZURE.ResourceGroup tempResGrp = new AZURE.ResourceGroup();
                tempResGrp.Location = data.AzureLocation;
                // tempResGrp.Name = data.Name;
                AZURE.ResourceGroup newResourceGroup = await azure.ResourceGroupService().AddResourceGroup(data.SubscriptionId, tempResGrp);
                ResourceGroup newRG = UnitOfWork.ResourceGroups.CreateEmpty();
                newRG.Name = newResourceGroup.Name;
                newRG.CustomerId = GetCurrentUser().CustomerId;
                newRG.Location = newResourceGroup.Location;
                newRG.AzureSubscriptionId = data.SubscriptionId;

                try
                {
                    UnitOfWork.SaveChanges();
                }
                catch (Exception e)
                {
                    return BadRequest("The resource group could not be created! " + e.Message);
                }

                var json = JsonConvert.SerializeObject(Mapper.Map<ResourceGroup, ResourceGroupViewModel>(newRG), serializerSettings);
                return new OkObjectResult(json);
            }
        }

        [HttpPut]
        [Route("validate/{customerId}/{subscriptionId}/{resGrpName}/{managed}")]
        [Authorize(Policy = Constants.Policies.Customer)]
        public IActionResult UniqueResGrpName([FromRoute] string resGrpName, [FromRoute] string subscriptionId, [FromRoute] string customerId, string managed)
        {
            AddResourceGroupViewModel data = new AddResourceGroupViewModel();
            data.Name = resGrpName;
            data.SubscriptionId = subscriptionId;
            data.CustomerId = customerId;
            data.Managed = managed;
            bool result = CheckResourceGroupExists(data);
            if (result)
            {
                return BadRequest("The resource group name is already taken!");
            }
            else
            {
                return new OkResult();
            }
        }

        private bool CheckResourceGroupExists([FromBody] AddResourceGroupViewModel data)
        {
            AzureCommunicationService azure;
            if (data.Managed == null || data.Managed != "Managed")
            {
                var creds = GetCEP(data.CustomerId);
                azure = new AzureCommunicationService(creds.TenantId, creds.ClientId, creds.ClientSecret);
            }
            else
            {
                // TODO: check system
                azure = new AzureCommunicationService(appSettings.DevelopmentTenantId, appSettings.DevelopmentClientId, appSettings.DevelopmentClientSecret);
            }

            bool result = azure.ResourceGroupService().GetRessourceGroupByName(data.Name, data.SubscriptionId);
            if (result)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}