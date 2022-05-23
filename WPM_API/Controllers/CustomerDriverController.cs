using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Linq.Dynamic.Core;
using WPM_API.Code.Infrastructure;
using WPM_API.Code.Infrastructure.LogOn;
using WPM_API.Common;
using WPM_API.Data.DataContext.Entities;
using WPM_API.Options;

namespace WPM_API.Controllers
{
    [Authorize(Policy = Constants.Roles.Customer)]
    [Route("customerDrivers/{customerId}")]
    public class CustomerDriverController : BasisController
    {
        public CustomerDriverController(AppSettings appSettings, ConnectionStrings connectionStrings, OrderEmailOptions orderEmailOptions, AgentEmailOptions agentEmailOptions, SendMailCreds sendMailCreds, SiteOptions siteOptions, ILogonManager logonManager) : base(appSettings, connectionStrings, orderEmailOptions, agentEmailOptions, sendMailCreds, siteOptions, logonManager)
        {
        }

        [HttpGet]
        public IActionResult GetCustomerDrivers([FromRoute] string customerId)
        {
            List<CustomerDriver> result = UnitOfWork.CustomerDrivers.GetAll().Where(x => x.CustomerId == customerId).ToList();

            var json = JsonConvert.SerializeObject(result, serializerSettings);
            return Ok(json);
        }
    }
}
