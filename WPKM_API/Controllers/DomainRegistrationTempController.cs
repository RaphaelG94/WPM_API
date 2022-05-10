using WPM_API.Common;
using WPM_API.Data.DataContext.Entities;
using WPM_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WPM_API.Controllers
{
    [Route("domainRegistrationTemp")]
    public class DomainRegistrationTempController : BasisController
    {
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
                var json = JsonConvert.SerializeObject(Mapper.Map<DomainRegistrationTempViewModel>(newDomain), _serializerSettings);
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
            var json = JsonConvert.SerializeObject(Mapper.Map<List<DomainRegistrationTempViewModel>>(domainRegistrations), _serializerSettings);
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
                var json = JsonConvert.SerializeObject(Mapper.Map<DomainRegistrationTempViewModel>(editDomain), _serializerSettings);
                return new OkObjectResult(json);
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult("ERROR :" + e.Message);
            }
        }
    }
}
