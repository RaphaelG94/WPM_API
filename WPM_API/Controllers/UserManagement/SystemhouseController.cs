using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Globalization;
using System.Reflection;
using System.Text;
using WPM_API.Code.Infrastructure;
using WPM_API.Code.Infrastructure.LogOn;
using WPM_API.Code.Mappers.CSV_Mapper;
using WPM_API.Common;
using WPM_API.Data.DataContext.Entities;
using WPM_API.Data.Models;
using WPM_API.Models;
using WPM_API.Options;

namespace WPM_API.Controllers
{
    [Route("systemhouses")]
    public class SystemhouseController : BasisController
    {
        public SystemhouseController(AppSettings appSettings, ConnectionStrings connectionStrings, OrderEmailOptions orderEmailOptions, AgentEmailOptions agentEmailOptions, SendMailCreds sendMailCreds, SiteOptions siteOptions, ILogonManager logonManager) : base(appSettings, connectionStrings, orderEmailOptions, agentEmailOptions, sendMailCreds, siteOptions, logonManager)
        {
        }

        /// <summary>
        /// Retrieve all systemhouses.
        /// </summary>
        /// <returns>[Systemhouse]</returns>
        [HttpGet]
        [Authorize(Policy = Constants.Policies.Admin)]
        public IActionResult GetSystemhouses()
        {
            List<SystemhouseViewModel> result = new List<SystemhouseViewModel>();

            var cust = UnitOfWork.Systemhouses.GetAll().ToList();
            result = Mapper.Map<List<Systemhouse>, List<SystemhouseViewModel>>(cust);

            var json = JsonConvert.SerializeObject(result, serializerSettings);
            return new OkObjectResult(json);
        }

        /// <summary>
        /// Create new systemhouse.
        /// </summary>
        /// <param name="systemhouseEdit">New Systemhouse</param>
        /// <returns>Systemhouse</returns>
        [HttpPost]
        [Authorize(Policy = Constants.Policies.Admin)]
        public IActionResult AddSystemhouse([FromBody] SystemhouseEditViewModel systemhouseEdit)
        {
            Systemhouse systemhouse = new Systemhouse()
            {
                Name = systemhouseEdit.Name,
                Deletable = true
            };

            UnitOfWork.Systemhouses.MarkForInsert(systemhouse);
            try
            {
                UnitOfWork.SaveChanges();
            }
            catch (Exception)
            {
                return BadRequest("Systemhouse could not be created.");
            }

            // Systemhouse was created and is returned.
            var json = JsonConvert.SerializeObject(Mapper.Map<Systemhouse, SystemhouseViewModel>(systemhouse), serializerSettings);
            return new OkObjectResult(json);
        }

        /// <summary>
        /// Change an existing systemhouse.
        /// </summary>
        /// <param name="systemhouseId">Id of the Systemhouse</param>
        /// <param name="systemhouseEdit">Modified Systemhouse</param>
        /// <returns>Systemhouse</returns>
        [HttpPut]
        [Authorize(Policy = Constants.Policies.Admin)]
        [Route("{systemhouseId}")]
        public IActionResult UpdateSystemhouse([FromRoute] string systemhouseId, [FromBody] SystemhouseEditViewModel systemhouseEdit)
        {
            Systemhouse systemhouse;

            systemhouse = UnitOfWork.Systemhouses.Get(systemhouseId);
            if (systemhouse == null)
            {
                return NotFound("Systemhouse not found.");
            }
            else
            {
                systemhouse.Name = systemhouseEdit.Name;
                try
                {
                    UnitOfWork.SaveChanges();
                }
                catch (Exception)
                {
                    return BadRequest("Systemhouse could not be changed.");
                }
            }

            // Systemhouse was changed and is returned.
            var json = JsonConvert.SerializeObject(Mapper.Map<Systemhouse, SystemhouseViewModel>(systemhouse), serializerSettings);
            return new OkObjectResult(json);
        }

        /// <summary>
        /// Delete an existing systemhouse.
        /// </summary>
        /// <param name="systemhouseId">Id of the Systemhouse</param>
        /// <returns></returns>
        [HttpDelete]
        [Authorize(Policy = Constants.Policies.Admin)]
        [Route("{systemhouseId}")]
        public IActionResult DeleteSystemhouse([FromRoute] string systemhouseId)
        {
            Systemhouse systemhouse;
            systemhouse = UnitOfWork.Systemhouses.Get(systemhouseId);
            if (systemhouse == null)
            {
                return NotFound("Systemhouse not found.");
            }
            else
            {
                UnitOfWork.Systemhouses.MarkForDelete(systemhouse, GetCurrentUser().Id);
                try
                {
                    UnitOfWork.SaveChanges();
                }
                catch (Exception)
                {
                    return BadRequest("Systemhouse could not be deleted.");
                }
            }

            // Systemhouse was deleted.
            return NoContent();
        }

