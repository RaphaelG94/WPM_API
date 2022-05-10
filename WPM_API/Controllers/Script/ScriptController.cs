using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using WPM_API.Common;
using DATA = WPM_API.Data.DataContext.Entities;
using WPM_API.Data.Models;
using WPM_API.FileRepository;
using WPM_API.Code;
using WPM_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.CodeAnalysis.Options;
using MimeKit;
using Azure.Storage.Blobs.Models;


/**************** Scripts not used atm. All functionality in device-options. *********************/
/******** TODO:  Refactor scripts + deviceoptions to options. *************/
namespace WPM_API.Controllers
{
    /// <summary>
    /// Manage the Script-Backend
    /// </summary>
    [Route("scripts")]
    public class ScriptController : BasisController
    {
        /// <summary>
        /// Retrive all scripts.
        /// </summary>
        /// <returns>[Script]</returns>
        [HttpGet]
        [Authorize(Policy = Constants.Roles.Customer)]
        public IActionResult GetScripts()
        {
            List<ScriptViewModel> scripts = new List<ScriptViewModel>();
            List<DATA.Script> dbEntries = UnitOfWork.Scripts.GetAll("Versions")
                .Where(x => x.Type.Equals(DATA.ScriptType.DomainOption)).ToList();
            scripts = Mapper.Map<List<DATA.Script>, List<ScriptViewModel>>(dbEntries);
            // Serialize and return the response
            var json = JsonConvert.SerializeObject(scripts, _serializerSettings);
            return new OkObjectResult(json);
        }

        [HttpDelete]
        [Route("{optionId}")]
        [Authorize(Policy = Constants.Roles.Systemhouse)]
        public async Task<IActionResult> DeleteOption([FromRoute] string optionId)
        {
            // TODO: delete attachements
            try
            {
                using (var unitOfWork = CreateUnitOfWork())
                {
                    DATA.Script toDelete = unitOfWork.Scripts.GetOrNull(optionId, "Versions", "Versions.Attachments");
                    if (toDelete == null)
                    {
                        return BadRequest("ERROR: The script does not exist");
                    }

                    // Connect to Azure script container
                    FileRepository.FileRepository repository =
                        new FileRepository.FileRepository(_connectionStrings.FileRepository, _appSettings.FileRepositoryFolder);

                    // Fetch all relations
                    List<DATA.ScriptVersion> scriptVersions = toDelete.Versions;
                    List<DATA.Parameter> parameters = new List<DATA.Parameter>();
                    List<DATA.ClientOption> clientOptions = new List<DATA.ClientOption>();
                    List<DATA.File> files = new List<DATA.File>();
                    List<DATA.ExecutionLog> executionLogs = new List<DATA.ExecutionLog>();

                    // Delete ScriptVersions
                    foreach (DATA.ScriptVersion version in scriptVersions)
                    {
                        clientOptions.AddRange(unitOfWork.ClientOptions.GetAll("Parameters").Where(x => x.DeviceOptionId == version.Id).ToList());
                        files.AddRange(version.Attachments);
                        executionLogs.AddRange(unitOfWork.ExecutionLogs.GetAll().Where(x => x.ScriptVersionId == version.Id).ToList());

                        // Delete script version in Azure
                        string versionNumber;
                        if (version.Number < 10)
                        {
                            versionNumber = "00" + version.Number;
                        }
                        else if (version.Number < 100)
                        {
                            versionNumber = "0" + version.Number;
                        }
                        else
                        {
                            versionNumber = "" + version.Number;
                        }
                        var fileExists = await repository.FindFileAsync(version.Name + versionNumber + ".ps1");
                        if (fileExists)
                        {
                            var successAzure = await repository.DeleteFile(version.Name + versionNumber + ".ps1");
                            if (!successAzure)
                            {
                                return BadRequest("ERROR: The script version " + version.Name + versionNumber + ".ps1 could not be deleted in Azure!");
                            }
                        }
                        unitOfWork.ScriptVersions.MarkForDelete(version);
                    }

                    // Delete ExecutionLogs
                    foreach(DATA.ExecutionLog el in executionLogs)
                    {
                        unitOfWork.ExecutionLogs.MarkForDelete(el, GetCurrentUser().Id);
                    }

                    // Delete ClientOptions
                    foreach (DATA.ClientOption option in clientOptions)
                    {
                        parameters.AddRange(option.Parameters);
                        unitOfWork.ClientOptions.MarkForDelete(option, GetCurrentUser().Id);
                    }

                    // Delete Parameters
                    foreach(DATA.Parameter parameter in parameters)
                    {
                        unitOfWork.Parameters.MarkForDelete(parameter, GetCurrentUser().Id);
                    }

                    // Delete original script & save DB changes
                    unitOfWork.Scripts.MarkForDelete(toDelete, GetCurrentUser().Id);
                    unitOfWork.SaveChanges();

                    return Ok();
                }
            } catch (Exception e)
            {
                return BadRequest("ERROR: " + e.Message);
            }
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
                new FileRepository.FileRepository(_connectionStrings.FileRepository,
                    _appSettings.FileRepositoryFolder);
            DATA.Script newScript = UnitOfWork.Scripts.CreateEmpty(GetCurrentUser().Id);
            newScript.Description = scriptAdd.Description;
            newScript.Name = scriptAdd.Name;
            newScript.Versions = new List<DATA.ScriptVersion>();
            newScript.Type = DATA.ScriptType.DomainOption;
            DATA.ScriptVersion newScriptVersion = new DATA.ScriptVersion();

            // Script in Azure-Cloud speichern
            newScriptVersion.ContentUrl =
                await repository.UploadFile(newScript.Name + "000" + ".ps1", scriptAdd.Content);
            newScriptVersion.Number = 1;
            newScriptVersion.Name = scriptAdd.Name;
            newScriptVersion.Attachments = new List<DATA.File>();

            // persist attachments from temp storage
            foreach (var fileRef in scriptAdd.Attachments)
            {
                //await PersistFileAsync(fileRef);
                var file = UnitOfWork.Files.GetByGuid(fileRef.Id);
                newScriptVersion.Attachments.Add(file);
            }

            newScript.Versions.Add(newScriptVersion);
            UnitOfWork.SaveChanges();
            ScriptViewModel script =
                Mapper.Map<DATA.Script, ScriptViewModel>(UnitOfWork.Scripts.Get(newScript.Id, "Versions"));

            // Serialize and return the response
            var json = JsonConvert.SerializeObject(script, _serializerSettings);
            return new OkObjectResult(json);
        }

