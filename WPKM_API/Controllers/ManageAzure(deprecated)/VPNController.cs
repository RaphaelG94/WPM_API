using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using  WPM_API.Azure;
using Newtonsoft.Json;
using WPM_API.Models;
using System;
using System.Reflection;
using System.IO;
using WPM_API.Data.DataContext.Entities;
using WPM_API.Common;

namespace WPM_API.Controllers
{
    [Route("vpns")]
    public class VPNController : BasisController
    {
        public VPNController()
        {
        }

        //[HttpGet]
        //[Authorize(Policy = Constants.Roles.Customer)]
        //public async Task<IActionResult> GetVPN([FromBody] VpnRef vpnRef)
        //{
        //    AzureCredentials credentials = GetCurrentAzureCredentials(vpnRef.CustomerId);

        //    AzureCommunicationService azure = new AzureCommunicationService(credentials.TenantId, credentials.ClientId, credentials.ClientSecret);
        //    var vpn = Mapper.Map<AzureCommunication.Models.VPN, VpnViewModel>(await azure.GetVPNAsync(vpnRef.SubscriptionId, vpnRef.ResourceGroupName, vpnRef.Id));

        //    vpn.ResourceGroupName = vpnRef.ResourceGroupName;
        //    vpn.SubscriptionId = vpnRef.SubscriptionId;
        //    vpn.CustomerId = vpnRef.CustomerId;

        //    var json = JsonConvert.SerializeObject(vpn, _serializerSettings);
        //    return new OkObjectResult(json);
        //}

        //[HttpPost]
        //[Authorize(Policy = Constants.Roles.Customer)]
        //public async Task<IActionResult> AddVpn([FromBody] AzureCommunication.Models.VpnAdd vpn)
        //{
        //    AzureCredentials credentials = GetCurrentAzureCredentials(vpn.CustomerId);
        //    AzureCommunicationService azure = new AzureCommunicationService(credentials.TenantId, credentials.ClientId, credentials.ClientSecret);

        //    try
        //    {
        //        var list = await azure.AddVPNAsync(vpn.SubscriptionId, vpn.ResourceGroupName, vpn);
        //        var json = JsonConvert.SerializeObject(list, _serializerSettings);
        //        return new OkObjectResult(json);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        //[HttpPut]
        //[Authorize(Policy = Constants.Roles.Customer)]
        //public async Task<IActionResult> EditVpn([FromBody] AzureCommunication.Models.VpnEdit vpn)
        //{
        //    AzureCredentials credentials = GetCurrentAzureCredentials(vpn.CustomerId);
        //    AzureCommunicationService azure = new AzureCommunicationService(credentials.TenantId, credentials.ClientId, credentials.ClientSecret);

        //    try
        //    {
        //        var list = await azure.EditVPNAsync(vpn.SubscriptionId, vpn.ResourceGroupName, vpn);
        //        var json = JsonConvert.SerializeObject(list, _serializerSettings);
        //        return new OkObjectResult(json);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        //[HttpDelete]
        //[Authorize(Policy = Constants.Roles.Customer)]
        //public IActionResult DeleteVpn([FromBody] VpnRef vpnRef)
        //{
        //    AzureCredentials credentials = GetCurrentAzureCredentials(vpnRef.CustomerId);
        //    AzureCommunicationService azure = new AzureCommunicationService(credentials.TenantId, credentials.ClientId, credentials.ClientSecret);

        //    try
        //    {
        //        azure.DeleteVPNAsync(vpnRef.SubscriptionId, vpnRef.ResourceGroupName, vpnRef.Id);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //    return NoContent();
        //}

        //[HttpGet]
        //[Authorize(Policy = Constants.Roles.Customer)]
        //[Route("configuration")]
        //public IActionResult DownloadConfig([FromBody] VpnRef vpnRef)
        //{
        //    try
        //    {
        //        var assembly = typeof(AMS.Program).GetTypeInfo().Assembly;
        //        Stream resource = assembly.GetManifestResourceStream("AMS.Templates.Fortigate60E.conf");
        //        return File(resource, "application/octet-stream");
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
    }
}