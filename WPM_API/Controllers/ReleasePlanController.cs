using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WPM_API.Code.Infrastructure;
using WPM_API.Code.Infrastructure.LogOn;
using WPM_API.Models;
using WPM_API.Options;
using DATA = WPM_API.Data.DataContext.Entities;

namespace WPM_API.Controllers
{
    [Route("release-plans")]
    [Authorize(Policy = Common.Constants.Policies.Customer)]
    public class ReleasePlanController : BasisController
    {
        public ReleasePlanController(AppSettings appSettings, ConnectionStrings connectionStrings, OrderEmailOptions orderEmailOptions, AgentEmailOptions agentEmailOptions, SendMailCreds sendMailCreds, SiteOptions siteOptions, ILogonManager logonManager) : base(appSettings, connectionStrings, orderEmailOptions, agentEmailOptions, sendMailCreds, siteOptions, logonManager)
        {
        }

        [HttpPost]
        public IActionResult AddReleasePlan([FromBody] ReleasePlanViewModel releasePlan)
        {
            DATA.Customer customer = UnitOfWork.Customers.GetOrNull(releasePlan.CustomerId);
            if (customer == null)
            {
                return BadRequest("ERROR: The customer does not exist");
            }
            DATA.ReleasePlan newReleasePlan = Mapper.Map<DATA.ReleasePlan>(releasePlan);
            UnitOfWork.ReleasePlans.MarkForInsert(newReleasePlan, GetCurrentUser().Id);
            UnitOfWork.SaveChanges();

            var json = JsonConvert.SerializeObject(Mapper.Map<ReleasePlanViewModel>(newReleasePlan), serializerSettings);
            return Ok(json);
        }

        [HttpPut]
        public IActionResult EditReleasePlan([FromBody] ReleasePlanViewModel data)
        {
            DATA.ReleasePlan toEdit = UnitOfWork.ReleasePlans.GetOrNull(data.Id);
            if (toEdit == null)
            {
                return BadRequest("ERROR: The Release-Plan does not exist");
            }

            toEdit.Name = data.Name;

            UnitOfWork.ReleasePlans.MarkForUpdate(toEdit, GetCurrentUser().Id);
            UnitOfWork.SaveChanges();

            var json = JsonConvert.SerializeObject(Mapper.Map<ReleasePlanViewModel>(toEdit), serializerSettings);
            return Ok(json);
        }

        [HttpDelete]
        [Route("delete/{releasePlanId}")]
        public IActionResult DeleteReleasePlan([FromRoute] string releasePlanId)
        {
            DATA.ReleasePlan toDelete = UnitOfWork.ReleasePlans.GetOrNull(releasePlanId);
            if (toDelete == null)
            {
                return BadRequest("ERROR: The Release-Plan does not exist");
            }

            UnitOfWork.ReleasePlans.MarkForDelete(toDelete, GetCurrentUser().Id);
            UnitOfWork.SaveChanges();

            var json = JsonConvert.SerializeObject(Mapper.Map<ReleasePlanViewModel>(toDelete), serializerSettings);
            return Ok(json);
        }

        [HttpGet]
        [Route("{customerId}")]
        public IActionResult GetCustomersReleasePlans([FromRoute] string customerId)
        {
            List<DATA.ReleasePlan> releasePlans = UnitOfWork.ReleasePlans.GetAll("Customer").Where(x => x.CustomerId == customerId).ToList();

            var json = JsonConvert.SerializeObject(Mapper.Map<List<ReleasePlanViewModel>>(releasePlans), serializerSettings);
            return Ok(json);
        }

        public class ReleasePlanViewModel
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string CustomerId { get; set; }
            public CustomerViewModel Customer { get; set; }
        }
    }
}
