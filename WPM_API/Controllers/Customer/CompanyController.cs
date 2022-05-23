using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WPM_API.Code.Infrastructure;
using WPM_API.Code.Infrastructure.LogOn;
using WPM_API.Common;
using WPM_API.Data.DataContext.Entities;
using WPM_API.Data.Models;
using WPM_API.Models;
using WPM_API.Options;

namespace WPM_API.Controllers
{
    [Route("companies")]
    public class CompanyController : BasisController
    {
        public CompanyController(AppSettings appSettings, ConnectionStrings connectionStrings, OrderEmailOptions orderEmailOptions, AgentEmailOptions agentEmailOptions, SendMailCreds sendMailCreds, SiteOptions siteOptions, ILogonManager logonManager) : base(appSettings, connectionStrings, orderEmailOptions, agentEmailOptions, sendMailCreds, siteOptions, logonManager)
        {
        }

        /// <summary>
        /// Create a new company with it's headquarter and (optionally) it's expert
        /// </summary>
        /// <param name="companyAdd">New company</param>
        /// <returns>Company</returns>
        [HttpPost]
        [Authorize(Policy = Constants.Roles.Customer)]
        public IActionResult AddCompany([FromBody] AddCompanyViewModel companyAdd)
        {
            Company company = UnitOfWork.Companies.CreateEmpty();
            company.CorporateName = companyAdd.CorporateName;
            company.Description = companyAdd.Description;
            company.FormOfOrganization = companyAdd.FormOfOrganization;
            company.LinkWebsite = companyAdd.LinkWebsite;
            company.Type = companyAdd.Type;
            company.CustomerId = companyAdd.CustomerId;
            Location headquarter = null;
            try
            {
                headquarter = UnitOfWork.Locations.Get(companyAdd.HeadquarterId);
                company.HeadquarterId = companyAdd.HeadquarterId;
                company.Locations = new List<Location>();
                company.Locations.Add(headquarter);
            }
            catch (Exception exc)
            {
                // Company does not get any HQ. Needed for adding companies in nested dialogs (e.g: Customer -> Settings -> Add Location)                   
            }

            Person expert = null;
            try
            {
                expert = UnitOfWork.Persons.Get(companyAdd.ExpertId);
                company.ExpertId = companyAdd.ExpertId;
                company.Employees = new List<Person>();
                company.Employees.Add(expert);
            }
            catch (Exception exc)
            {
                // Company does not get any expert.
            }

            try
            {
                UnitOfWork.SaveChanges();
            }
            catch (Exception exc)
            {
                return BadRequest("Company could not be created. " + exc.Message);
            }

            company = UnitOfWork.Companies.GetAll(CompanyIncludes.Expert, CompanyIncludes.Headquarter).Where(x => x.Id == company.Id).Single();

            // Check if the company will be the main company of a customer & add it if it is true
            if (companyAdd.IsMainCompany)
            {
                WPM_API.Data.DataContext.Entities.Customer customer = UnitOfWork.Customers.Get(companyAdd.CustomerId);
                if (customer == null)
                {
                    return BadRequest("Customer does not exist");
                }
                customer.MainCompanyId = company.Id;

                try
                {
                    UnitOfWork.SaveChanges();
                }
                catch (Exception exc)
                {
                    return BadRequest("Internal Server Error! " + exc.Message);
                }
            }


            var json = JsonConvert.SerializeObject(Mapper.Map<Company, CompanyViewModel>(company), serializerSettings);
            return new OkObjectResult(json);
        }

        /// <summary>
        /// Delete a specific company
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns>CompanyViewModel</returns>
        [HttpDelete]
        [Authorize(Policy = Constants.Policies.Customer)]
        [Route("{companyId}")]
        public IActionResult DeleteCompany([FromRoute] string companyId)
        {
            Company toDelete = UnitOfWork.Companies.Get(companyId);
            if (toDelete == null)
            {
                return BadRequest("Company does not exist");
            }

            WPM_API.Data.DataContext.Entities.Customer customer = UnitOfWork.Customers.Get(toDelete.CustomerId);
            if (customer != null)
            {
                if (customer.MainCompanyId == toDelete.Id)
                {
                    customer.MainCompanyId = null;
                }
            }

            // 'Fire' all employees of the deleted company
            List<Person> companiesEmployees = UnitOfWork.Persons.GetAll().Where(x => x.CompanyId == companyId).ToList();
            foreach (Person employee in companiesEmployees)
            {
                employee.CompanyId = null;
            }

            // Tell all locations that the company does not exist anymore
            List<Location> locations = UnitOfWork.Locations.GetAll().Where(x => x.CompanyId == companyId).ToList();
            foreach (Location location in locations)
            {
                location.CompanyId = null;
            }

            // Mark company for delete
            UnitOfWork.Companies.MarkForDelete(toDelete, GetCurrentUser().Id);

            try
            {
                UnitOfWork.SaveChanges();
            }
            catch (Exception exc)
            {
                return BadRequest("The company could not be deleted. " + exc.Message);
            }

            var json = JsonConvert.SerializeObject(Mapper.Map<Company, CompanyViewModel>(toDelete), serializerSettings);
            return new OkObjectResult(json);
        }

