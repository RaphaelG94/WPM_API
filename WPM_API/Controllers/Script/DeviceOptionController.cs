using Azure.Storage.Blobs.Models;
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
using File = WPM_API.Data.DataContext.Entities.File;

namespace WPM_API.Controllers
{
    /// <summary>
    /// Manage the Script-Backend
    /// </summary>
    [Route("device-options")]
    public class DeviceOptionController : BasisController
    {
        public DeviceOptionController(AppSettings appSettings, ConnectionStrings connectionStrings, OrderEmailOptions orderEmailOptions, AgentEmailOptions agentEmailOptions, SendMailCreds sendMailCreds, SiteOptions siteOptions, ILogonManager logonManager) : base(appSettings, connectionStrings, orderEmailOptions, agentEmailOptions, sendMailCreds, siteOptions, logonManager)
        {
        }

        /// <summary>
        /// Retrive all scripts.
        /// </summary>
        /// <returns>[Script]</returns>
        [HttpGet]
        [Authorize(Policy = Constants.Roles.Customer)]
        public IActionResult GetScripts()
        {
            List<ScriptViewModel> scripts = new List<ScriptViewModel>();
            List<Script> dbEntries = UnitOfWork.Scripts.GetAll("Versions")
                .Where(x => x.Type.Equals(ScriptType.DeviceOption) && x.CreatedByUserId == GetCurrentUser().Id).ToList();
            scripts = Mapper.Map<List<Script>, List<ScriptViewModel>>(dbEntries);
            // Serialize and return the response
            var json = JsonConvert.SerializeObject(scripts, serializerSettings);
            return new OkObjectResult(json);
        }

        /// <summary>
        /// Create new script.
        /// </summary>
        /// <param name="scriptAdd">New Script</param>
        /// <returns>Script</returns>
        [HttpPost]
        [Authorize(Policy = Constants.Roles.Systemhouse)]
        public async Task<IActionResult> AddScript([FromBody] ScriptAddViewModel scriptAdd)
        {
            FileRepository.FileRepository repository =
                new FileRepository.FileRepository(connectionStrings.FileRepository,
                    appSettings.FileRepositoryFolder);
            Script newScript = UnitOfWork.Scripts.CreateEmpty(GetCurrentUser().Id);
            newScript.Description = scriptAdd.Description;
            newScript.Name = scriptAdd.Name;
            newScript.PEOnly = scriptAdd.PEOnly;
            newScript.Versions = new List<ScriptVersion>();
            newScript.Type = ScriptType.DeviceOption;
            newScript.OSType = scriptAdd.OSType;
            if (scriptAdd.BitstreamScript)
            {
                newScript.showToCustomer = false;
            }
            else
            {
                newScript.showToCustomer = true;
            }
            // Set author type

            if (CurrentUserIsInRole(Constants.Roles.Admin))
            {
                newScript.AuthorType = AuthorType.BitStream;
            }
            else
            {
                newScript.AuthorType = AuthorType.Customer;
            }
            ScriptVersion newScriptVersion = new ScriptVersion();

            if (scriptAdd.OSType == "Windows")
            {
                // Script in Azure-Cloud speichern
                newScriptVersion.ContentUrl =
                    await repository.UploadFile(newScript.Name + "000" + ".ps1", scriptAdd.Content);
                newScriptVersion.Number = 1;
                newScriptVersion.Name = scriptAdd.Name;
                newScriptVersion.Attachments = new List<File>();
            }
            else
            {
                newScriptVersion.ContentUrl =
                    await repository.UploadFile(newScript.Name + "000" + ".sh", scriptAdd.Content);
                newScriptVersion.Number = 1;
                newScriptVersion.Name = scriptAdd.Name;
                newScriptVersion.Attachments = new List<File>();
            }

            // persist attachments from temp storage
            foreach (var fileRef in scriptAdd.Attachments)
            {
                //await PersistFileAsync(fileRef);
                var file = UnitOfWork.Files.GetByGuid(fileRef.Id);
                newScriptVersion.Attachments.Add(file);
            }

            newScript.Versions.Add(newScriptVersion);
            UnitOfWork.Scripts.MarkForInsert(newScript);
            UnitOfWork.SaveChanges();
            ScriptViewModel script =
                Mapper.Map<Script, ScriptViewModel>(UnitOfWork.Scripts.Get(newScript.Id, "Versions"));
            script.ShowToCustomer = newScript.showToCustomer;

            // Serialize and return the response
            var json = JsonConvert.SerializeObject(script, serializerSettings);
            return new OkObjectResult(json);
        }

