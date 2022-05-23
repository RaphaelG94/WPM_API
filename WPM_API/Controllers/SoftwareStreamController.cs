using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WPM_API.Code.Infrastructure;
using WPM_API.Code.Infrastructure.LogOn;
using WPM_API.Common;
using WPM_API.Data.DataContext.Entities;
using WPM_API.Models;
using WPM_API.Options;
using WPM_API.TransferModels.SmartDeploy;
using File = WPM_API.Data.DataContext.Entities.File;

namespace WPM_API.Controllers
{
    [Route("/softwareStreams")]
    [Authorize(Policy = Constants.Policies.Systemhouse)]
    public class SoftwareStreamController : BasisController
    {
        public SoftwareStreamController(AppSettings appSettings, ConnectionStrings connectionStrings, OrderEmailOptions orderEmailOptions, AgentEmailOptions agentEmailOptions, SendMailCreds sendMailCreds, SiteOptions siteOptions, ILogonManager logonManager) : base(appSettings, connectionStrings, orderEmailOptions, agentEmailOptions, sendMailCreds, siteOptions, logonManager)
        {
        }

        [HttpPost]
        public IActionResult AddSoftwareStream([FromBody] SoftwareStreamViewModel data)
        {
            try
            {
                SoftwareStream newStream = Mapper.Map<SoftwareStream>(data);
                if (newStream.Icon != null && (newStream.Icon.Guid == "" || newStream.Icon.Id == ""))
                {
                    newStream.Icon = null;
                }
                UnitOfWork.SoftwareStreams.MarkForInsert(newStream, GetCurrentUser().Id);
                UnitOfWork.SaveChanges();

                var json = JsonConvert.SerializeObject(Mapper.Map<SoftwareStreamViewModel>(newStream), serializerSettings);
                return Ok(json);
            }
            catch (Exception e)
            {
                return BadRequest("ERROR: " + e.Message);
            }
        }

        [HttpGet]
        public IActionResult GetSoftwareStreams()
        {
            List<SoftwareStream> streams = UnitOfWork.SoftwareStreams.GetAll("StreamMembers", "Icon", "StreamMembers.RuleDetection").Where(x => x.CreatedByUserId == GetCurrentUser().Id).ToList();

            List<SoftwareStreamViewModel> result = Mapper.Map<List<SoftwareStreamViewModel>>(streams);

            var json = JsonConvert.SerializeObject(result, serializerSettings);

            return Ok(json);
        }

        [HttpGet]
        [Route("{streamName}")]
        public IActionResult CustomerSoftwareStreamNameExists([FromRoute] string streamName)
        {
            SoftwareStream stream = UnitOfWork.SoftwareStreams.GetAll().Where(x => x.Name == streamName).FirstOrDefault();
            if (stream == null)
            {
                return Ok();
            }
            else
            {
                return BadRequest("ERROR: The stream does exist already");
            }
        }

        [HttpGet]
        [Route("edit/{streamName}/{streamId}")]
        public IActionResult CustomerSoftwareStreamNameExistsEdit([FromRoute] string streamName, [FromRoute] string streamId)
        {
            SoftwareStream stream = UnitOfWork.SoftwareStreams.GetOrNull(streamId);
            if (stream == null)
            {
                return BadRequest("ERROR: The software stream does not exist");
            }
            if (stream.Name == streamName)
            {
                return Ok();
            }
            else
            {
                SoftwareStream alreadyExists = UnitOfWork.SoftwareStreams.GetAll().Where(x => x.Name == streamName).FirstOrDefault();
                if (alreadyExists == null)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest("ERROR: The stream does exist already");
                }
            }
        }

        [HttpPost]
        [Route("edit")]
        public IActionResult EditSoftwareStream([FromBody] SoftwareStreamViewModel data)
        {
            using (var unitOfWork = CreateUnitOfWork())
            {
                SoftwareStream toEdit = unitOfWork.SoftwareStreams.GetOrNull(data.Id, "Icon", "StreamMembers");
                if (toEdit == null)
                {
                    return BadRequest("ERROR: The software stream does not exist");
                }

                toEdit.Name = data.Name;
                toEdit.UpdateSettings = data.UpdateSettings;
                toEdit.Architecture = data.Architecture;
                toEdit.Description = data.Description;
                toEdit.DescriptionShort = data.DescriptionShort;
                toEdit.DownloadLink = data.DownloadLink;
                toEdit.GnuLicence = data.GnuLicence;
                toEdit.Language = data.Language;
                toEdit.Vendor = data.Vendor;
                toEdit.Website = data.Website;
                toEdit.Type = data.Type;
                if (data.Icon != null && data.Icon.Guid == null)
                {
                    File icon = unitOfWork.Files.GetOrNull(data.Icon.Id);
                    if (icon == null)
                    {
                        File newIcon = Mapper.Map<File>(data.Icon);
                        newIcon.Guid = data.Icon.Id;
                        unitOfWork.Files.MarkForInsert(newIcon);
                        toEdit.Icon = newIcon;
                    }
                    else
                    {
                        toEdit.Icon = icon;
                    }
                }
                else
                {
                    if (data.Icon != null)
                    {
                        if (toEdit.Icon == null)
                        {
                            File newIcon = Mapper.Map<File>(data.Icon);
                            newIcon.Guid = data.Icon.Id;
                            unitOfWork.Files.MarkForInsert(newIcon);
                            toEdit.Icon = newIcon;
                        }
                        else if (data.Icon != null && data.Icon.Id != toEdit.Icon.Id)
                        {
                            File newIcon = Mapper.Map<File>(data.Icon);
                            newIcon.Guid = data.Icon.Id;
                            unitOfWork.Files.MarkForInsert(newIcon);
                            toEdit.Icon = newIcon;
                        }
                        else if (data.Icon == null && toEdit.Icon != null)
                        {
                            unitOfWork.Files.MarkForDelete(toEdit.Icon);
                            toEdit.Icon = null;
                        }
                    }
                }

                foreach (Software sw in toEdit.StreamMembers)
                {
                    sw.Name = toEdit.Name;
                    sw.Type = toEdit.Type;
                    unitOfWork.Software.MarkForUpdate(sw, GetCurrentUser().Id);
                }
                unitOfWork.SoftwareStreams.MarkForUpdate(toEdit, GetCurrentUser().Id);
                unitOfWork.SaveChanges();

                var json = JsonConvert.SerializeObject(Mapper.Map<SoftwareStreamViewModel>(toEdit), serializerSettings);
                return Ok(json);
            }
        }

