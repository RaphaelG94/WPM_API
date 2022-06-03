using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WPM_API.Code.Infrastructure;
using WPM_API.Code.Infrastructure.LogOn;
using WPM_API.Common;
using WPM_API.Data.DataContext.Entities;
using WPM_API.Models;
using WPM_API.Options;

namespace WPM_API.Controllers.Base
{
    [Route("imageStreams")]
    [Authorize(Policy = Constants.Policies.Systemhouse)]
    public class ImageStreamController : BasisController
    {
        public ImageStreamController(AppSettings appSettings, ConnectionStrings connectionStrings, OrderEmailOptions orderEmailOptions, AgentEmailOptions agentEmailOptions, SendMailCreds sendMailCreds, SiteOptions siteOptions, ILogonManager logonManager) : base(appSettings, connectionStrings, orderEmailOptions, agentEmailOptions, sendMailCreds, siteOptions, logonManager)
        {
        }

        [HttpGet]
        public IActionResult GetImageStreams()
        {
            List<ImageStream> imageStreams = UnitOfWork.ImageStreams.GetAll("Images", "Icon", "Images.Systemhouses", "Images.Customers").Where(x => x.CreatedByUserId == GetCurrentUser().Id).ToList();
            List<ImageStreamViewModel> result = Mapper.Map<List<ImageStreamViewModel>>(imageStreams);
            var json = JsonConvert.SerializeObject(result, serializerSettings);
            return Ok(json);
        }

        [HttpPost]
        public IActionResult AddImageStream([FromBody] ImageStreamViewModel data)
        {
            ImageStream newStream = Mapper.Map<ImageStream>(data);
            newStream.Images = new List<Image>();
            UnitOfWork.ImageStreams.MarkForInsert(newStream, GetCurrentUser().Id);
            UnitOfWork.SaveChanges();

            var json = JsonConvert.SerializeObject(newStream, serializerSettings);
            return Ok(json);
        }

        [HttpPost]
        [Route("edit")]
        public IActionResult UpdateImageStream([FromBody] ImageStreamViewModel data)
        {
            using (var unitOfWork = CreateUnitOfWork())
            {
                ImageStream toEdit = unitOfWork.ImageStreams.GetOrNull(data.Id, "Images", "Icon");
                if (toEdit == null)
                {
                    return BadRequest("ERROR: The image stream does not exist");
                }

                toEdit.Edition = data.Edition;
                toEdit.Name = data.Name;
                toEdit.Architecture = data.Architecture;
                toEdit.Description = data.Description;
                toEdit.DescriptionShort = data.DescriptionShort;
                toEdit.Language = data.Language;
                toEdit.SubFolderName = data.SubFolderName;
                toEdit.Vendor = data.Vendor;
                toEdit.Website = data.Website;
                toEdit.PrefixUrl = data.PrefixUrl;
                toEdit.SASKey = data.SASKey;
                if (data.Icon != null && data.Icon.Guid == null)
                {
                    Data.DataContext.Entities.File icon = unitOfWork.Files.GetOrNull(data.Icon.Id);
                    if (icon == null)
                    {
                        Data.DataContext.Entities.File newIcon = Mapper.Map<Data.DataContext.Entities.File>(data.Icon);
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
                            Data.DataContext.Entities.File newIcon = Mapper.Map<Data.DataContext.Entities.File>(data.Icon);
                            newIcon.Guid = data.Icon.Id;
                            unitOfWork.Files.MarkForInsert(newIcon);
                            toEdit.Icon = newIcon;
                        }
                        else if (data.Icon != null && data.Icon.Id != toEdit.Icon.Id)
                        {
                            Data.DataContext.Entities.File newIcon = Mapper.Map<Data.DataContext.Entities.File>(data.Icon);
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

                unitOfWork.ImageStreams.MarkForUpdate(toEdit, GetCurrentUser().Id);
                unitOfWork.SaveChanges();

                var json = JsonConvert.SerializeObject(toEdit, serializerSettings);
                return Ok(json);
            }
        }


        [HttpDelete]
        [Route("{streamId}")]
        public IActionResult DeleteImageStream([FromRoute] string streamId)
        {
            try
            {
                using (var unitOfWork = CreateUnitOfWork())
                {
                    ImageStream stream = unitOfWork.ImageStreams.GetOrNull(streamId, "Images");
                    foreach (Image toDelete in stream.Images)
                    {
                        unitOfWork.Images.MarkForDelete(toDelete, GetCurrentUser().Id);
                        // TODO: Delete files
                    }
                    stream.Images.Clear();
                    unitOfWork.ImageStreams.MarkForDelete(stream, GetCurrentUser().Id);
                    unitOfWork.SaveChanges();
                }
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest("ERROR: Could not delete software stream: " + e.Message);
            }
        }
        [HttpPost]
        [Route("icon/upload")]
        public async Task<IActionResult> UploadIconAsync(Microsoft.AspNetCore.Http.IFormFile file)
        {
            FileRepository.FileRepository iconRepo = new FileRepository.FileRepository(connectionStrings.FileRepository, appSettings.FileRepositoryFolder);
            string id = await iconRepo.UploadFile(file.OpenReadStream());
            var json = JsonConvert.SerializeObject(new { Id = id }, serializerSettings);
            return new OkObjectResult(json);
        }

        [HttpGet]
        [Route("icon/{fileId}")]
        public async Task<IActionResult> DownloadFileAsync([FromRoute] string fileId)
        {
            // TODO: Get files from CSDP
            FileRepository.FileRepository software = new FileRepository.FileRepository(connectionStrings.FileRepository, appSettings.FileRepositoryFolder);
            var file = UnitOfWork.Files.Get(fileId);
            if (file.Guid != null && file.Guid != "")
            {
                var blob = software.GetBlobFile(file.Guid);
                var ms = new MemoryStream();
                await blob.DownloadToAsync(ms);
                ms.Seek(0, SeekOrigin.Begin);
                return File(ms, System.Net.Mime.MediaTypeNames.Application.Octet, file.Name);
            }
            else
            {
                return Ok();
            }
        }

        [HttpGet]
        [Route("getIconRef/{streamId}")]
        public IActionResult GetStreamIconRef([FromRoute] string streamId)
        {
            ImageStream imageStream = UnitOfWork.ImageStreams.GetOrNull(streamId, "Icon");
            if (imageStream == null)
            {
                return BadRequest("ERROR: The software stream does not exist");
            }
            FileRefModel result = null;
            if (imageStream.Icon == null)
            {
                result = new FileRefModel();
            }
            else
            {
                result = Mapper.Map<FileRefModel>(imageStream.Icon);
            }

            var json = JsonConvert.SerializeObject(result, serializerSettings);
            return Ok(json);
        }

        public class ImageStreamViewModel
        {
            public string? Id { get; set; }
            public string? Name { get; set; }
            public string? Description { get; set; }
            public string? DescriptionShort { get; set; }
            public string? Architecture { get; set; }
            public string? Language { get; set; }
            public string? Website { get; set; }
            public string? Vendor { get; set; }
            public FileRefModel? Icon { get; set; }
            public List<ImageViewModel>? Images { get; set; }
            public string? SubFolderName { get; set; }
            public string? Edition { get; set; }
            public string? Type { get; set; }
            public string? PrefixUrl { get; set; }
            public string? SASKey { get; set; }
        }
    }
}
