//using System;
//using System.Threading.Tasks;
//using WPM_API.Controllers;
//using WPM_API.Models;
//using  WPM_API.Azure;
//using  WPM_API.Azure.Models;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Newtonsoft.Json;
//using WPM_API.Data.DataContext.Entities;
//using WPM_API.Common;

//[Route("virtual-machines")]
//public class VirtualMachineController : BasisController
//{
//    public VirtualMachineController()
//    {
//    }

//    //[HttpGet]
//    //[Authorize(Policy = Constants.Roles.Customer)]
//    //public async Task<IActionResult> GetVirtualMachines([FromBody] VirtualMachineRefViewModel vm)
//    //{
//    //    AzureCredentials credentials = GetCurrentAzureCredentials(vm.CustomerId);
//    //    AzureCommunicationService azure = new AzureCommunicationService(credentials.TenantId, credentials.ClientId, credentials.ClientSecret);

//    //    try
//    //    {
//    //        var machine = await azure.GetVirtualMachineAsync(vm.SubscriptionId, vm.ResourceGroupName, vm.Id);
//    //        var json = JsonConvert.SerializeObject(Mapper.Map<VirtualMachineViewModel>(machine), _serializerSettings);
//    //        return new OkObjectResult(json);
//    //    }
//    //    catch (Exception ex)
//    //    {
//    //        return BadRequest(ex.Message);
//    //    }
//    //}

//    //[HttpPost]
//    //[Authorize(Policy = Constants.Roles.Customer)]
//    //public async Task<IActionResult> AddVirtualMachines([FromBody] VirtualMachineAddViewModel vm)
//    //{
//    //    AzureCredentials credentials = GetCurrentAzureCredentials(vm.CustomerId);
//    //    AzureCommunicationService azure = new AzureCommunicationService(credentials.TenantId, credentials.ClientId, credentials.ClientSecret);

//    //    try
//    //    {
//    //        var machine = await azure.AddVirtualMachineAsync(Mapper.Map<VirtualMachineAddModel>(vm));
//    //        var json = JsonConvert.SerializeObject(Mapper.Map<VirtualMachineViewModel>(machine), _serializerSettings);
//    //        return new OkObjectResult(json);
//    //    }
//    //    catch (Exception ex)
//    //    {
//    //        return BadRequest(ex.Message);
//    //    }
//    //}

//    //[HttpPut]
//    //[Authorize(Policy = Constants.Roles.Customer)]
//    //public async Task<IActionResult> EditVirtualMachines([FromBody] VirtualMachineEditViewModel vm)
//    //{
//    //    AzureCredentials credentials = GetCurrentAzureCredentials(vm.CustomerId);
//    //    AzureCommunicationService azure = new AzureCommunicationService(credentials.TenantId, credentials.ClientId, credentials.ClientSecret);

//    //    try
//    //    {
//    //        var machine = await azure.EditVirtualMachineAsync(Mapper.Map<VirtualMachineEditModel>(vm));
//    //        var json = JsonConvert.SerializeObject(Mapper.Map<VirtualMachineViewModel>(machine), _serializerSettings);
//    //        return new OkObjectResult(json);
//    //    }
//    //    catch (Exception ex)
//    //    {
//    //        return BadRequest(ex.Message);
//    //    }
//    //}

//    //[HttpDelete]
//    //[Authorize(Policy = Constants.Roles.Customer)]
//    //public IActionResult DeleteVirtualMachines([FromBody] VirtualMachineRefViewModel vm)
//    //{
//    //    AzureCredentials credentials = GetCurrentAzureCredentials(vm.CustomerId);
//    //    AzureCommunicationService azure = new AzureCommunicationService(credentials.TenantId, credentials.ClientId, credentials.ClientSecret);
//    //    try
//    //    {
//    //        azure.DeleteVirtualMachineAsync(vm.SubscriptionId, vm.ResourceGroupName, vm.Id);
//    //    }
//    //    catch (Exception ex)
//    //    {
//    //        return BadRequest(ex.Message);
//    //    }
//    //    return NoContent();
//    //}

//    //[HttpPost]
//    //[Authorize(Policy = Constants.Roles.Customer)]
//    //[Route("execute")]
//    //public async Task<IActionResult> ExecuteScript([FromBody] VirtualMachineExecuteViewModel vmScript)
//    //{
//    //    AzureCredentials credentials = GetCurrentAzureCredentials(vmScript.CustomerId);
//    //    AzureCommunicationService azure = new AzureCommunicationService(credentials.TenantId, credentials.ClientId, credentials.ClientSecret);

//    //    try
//    //    {
//    //        await azure.ExecuteVirtualMachineScriptAsync(Mapper.Map<VirtualMachineRefModel>(vmScript), vmScript.ScriptName, vmScript.Arguments);
//    //        return Ok();
//    //    }
//    //    catch (Exception ex)
//    //    {
//    //        return BadRequest(ex.Message);
//    //    }
//    //}
//}