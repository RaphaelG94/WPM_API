using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WPM_API.Code.Infrastructure;
using WPM_API.Code.Infrastructure.LogOn;
using WPM_API.Common;
using WPM_API.Models;
using WPM_API.Options;
using DATA = WPM_API.Data.DataContext.Entities;

namespace WPM_API.Controllers.Certification
{
    [Route("certifications")]
    public class CertificationController : BasisController
    {
        public CertificationController(AppSettings appSettings, ConnectionStrings connectionStrings, OrderEmailOptions orderEmailOptions, AgentEmailOptions agentEmailOptions, SendMailCreds sendMailCreds, SiteOptions siteOptions, ILogonManager logonManager) : base(appSettings, connectionStrings, orderEmailOptions, agentEmailOptions, sendMailCreds, siteOptions, logonManager)
        {
        }

        [HttpPost]
        [Authorize(Policy = Constants.Policies.Customer)]
        public IActionResult CreateCertification([FromBody] CertificationViewModels data)
        {
            try
            {
                DATA.Certification newCertification = UnitOfWork.Certifications.CreateEmpty();
                newCertification.Name = data.Name;
                newCertification.Description = data.Description;
                UnitOfWork.SaveChanges();
                var json = JsonConvert.SerializeObject(Mapper.Map<DATA.Certification, CertificationViewModels>(newCertification), serializerSettings);
                return new OkObjectResult(json);
            }
            catch (Exception e)
            {
                return BadRequest("Error: " + e.Message);
            }
        }

        [HttpGet]
        [Authorize(Policy = Constants.Policies.Customer)]
        public IActionResult GetCertifications()
        {
            List<DATA.Certification> certifications = UnitOfWork.Certifications.GetAll().ToList();
            var json = JsonConvert.SerializeObject(Mapper.Map<List<CertificationViewModels>>(certifications), serializerSettings);
            return new OkObjectResult(json);
        }

        [HttpPut]
        [Authorize(Policy = Constants.Policies.Admin)]
        public IActionResult UpdateCertification([FromBody] CertificationViewModels editData)
        {
            try
            {
                DATA.Certification cert = UnitOfWork.Certifications.Get(editData.Id);
                cert.Description = editData.Description;
                cert.Name = editData.Name;
                UnitOfWork.SaveChanges();
                var json = JsonConvert.SerializeObject(Mapper.Map<CertificationViewModels>(cert), serializerSettings);
                return new OkObjectResult(json);
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult("ERROR: " + e.Message);
            }
        }
    }
}