        /// <summary>
        /// Update existing script.
        /// </summary>
        /// <param name="scriptId">Id from the Script</param>
        /// <param name="scriptEdit">Modified Script</param>
        /// <returns>Script</returns>
        [HttpPut]
        [Route("{scriptId}")]
        [Authorize(Policy = Constants.Roles.Systemhouse)]
        public IActionResult UpdateScripts([FromRoute] string scriptId, [FromBody] ScriptEditViewModel scriptEdit)
        {
            DATA.Script script = UnitOfWork.Scripts.Get(scriptId);
            script.Name = scriptEdit.Name;
            script.Description = scriptEdit.Description;
            UnitOfWork.Scripts.MarkForUpdate(script, GetCurrentUser().Id);
            UnitOfWork.SaveChanges();
            ScriptViewModel result = Mapper.Map<DATA.Script, ScriptViewModel>(UnitOfWork.Scripts.Get(script.Id, "Versions"));
            // Serialize and return the response
            var json = JsonConvert.SerializeObject(result, _serializerSettings);
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
                new FileRepository.FileRepository(_connectionStrings.FileRepository,
                    _appSettings.FileRepositoryFolder);
            DATA.Script script = UnitOfWork.Scripts.Get(scriptId, "Versions");
            DATA.ScriptVersion newScriptVersion = new DATA.ScriptVersion();
            if (newScriptVersion.Attachments == null)
            {
                newScriptVersion.Attachments = new List<DATA.File>();
            }

            // persist attachments from temp storage
            foreach (var fileRef in scriptVersionAdd.Attachments)
            {
                //await PersistFileAsync(fileRef);
                var file = UnitOfWork.Files.GetByGuid(fileRef.Id);
                newScriptVersion.Attachments.Add(file);
            }

            // Script in Azure-Cloud speichern
            newScriptVersion.Number = script.Versions.Count() + 1;
            newScriptVersion.Name = script.Name;
            newScriptVersion.ContentUrl =
                await repository.UploadFile(script.Name + newScriptVersion.Number.ToString("000") + ".ps1",
                    scriptVersionAdd.Content);
            script.Versions.Add(newScriptVersion);
            UnitOfWork.Scripts.MarkForUpdate(script);
            UnitOfWork.SaveChanges();
            ScriptViewModel result = Mapper.Map<DATA.Script, ScriptViewModel>(UnitOfWork.Scripts.Get(script.Id, "Versions"));

            // Serialize and return the response
            var json = JsonConvert.SerializeObject(result, _serializerSettings);
            return new OkObjectResult(json);
        }