        /// <summary>
        /// Retrive all scripts.
        /// </summary>
        /// <returns>[Script]</returns>
        [HttpGet]
        [Route("versions")]
        [Authorize(Policy = Constants.Roles.Customer)]
        public IActionResult GetAllVersions()
        {
            List<DeviceOptionVersionRefViewModel> versions = new List<DeviceOptionVersionRefViewModel>();
            List<Script> dbEntries = UnitOfWork.Scripts.GetAll("Versions")
                .Where(x => x.Type.Equals(ScriptType.DeviceOption) && x.showToCustomer).ToList();
            foreach (Script entry in dbEntries)
            {
                var scriptVersions = entry.Versions;
                entry.Versions = new List<ScriptVersion> { scriptVersions.OrderBy(x => x.Number).Last() };
                entry.Versions.ForEach(x => versions.Add(new DeviceOptionVersionRefViewModel()
                { Id = x.Id, Name = entry.Name + " v" + x.Number, OSType = entry.OSType }));
            }

            // Serialize and return the response
            var json = JsonConvert.SerializeObject(versions, serializerSettings);
            return new OkObjectResult(json);
        }

        [HttpGet]
        [Route("{scriptId}")]
        [Authorize(Policy = Constants.Roles.Customer)]
        public async Task<IActionResult> GetLatestScriptVersion([FromRoute] string scriptId)
        {
            Script script = UnitOfWork.Scripts.Get(scriptId, "Versions");
            ScriptVersion lastVersion = script.Versions.OrderBy(x => x.Number).Last();
            FileRepository.FileRepository repository =
                new FileRepository.FileRepository(connectionStrings.FileRepository,
                    appSettings.FileRepositoryFolder);
            BlobDownloadResult downloadResult = await repository.GetBlobFile(lastVersion.ContentUrl).DownloadContentAsync();
            var scriptContent = downloadResult.Content.ToString();
            ScriptVersionContentViewModel scriptView = Mapper.Map<ScriptVersionContentViewModel>(lastVersion);
            scriptView.Content = scriptContent.ToString();

            // Serialize and return the response
            var json = JsonConvert.SerializeObject(scriptView, serializerSettings);
            return new OkObjectResult(json);
        }

        /// <summary>
        /// Update existing script.
        /// </summary>
        /// <param name="scriptId">Id of the Script</param>
        /// <param name="scriptEdit">Modified Script</param>
        /// <returns>Script</returns>
        [HttpPut]
        [Route("{scriptId}")]
        [Authorize(Policy = Constants.Roles.Systemhouse)]
        public IActionResult UpdateScripts([FromRoute] string scriptId, [FromBody] ScriptEditViewModel scriptEdit)
        {
            Script script = UnitOfWork.Scripts.Get(scriptId);
            script.Name = scriptEdit.Name;
            script.Description = scriptEdit.Description;
            script.PEOnly = scriptEdit.PEOnly;
            script.OSType = scriptEdit.OSType;
            if (scriptEdit.BitstreamScript)
            {
                script.showToCustomer = false;
            }
            else
            {
                script.showToCustomer = true;
            }
            UnitOfWork.Scripts.MarkForUpdate(script, GetCurrentUser().Id);
            UnitOfWork.SaveChanges();
            ScriptViewModel result = Mapper.Map<Script, ScriptViewModel>(UnitOfWork.Scripts.Get(script.Id, "Versions"));
            result.ShowToCustomer = script.showToCustomer;

            // Serialize and return the response
            var json = JsonConvert.SerializeObject(result, serializerSettings);
            return new OkObjectResult(json);
        }

        /// <summary>
        /// Create new script version.
        /// </summary>
        /// <param name="scriptId">Id from the Script</param>
        /// <param name="scriptVersionAdd">New Version of the Script</param>
        /// <returns>ScriptVersion</returns>
        [HttpPost]
        [Route("{scriptId}/versions")]
        [Authorize(Policy = Constants.Roles.Systemhouse)]
        public async Task<IActionResult> AddScriptVersion([FromRoute] string scriptId,
            [FromBody] ScriptVersionAddViewModel scriptVersionAdd)
        {
            FileRepository.FileRepository repository =
                    new FileRepository.FileRepository(connectionStrings.FileRepository,
                        appSettings.FileRepositoryFolder);

            Script script = UnitOfWork.Scripts.Get(scriptId, "Versions");
            ScriptVersion newScriptVersion = new ScriptVersion();
            if (newScriptVersion.Attachments == null)
            {
                newScriptVersion.Attachments = new List<File>();
            }

            // Check for 
            if (scriptVersionAdd.Attachments != null)
            {
                // persist attachments from temp storage
                foreach (var fileRef in scriptVersionAdd.Attachments)
                {
                    //await PersistFileAsync(fileRef);
                    var file = UnitOfWork.Files.GetByGuid(fileRef.Id);
                    newScriptVersion.Attachments.Add(file);
                }
            }

            // Script in Azure-Cloud speichern
            newScriptVersion.Number = script.Versions.Count() + 1;
            newScriptVersion.Name = script.Name;
            if (script.OSType == "Windows")
            {
                newScriptVersion.ContentUrl =
                    await repository.UploadFile(script.Name + newScriptVersion.Number.ToString("000") + ".ps1",
                        scriptVersionAdd.Content);
            }
            else
            {
                newScriptVersion.ContentUrl =
                    await repository.UploadFile(script.Name + newScriptVersion.Number.ToString("000") + ".sh",
                        scriptVersionAdd.Content);
            }

            script.Versions.Add(newScriptVersion);
            UnitOfWork.Scripts.MarkForUpdate(script);
            UnitOfWork.SaveChanges();
            var scriptVersions = UnitOfWork.Scripts.Get(script.Id, "Versions");
            scriptVersions.Versions.ForEach(x => UnitOfWork.ScriptVersions.Get(x.Id, "Attachments"));
            ScriptViewModel result = Mapper.Map<Script, ScriptViewModel>(scriptVersions);

            // Serialize and return the response
            var json = JsonConvert.SerializeObject(result, serializerSettings);
            return new OkObjectResult(json);
        }