        /// <summary>
        /// Get all companies of a specific customer
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Constants.Roles.Customer)]
        [Route("{customerId}")]
        public IActionResult GetCompaniesOfCustomer([FromRoute] string customerId)
        {
            List<CompanyOverviewViewModel> companies = new List<CompanyOverviewViewModel>();
            List<Company> dbEntries = UnitOfWork.Companies.GetAll(CompanyIncludes.Expert, CompanyIncludes.Headquarter)
                                        .Where(x => x.CustomerId == customerId)
                                        .ToList();

            companies = Mapper.Map<List<Company>, List<CompanyOverviewViewModel>>(dbEntries);

            // Serialize and return result
            var json = JsonConvert.SerializeObject(companies, serializerSettings);
            return new OkObjectResult(json);
        }

        [HttpPost]
        [Authorize(Policy = Constants.Policies.Systemhouse)]
        [Route("{companyId}/addHeadquarter")]
        public IActionResult CreateAndAddHeadquarter([FromBody] AddLocationViewModel headquarterData, [FromRoute] string companyId)
        {
            // Fetch company & proof for existence
            Company company = UnitOfWork.Companies.Get(companyId);
            if (company == null)
            {
                return BadRequest("The company does not exist");
            }
            // Load customer & proof for existence
            WPM_API.Data.DataContext.Entities.Customer customer = UnitOfWork.Customers.Get(company.CustomerId);
            if (customer == null)
            {
                return BadRequest("The customer does not exist");
            }
            Location headquarter = UnitOfWork.Locations.CreateEmpty();
            headquarter.Name = headquarterData.Name;
            headquarter.Country = headquarterData.Country;
            headquarter.Street = headquarterData.Street;
            headquarter.Number = headquarterData.Number;
            headquarter.Postcode = headquarterData.Postcode;
            headquarter.PublicIP = headquarterData.PublicIP;
            headquarter.UpdatedByUserId = LoggedUser.Id;
            headquarter.UpdatedDate = DateTime.Now;

            // Add headquarter to company and customer
            headquarter.CustomerId = company.CustomerId;
            headquarter.CompanyId = companyId;
            // customer.Locations.Add(headquarter);
            company.HeadquarterId = headquarter.Id;
            company.Locations.Add(headquarter);

            // Finish DB transaction
            try
            {
                UnitOfWork.SaveChanges();
            }
            catch (Exception exc)
            {
                return BadRequest("Headquarter could not be created. " + exc.Message);
            }

            // Serialize & return result
            var json = JsonConvert.SerializeObject(Mapper.Map<Location, LocationViewModel>(headquarter), serializerSettings);
            return new OkObjectResult(json);
        }

        [HttpPost]
        [Authorize(Policy = Constants.Policies.Systemhouse)]
        [Route("{companyId}/addExpert")]
        public IActionResult CreateAndAddExpert([FromRoute] string companyId, [FromBody] PersonViewModel expertData)
        {
            // Load company & proof existence
            Company company = UnitOfWork.Companies.Get(companyId);
            if (company == null)
            {
                return BadRequest("Company does not exist");
            }

            // Create expert and set values
            Person expert = UnitOfWork.Persons.CreateEmpty();
            expert.Title = expertData.Title;
            expert.GivenName = expertData.GivenName;
            expert.MiddleName = expertData.MiddleName;
            expert.Surname = expertData.Surname;
            expert.AcademicDegree = expertData.AcademicDegree;
            expert.EmployeeType = expertData.EmployeeType;
            expert.CostCenter = expertData.CostCenter;
            expert.PhoneNr = expertData.PhoneNr;
            expert.FaxNr = expertData.FaxNr;
            expert.MobileNr = expertData.MobileNr;
            expert.EmailPrimary = expertData.EmailPrimary;
            expert.State = expertData.State;
            expert.EmailOptional = expertData.EmailOptional;
            expert.UpdatedByUserId = LoggedUser.Id;
            expert.UpdatedDate = DateTime.Now;

            expert.CustomerId = company.CustomerId;
            expert.CompanyId = companyId;
            company.ExpertId = expert.Id;

            // Finish DB transaction
            try
            {
                UnitOfWork.SaveChanges();
            }
            catch (Exception exc)
            {
                return BadRequest("The expert could not be created. " + exc.Message);
            }

            // Serialize & return result
            var json = JsonConvert.SerializeObject(Mapper.Map<Person, PersonViewModel>(expert), serializerSettings);
            return new OkObjectResult(json);
        }

        [HttpPut]
        [Authorize(Policy = Constants.Policies.Customer)]
        [Route("{companyId}")]
        public IActionResult UpdateMainCompany([FromRoute] string companyId, [FromBody] UpdateMainCompanyViewModel updateData)
        {
            Company mainCompany = UnitOfWork.Companies.Get(companyId);
            if (mainCompany == null)
            {
                return BadRequest("The company does not exist");
            }
            // Update mainCompany
            mainCompany.CorporateName = updateData.CorporateName;
            mainCompany.FormOfOrganization = updateData.FormOfOrganization;
            mainCompany.LinkWebsite = updateData.LinkWebsite;
            mainCompany.Description = updateData.Description;
            mainCompany.Type = updateData.Type;

            if (updateData.ExpertId != null && updateData.ExpertId != "")
            {
                mainCompany.ExpertId = updateData.ExpertId;
                // Load Expert & set company
                Person expert = UnitOfWork.Persons.Get(mainCompany.ExpertId);
                expert.CompanyId = companyId;
            }
            if (updateData.HeadquarterId != null && updateData.HeadquarterId != "")
            {
                mainCompany.HeadquarterId = updateData.HeadquarterId;
                // Load Location & set company
                Location headquarter = UnitOfWork.Locations.Get(mainCompany.HeadquarterId);
                headquarter.CompanyId = companyId;
            }

            try
            {
                UnitOfWork.SaveChanges();
            }
            catch (Exception exc)
            {
                return BadRequest("The changes could not be saved. " + exc.Message);
            }

            // Serialize and return the result
            var json = JsonConvert.SerializeObject(Mapper.Map<Company, CompanyOverviewViewModel>(mainCompany), serializerSettings);
            return new OkObjectResult(json);
        }
    }

}
