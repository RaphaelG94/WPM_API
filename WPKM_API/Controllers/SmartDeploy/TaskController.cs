using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPM_API.Common;
using WPM_API.Data.DataContext.Entities;
using WPM_API.FileRepository;
using WPM_API.TransferModels.SmartDeploy;
using WPM_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using File = WPM_API.Data.DataContext.Entities.File;

namespace WPM_API.Controllers
{
    [Authorize(Policy = Constants.Roles.Systemhouse)]
    [Route("tasks")]
    public class TaskController : BasisController
    {
        [HttpGet]
        public IActionResult GetTasks()
        {
            var taskList = new List<TaskViewModel>();
            var json = JsonConvert.SerializeObject(taskList, _serializerSettings);
            return new OkObjectResult(json);
        }

        [HttpPost]
        [Route("upload/releaseMgmt")]
        public async Task<IActionResult> UploadFileAsyncWithName([FromForm] IFormFile file)
        {
            try
            {
                File newFile = UnitOfWork.Files.CreateEmpty();
                ResourcesRepository resourcesRepository = new ResourcesRepository(_connectionStrings.FileRepository, _appSettings.ResourcesRepositoryFolder);
                string fileName = await resourcesRepository.UploadFile(file.OpenReadStream(), file.FileName);
                newFile.Name = fileName;
                UnitOfWork.SaveChanges();
                var json = JsonConvert.SerializeObject(new FileRefModel() { Id = newFile.Id, Name = fileName }, _serializerSettings);
                return new OkObjectResult(json);
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult("ERROR: " + e.Message);
            }
        }

        [HttpPost]
        [Route("upload")]
        public async Task<IActionResult> UploadFileAsync([FromForm] IFormFile file)
        {
            FileRepository.FileRepository software = new FileRepository.FileRepository(_connectionStrings.FileRepository, _appSettings.FileRepositoryFolder);
            string id = await software.UploadFile(file.OpenReadStream());
            var json = JsonConvert.SerializeObject(new { Id = id }, _serializerSettings);
            return new OkObjectResult(json);
        } 

        [HttpDelete]
        [Route("delete/{fileName}")]
        public async Task<IActionResult> DeleteFileAsync([FromRoute] string fileName)
        {
            File toDelete = UnitOfWork.Files.GetAll().Where(x => x.Name == fileName).First();
            if (toDelete == null)
            {
                return BadRequest();
            }
            ResourcesRepository resourcesRepository = new ResourcesRepository(_connectionStrings.FileRepository, _appSettings.ResourcesRepositoryFolder);
            var deleteSuccess = await resourcesRepository.DeleteFile(fileName);
            if (deleteSuccess)
            {
                UnitOfWork.Files.MarkForDelete(toDelete);
                UnitOfWork.SaveChanges();
                return Ok();
            } else
            {
                return BadRequest("ERROR: File " + fileName + "could not be deleted");
            }
        }

        [HttpGet]
        [Authorize(Policy = Constants.Policies.Customer)]
        [Route("resources-sas")]
        public IActionResult GetResourcesReleaseSASURL()
        {
            ResourcesRepository resourcesRepository = new ResourcesRepository(_connectionStrings.FileRepository, _appSettings.ResourcesRepositoryFolder);
            string sasURL = resourcesRepository.GetSASBlobContainer();
            if (sasURL == null)
            {
                return BadRequest("ERROR: The SAS Uri could not be fetched");
            }

            var json = JsonConvert.SerializeObject(new SASURLViewModel() { Url = sasURL }, _serializerSettings);
            return Ok(json);
        }

    }
}