        [HttpGet]
        [Route("getIconRef/{streamId}")]
        public IActionResult GetStreamIconRef([FromRoute] string streamId)
        {
            SoftwareStream swStream = UnitOfWork.SoftwareStreams.GetOrNull(streamId, "Icon");
            if (swStream == null)
            {
                return BadRequest("ERROR: The software stream does not exist");
            }
            FileRefModel result = null;
            if (swStream.Icon == null)
            {
                result = new FileRefModel();
            }
            else
            {
                result = Mapper.Map<FileRefModel>(swStream.Icon);
            }

            var json = JsonConvert.SerializeObject(result, serializerSettings);
            return Ok(json);
        }

        [HttpPost]
        [Route("{streamId}")]
        public IActionResult AddSoftwareToStream([FromBody] SoftwaresViewModel softwaresModel, [FromRoute] string streamId)
        {
            SoftwareStream stream = UnitOfWork.SoftwareStreams.GetOrNull(streamId, "StreamMembers");

            if (stream == null)
            {
                return BadRequest("ERROR: The software stream does not exist");
            }

            if (stream.StreamMembers == null)
            {
                stream.StreamMembers = new List<Software>();
            }

            List<Software> softwares = new List<Software>();
            foreach (SoftwareViewModel model in softwaresModel.Softwares)
            {
                softwares.Add(UnitOfWork.Software.GetOrNull(model.Id));
            }

            stream.StreamMembers.AddRange(softwares);
            UnitOfWork.SoftwareStreams.MarkForUpdate(stream, GetCurrentUser().Id);
            UnitOfWork.SaveChanges();

            foreach (SoftwareViewModel model in softwaresModel.Softwares)
            {
                softwares.Add(UnitOfWork.Software.GetOrNull(model.Id));
            }

            var json = JsonConvert.SerializeObject(Mapper.Map<List<SoftwareViewModel>>(softwares), serializerSettings);
            return Ok(json);
        }

        [HttpDelete]
        [Route("{streamId}")]
        public IActionResult DeleteSoftwareStream([FromRoute] string streamId)
        {
            try
            {
                using (var unitOfWork = CreateUnitOfWork())
                {
                    SoftwareStream stream = unitOfWork.SoftwareStreams.GetOrNull(streamId, "StreamMembers");
                    List<CustomerSoftwareStream> customerStreams = unitOfWork.CustomerSoftwareStreamss.GetAll().Where(x => x.SoftwareStreamId == streamId).ToList();
                    if (customerStreams.Count > 0)
                    {
                        return new ObjectResult("The stream is used by customers") { StatusCode = 403 };
                    }
                    foreach (Software sw in stream.StreamMembers)
                    {
                        sw.SoftwareStreamId = null;
                    }
                    stream.StreamMembers.Clear();
                    unitOfWork.SoftwareStreams.MarkForDelete(stream, GetCurrentUser().Id);
                    unitOfWork.SaveChanges();

                }
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest("ERROR: Could not delete software stream: " + e.Message);
            }
        }
    }

    public class SoftwareStreamViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string UpdateSettings { get; set; }
        public List<SoftwareViewModel> StreamMembers { get; set; }
        public string Vendor { get; set; }
        public string Description { get; set; }
        public string DescriptionShort { get; set; }
        public bool GnuLicence { get; set; }
        public string Architecture { get; set; }
        public string Language { get; set; }
        public string Website { get; set; }
        public string DownloadLink { get; set; }
        public FileRefModel Icon { get; set; }
        public string Type { get; set; }
    }

    public class SoftwaresViewModel
    {
        public List<SoftwareViewModel> Softwares { get; set; }
    }
}
