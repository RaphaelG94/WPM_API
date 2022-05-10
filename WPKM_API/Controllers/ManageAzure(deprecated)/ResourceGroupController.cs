using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using WPM_API.Models;
using  WPM_API.Azure;
using Azure = Microsoft.Azure.Management.ResourceManager.Models;
using Microsoft.Rest.Azure;
using WPM_API.Common;
using WPM_API.Data.DataContext.Entities;

namespace WPM_API.Controllers
{
    [Route("resource-groups")]
    public class ResourceGroupController : BasisController
    {
        public ResourceGroupController()
        {
        }

        //[HttpGet]
        //[Authorize(Policy = Constants.Roles.Customer)]
        //public async Task<IActionResult> GetRessourceGroup([FromBody] ResourceGroupRefViewModel resourceGroupRef)
        //{
        //    AzureCredentials credentials = GetCurrentAzureCredentials(resourceGroupRef.CustomerId);
        //    AzureCommunicationService azure = new AzureCommunicationService(credentials.TenantId, credentials.ClientId, credentials.ClientSecret);

        //    Azure.Subscription subscription = await azure.GetSubscriptionAsync(resourceGroupRef.SubscriptionId);
        //    List<ResourceGroupViewModel> ressourceGroups = new List<ResourceGroupViewModel>();

        //    List<Azure.ResourceGroup> azureResourceGroups = await azure.GetResourceGroupsAsync(subscription.SubscriptionId);
        //    List<ResourceGroupViewModel> amsResourceGroups = Mapper.Map<List<Azure.ResourceGroup>, List<ResourceGroupViewModel>>(azureResourceGroups);
        //    SubscriptionViewModel subscriptionViewModel = new SubscriptionViewModel { SubscriptionId = subscription.SubscriptionId, Name = subscription.DisplayName, CustomerId = resourceGroupRef.CustomerId };
        //    amsResourceGroups.ForEach(x => x.Subscription = subscriptionViewModel);
        //    ressourceGroups.AddRange(amsResourceGroups);

        //    // Serialize and return the response
        //    var json = JsonConvert.SerializeObject(ressourceGroups, _serializerSettings);
        //    return new OkObjectResult(json);
        //}

        //[HttpPost]
        //[Authorize(Policy = Constants.Roles.Customer)]
        //public async Task<IActionResult> AddRessourceGroup([FromBody] ResourceGroupAddViewModel resourceGroupAdd)
        //{

        //    // Richtige customerId?
        //    if (CurrentUserIsInRole(Constants.Roles.Admin))
        //    {
        //        // No Errors
        //    }
        //    else if (CurrentUserIsInRole(Constants.Roles.Systemhouse))
        //    {
        //        // is customerId in my systemhouse?

        //        if (UnitOfWork.Systemhouses.Get(GetCurrentUser().SystemhouseId)
        //            .Customer.Where(x => x.Id == resourceGroupAdd.CustomerId).Count() == 0)
        //        {
        //            return new ForbidResult();
        //        }
        //    }
        //    else if (CurrentUserIsInRole(Constants.Roles.Customer) && (GetCurrentUser()).CustomerId != resourceGroupAdd.CustomerId)
        //    {
        //        return new ForbidResult();
        //    }

        //    ResourceGroupViewModel result = new ResourceGroupViewModel();
        //    AzureCommunicationService azure;
        //    AzureCredentials credentials = GetCurrentAzureCredentials(resourceGroupAdd.CustomerId);
        //    azure = new AzureCommunicationService(credentials.TenantId, credentials.ClientId, credentials.ClientSecret);

        //    var rgs = await azure.GetResourceGroupsAsync(resourceGroupAdd.SubscriptionId);

        //    if (rgs.Where(x => x.Name == resourceGroupAdd.ResourceGroupName).Count() > 0)
        //    {
        //        // Name existiert schon
        //        return BadRequest("Resourcegroup-name already exists.");
        //    }
        //    else
        //    {
        //        // neue RessourceGroup erstellen
        //        Azure.ResourceGroup azureResourceGroup = new Azure.ResourceGroup()
        //        {
        //            Location = resourceGroupAdd.Location,
        //            Name = resourceGroupAdd.ResourceGroupName
        //        };
        //        var subscription = await azure.GetSubscriptionAsync(resourceGroupAdd.SubscriptionId);
        //        azureResourceGroup = await azure.AddResourceGroupAsync(resourceGroupAdd.SubscriptionId, azureResourceGroup);

        //        result.Location = azureResourceGroup.Location;
        //        result.ResourceGroupName = azureResourceGroup.Name;
        //        result.Subscription = new SubscriptionViewModel { SubscriptionId = subscription.SubscriptionId, Name = subscription.DisplayName, CustomerId = resourceGroupAdd.CustomerId };
        //    }
        //    // Serialize and return the response
        //    var json = JsonConvert.SerializeObject(result, _serializerSettings);
        //    return new OkObjectResult(json);
        //}


        //[HttpDelete]
        //[Authorize(Policy = Constants.Roles.Customer)]
        //public async Task<IActionResult> DeleteResourceGroup([FromRoute] ResourceGroupRefViewModel resourceGroupRef)
        //{
        //    // Richtige customerId?
        //    if (CurrentUserIsInRole(Constants.Roles.Admin))
        //    {
        //        // No Errors
        //    }
        //    else if (CurrentUserIsInRole(Constants.Roles.Systemhouse))
        //    {
        //        // is customerId in my systemhouse?
        //        if (UnitOfWork.Systemhouses.Get((GetCurrentUser()).SystemhouseId)
        //            .Customer.Where(x => x.Id == resourceGroupRef.CustomerId).Count() == 0)
        //        {
        //            return new ForbidResult();
        //        }

        //    }
        //    else if (CurrentUserIsInRole(Constants.Roles.Customer) && (GetCurrentUser()).CustomerId != resourceGroupRef.CustomerId)
        //    {
        //        return new ForbidResult();
        //    }


        //    AzureCredentials credentials = GetCurrentAzureCredentials(resourceGroupRef.CustomerId);
        //    AzureCommunicationService azure = new AzureCommunicationService(credentials.TenantId, credentials.ClientId, credentials.ClientSecret);

        //    AzureOperationResponse result = await azure.DeleteResourceGroupAsync(resourceGroupRef.SubscriptionId, resourceGroupRef.ResourceGroupName);
        //    if (result.Response.IsSuccessStatusCode)
        //    {
        //        return new StatusCodeResult(204);
        //    }
        //    else
        //    {
        //        return new BadRequestResult();
        //    }
        //}
    }
}