        [HttpGet]
        [Route("{scriptId}/{version}")]
        [Authorize(Policy = Constants.Roles.Customer)]
        public async Task<IActionResult> GetScriptVersion([FromRoute] string scriptId, [FromRoute] int version)
        {
            Script script = UnitOfWork.Scripts.Get(scriptId, "Versions");
            ScriptVersion scriptVersion = script.Versions.FirstOrDefault(x => x.Number == version);
            scriptVersion = UnitOfWork.ScriptVersions.Get(scriptVersion?.Id, "Attachments");
            ScriptVersionViewModel scriptVersionView = Mapper.Map<ScriptVersion, ScriptVersionViewModel>(scriptVersion);

            // Serialize and return the response
            var json = JsonConvert.SerializeObject(scriptVersionView, serializerSettings);
            return new OkObjectResult(json);
        }

        /// <summary>
        /// Download a script version with all attached files as zip archive.
        /// HttpPut is used since get requests do  not support a request body.
        /// </summary>
        /// <param name="scriptId"></param>
        /// <param name="scriptVersionView"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{scriptId}/zip/{number}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetScriptVersionWithAttachmentsAsZip([FromRoute] string scriptId,
            [FromRoute] int number)
        {
            Script script = UnitOfWork.Scripts.Get(scriptId, ScriptIncludes.GetAllIncludes());
            ScriptVersion scriptVersion = script.Versions.Find(x => x.Number == number);
            scriptVersion = UnitOfWork.ScriptVersions.Get(scriptVersion.Id, "Attachments");
            FileRepository.FileRepository repository =
                new FileRepository.FileRepository(connectionStrings.FileRepository, appSettings.FileRepositoryFolder);

            Dictionary<string, string> files = new Dictionary<string, string>();
            files.Add(scriptVersion.ContentUrl, script.Name + "_v" + number + ".ps1");
            scriptVersion.Attachments?.ForEach(x => files.Add(x.Guid, x.Name));

            var zip = await repository.CreateZipFileFromBlobs(files);
            return File(zip, System.Net.Mime.MediaTypeNames.Application.Zip, script.Name + "_v" + number + ".zip");
        }

        /********************************************** Should be moved to some global utility class/controller. Especially temp endpoint. *******************************************************/

        //        private async Task<FileRef> PersistFileAsync(FileRef file)
        //        {
        //            if (file != null)
        //            {
        //                TempRepository tempRepository =
        //                    new TempRepository(connectionStrings.FileRepository, appSettings.TempFolder);
        //                string id = await tempRepository.MoveFileAsync(file.Id, appSettings.FileRepositoryFolder, true);
        //                return new FileRef() {Id = id, Name = file.Name};
        //            }
        //
        //            return null;
        //        }


        [HttpPost]
        [Route("temp")]
        [Authorize(Policy = Constants.Roles.Customer)]
        public async Task<IActionResult> UploadTempFileAsync([FromForm] IFormFile file)
        {
            var newFile = UnitOfWork.Files.CreateEmpty();
            FileRepository.FileRepository temp =
                new FileRepository.FileRepository(connectionStrings.FileRepository, appSettings.FileRepositoryFolder);
            string id = await temp.UploadFile(file.OpenReadStream());
            newFile.Guid = id;
            newFile.Name = file.FileName;
            UnitOfWork.Files.MarkForInsert(newFile);
            UnitOfWork.SaveChanges();
            var json = JsonConvert.SerializeObject(new { Id = id, Name = file.FileName }, serializerSettings);
            return new OkObjectResult(json);
        }
    }
}