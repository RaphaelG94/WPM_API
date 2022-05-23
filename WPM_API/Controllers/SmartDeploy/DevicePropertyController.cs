using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WPM_API.Code.Infrastructure;
using WPM_API.Code.Infrastructure.LogOn;
using WPM_API.Common;
using WPM_API.Models;
using WPM_API.Options;
using DATA = WPM_API.Data.DataContext.Entities;

namespace WPM_API.Controllers
{
    [Authorize(Policy = Constants.Roles.Systemhouse)]
    [Route("device-properties")]
    public class DevicePropertyController : BasisController
    {
        public DevicePropertyController(AppSettings appSettings, ConnectionStrings connectionStrings, OrderEmailOptions orderEmailOptions, AgentEmailOptions agentEmailOptions, SendMailCreds sendMailCreds, SiteOptions siteOptions, ILogonManager logonManager) : base(appSettings, connectionStrings, orderEmailOptions, agentEmailOptions, sendMailCreds, siteOptions, logonManager)
        {
        }

        [HttpGet]
        public IActionResult GetProperties()
        {
            // Get Properties from Database
            var propList = Mapper.Map<List<DevicePropertyViewModel>>(UnitOfWork.ClientProperties.GetAll("Category").ToList());
            var json = JsonConvert.SerializeObject(propList, serializerSettings);
            return new OkObjectResult(json);
        }

        [HttpPost]
        public IActionResult AddProperty([FromBody] DevicePropertyAddViewModel prop)
        {
            List<DATA.ClientParameter> parameters = UnitOfWork.ClientParameters.GetAll().Where(x => x.ParameterName == prop.ParameterName).ToList();
            if (parameters.Count != 0)
            {
                return BadRequest("The parameter name is already used.");
            }

            var result = new DevicePropertyViewModel();
            var property = CreateNewProperty(prop);
            using (var unitOfWork = CreateUnitOfWork())
            {

                // Create Parameter for every Client
                List<DATA.Client> clients = UnitOfWork.Clients.GetAll().ToList();
                {
                    foreach (DATA.Client client in clients)
                    {
                        if (prop.ParameterName.Length > 0)
                        {
                            DATA.ClientParameter newParam = UnitOfWork.ClientParameters.CreateEmpty();
                            newParam.PropertyName = prop.PropertyName;
                            newParam.ParameterName = prop.ParameterName;
                            newParam.Value = "";
                            newParam.ClientId = client.Id;
                            newParam.IsEditable = false;
                            unitOfWork.ClientParameters.MarkForInsert(newParam);
                        }

                        if (client.Properties == null)
                        {
                            client.Properties = new List<DATA.ClientClientProperty>();
                        }
                        DATA.ClientClientProperty clientClientProperty = new DATA.ClientClientProperty();
                        clientClientProperty.ClientId = client.Id;
                        clientClientProperty.Client = client;
                        clientClientProperty.ClientProperty = property;
                        //                            clientClientProperty.ClientPropertyId = property.Id;
                        client.Properties.Add(clientClientProperty);
                        unitOfWork.Clients.MarkForUpdate(client);
                    }
                }
                unitOfWork.SaveChanges();
            }

            result = Mapper.Map<DevicePropertyViewModel>(property);
            var json = JsonConvert.SerializeObject(result, serializerSettings);
            return new OkObjectResult(json);
        }

        private DATA.ClientProperty CreateNewProperty(DevicePropertyAddViewModel prop)
        {
            using (var unitOfWork = CreateUnitOfWork())
            {
                DATA.ClientProperty property = unitOfWork.ClientProperties.CreateEmpty();
                property.Command = prop.Command;
                property.PropertyName = prop.PropertyName;
                property.ParameterName = prop.ParameterName;
                DATA.Category c = unitOfWork.ClientProperties.GetAll("Category").Select(x => x.Category).FirstOrDefault(x => x.Name.Equals(prop.Category.Name));
                if (c == null)
                {
                    // new category
                    property.Category = new DATA.Category() { Name = prop.Category.Name, Type = DATA.CategoryType.DeviceProperty };
                }
                else
                {
                    property.Category = c;
                    property.Category.Type = DATA.CategoryType.DeviceProperty;
                }
                unitOfWork.ClientProperties.MarkForInsert(property);
                unitOfWork.SaveChanges();
                return property;
            }
        }

        [HttpPut]
        [Route("{devicePropertyId}")]
        public IActionResult EditProperty([FromRoute] string devicePropertyId, [FromBody] DevicePropertyEditViewModel prop)
        {
            var result = new DevicePropertyViewModel();
            // Save Properties in DB
            var property = UnitOfWork.ClientProperties.GetAll("Category").First(x => x.Id.Equals(devicePropertyId));
            DATA.Category c = UnitOfWork.ClientProperties.GetAll("Category").Select(x => x.Category).FirstOrDefault(x => x.Name.Equals(prop.Category.Name));
            if (c == null)
            {
                // new category
                property.Category = new DATA.Category() { Name = prop.Category.Name, Type = DATA.CategoryType.DeviceProperty };
            }
            else
            {
                property.Category = c;
                property.Category.Type = DATA.CategoryType.DeviceProperty;
            }
            property.Command = prop.Command;
            property.PropertyName = prop.PropertyName;
            UnitOfWork.SaveChanges();
            result = Mapper.Map<DevicePropertyViewModel>(property);
            var json = JsonConvert.SerializeObject(result, serializerSettings);
            return new OkObjectResult(json);
        }

        [HttpGet]
        [Route("categories")]
        public IActionResult GetCategories()
        {
            var list = UnitOfWork.ClientProperties.GetAll("Category").Where(y => y.Category.Type.Equals(DATA.CategoryType.DeviceProperty)).Select(x => x.Category).Distinct().ToList();
            var result = Mapper.Map<List<CategoryViewModel>>(list);
            var json = JsonConvert.SerializeObject(result, serializerSettings);
            return new OkObjectResult(json);
        }
    }
}