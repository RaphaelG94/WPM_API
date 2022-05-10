//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Authorization;
//using WPM_API.Common;
//using WPM_API.Models;
//using Newtonsoft.Json;
//using WPM_API.Data.DataContext.Entities;
//using System.Reflection;
//using System.IO;
//using System.Text;
//using CsvHelper;
//using WPM_API.Code.Mappers.CSV_Mapper;

//namespace WPM_API.Controllers.Base
//{
//    [Route("customers/{customerId}/variables")]
//    public class VariableController : BasisController
//    {
//        /// <summary>
//        /// Retrive all variables.
//        /// </summary>
//        /// <param name="customerId"></param>
//        /// <returns></returns>
//        [HttpGet]
//        [Authorize(Policy = Constants.Roles.Customer)]
//        public IActionResult GetVariables([FromRoute] string customerId)
//        {
//            List<VariableViewModel> result = new List<VariableViewModel>();
//            List<VariableViewModel> dbList = new List<VariableViewModel>();
//            using (var unitOfWork = CreateUnitOfWork())
//            {
//                dbList.AddRange(Mapper.Map<List<VariableViewModel>>(unitOfWork.Customers.Get(customerId, "Variables").Variables));
//            }
//            result = AccumulateVariablesList(dbList);
//            result.OrderBy(x => x.Reference);
//            // Serialize and return the response
//            var json = JsonConvert.SerializeObject(result.OrderByDescending(x => x.Reference).ToList(), _serializerSettings);
//            return new OkObjectResult(json);
//        }

//        /// <summary>
//        /// Add a new variable.
//        /// </summary>
//        /// <param name="customerId"></param>
//        /// <param name="newVariable"></param>
//        /// <returns></returns>
//        [HttpPost]
//        [Authorize(Policy = Constants.Roles.Customer)]
//        public IActionResult AddVariable([FromRoute] string customerId, [FromBody] VariableAddViewModel newVariable)
//        {
//            VariableViewModel result = new VariableViewModel();
//            using (var unitOfWork = CreateUnitOfWork())
//            {
//                var customer = unitOfWork.Customers.Get(customerId, "Variables");
//                // Variable existiert schon
//                if (customer.Variables.Exists(x => x.Name == newVariable.Name))
//                {
//                    return StatusCode(422, "A variable with this name already exists.");
//                }
//                customer.Variables.Add(new Data.DataContext.Entities.Variable() { Name = newVariable.Name, Default = newVariable.Default });
//                unitOfWork.Customers.MarkForUpdate(customer, GetCurrentUser().Id);
//                unitOfWork.SaveChanges();
//                result = Mapper.Map<VariableViewModel>(unitOfWork.Customers.Get(customerId, "Variables").Variables.First(x => x.Name.Equals(newVariable.Name)));
//            }
//            // Serialize and return the response
//            var json = JsonConvert.SerializeObject(result, _serializerSettings);
//            return StatusCode(201, json);
//        }

//        /// <summary>
//        /// Update existing variable (default only).
//        /// </summary>
//        /// <param name="customerId"></param>
//        /// <param name="variableId"></param>
//        /// <param name="updateVariable"></param>
//        /// <returns></returns>
//        [HttpPut]
//        [Authorize(Policy = Constants.Roles.Customer)]
//        [Route("{variableId}")]
//        public IActionResult UpdateVariable([FromRoute] string customerId, [FromRoute] string variableId, [FromBody] VariableEditViewModel updateVariable)
//        {
//            VariableViewModel result = new VariableViewModel();
//            using (var unitOfWork = CreateUnitOfWork())
//            {
//                var customer = unitOfWork.Customers.Get(customerId, "Variables");
//                Variable variable = null;
//                // Check if it is a reference -> create this variable
//                if (variableId.StartsWith("000_"))
//                {
//                    // get VariablenNamen
//                    string name = variableId.Split('_')[1];
//                    // add new Variable
//                    variable = new Variable() { Default = updateVariable.Default, Name = name };
//                    customer.Variables.Add(variable);
//                }
//                // Variable existiert nicht
//                else if (!customer.Variables.Exists(x => x.Id == variableId))
//                {
//                    return new NotFoundResult();
//                }
//                else
//                {
//                    variable = customer.Variables.Find(x => x.Id.Equals(variableId));
//                    variable.Default = updateVariable.Default;
//                }
//                unitOfWork.Customers.MarkForUpdate(customer, GetCurrentUser().Id);
//                unitOfWork.SaveChanges();
//                result = Mapper.Map<VariableViewModel>(unitOfWork.Customers.Get(customerId, "Variables").Variables.First(x => x.Name.Equals(variable.Name)));
//                List<ReferenceViewModel> references = GetReferences();
//                foreach (ReferenceViewModel r in references)
//                {
//                    if (result.Name.Equals(r.Name))
//                    {
//                        result.Reference = r.Reference;
//                     }
//                }
              
//            }
//            // Serialize and return the response
//            var json = JsonConvert.SerializeObject(result, _serializerSettings);
//            return StatusCode(201, json);
//        }

//        private List<VariableViewModel> AccumulateVariablesList(List<VariableViewModel> variables)
//        {
//            // Add references to List
//            List<ReferenceViewModel> references = GetReferences();
//            foreach(ReferenceViewModel r in references)
//            {
//                if(variables.Exists(x => x.Name.Equals(r.Name)))
//                {
//                    VariableViewModel v = variables.Where(x => x.Name.Equals(r.Name)).First();
//                    v.Reference = r.Reference;
//                } else
//                {
//                    variables.Add(new VariableViewModel() { Id = "000_" + r.Name, Name = r.Name, Reference = r.Reference });
//                }
//            }
//            return variables;
//        }

//        private List<ReferenceViewModel> GetReferences()
//        {
//            var assembly = typeof(WPM_API.Program).GetTypeInfo().Assembly;
//            Stream resource = assembly.GetManifestResourceStream("WPM_API.Resources.Referencelist.CSV");
//            var utf8WithoutBom = new UTF8Encoding(false);
//            using (StreamReader reader = new StreamReader(resource, utf8WithoutBom))
//            {
//                var csv = new CsvReader(reader);
//                csv.Configuration.RegisterClassMap<ReferenceMap>();
//                csv.Configuration.HasHeaderRecord = false;
//                csv.Configuration.MissingFieldFound = null;
//                return csv.GetRecords<ReferenceViewModel>().ToList();
//            }
//        }
//    }
//}