        [HttpGet]
        [Route("{scriptId}")]
        [Authorize(Policy = Constants.Roles.Customer)]
        public async Task<IActionResult> GetLatestScriptVersion([FromRoute] string scriptId)
        {
            DATA.Script script = UnitOfWork.Scripts.Get(scriptId, "Versions");
            DATA.ScriptVersion lastVersion = script.Versions.OrderBy(x => x.Number).Last();
            FileRepository.FileRepository repository =
                new FileRepository.FileRepository(_connectionStrings.FileRepository,
                    _appSettings.FileRepositoryFolder);
            BlobDownloadResult downloadResult = await repository.GetBlobFile(lastVersion.ContentUrl).DownloadContentAsync();
            var scriptContent = downloadResult.Content.ToString();
            ScriptVersionContentViewModel scriptView = Mapper.Map<ScriptVersionContentViewModel>(lastVersion);
            scriptView.Content = scriptContent.ToString();

            // Serialize and return the response
            var json = JsonConvert.SerializeObject(scriptView, _serializerSettings);
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
            DATA.Script script = UnitOfWork.Scripts.Get(scriptId, ScriptIncludes.GetAllIncludes());
            DATA.ScriptVersion scriptVersion = script.Versions.Find(x => x.Number == number);
            scriptVersion = UnitOfWork.ScriptVersions.Get(scriptVersion.Id, "Attachments");
            FileRepository.FileRepository repository =
                new FileRepository.FileRepository(_connectionStrings.FileRepository, _appSettings.FileRepositoryFolder);

            string fileName = script.Name + "_v" + number + ".ps1";
            Dictionary<string, string> files = new Dictionary<string, string>();
            files.Add(scriptVersion.ContentUrl, script.Name + "_v" + number + ".ps1");
            scriptVersion.Attachments?.ForEach(x => files.Add(x.Guid, x.Name));

            var zip = await repository.CreateZipFileFromBlobs(files);
            return File(zip, System.Net.Mime.MediaTypeNames.Application.Zip, fileName);
        }

        [HttpGet]
        [Route("{scriptId}/{version}")]
        [Authorize(Policy = Constants.Roles.Customer)]
        public async Task<IActionResult> GetScriptVersion([FromRoute] string scriptId, [FromRoute] int version)
        {
            DATA.Script script = UnitOfWork.Scripts.Get(scriptId, "Versions");
            DATA.ScriptVersion lastVersion = script.Versions.FirstOrDefault(x => x.Number == version);
            lastVersion = UnitOfWork.ScriptVersions.Get(lastVersion?.Id, "Attachments");
            ScriptVersionViewModel scriptVersionView = Mapper.Map<DATA.ScriptVersion, ScriptVersionViewModel>(lastVersion);

            // Serialize and return the response
            var json = JsonConvert.SerializeObject(scriptVersionView, _serializerSettings);
            return new OkObjectResult(json);
        }

        /********************************************** Should be moved to some global utility class/controller. Especially temp endpoint. *******************************************************/
//        private async Task<FileRef> PersistFileAsync(FileRef file)
//        {
//            if (file != null)
//            {
//                TempRepository tempRepository =
//                    new TempRepository(_connectionStrings.FileRepository, _appSettings.TempFolder);
//                string id = await tempRepository.MoveFileAsync(file.Id, _appSettings.FileRepositoryFolder, true);
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
                new FileRepository.FileRepository(_connectionStrings.FileRepository, _appSettings.FileRepositoryFolder);
            string id = await temp.UploadFile(file.OpenReadStream());
            newFile.Guid = id;
            newFile.Name = file.FileName;
            UnitOfWork.Files.MarkForInsert(newFile);
            UnitOfWork.SaveChanges();
            var json = JsonConvert.SerializeObject(new {Id = id, Name = file.FileName}, _serializerSettings);
            return new OkObjectResult(json);
        }

