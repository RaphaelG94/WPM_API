using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Management.Network.Models;
using Newtonsoft.Json;
using WPM_API.Code.Infrastructure;
using WPM_API.Code.Infrastructure.LogOn;
using WPM_API.Common;
using WPM_API.Data.DataContext.Entities;
using WPM_API.Models;
using WPM_API.Options;

namespace WPM_API.Controllers
{
    [Route("locations")]
    public class LocationController : BasisController
    {
        public LocationController(AppSettings appSettings, ConnectionStrings connectionStrings, OrderEmailOptions orderEmailOptions, AgentEmailOptions agentEmailOptions, SendMailCreds sendMailCreds, SiteOptions siteOptions, ILogonManager logonManager) : base(appSettings, connectionStrings, orderEmailOptions, agentEmailOptions, sendMailCreds, siteOptions, logonManager)
        {
        }

        [HttpPost]
        [Authorize(Policy = Constants.Roles.Customer)]
        public IActionResult AddLocation([FromBody] AddLocationViewModel locationAdd)
        {
            Location location = UnitOfWork.Locations.CreateEmpty();
            location.Name = locationAdd.Name;
            location.NameAbbreviation = locationAdd.NameAbbreviation;
            location.Country = locationAdd.Country;
            location.CountryAbbreviation = locationAdd.CountryAbbreviation;
            location.City = locationAdd.City;
            location.CityAbbreviation = locationAdd.CityAbbreviation;
            location.Street = locationAdd.Street;
            location.Number = locationAdd.Number;
            location.Postcode = locationAdd.Postcode;
            location.PublicIP = locationAdd.PublicIP;
            location.TimeZone = locationAdd.TimeZone;
            location.DownloadSpeed = locationAdd.DownloadSpeed;
            location.UploadSpeed = locationAdd.UploadSpeed;
            location.Type = locationAdd.Type;
            location.AzureLocation = locationAdd.AzureLocation;
            location.CustomerId = locationAdd.CustomerId;
            if (locationAdd.CompanyId.Length != 0)
            {
                location.CompanyId = locationAdd.CompanyId;
            }

            location.UpdatedByUserId = LoggedUser.Id;
            location.UpdatedDate = DateTime.Now;

            try
            {
                UnitOfWork.SaveChanges();
            }
            catch (Exception ex)
            {
                return BadRequest("Location could not be created. " + ex.Message);
            }

            var json = JsonConvert.SerializeObject(Mapper.Map<Location, AddLocationViewModel>(location), serializerSettings);
            return new OkObjectResult(json);
        }

        [HttpGet]
        [Authorize(Constants.Roles.Customer)]
        [Route("{customerId}")]
        public IActionResult GetLocationsOfCustomer([FromRoute] string customerId)
        {
            List<LocationViewModel> locations = new List<LocationViewModel>();
            List<Location> dbEntries = UnitOfWork.Locations
                                        .GetAll().Where(x => x.CustomerId == customerId)
                                        .ToList();

            locations = Mapper.Map<List<Location>, List<LocationViewModel>>(dbEntries);

            // Serialize and return the result
            var json = JsonConvert.SerializeObject(locations, serializerSettings);
            return new OkObjectResult(json);
        }

        [HttpDelete]
        [Authorize(Policy = Constants.Policies.Customer)]
        [Route("{locationId}")]
        public IActionResult DeleteLocation([FromRoute] string locationId)
        {
            Location location = UnitOfWork.Locations.Get(locationId);
            if (location == null)
            {
                return BadRequest("Location does not exist");
            }

            UnitOfWork.Locations.MarkForDelete(location, GetCurrentUser().Id);

            // Check for being a headquarter
            Company company = UnitOfWork.Companies.Get(location.CompanyId);
            if (company != null)
            {
                if (company.HeadquarterId == locationId)
                {
                    company.HeadquarterId = null;
                }
            }

            try
            {
                UnitOfWork.SaveChanges();
            }
            catch (Exception exc)
            {
                return BadRequest("Could not delete location. " + exc.Message);
            }

            var json = JsonConvert.SerializeObject(Mapper.Map<Location, AddLocationViewModel>(location), serializerSettings);
            return new OkObjectResult(json);
        }

        /// <summary>
        /// Update a specific location database entry.
        /// </summary>
        /// <param name="locationData"></param>
        /// <returns>AddLocation</returns>
        [HttpPut]
        [Authorize(Policy = Constants.Policies.Customer)]
        public IActionResult UpdateLocation([FromBody] AddLocationViewModel locationData)
        {
            // Load location entity & check for existence
            Location toUpdate = UnitOfWork.Locations.Get(locationData.Id);
            if (toUpdate == null)
            {
                return BadRequest("Location does not exist");
            }

            // Set name, company & update data
            toUpdate.Name = locationData.Name;
            toUpdate.NameAbbreviation = locationData.NameAbbreviation;
            toUpdate.CompanyId = locationData.CompanyId;
            toUpdate.UpdatedByUserId = GetCurrentUser().Id;
            toUpdate.UpdatedDate = DateTime.Now;

            // Edit location db entry
            if (locationData.Type == "in-cloud")
            {
                toUpdate = UpdateInCloudLocation(toUpdate, locationData);
            }
            else
            {
                toUpdate = UpdateOnPremLocation(toUpdate, locationData);
            }

            // Save changes
            try
            {
                UnitOfWork.SaveChanges();
            }
            catch (Exception exc)
            {
                return BadRequest("The location could not be updated. " + exc.Message);
            }

            var json = JsonConvert.SerializeObject(Mapper.Map<Location, AddLocationViewModel>(toUpdate), serializerSettings);
            return new OkObjectResult(json);
        }

        private Location UpdateInCloudLocation(Location toUpdate, AddLocationViewModel data)
        {
            toUpdate.AzureLocation = data.AzureLocation;
            return toUpdate;
        }

        private Location UpdateOnPremLocation(Location toUpdate, AddLocationViewModel data)
        {
            toUpdate.City = data.City;
            toUpdate.CityAbbreviation = data.CityAbbreviation;
            toUpdate.Country = data.Country;
            toUpdate.CountryAbbreviation = data.CountryAbbreviation;
            toUpdate.DownloadSpeed = data.DownloadSpeed;
            toUpdate.UploadSpeed = data.UploadSpeed;
            toUpdate.Number = data.Number;
            toUpdate.Postcode = data.Postcode;
            toUpdate.PublicIP = data.PublicIP;
            toUpdate.Street = data.Street;
            toUpdate.TimeZone = data.TimeZone;
            return toUpdate;
        }
    }
}
