using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Linq.Dynamic.Core;
using WPM_API.Code.Infrastructure;
using WPM_API.Code.Infrastructure.LogOn;
using WPM_API.Common;
using WPM_API.Models;
using WPM_API.Options;
using DATA = WPM_API.Data.DataContext.Entities;

namespace WPM_API.Controllers.Client
{
    [Route("wmiInventory")]
    public class WMIInventoryCmdsController : BasisController
    {
        public WMIInventoryCmdsController(AppSettings appSettings, ConnectionStrings connectionStrings, OrderEmailOptions orderEmailOptions, AgentEmailOptions agentEmailOptions, SendMailCreds sendMailCreds, SiteOptions siteOptions, ILogonManager logonManager) : base(appSettings, connectionStrings, orderEmailOptions, agentEmailOptions, sendMailCreds, siteOptions, logonManager)
        {
        }

        [HttpPost]
        [Authorize(Policy = Constants.Policies.Customer)]
        public IActionResult AddWMIInventoryCmd([FromBody] WMIInventoryCmdViewModel addData)
        {
            // Check if client exists
            DATA.Client client = UnitOfWork.Clients.GetOrNull(addData.ClientId);
            if (client == null)
            {
                return new BadRequestObjectResult("The client does not exist!");
            }
            // Create new entity & save in DB
            DATA.WMIInvenotryCmds newWMI = UnitOfWork.WMIInventoryCmds.CreateEmpty();
            newWMI.ClientId = addData.ClientId;
            newWMI.Command = addData.Command;
            newWMI.Name = addData.Name;
            UnitOfWork.SaveChanges();

            // Return new entity
            var json = JsonConvert.SerializeObject(Mapper.Map<WMIInventoryCmdViewModel>(newWMI), serializerSettings);
            return new OkObjectResult(json);
        }

        [HttpGet]
        [Authorize(Policy = Constants.Policies.Customer)]
        [Route("{clientId}")]
        public IActionResult GetAllWMIInventoryCmds([FromRoute] string clientId)
        {
            List<DATA.WMIInvenotryCmds> wmiCmds = UnitOfWork.WMIInventoryCmds.GetAll("Client").Where(x => x.ClientId == clientId).ToList();
            List<WMIInventoryCmdViewModel> result = Mapper.Map<List<WMIInventoryCmdViewModel>>(wmiCmds);
            var json = JsonConvert.SerializeObject(result, serializerSettings);
            return new OkObjectResult(json);
        }
    }
}