        [HttpPost]
        [Route("bitstreamAction/{actionName}/{clientId}/{customerId}")]
        [Authorize(Policy = Constants.Policies.Customer)]
        public async Task<IActionResult> AssignBitstreamScript([FromRoute] string actionName, [FromRoute] string clientId, [FromRoute] string customerId, [FromBody] SmbStorageViewModel storageData)
        {
            // Match BitStream script name for specific action
            List<string> BitStreamScriptNames = CreateBitStreamScriptNames();
            string scriptName = "";
            switch (actionName)
            {
                case "SMB": scriptName = BitStreamScriptNames[0];
                        break;
                case "INVENTORY": scriptName = BitStreamScriptNames[1];
                    break;
            }
            if (scriptName.Length == 0)
            {
                return BadRequest("The script was not found in the BitStream default set!");
            }

            // Load the script 
            DATA.Script script = UnitOfWork.Scripts.GetAll("Versions").Where(x => x.AuthorType == 0 && x.Name == scriptName).FirstOrDefault();

            if (script == null)
            {
                return BadRequest("The script does not exist!");
            }

            // Load latest script version
            DATA.ScriptVersion lastVersion = script.Versions.OrderBy(x => x.Number).Last();
            // Get Script from FileRepository
            FileRepository.FileRepository repository =
                new FileRepository.FileRepository(_connectionStrings.FileRepository,
                    _appSettings.FileRepositoryFolder);
            string scriptContent = string.Empty;
            try
            {
                BlobDownloadResult downloadResult = await repository.GetBlobFile(lastVersion.ContentUrl).DownloadContentAsync();
                scriptContent = downloadResult.Content.ToString();
            }
            catch (Exception)
            {
                return new NotFoundObjectResult("Script in script-repository not found.");
            }

            // Get script parameters
            List<ParameterViewModel> parameters = ScriptHelper.GetParametersFromScript(scriptContent, script.OSType);

            // Load client
            DATA.Client client = UnitOfWork.Clients.GetOrNull(clientId, "AssignedOptions");
            if (client == null)
            {
                return BadRequest("The client does not exist");
            }
            if (client.AssignedOptions == null)
            {
                client.AssignedOptions = new List<DATA.ClientOption>();
            }
            // Add Order as last if there are already options assigned.
            int orderIndex = 0;
            DATA.ClientOption lastOption = client.AssignedOptions.LastOrDefault();
            if (lastOption != null)
            {
                orderIndex = lastOption.Order + 1;
            }
            List<DATA.Parameter> dataParameters = Mapper.Map<List<DATA.Parameter>>(parameters);
            if (actionName == "SMB") {
                foreach (DATA.Parameter parameter in dataParameters)
                {
                    if (parameter.Key == "$CSDPShareName")
                    {
                        parameter.Value = storageData.ShareName;
                        UnitOfWork.SaveChanges();
                    } else if (parameter.Key == "$CustomRepository")
                    {
                        parameter.Value = storageData.DataDriveLetter + ":\\";
                        UnitOfWork.SaveChanges();
                    }
                }
            }

            // Create ClientOption for SMB creation
            DATA.ClientOption smbCreationOptipon = new DATA.ClientOption
            {
                DeviceOptionId = lastVersion.Id,
                ClientId = clientId,
                Order = orderIndex,
                Parameters = dataParameters
            };

            // Assign SMB creation option
            client.AssignedOptions.Add(smbCreationOptipon);

            // Save changes
            UnitOfWork.Clients.MarkForUpdate(client, GetCurrentUser().Id);
            UnitOfWork.SaveChanges();

            // Serialize & return result
            var json = JsonConvert.SerializeObject(Mapper.Map<ClientViewModel>(client), _serializerSettings);
            return new OkObjectResult(json);
        }

        private List<string> CreateBitStreamScriptNames()
        {
            return new List<string>() { "SHOWCASE 04: Create CSDP share", "HWOSWinInventory" };
        }
    }

}