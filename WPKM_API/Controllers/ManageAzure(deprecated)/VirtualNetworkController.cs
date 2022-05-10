using WPM_API.Models;
using  WPM_API.Azure;
using WPM_API.Common;
using WPM_API.Data.DataContext.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;

namespace WPM_API.Controllers
{
    [Route("virtual-networks")]
    public class VirtualNetworkController : BasisController
    {
        public VirtualNetworkController()
        {
        }

        //[HttpPost]
        //[Authorize(Policy = Constants.Roles.Customer)]
        //public async Task<IActionResult> AddVirtualNetwork([FromBody] VirtualNetworkAddViewModel virtualNetworkAdd)
        //{
        //    AzureCredentials credentials = GetCurrentAzureCredentials(virtualNetworkAdd.CustomerId);
        //    AzureCommunicationService azure = new AzureCommunicationService(credentials.TenantId, credentials.ClientId, credentials.ClientSecret);

        //    if ((await azure.GetVirtualNetworksAsync(virtualNetworkAdd.SubscriptionId, virtualNetworkAdd.ResourceGroupName)).Where(x => x.Name == virtualNetworkAdd.Name).Count() > 0)
        //    {
        //        return BadRequest("Virtual network already exists");
        //    }
        //    else
        //    {
        //        var virtualNetwork = await azure.AddOrModifyVirtualNetworkAsync(virtualNetworkAdd.SubscriptionId, virtualNetworkAdd.ResourceGroupName, Mapper.Map<AzureCommunication.Models.VirtualNetworkAddOrEditViewModel>(virtualNetworkAdd));
        //        // Serialize and return the response
        //        var json = JsonConvert.SerializeObject(Mapper.Map<VirtualNetworkViewModel>(virtualNetwork), _serializerSettings);
        //        return new OkObjectResult(json);
        //    }
        //}

        //[HttpPut]
        //[Authorize(Policy = Constants.Roles.Customer)]
        //public async Task<IActionResult> ModifyVirtualNetwork([FromRoute] string virtualNetworkName, [FromBody] VirtualNetworkEditViewModel virtualNetworkEdit)
        //{
        //    AzureCredentials credentials = GetCurrentAzureCredentials(virtualNetworkEdit.CustomerId);
        //    AzureCommunicationService azure = new AzureCommunicationService(credentials.TenantId, credentials.ClientId, credentials.ClientSecret);

        //    if ((await azure.GetVirtualNetworksAsync(virtualNetworkEdit.SubscriptionId, virtualNetworkEdit.ResourceGroupName)).Where(x => x.Name == virtualNetworkName).Count() == 0)
        //    {
        //        return BadRequest("Virtual network doesent exists");
        //    }
        //    else
        //    {
        //        var virtualNetwork = await azure.AddOrModifyVirtualNetworkAsync(virtualNetworkEdit.SubscriptionId, virtualNetworkEdit.ResourceGroupName, Mapper.Map<AzureCommunication.Models.VirtualNetworkAddOrEditViewModel>(virtualNetworkEdit));
        //        // Serialize and return the response
        //        var json = JsonConvert.SerializeObject(Mapper.Map<VirtualNetworkViewModel>(virtualNetwork), _serializerSettings);
        //        return new OkObjectResult(json);
        //    }
        //}

        //[HttpDelete]
        //[Authorize(Policy = Constants.Roles.Customer)]
        //public IActionResult DeleteVirtualNetwork([FromRoute] VirtualNetworkRefViewModel virtualNetworkEdit)
        //{
        //    AzureCredentials credentials = GetCurrentAzureCredentials(virtualNetworkEdit.CustomerId);
        //    AzureCommunicationService azure = new AzureCommunicationService(credentials.TenantId, credentials.ClientId, credentials.ClientSecret);

        //    azure.DeleteVirtualNetwork(virtualNetworkEdit.SubscriptionId, virtualNetworkEdit.Id);

        //    return Ok();
        //}
    }
}