        /// <summary>
        /// Add new Customer to the Systemhouse.
        /// </summary>
        /// <param name="systemhouseId">Id of the Systemhouse</param>
        /// <param name="customerData">New Customer</param>
        /// <returns>Customer</returns>
        [HttpPost]
        [Authorize(Policy = Constants.Policies.Systemhouse)]
        [Route("{systemhouseId}/customers")]
        public IActionResult AddCustomer([FromRoute] string systemhouseId, [FromBody] CustomerRefViewModel customerData)
        {
            using (var unitOfWork = CreateUnitOfWork())
            {
                // Create Customer
                Systemhouse systemhouse = null;
                WPM_API.Data.DataContext.Entities.Customer customer = unitOfWork.Customers.CreateEmpty();
                customer.Name = customerData.Name;
                customer.WinPEDownloadLink = customerData.WinPEDownloadLink;
                customer.Defaults = fillDefaults();

                systemhouse = unitOfWork.Systemhouses.Get(systemhouseId);
                if (systemhouse == null)
                {
                    return NotFound("Systemhouse not found.");
                }
                customer.SystemhouseId = systemhouseId;

                customer.CreatedByUserId = GetCurrentUser().Id;
                customer.CreatedDate = DateTime.Now;

                customer.Parameters = new List<Parameter>();
                var FixedParams = GenerateFixedCustomerParams(customer, customer.CreatedByUserId);
                foreach (var param in FixedParams)
                {
                    customer.Parameters.Add(param);
                }
                try
                {
                    unitOfWork.SaveChanges();
                }
                catch (Exception exc)
                {
                    return BadRequest("Customer could not be created. " + exc.Message);
                }

                var json = JsonConvert.SerializeObject(Mapper.Map<WPM_API.Data.DataContext.Entities.Customer, CustomerViewModel>(customer), serializerSettings);
                return new OkObjectResult(json);
            }
        }

        [HttpPost]
        [Authorize(Policy = Constants.Policies.Systemhouse)]
        [Route("getSystemhousesCustomers")]
        public IActionResult GetSystemhousesCustomers([FromBody] List<string> systemhouses)
        {
            using (var unitOfWork = CreateUnitOfWork())
            {
                List<WPM_API.Data.DataContext.Entities.Customer> result = new List<WPM_API.Data.DataContext.Entities.Customer>();
                foreach (string id in systemhouses)
                {
                    result.AddRange(unitOfWork.Customers.GetAll().Where(x => x.SystemhouseId == id).ToList());
                }
                var json = JsonConvert.SerializeObject(Mapper.Map<List<CustomerViewModel>>(result), serializerSettings);
                return Ok(json);
            }
        }

        [HttpGet]
        [Route("getSystemhouseUserData")]
        [Authorize(Policy = Constants.Policies.Systemhouse)]
        public IActionResult GetSystemhouseUserData()
        {
            Systemhouse sysHouse = UnitOfWork.Systemhouses.Get(GetCurrentUser().SystemhouseId);
            SystemhouseViewModel result = Mapper.Map<SystemhouseViewModel>(sysHouse);
            var json = JsonConvert.SerializeObject(result, serializerSettings);
            return new OkObjectResult(json);
        }

        private HashSet<Parameter> GenerateFixedCustomerParams(WPM_API.Data.DataContext.Entities.Customer customer, string createdByUserId)
        {
            var FixedParams = new HashSet<Parameter>();
            // FixedParams.Add(new Parameter{ Key = "$AzureBlobRoot", Value = customer.CsdpRoot, IsEditable = true});
            FixedParams.Add(new Parameter { Key = "$LtSASread", Value = null, IsEditable = false, CreatedByUserId = createdByUserId });
            FixedParams.Add(new Parameter { Key = "$LtSASwrite", Value = null, IsEditable = false, CreatedByUserId = createdByUserId });
            FixedParams.Add(new Parameter { Key = "$CustomerName", Value = customer.Name, IsEditable = false, CreatedByUserId = createdByUserId });
            //  FixedParams.Add(new Parameter{ Key = "$CSDPcontainer", Value = customer.CsdpContainer, IsEditable = true });

            return FixedParams;
        }


        private CreateCustomerResultViewModel CreateCustomerResult(WPM_API.Data.DataContext.Entities.Customer customer, string companyId)
        {
            CreateCustomerResultViewModel result = new CreateCustomerResultViewModel();
            // Customer data
            CustomerViewModel customerResult = Mapper.Map<WPM_API.Data.DataContext.Entities.Customer, CustomerViewModel>(customer);

            // Company data
            Company mainComp = UnitOfWork.Companies.GetAll(CompanyIncludes.Expert, CompanyIncludes.Headquarter).Where(x => x.Id == companyId).Single();
            if (mainComp == null)
            {
                return null;
            }
            CompanyViewModel companyResult = Mapper.Map<Company, CompanyViewModel>(mainComp);

            result.Company = companyResult;
            result.Customer = customerResult;
            return result;
        }

        private List<Default> fillDefaults()
        {
            var assembly = typeof(Program).GetTypeInfo().Assembly;
            var resources = assembly.GetManifestResourceNames();
            Stream resource = assembly.GetManifestResourceStream("WPM_API.Resources.Defaults.CSV");
            var utf8WithoutBom = new UTF8Encoding(false);
            using (StreamReader reader = new StreamReader(resource, utf8WithoutBom))
            {
                CsvConfiguration csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Delimiter = ";",
                    IgnoreBlankLines = true,
                    HasHeaderRecord = false,
                    MissingFieldFound = null,

                };
                var csv = new CsvReader(reader, csvConfig);
                csv.Context.RegisterClassMap<DefaultMap>();
                return csv.GetRecords<Default>().ToList();
            }
        }
    }
}