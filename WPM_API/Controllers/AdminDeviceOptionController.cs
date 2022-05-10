using Azure.Storage.Blobs.Models;
using WPM_API.Data.DataContext.Entities;
using WPM_API.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using File = WPM_API.Data.DataContext.Entities.File;

namespace WPM_API.Controllers
{
    [Route("admin-device-option")]
    public class AdminDeviceOptionController : BasisController
    {
        [HttpPost]
        public async Task<IActionResult> AddAdminDeviceOption([FromBody] AdminDeviceOptionViewModel data)
        {
            using (var unitOfWork = CreateUnitOfWork())
            {
                FileRepository.FileRepository repository =
                new FileRepository.FileRepository(_connectionStrings.FileRepository,
                    _appSettings.FileRepositoryFolder);
                AdminDeviceOption newOption = new AdminDeviceOption();
                newOption.Name = data.Name;
                newOption.Description = data.Description;
                newOption.Versions = new List<ScriptVersion>();
                newOption.Type = data.Type;
                newOption.PEOnly = data.PEOnly;
                newOption.OSType = data.OSType;

                ScriptVersion newScriptVersion = new ScriptVersion();

                // Script in Azure-Cloud speichern
                newScriptVersion.ContentUrl =
                    await repository.UploadFile(newOption.Name + "000" + ".ps1", data.Content);
                newScriptVersion.Number = 1;
                newScriptVersion.Name = data.Name;
                newScriptVersion.Attachments = new List<File>();                
                newOption.Versions.Add(newScriptVersion);
                unitOfWork.AdminOptions.MarkForInsert(newOption);
                unitOfWork.SaveChanges();

                string json = JsonConvert.SerializeObject(newOption, _serializerSettings);
                return Ok(json);
            }
        }

        [HttpGet]
        public IActionResult GetAdminDeviceOptions()
        {
            List<AdminDeviceOptionViewModel> options = new List<AdminDeviceOptionViewModel>();
            List<AdminDeviceOption> dbEntries = UnitOfWork.AdminOptions.GetAll("Versions").ToList();
            options = Mapper.Map<List<AdminDeviceOption>, List<AdminDeviceOptionViewModel>>(dbEntries);
            // Serialize and return the response
            var json = JsonConvert.SerializeObject(options, _serializerSettings);
            return new OkObjectResult(json);
        }

        [HttpPut]
        public IActionResult CheckNameExisting()
        {
            return null;
        }

        [HttpGet]
        [Route("{scriptId}")]
        public async Task<IActionResult> GetLatestScriptVersion([FromRoute] string scriptId)
        {           
            AdminDeviceOption adminOption = UnitOfWork.AdminOptions.Get(scriptId, "Versions");
            ScriptVersion lastVersion = adminOption.Versions.OrderBy(x => x.Number).Last();
            FileRepository.FileRepository repository =
                new FileRepository.FileRepository(_connectionStrings.FileRepository,
                    _appSettings.FileRepositoryFolder);
            BlobDownloadResult dlResult = await repository.GetBlobFile(lastVersion.ContentUrl).DownloadContentAsync();
            var scriptContent = dlResult.Content.ToString();
            ScriptVersionContentViewModel scriptView = Mapper.Map<ScriptVersionContentViewModel>(lastVersion);
            scriptView.Content = scriptContent.ToString();

            // Serialize and return the response
            var json = JsonConvert.SerializeObject(scriptView, _serializerSettings);
            return new OkObjectResult(json);
        }

