using DATA = WPM_API.Data.DataContext.Entities;
using WPM_API.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;

namespace WPM_API.Controllers
{
    [Route("release-plans")]
    [Authorize(Policy = Common.Constants.Policies.Customer)]
    public class ReleasePlanController : BasisController
    {
        [HttpPost]
        public IActionResult AddReleasePlan ([FromBody] ReleasePlanViewModel releasePlan)
        {
            DATA.Customer customer = UnitOfWork.Customers.GetOrNull(releasePlan.CustomerId);
            if (customer == null)
            {
                return BadRequest("ERROR: The customer does not exist");
            }
            DATA.ReleasePlan newReleasePlan = Mapper.Map<DATA.ReleasePlan>(releasePlan);
            UnitOfWork.ReleasePlans.MarkForInsert(newReleasePlan, GetCurrentUser().Id);
            UnitOfWork.SaveChanges();

            var json = JsonConvert.SerializeObject(Mapper.Map<ReleasePlanViewModel>(newReleasePlan), _serializerSettings);
            return Ok(json);
        }

        [HttpPut]
        public IActionResult EditReleasePlan ([FromBody] ReleasePlanViewModel data)
        {
            DATA.ReleasePlan toEdit = UnitOfWork.ReleasePlans.GetOrNull(data.Id);
            if (toEdit == null)
            {
                return BadRequest("ERROR: The Release-Plan does not exist");
            }

            toEdit.Name = data.Name;

            UnitOfWork.ReleasePlans.MarkForUpdate(toEdit, GetCurrentUser().Id);
            UnitOfWork.SaveChanges();

            var json = JsonConvert.SerializeObject(Mapper.Map<ReleasePlanViewModel>(toEdit), _serializerSettings);
            return Ok(json);
        }

        [HttpDelete]
        [Route("delete/{releasePlanId}")]
        public  IActionResult DeleteReleasePlan([FromRoute] string releasePlanId) {
            DATA.ReleasePlan toDelete = UnitOfWork.ReleasePlans.GetOrNull(releasePlanId);
            if (toDelete == null)
            {
                return BadRequest("ERROR: The Release-Plan does not exist");
            }

            UnitOfWork.ReleasePlans.MarkForDelete(toDelete, GetCurrentUser().Id);
            UnitOfWork.SaveChanges();
            
            var json = JsonConvert.SerializeObject(Mapper.Map<ReleasePlanViewModel>(toDelete), _serializerSettings);
            return Ok(json);
        }

        [HttpGet]
        [Route("{customerId}")]
        public IActionResult GetCustomersReleasePlans([FromRoute] string customerId)
        {
            List<DATA.ReleasePlan> releasePlans = UnitOfWork.ReleasePlans.GetAll("Customer").Where(x => x.CustomerId == customerId).ToList();

            var json = JsonConvert.SerializeObject(Mapper.Map<List<ReleasePlanViewModel>>(releasePlans), _serializerSettings);
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
