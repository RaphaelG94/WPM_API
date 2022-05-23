using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WPM_API.Code.Infrastructure;
using WPM_API.Code.Infrastructure.LogOn;
using WPM_API.Common;
using WPM_API.Data.DataContext.Entities;
using WPM_API.Options;
using WPM_API.TransferModels;

namespace WPM_API.Controllers
{
    [Route("customerHardwareModels/{customerId}")]
    [Authorize(Policy = Constants.Policies.Customer)]
    public class CustomerHardwareModelController : BasisController
    {
        public CustomerHardwareModelController(AppSettings appSettings, ConnectionStrings connectionStrings, OrderEmailOptions orderEmailOptions, AgentEmailOptions agentEmailOptions, SendMailCreds sendMailCreds, SiteOptions siteOptions, ILogonManager logonManager) : base(appSettings, connectionStrings, orderEmailOptions, agentEmailOptions, sendMailCreds, siteOptions, logonManager)
        {
        }

        [HttpGet]
        public IActionResult GetCustomerHardwareModels([FromRoute] string customerId)
        {
            List<CustomerHardwareModel> hardwareModels = UnitOfWork.CustomerHardwareModels.GetAll("Drivers").Where(x => x.CustomerId == customerId).ToList();

            var json = JsonConvert.SerializeObject(Mapper.Map<List<CustomerHardwareModelViewModel>>(hardwareModels), serializerSettings);

            return Ok(json);
        }
    }

    public class CustomerHardwareModelViewModel
    {
        public string Id { get; set; }
        public int Counter { get; set; }
        public string Name { get; set; }
        public List<FileRef> Drivers { get; set; }
        public string CustomerId { get; set; }
    }
}
