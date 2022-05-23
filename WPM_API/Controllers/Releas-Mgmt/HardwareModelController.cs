using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WPM_API.Code.Infrastructure;
using WPM_API.Code.Infrastructure.LogOn;
using WPM_API.Common;
using WPM_API.Data.DataContext;
using WPM_API.Models.Release_Mgmt;
using WPM_API.Options;

namespace WPM_API.Controllers.Releas_Mgmt
{
    [Route("hardwareModels")]
    public class HardwareModelController : BasisController
    {
        public HardwareModelController(AppSettings appSettings, ConnectionStrings connectionStrings, OrderEmailOptions orderEmailOptions, AgentEmailOptions agentEmailOptions, SendMailCreds sendMailCreds, SiteOptions siteOptions, ILogonManager logonManager) : base(appSettings, connectionStrings, orderEmailOptions, agentEmailOptions, sendMailCreds, siteOptions, logonManager)
        {
        }

        [HttpGet]
        [Authorize(Policy = Constants.Policies.Admin)]
        public IActionResult GetHardwareModels()
        {
            List<HardwareModel> hardwareModels = UnitOfWork.HardwareModels.GetAll().ToList();
            HardwareModelsViewModel result = new HardwareModelsViewModel();
            result.HardwareModels = new List<HardwareModelViewModel>();
            foreach (HardwareModel hardwareModel in hardwareModels)
            {
                HardwareModelViewModel hardwareModelData = Mapper.Map<HardwareModelViewModel>(hardwareModel);
                result.HardwareModels.Add(hardwareModelData);
            }
            var json = JsonConvert.SerializeObject(result, serializerSettings);
            return new OkObjectResult(json);
        }

        [HttpPost]
        [Authorize(Policy = Constants.Policies.Admin)]
        public IActionResult AddHardwareModel([FromBody] HardwareModelViewModel hardwareModel)
        {
            HardwareModel newModel = new HardwareModel();
            newModel = Mapper.Map<HardwareModel>(hardwareModel);
            UnitOfWork.HardwareModels.MarkForInsert(newModel, GetCurrentUser().Id);
            UnitOfWork.SaveChanges();
            var json = JsonConvert.SerializeObject(Mapper.Map<HardwareModelViewModel>(newModel), serializerSettings);
            return new OkObjectResult(json);
        }

        [HttpPut]
        [Authorize(Policy = Constants.Policies.Admin)]
        public IActionResult UpdateHardwareModel([FromBody] HardwareModelViewModel updateData)
        {
            HardwareModel toUpdate = UnitOfWork.HardwareModels.Get(updateData.Id);
            if (toUpdate == null)
            {
                return BadRequest("ERROR: The hardware model does not exist");
            }
            toUpdate.ModelFamily = updateData.ModelFamily;
            toUpdate.ModelType = updateData.ModelType;
            toUpdate.ProductionEnd = updateData.ProductionEnd;
            toUpdate.ProductionStart = updateData.ProductionStart;
            toUpdate.Vendor = updateData.Vendor;
            UnitOfWork.HardwareModels.MarkForUpdate(toUpdate, GetCurrentUser().Id);
            UnitOfWork.SaveChanges();
            var json = JsonConvert.SerializeObject(Mapper.Map<HardwareModelViewModel>(toUpdate), serializerSettings);
            return new OkObjectResult(json);
        }

        [HttpDelete]
        [Authorize(Policy = Constants.Policies.Admin)]
        [Route("{hardwareModelId}")]
        public IActionResult DeleteHardwareModel([FromRoute] string hardwareModelId)
        {
            HardwareModel toDelete = UnitOfWork.HardwareModels.Get(hardwareModelId);
            if (toDelete == null)
            {
                return BadRequest("ERROR: The hardware model does not exist");
            }
            UnitOfWork.HardwareModels.MarkForDelete(toDelete, GetCurrentUser().Id);
            UnitOfWork.SaveChanges();
            return new OkResult();
        }
    }
}
