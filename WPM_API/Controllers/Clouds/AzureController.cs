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
    [Route("azureClouds")]
    public class AzureController : BasisController
    {
        private static readonly string key = "N43Kn90tbubxJZeLCIZIIjxagKyq4ik0";

        public AzureController(AppSettings appSettings, ConnectionStrings connectionStrings, OrderEmailOptions orderEmailOptions, AgentEmailOptions agentEmailOptions, SendMailCreds sendMailCreds, SiteOptions siteOptions, ILogonManager logonManager) : base(appSettings, connectionStrings, orderEmailOptions, agentEmailOptions, sendMailCreds, siteOptions, logonManager)
        {
        }

        [HttpPut]
        [Authorize(Policy = Constants.Policies.Customer)]
        [Route("{customerId}")]
        public IActionResult UpdateAzureCredentials([FromRoute] string customerId, [FromBody] AzureCredentialViewModel updateData)
        {
            using (var unitOfWork = CreateUnitOfWork())
            {
                Customer customer = UnitOfWork.Customers.GetOrNull(customerId, CustomerIncludes.Parameters, "CloudEntryPoints");
                if (customer == null)
                {
                    return BadRequest("ERROR: The customer does not exist!");
                }

                CloudEntryPoint toUpdate = unitOfWork.CloudEntryPoints.GetOrNull(updateData.Id);
                if (toUpdate == null)
                {
                    return BadRequest("ERROR: The CEP does not exist!");
                }

                // Encrypt credentials
                toUpdate.ClientId = EncryptString(updateData.ClientId);
                if (updateData.ClientSecret != "" && updateData.ClientSecret != null)
                {
                    toUpdate.ClientSecret = EncryptString(updateData.ClientSecret);
                }
                toUpdate.TenantId = EncryptString(updateData.TenantId);
                toUpdate.IsStandard = updateData.IsStandard;
                toUpdate.Name = updateData.Name;

                // Update parameters for Azure CEP
                GenerateOrUpdateAzureParameters(customer, updateData);

                unitOfWork.CloudEntryPoints.MarkForUpdate(toUpdate, GetCurrentUser().Id);
                unitOfWork.SaveChanges();

                // Decrypt values for UI
                toUpdate.ClientId = DecryptString(toUpdate.ClientId);
                toUpdate.ClientSecret = DecryptString(toUpdate.ClientSecret);
                toUpdate.TenantId = DecryptString(toUpdate.TenantId);

                var json = JsonConvert.SerializeObject(Mapper.Map<AzureCredsNoSecret>(toUpdate), serializerSettings);
                return new OkObjectResult(json);
            }
        }

        [HttpPost]
        [Authorize(Policy = Constants.Policies.Customer)]
        [Route("{customerId}")]
        public IActionResult AddAzureCloudData([FromRoute] string customerId, [FromBody] AzureCredentialViewModel azureEntryData)
        {

            // Fetch customer by ID
            Customer customer = UnitOfWork.Customers.Get(customerId, CustomerIncludes.Parameters, "CloudEntryPoints");
            if (customer != null)
            {
                if (customer.CloudEntryPoints == null)
                {
                    customer.CloudEntryPoints = new List<CloudEntryPoint>();
                }

                CloudEntryPoint newCEP = UnitOfWork.CloudEntryPoints.CreateEmpty();
                newCEP.Name = azureEntryData.Name;
                // Encrypt Azure credentials
                newCEP.ClientId = EncryptString(azureEntryData.ClientId);
                newCEP.ClientSecret = EncryptString(azureEntryData.ClientSecret);
                newCEP.TenantId = EncryptString(azureEntryData.TenantId);
                newCEP.IsStandard = azureEntryData.IsStandard;
                newCEP.Type = azureEntryData.Type;

                if (azureEntryData.IsStandard)
                {
                    foreach (CloudEntryPoint cep in customer.CloudEntryPoints)
                    {
                        cep.IsStandard = false;
                    }
                }

                customer.CloudEntryPoints.Add(newCEP);
                GenerateOrUpdateAzureParameters(customer, azureEntryData);
                UnitOfWork.SaveChanges();

                // Decrypt values for UI
                List<AzureCredsNoSecret> result = Mapper.Map<List<AzureCredsNoSecret>>(customer.CloudEntryPoints);
                foreach (AzureCredsNoSecret avm in result)
                {
                    avm.TenantId = DecryptString(avm.TenantId);
                    // avm.ClientSecret = DecryptString(avm.ClientSecret);
                    avm.ClientId = DecryptString(avm.ClientId);
                }

                var json = JsonConvert.SerializeObject(result, serializerSettings);

                return new OkObjectResult(json);
            }
            else
            {
                return BadRequest("The customer does not exist");
            }
        }

        private void GenerateOrUpdateAzureParameters(Customer customer, AzureCredentialViewModel azureEntryData)
        {
            // Check for tenantId Existance should be sufficient as azure parameters are only ever added/updated as triples.
            if (customer.Parameters.Exists(x => x.Key == "$tenantId"))
            {
                customer.Parameters[customer.Parameters.FindIndex(x => x.Key == "$tenantId")].Value = azureEntryData.TenantId;
                customer.Parameters[customer.Parameters.FindIndex(x => x.Key == "$clientId")].Value = azureEntryData.ClientId;
                customer.Parameters[customer.Parameters.FindIndex(x => x.Key == "$clientSecret")].Value = azureEntryData.ClientSecret;
            }
            else
            {
                customer.Parameters.Add(new Parameter { Key = "$tenantId", Value = azureEntryData.TenantId });
                customer.Parameters.Add(new Parameter { Key = "$clientId", Value = azureEntryData.ClientId });
                customer.Parameters.Add(new Parameter { Key = "$clientSecret", Value = azureEntryData.ClientSecret });
            }
        }

        [HttpGet]
        [Authorize(Policy = Constants.Policies.Customer)]
        [Route("{customerId}")]
        public IActionResult GetCEPs([FromRoute] string customerId)
        {
            Customer customer = UnitOfWork.Customers.GetOrNull(customerId, "CloudEntryPoints");
            if (customer == null)
            {
                return BadRequest("ERROR: The customer does not exist!");
            }

            // Decrypt values
            foreach (CloudEntryPoint cep in customer.CloudEntryPoints)
            {
                cep.ClientId = DecryptString(cep.ClientId);
                cep.ClientSecret = DecryptString(cep.ClientSecret);
                cep.TenantId = DecryptString(cep.TenantId);
            }

            List<AzureCredsNoSecret> result = Mapper.Map<List<AzureCredsNoSecret>>(customer.CloudEntryPoints);

            var json = JsonConvert.SerializeObject(result, serializerSettings);

            return new OkObjectResult(json);
        }

        [HttpDelete]
        [Route("{customerId}/{cepId}")]
        [Authorize(Policy = Constants.Policies.Customer)]
        public IActionResult DeleteCEP([FromRoute] string customerId, [FromRoute] string cepId)
        {
            using (var unitOfWork = CreateUnitOfWork())
            {
                Customer customer = unitOfWork.Customers.GetOrNull(customerId, "CloudEntryPoints");
                if (customer == null)
                {
                    return BadRequest("ERROR: The customer does not exist!");
                }

                CloudEntryPoint toDelete = unitOfWork.CloudEntryPoints.GetOrNull(cepId);
                if (toDelete == null)
                {
                    return BadRequest("ERROR: The CEP does not exist!");
                }

                customer.CloudEntryPoints.Remove(toDelete);
                unitOfWork.CloudEntryPoints.MarkForDelete(toDelete, GetCurrentUser().Id);
                unitOfWork.SaveChanges();

                var json = JsonConvert.SerializeObject(Mapper.Map<AzureCredsNoSecret>(toDelete), serializerSettings);
                return new OkObjectResult(json);
            }
        }
    }
}