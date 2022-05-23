using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WPM_API.Code.Infrastructure;
using WPM_API.Code.Infrastructure.LogOn;
using WPM_API.Common;
using WPM_API.Models;
using WPM_API.Options;
using DATA = WPM_API.Data.DataContext.Entities;

namespace WPM_API.Controllers.Workflows
{
    [Route("workflows")]
    public class WorkflowsController : BasisController
    {
        public WorkflowsController(AppSettings appSettings, ConnectionStrings connectionStrings, OrderEmailOptions orderEmailOptions, AgentEmailOptions agentEmailOptions, SendMailCreds sendMailCreds, SiteOptions siteOptions, ILogonManager logonManager) : base(appSettings, connectionStrings, orderEmailOptions, agentEmailOptions, sendMailCreds, siteOptions, logonManager)
        {
        }

        /// <summary>
        /// Create a new workflow entity and save it in the database
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = Constants.Policies.Customer)]
        public IActionResult CreateWorkflow([FromBody] WorkflowViewModels data)
        {
            try
            {
                DATA.Workflow newWorkflow = UnitOfWork.Workflows.CreateEmpty();
                newWorkflow.Description = data.Description;
                newWorkflow.Name = data.Name;
                UnitOfWork.SaveChanges();
                var json = JsonConvert.SerializeObject(Mapper.Map<WorkflowViewModels>(newWorkflow), serializerSettings);
                return new OkObjectResult(json);
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult("Error: " + e.Message);
            }
        }

        /// <summary>
        /// Get all workflows from database.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Policy = Constants.Policies.Customer)]
        public IActionResult GetWorkflows()
        {
            List<DATA.Workflow> workflows = UnitOfWork.Workflows.GetAll().ToList();
            var json = JsonConvert.SerializeObject(Mapper.Map<List<WorkflowViewModels>>(workflows), serializerSettings);
            return new OkObjectResult(json);
        }

        /// <summary>
        /// Load and update an existing workflow entity.
        /// </summary>
        /*
        public IActionResult UpdateWorkflow([FromBody] WorkflowViewModels editData)
        {
            try
            {
                DATA.Workflow workflow = UnitOfWork.Workflows.Get(editData.Id);
                workflow.Description = editData.Description;
                workflow.Name = editData.Name;
                UnitOfWork.SaveChanges();

                // Return result
                var json = JsonConvert.SerializeObject(Mapper.Map<WorkflowViewModels>(workflow), serializerSettings);
                return new OkObjectResult(json);
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult("ERROR: " + e.Message);
            }
        }
        */
    }
}