        [HttpDelete]
        [Route("{optionId}")]
        public async Task<IActionResult> DeleteAdminOption([FromRoute] string optionId)
        {
            try
            {
                using (var unitOfWork = CreateUnitOfWork())
                {
                    AdminDeviceOption toDelete = unitOfWork.AdminOptions.Get(optionId, "Versions");
                    FileRepository.FileRepository repository =
                            new FileRepository.FileRepository(_connectionStrings.FileRepository, _appSettings.FileRepositoryFolder);

                    List<ScriptVersion> scriptVersions = toDelete.Versions;
                    List<Parameter> parameters = new List<Parameter>();
                    List<ClientOption> clientOptions = new List<ClientOption>();
                    List<ExecutionLog> executionLogs = new List<ExecutionLog>();

                    // Delete ScriptVersions
                    foreach (ScriptVersion version in scriptVersions)
                    {
                        clientOptions.AddRange(unitOfWork.ClientOptions.GetAll("Parameters").Where(x => x.DeviceOptionId == version.Id).ToList());
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
                        var fileExists = await repository.FindFileAsync(version.ContentUrl);
                        if (fileExists)
                        {
                            var successAzure = await repository.DeleteFile(version.ContentUrl);
                            if (!successAzure)
                            {
                                return BadRequest("ERROR: The script version " + version.Name + versionNumber + ".ps1 could not be deleted in Azure!");
                            }
                        }
                        unitOfWork.ScriptVersions.MarkForDelete(version);
                    }

                    // Delete ExecutionLogs
                    foreach (ExecutionLog el in executionLogs)
                    {
                        unitOfWork.ExecutionLogs.MarkForDelete(el, GetCurrentUser().Id);
                    }

                    // Delete ClientOptions
                    foreach (ClientOption option in clientOptions)
                    {
                        parameters.AddRange(option.Parameters);
                        unitOfWork.ClientOptions.MarkForDelete(option, GetCurrentUser().Id);
                    }

                    // Delete Parameters
                    foreach (Parameter parameter in parameters)
                    {
                        unitOfWork.Parameters.MarkForDelete(parameter, GetCurrentUser().Id);
                    }

                    // Delete original script & save DB changes
                    unitOfWork.AdminOptions.MarkForDelete(toDelete, GetCurrentUser().Id);
                    unitOfWork.SaveChanges();
                    return Ok();
                }
            }
            catch (Exception e)
            {
                return BadRequest("ERROR: " + e.Message);
            }
        }

        [Route("{scriptId}/versions")]
        [HttpPost]
        public async Task<IActionResult> AddScriptVersion([FromRoute] string scriptId,
            [FromBody] ScriptVersionAddViewModel scriptVersionAdd)
        {
            FileRepository.FileRepository repository =
                    new FileRepository.FileRepository(_connectionStrings.FileRepository,
                        _appSettings.FileRepositoryFolder);

            AdminDeviceOption adminOption = UnitOfWork.AdminOptions.Get(scriptId, "Versions");
            ScriptVersion newScriptVersion = new ScriptVersion();

            // Script in Azure-Cloud speichern
            newScriptVersion.Number = adminOption.Versions.Count() + 1;
            newScriptVersion.Name = adminOption.Name;
            newScriptVersion.ContentUrl =
                await repository.UploadFile(adminOption.Name + newScriptVersion.Number.ToString("000") + ".ps1",
                    scriptVersionAdd.Content);

            adminOption.Versions.Add(newScriptVersion);
            UnitOfWork.AdminOptions.MarkForUpdate(adminOption);
            UnitOfWork.SaveChanges();
            var scriptVersions = UnitOfWork.AdminOptions.Get(adminOption.Id, "Versions");
            scriptVersions.Versions.ForEach(x => UnitOfWork.ScriptVersions.Get(x.Id, "Attachments"));
            AdminDeviceOptionViewModel result = Mapper.Map<AdminDeviceOption, AdminDeviceOptionViewModel>(scriptVersions);

            // Serialize and return the response
            var json = JsonConvert.SerializeObject(result, _serializerSettings);
            return new OkObjectResult(json);
        }

        [HttpPost]
        [Route("edit")]
        public IActionResult EditAdminOption([FromBody] AdminDeviceOptionViewModel data)
        {
            using (var unitOfWork = CreateUnitOfWork())
            {
                AdminDeviceOption toEdit = unitOfWork.AdminOptions.Get(data.Id, "Versions");
                toEdit.Name = data.Name;
                toEdit.Description = data.Description;
                toEdit.Type = data.Type;
                toEdit.PEOnly = data.PEOnly;
                toEdit.OSType = data.OSType;

                unitOfWork.AdminOptions.MarkForUpdate(toEdit, GetCurrentUser().Id);
                unitOfWork.SaveChanges();

                var json = JsonConvert.SerializeObject(toEdit, _serializerSettings);
                return Ok(json);
            }

        }
    }

    public class AdminDeviceOptionViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public List<ScriptVersion> Versions { get; set; }
        public string Type { get; set; }
        public bool PEOnly { get; set; }
        public string OSType { get; set; }
    }
}
