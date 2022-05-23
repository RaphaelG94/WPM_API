using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WPM_API.Code.Infrastructure;
using WPM_API.Code.Infrastructure.LogOn;
using WPM_API.Common;
using WPM_API.Data.DataContext.Entities;
using WPM_API.Models.Release_Mgmt;
using WPM_API.Options;

namespace WPM_API.Controllers.Releas_Mgmt
{
    [Route("biosModels")]
    public class BIOSModelController : BasisController
    {
        public BIOSModelController(AppSettings appSettings, ConnectionStrings connectionStrings, OrderEmailOptions orderEmailOptions, AgentEmailOptions agentEmailOptions, SendMailCreds sendMailCreds, SiteOptions siteOptions, ILogonManager logonManager) : base(appSettings, connectionStrings, orderEmailOptions, agentEmailOptions, sendMailCreds, siteOptions, logonManager)
        {
        }

        [HttpGet]
        [Authorize(Policy = Constants.Policies.Admin)]
        public IActionResult GetBIOSModels()
        {
            List<BIOSModel> biosModels = UnitOfWork.BiosModels.GetAll().ToList();
            BIOSModelsViewModel result = new BIOSModelsViewModel();
            foreach (BIOSModel biosModel in biosModels)
            {
                BIOSModelViewModel biosData = Mapper.Map<BIOSModelViewModel>(biosModel);
                result.BiosModels.Add(biosData);
            }
            var json = JsonConvert.SerializeObject(result, serializerSettings);
            return new OkObjectResult(json);
        }

        [HttpPost]
        [Authorize(Policy = Constants.Policies.Admin)]
        public IActionResult AddBIOSModel([FromBody] BIOSModelsViewModel biosModel)
        {
            // TODO: Save data in db
            return null;
        }

        [HttpPut]
        [Authorize(Policy = Constants.Policies.Admin)]
        public IActionResult UpdateBIOSModel([FromBody] BIOSModelViewModel updateData)
        {
            // TODO: update data
            return null;
        }

        [HttpDelete]
        [Authorize(Policy = Constants.Policies.Admin)]
        [Route("{biosModelId}")]
        public IActionResult DeleteBIOSModel([FromRoute] string biosModelId)
        {
            BIOSModel biosModel = UnitOfWork.BiosModels.Get(biosModelId);
            if (biosModel == null)
            {
                return BadRequest("ERROR: The bios model does not exist");
            }
            UnitOfWork.BiosModels.MarkForDelete(biosModel, GetCurrentUser().Id);
            UnitOfWork.SaveChanges();
            // TODO: remove file from Azure
            return new OkResult();

        }
    }
}
