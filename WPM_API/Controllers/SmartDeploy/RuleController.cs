using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WPM_API.Code.Infrastructure;
using WPM_API.Code.Infrastructure.LogOn;
using WPM_API.Common;
using WPM_API.Data.DataContext.Entities;
using WPM_API.Options;
using WPM_API.TransferModels.SmartDeploy;
using File = WPM_API.Data.DataContext.Entities.File;

namespace WPM_API.Controllers
{
    [Authorize(Policy = Constants.Roles.Systemhouse)]
    [Route("rules")]
    public class RuleController : BasisController
    {
        public RuleController(AppSettings appSettings, ConnectionStrings connectionStrings, OrderEmailOptions orderEmailOptions, AgentEmailOptions agentEmailOptions, SendMailCreds sendMailCreds, SiteOptions siteOptions, ILogonManager logonManager) : base(appSettings, connectionStrings, orderEmailOptions, agentEmailOptions, sendMailCreds, siteOptions, logonManager)
        {
        }

        [HttpGet]
        public IActionResult GetRules()
        {
            var ruleList = UnitOfWork.Rules.GetAll("Type", "Data").ToList();
            var json = JsonConvert.SerializeObject(Mapper.Map<List<RulesViewModel>>(ruleList), serializerSettings);
            return new OkObjectResult(json);
        }

        [HttpPost]
        [Authorize(Policy = Constants.Policies.Systemhouse)]
        public IActionResult AddRule([FromBody] RuleAddViewModel addRule)
        {
            Rule newRule = Mapper.Map<Rule>(addRule);
            UnitOfWork.Rules.MarkForInsert(newRule, GetCurrentUser().Id);
            if (newRule.Type.Name == "script")
            {
                newRule.Data.Guid = newRule.Data.Id;
            }
            UnitOfWork.SaveChanges();

            var json = JsonConvert.SerializeObject(Mapper.Map<RulesViewModel>(newRule), serializerSettings);
            return Ok(json);
        }

        [HttpPut]
        [Authorize(Policy = Constants.Policies.Systemhouse)]
        public IActionResult UpdateRule([FromBody] RuleAddViewModel updateRule)
        {
            Rule toUpdate = UnitOfWork.Rules.GetOrNull(updateRule.Id, "Type", "Data");

            if (toUpdate == null)
            {
                return BadRequest("ERROR: The rule does not exist");
            }

            toUpdate.Name = updateRule.Name;
            toUpdate.Type.Name = updateRule.Type;
            toUpdate.Successon = updateRule.Successon;
            toUpdate.CheckVersionNr = updateRule.CheckVersionNr;
            toUpdate.VersionNr = updateRule.VersionNr;
            if (updateRule.Type == "fileExists")
            {
                toUpdate.Path = updateRule.Path;
                //toUpdate.Architecture = updateRule.Architecture;
            }
            else
            {
                toUpdate.Data = Mapper.Map<File>(updateRule.Data);
                toUpdate.Data.Guid = toUpdate.Data.Id;
            }
            List<Software> softwares = UnitOfWork.Software.GetAll("TaskInstall").Where(x => x.RuleDetection.Id == toUpdate.Id).ToList();
            foreach (Software sw in softwares)
            {
                sw.TaskInstall.VersionNr = updateRule.VersionNr;
            }
            UnitOfWork.SaveChanges();

            var json = JsonConvert.SerializeObject(Mapper.Map<RuleViewModel>(toUpdate));
            return Ok(json);
        }

        [HttpDelete]
        [Authorize(Policy = Constants.Policies.Systemhouse)]
        [Route("{ruleId}")]
        public IActionResult DeleteRule([FromRoute] string ruleId)
        {
            using (var unitOfWork = CreateUnitOfWork())
            {
                Rule toDelete = unitOfWork.Rules.GetOrNull(ruleId);
                if (toDelete == null)
                {
                    return BadRequest("ERROR: The rule does not exist");
                }

                // Search Software and reset rule
                List<Software> softwares = unitOfWork.Software.GetAll("RuleDetection", "RuleApplicability").Where(x => x.RuleApplicability.Id == ruleId || x.RuleDetection.Id == ruleId).ToList();
                foreach (Software software in softwares)
                {
                    if (software.RuleApplicability != null && software.RuleApplicability.Id == ruleId)
                    {
                        software.RuleApplicability = null;
                    }
                    else if (software.RuleDetection != null && software.RuleDetection.Id == ruleId)
                    {
                        software.RuleDetection = null;
                    }
                }
                unitOfWork.SaveChanges();

                unitOfWork.Rules.MarkForDelete(toDelete, GetCurrentUser().Id);
                unitOfWork.SaveChanges();
            }
            return Ok();
        }

        [HttpGet]
        [Authorize(Policy = Constants.Policies.Customer)]
        [Route("{softwareId}")]
        public IActionResult GetSoftwareRules([FromRoute] string softwareId)
        {
            Software software = UnitOfWork.Software.Get(softwareId, "RuleDetection", "RuleApplicability");

            SoftwareRulesViewModel result = new SoftwareRulesViewModel();
            result.RuleApplicability = Mapper.Map<RuleViewModel>(software.RuleApplicability);
            result.RuleDetection = Mapper.Map<RuleViewModel>(software.RuleDetection);

            var json = JsonConvert.SerializeObject(result, serializerSettings);

            return Ok(json);
        }

        [HttpPost]
        [Route("upload")]
        public async Task<IActionResult> UploadFileAsync([FromForm] IFormFile file)
        {
            FileRepository.FileRepository software = new FileRepository.FileRepository(connectionStrings.FileRepository, appSettings.FileRepositoryFolder);
            string id = await software.UploadFile(file.OpenReadStream());
            var json = JsonConvert.SerializeObject(new { Id = id }, serializerSettings);
            return new OkObjectResult(json);
        }

        [HttpGet]
        [Route("types")]
        public IActionResult GetTypes()
        {
            List<string> types = new List<string>();
            types.Add("file_exists");
            var json = JsonConvert.SerializeObject(types, serializerSettings);
            return new OkObjectResult(json);
        }
    }
}