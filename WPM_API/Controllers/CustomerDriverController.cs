using WPM_API.Common;
using WPM_API.Data.DataContext.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace WPM_API.Controllers
{
    [Authorize(Policy = Constants.Roles.Customer)]
    [Route("customerDrivers/{customerId}")]
    public class CustomerDriverController : BasisController
    {
        [HttpGet]       
        public IActionResult GetCustomerDrivers([FromRoute] string customerId)
        {
            List<CustomerDriver> result = UnitOfWork.CustomerDrivers.GetAll().Where(x => x.CustomerId == customerId).ToList();

            var json = JsonConvert.SerializeObject(result, _serializerSettings);
            return Ok(json);
        }
    }
}
