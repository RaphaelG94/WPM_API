using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WPM_API.Code.Infrastructure;
using WPM_API.Code.Infrastructure.LogOn;
using WPM_API.Common;
using WPM_API.Data.DataContext.Entities;
using WPM_API.Models;
using WPM_API.Options;

namespace WPM_API.Controllers
{
    [Route("domainRegistrationTemp")]
    public class DomainRegistrationTempController : BasisController
    {
        public DomainRegistrationTempController(AppSettings appSettings, ConnectionStrings connectionStrings, OrderEmailOptions orderEmailOptions, AgentEmailOptions agentEmailOptions, SendMailCreds sendMailCreds, SiteOptions siteOptions, ILogonManager logonManager) : base(appSettings, connectionStrings, orderEmailOptions, agentEmailOptions, sendMailCreds, siteOptions, logonManager)
        {
        }

        [HttpPost]
        [Authorize(Policy = Constants.Policies.Admin)]
        public IActionResult CreateDomainRegistrationTemp([FromBody] DomainRegistrationTempViewModel addData)
        {
            try
            {
                DomainRegistrationTemp newDomain = UnitOfWork.DomainRegistrations.CreateEmpty();
                newDomain.Description = addData.Description;
                newDomain.Name = addData.Name;
                UnitOfWork.SaveChanges();

                // Return new data
                var json = JsonConvert.SerializeObject(Mapper.Map<DomainRegistrationTempViewModel>(newDomain), serializerSettings);
                return new OkObjectResult(json);
            }
            catch (Exception e)
            {
                return BadRequest("ERROR: " + e.Message);
            }
        }

        [HttpGet]
        [Authorize(Policy = Constants.Policies.Customer)]
        public IActionResult GetDomainRegistrationTemps()
        {
            List<DomainRegistrationTemp> domainRegistrations = UnitOfWork.DomainRegistrations.GetAll().ToList();
            var json = JsonConvert.SerializeObject(Mapper.Map<List<DomainRegistrationTempViewModel>>(domainRegistrations), serializerSettings);
            return new OkObjectResult(json);
        }

        [HttpPut]
        [Authorize(Policy = Constants.Policies.Admin)]
        public IActionResult UpdateDomainRegistrationTemp([FromBody] DomainRegistrationTempViewModel editData)
        {
            try
            {
                DomainRegistrationTemp editDomain = UnitOfWork.DomainRegistrations.Get(editData.Id);
                editDomain.Description = editData.Description;
                editDomain.Name = editData.Name;
                UnitOfWork.SaveChanges();

                // Return edited data
                var json = JsonConvert.SerializeObject(Mapper.Map<DomainRegistrationTempViewModel>(editDomain), serializerSettings);
                return new OkObjectResult(json);
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult("ERROR :" + e.Message);
            }
        }
    }
}
