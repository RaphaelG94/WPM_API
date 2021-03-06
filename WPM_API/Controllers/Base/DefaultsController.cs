using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WPM_API.Code.Infrastructure;
using WPM_API.Code.Infrastructure.LogOn;
using WPM_API.Common;
using WPM_API.Data.DataContext.Entities;
using WPM_API.Models;
using WPM_API.Options;

namespace WPM_API.Controllers.Base
{
    [Route("customers/{customerId}/defaults")]
    public class DefaultsController : BasisController
    {
        public DefaultsController(AppSettings appSettings, ConnectionStrings connectionStrings, OrderEmailOptions orderEmailOptions, AgentEmailOptions agentEmailOptions, SendMailCreds sendMailCreds, SiteOptions siteOptions, ILogonManager logonManager) : base(appSettings, connectionStrings, orderEmailOptions, agentEmailOptions, sendMailCreds, siteOptions, logonManager)
        {
        }

        /// <summary>
        /// Retrive all Defaults.
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Policy = Constants.Roles.Customer)]
        public IActionResult GetDefaults([FromRoute] string customerId)
        {
            List<DefaultViewModel> result = new List<DefaultViewModel>();
            using (var unitOfWork = CreateUnitOfWork())
            {
                result.AddRange(Mapper.Map<List<DefaultViewModel>>(unitOfWork.Customers.Get(customerId, "Defaults").Defaults));
            }
            // Serialize and return the response
            var json = JsonConvert.SerializeObject(result.OrderByDescending(x => x.Name).ToList(), serializerSettings);
            return new OkObjectResult(json);
        }

        /// <summary>
        /// Update existing Default (default only).
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="DefaultId"></param>
        /// <param name="updateDefault"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize(Policy = Constants.Roles.Customer)]
        [Route("{DefaultId}")]
        public IActionResult UpdateDefault([FromRoute] string customerId, [FromRoute] string DefaultId, [FromBody] DefaultEditViewModel updateDefault)
        {
            DefaultViewModel result = new DefaultViewModel();
            using (var unitOfWork = CreateUnitOfWork())
            {
                var customer = unitOfWork.Customers.Get(customerId, "Defaults");
                Default Default = null;
                // Default doesnt exists
                if (!customer.Defaults.Exists(x => x.Id == DefaultId))
                {
                    return new NotFoundResult();
                }
                else
                {
                    Default = customer.Defaults.Find(x => x.Id.Equals(DefaultId));
                    Default.Value = updateDefault.Value;
                }
                unitOfWork.Customers.MarkForUpdate(customer, GetCurrentUser().Id);
                unitOfWork.SaveChanges();
                result = Mapper.Map<DefaultViewModel>(unitOfWork.Customers.Get(customerId, "Defaults").Defaults.First(x => x.Name.Equals(Default.Name)));
            }
            // Serialize and return the response
            var json = JsonConvert.SerializeObject(result, serializerSettings);
            return StatusCode(201, json);
        }
    }
}