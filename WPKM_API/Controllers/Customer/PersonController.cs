using System;
using System.Collections.Generic;
using System.Linq;
using WPM_API.Common;
using WPM_API.Data.DataContext.Entities;
using WPM_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace WPM_API.Controllers
{
    [Route("persons")]
    public class PersonController : BasisController
    {
        /// <summary>
        /// Create new Person.
        /// </summary>
        /// <param name="personAdd">New Person</param>
        /// <returns>Person</returns>
        [HttpPost]
        [Authorize(Policy = Constants.Roles.Customer)]
        public IActionResult AddPerson([FromBody] PersonViewModel personAdd) {
            Person person = UnitOfWork.Persons.CreateEmpty();
            person.GivenName = personAdd.GivenName;
            person.MiddleName = personAdd.MiddleName;
            person.Surname = personAdd.Surname;
            person.Title = personAdd.Title;
            person.AcademicDegree = personAdd.AcademicDegree;
            person.EmployeeType = personAdd.EmployeeType;
            person.CostCenter = personAdd.CostCenter;
            person.PhoneNr = personAdd.PhoneNr;
            person.FaxNr = personAdd.FaxNr;
            person.MobileNr = personAdd.MobileNr;
            person.EmailPrimary = personAdd.EmailPrimary;
            person.EmailOptional = personAdd.EmailOptional;
            person.State = personAdd.State;
            person.Domain = personAdd.Domain;
            person.DepartementName = personAdd.DepartementName;
            person.DepartementShort = personAdd.DepartementShort;
            person.RoomNr = personAdd.RoomNr;
            person.EmployeeNr = personAdd.EmployeeNr;
            person.CustomerId = personAdd.CustomerId;
            if (personAdd.CompanyId != "")
            {
                person.CompanyId = personAdd.CompanyId;
            }

            person.UpdatedDate = DateTime.Now;
            person.UpdatedByUserId = LoggedUser.Id;
           
            try
            {
                UnitOfWork.SaveChanges();
            } catch (Exception exc)
            {
                return BadRequest("Person could not be created " + " " + exc.Message + " " + exc.InnerException);
            }

            person = UnitOfWork.Persons.Get(person.Id, "Company");

            var json = JsonConvert.SerializeObject(Mapper.Map<Person, PersonViewModel>(person), _serializerSettings);
            return new OkObjectResult(json);
        }

        /// <summary>
        /// Get all persons of a customer
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Constants.Roles.Customer)]
        [Route("{customerId}")]
        public IActionResult GetCustomersUnemployedPersons([FromRoute] string customerId)
        {
            // Load all person db entries of the customer and with no company set
            List<PersonViewModel> customerPersons = new List<PersonViewModel>();
            List<Person> allPersons = UnitOfWork.Persons.GetAll().Where(x => x.CustomerId == customerId).ToList();
            List<Person> dbEntries = UnitOfWork.Persons.GetAll().Where(x => x.CustomerId == customerId && x.Company == null).ToList();
            customerPersons = Mapper.Map<List<Person>, List<PersonViewModel>>(dbEntries);
            // Serialize and return the result
            var json = JsonConvert.SerializeObject(customerPersons, _serializerSettings);
            return new OkObjectResult(json);
        }

        [HttpGet]
        [Authorize(Constants.Roles.Customer)]
        [Route("all/{customerId}")]
        public IActionResult GetCustomersPersons ([FromRoute] string customerId)
        {
            List<PersonViewModel> customerPersons = new List<PersonViewModel>();
            List<Person> allPersons = UnitOfWork.Persons.GetAll().Where(x => x.CustomerId == customerId).ToList();
            customerPersons = Mapper.Map<List<PersonViewModel>>(allPersons);

            var json = JsonConvert.SerializeObject(customerPersons, _serializerSettings);
            return Ok(json);
        }

        [HttpPost]
        [Authorize(Policy = Constants.Policies.Customer)]
        [Route("expert")]
        public IActionResult CreateExpert([FromBody] PersonViewModel expertData)
        {
            // Load company
            Company company = UnitOfWork.Companies.Get(expertData.CompanyId);
            if (company == null)
            {
                return BadRequest("Company does not exist");
            }

            Person expert = UnitOfWork.Persons.CreateEmpty();
            expert.GivenName = expertData.GivenName;
            expert.MiddleName = expertData.MiddleName;
            expert.Surname = expertData.Surname;
            expert.Title = expertData.Title;
            expert.AcademicDegree = expertData.AcademicDegree;
            expert.EmployeeType = expertData.EmployeeType;
            expert.CostCenter = expertData.CostCenter;
            expert.PhoneNr = expertData.PhoneNr;
            expert.FaxNr = expertData.FaxNr;
            expert.MobileNr = expertData.MobileNr;
            expert.EmailPrimary = expertData.EmailPrimary;
            expert.EmailOptional = expertData.EmailOptional;
            expert.State = expertData.State;
            expert.Domain = expertData.Domain;
            expert.DepartementName = expertData.DepartementName;
            expert.DepartementShort = expertData.DepartementShort;
            expert.RoomNr = expertData.RoomNr;
            expert.EmployeeNr = expertData.EmployeeNr;
            expert.CustomerId = expertData.CustomerId;
            expert.CompanyId = expertData.CompanyId;

            expert.UpdatedDate = DateTime.Now;
            expert.UpdatedByUserId = LoggedUser.Id;

            company.ExpertId = expert.Id;

            // Finish DB transaction
            try
            {
                UnitOfWork.SaveChanges();
            }
            catch (Exception exc)
            {
                return BadRequest("Expert could not be created. " + exc.Message);
            }

            // Serialize & return result
            var json = JsonConvert.SerializeObject(Mapper.Map<Person, PersonViewModel>(expert), _serializerSettings);
            return new OkObjectResult(json);
        }

        /// <summary>
        /// Delete a person
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Authorize(Policy = Constants.Policies.Customer)]
        [Route("{personId}")]
        public IActionResult DeletePerson([FromRoute] string personId)
        {
            Person personToDelete = UnitOfWork.Persons.Get(personId, "Company.Expert");
            if (personToDelete == null)
            {
                return NotFound("Person not found");
            }
            if (personToDelete.Company != null && personToDelete.Company.Expert != null)
            {
                if (personToDelete.Company.Expert.Id == personToDelete.Id)
                {
                    Company company = UnitOfWork.Companies.Get(personToDelete.Company.Id);
                    company.ExpertId = null;
                }
            }            
            UnitOfWork.Persons.MarkForDelete(personToDelete, GetCurrentUser().Id);

            try
            {
                UnitOfWork.SaveChanges();
            }
            catch (Exception e)
            {
                return BadRequest("Person could not be deleted");
            }

            // Person succsessfully deleted
            return NoContent();
        }

        /// <summary>
        /// Edit a person db entry
        /// </summary>
        /// <param name="personData"></param>
        /// <returns>PersonViewModel</returns>
        [HttpPut]
        [Authorize(Policy = Constants.Policies.Customer)]
        public IActionResult EditPerson([FromBody] PersonViewModel personData)
        {
            Person toEdit = UnitOfWork.Persons.Get(personData.Id);
            if (toEdit == null)
            {
                return BadRequest("The person dies not exist");
            }

            toEdit.AcademicDegree = personData.AcademicDegree;
            toEdit.CompanyId = personData.CompanyId;
            toEdit.CostCenter = personData.CostCenter;
            toEdit.DepartementName = personData.DepartementName;
            toEdit.DepartementShort = personData.DepartementShort;
            toEdit.Domain = personData.Domain;
            toEdit.EmailOptional = personData.EmailOptional;
            toEdit.EmailPrimary = personData.EmailPrimary;
            toEdit.EmployeeNr = personData.EmployeeNr;
            toEdit.EmployeeType = personData.EmployeeType;
            toEdit.FaxNr = personData.FaxNr;
            toEdit.GivenName = personData.GivenName;
            toEdit.MiddleName = personData.MiddleName;
            toEdit.MobileNr = personData.MobileNr;
            toEdit.PhoneNr = personData.PhoneNr;
            toEdit.RoomNr = personData.RoomNr;
            toEdit.State = personData.State;
            toEdit.Surname = personData.Surname;
            toEdit.Title = personData.Title;

            toEdit.UpdatedByUserId = GetCurrentUser().Id;
            toEdit.UpdatedDate = DateTime.Now;

            try
            {
                UnitOfWork.SaveChanges();
            }
            catch (Exception exc)
            {
                return BadRequest("The person could not be edited. " + exc.Message);
            }
            toEdit = UnitOfWork.Persons.GetAll("Company").Where(x => x.Id == toEdit.Id).First();
            // Serialize and return result
            var json = JsonConvert.SerializeObject(Mapper.Map<Person, PersonViewModel>(toEdit), _serializerSettings);
            return new OkObjectResult(json);
        }
    }
}
