using WPM_API.Common;
using WPM_API.Data.DataContext;
using WPM_API.Data.DataContext.Entities;
using WPM_API.FileRepository;
using WPM_API.Models;
using WPM_API.Models.Release_Mgmt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace WPM_API.Controllers.Releas_Mgmt
{
    [Route("drivers")]
    public class DriverController : BasisController
    {
        [HttpGet]
        [Authorize(Policy = Constants.Policies.Systemhouse)]
        public IActionResult GetDrivers()
        {
            List<Driver> drivers = UnitOfWork.Drivers.GetAll().Where(x => x.CreatedByUserId == GetCurrentUser().Id).ToList();
            DriversViewModel result = new DriversViewModel();
            result.Drivers = new List<DriverViewModel>();
            foreach (Driver driver in drivers)
            {
                DriverViewModel driverData = Mapper.Map<DriverViewModel>(driver);
                result.Drivers.Add(driverData);
            }
            var json = JsonConvert.SerializeObject(result, _serializerSettings);
            return new OkObjectResult(json);
        }

        [HttpPost]
        [Authorize(Policy = Constants.Policies.Systemhouse)]
        public IActionResult AddDriver ([FromBody] DriverViewModel driverData)
        {
            // Create new driver
            Driver newDriver = Mapper.Map<Driver>(driverData);
            UnitOfWork.Drivers.MarkForInsert(newDriver, GetCurrentUser().Id);
            UnitOfWork.SaveChanges();

            // Return driver data
            DriverViewModel result = Mapper.Map<DriverViewModel>(newDriver);
      
            var json = JsonConvert.SerializeObject(result, _serializerSettings);
            return new OkObjectResult(json);
        }

        [HttpPost]
        [Route("edit")]
        public IActionResult EditDriver([FromBody] DriverViewModel data)
        {
            Driver toEdit = UnitOfWork.Drivers.GetOrNull(data.Id);
            toEdit.Name = data.Name;
            toEdit.SubFolderPath = data.SubFolderPath;
            toEdit.Vendor = data.Vendor;
            toEdit.Version = data.Version;
            toEdit.ConnectionString = data.ConnectionString;
            toEdit.Description = data.Description;
            toEdit.ContainerName = data.ContainerName;

            UnitOfWork.Drivers.MarkForUpdate(toEdit, GetCurrentUser().Id);
            UnitOfWork.SaveChanges();

            var json = JsonConvert.SerializeObject(Mapper.Map<DriverViewModel>(toEdit), _serializerSettings);
            return Ok(json);
        }

        [HttpDelete]
        [Authorize(Policy = Constants.Policies.Systemhouse)]
        [Route("{driverId}")]
        public async Task<IActionResult> DeleteDriverAsync ([FromRoute] string driverId)
        {
            Driver toDelete = UnitOfWork.Drivers.Get(driverId);
            if (toDelete == null)
            {
                return BadRequest("ERROR: The driver does not exist");
            }
            // Delete files
            ResourcesRepository resourcesRepository = new ResourcesRepository(_connectionStrings.FileRepository, _appSettings.ResourcesRepositoryFolder);

            // Delete driver and save changes
            UnitOfWork.Drivers.MarkForDelete(toDelete, GetCurrentUser().Id);
            UnitOfWork.SaveChanges();
            
            return new OkResult();
        }

        [HttpPost]
        [Route("publishInShop")]
        public IActionResult PublishDriver (DriverViewModel data)
        {
            using(var unitOfWork = CreateUnitOfWork())
            {
                Driver driver = unitOfWork.Drivers.Get(data.Id);

                driver.PublishInShop = true;

                unitOfWork.Drivers.MarkForUpdate(driver, GetCurrentUser().Id);
                unitOfWork.SaveChanges();

                DriverViewModel result = Mapper.Map<DriverViewModel>(driver);

                string json = JsonConvert.SerializeObject(result, _serializerSettings);

                return Ok(json);
            }            
        }
    }
}
