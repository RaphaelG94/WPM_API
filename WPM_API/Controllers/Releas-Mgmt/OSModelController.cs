using WPM_API.Common;
using WPM_API.Data.DataContext;
using WPM_API.Data.DataContext.Entities;
using WPM_API.FileRepository;
using WPM_API.TransferModels;
using WPM_API.Models.Release_Mgmt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using File = WPM_API.Data.DataContext.Entities.File;

namespace WPM_API.Controllers.Releas_Mgmt
{
    [Route("osModels")]
    public class OSModelController : BasisController
    {
        [HttpGet]
        [Authorize(Policy = Constants.Policies.Admin)]
        public IActionResult GetOSModels()
        {
            List<OSModel> osModels = UnitOfWork.OSModels.GetAll("Content", "HardwareModels").ToList();
            OSModelsViewModel result = new OSModelsViewModel();
            result.OsModels = new List<OSModelViewModel>();
            foreach (OSModel osModel in osModels)
            {
                OSModelViewModel osModelData = Mapper.Map<OSModelViewModel>(osModel);
                osModelData.HardwareModels = Mapper.Map<List<HardwareModelViewModel>>(osModel.HardwareModels);
                osModelData.Content = Mapper.Map<FileRef>(osModel.Content);
                result.OsModels.Add(osModelData);
            }
            var json = JsonConvert.SerializeObject(result, _serializerSettings);
            return new OkObjectResult(json);
        }

        [HttpPost]
        [Authorize(Policy = Constants.Policies.Admin)]
        public IActionResult AddOSModel([FromBody] OSModelViewModel osModel)
        {
            List<HardwareModel> hardwareModels = new List<HardwareModel>();
            foreach (string hardwareModelId in osModel.ValidModels)
            {
                HardwareModel currentHardwareModel = UnitOfWork.HardwareModels.Get(hardwareModelId);
                if (currentHardwareModel != null)
                {
                    hardwareModels.Add(currentHardwareModel);
                }
                else
                {
                    return BadRequest("ERROR: The hardware model with id " + hardwareModelId + " was not found");
                }
            }

            File contentFile = UnitOfWork.Files.GetAll().Where(x => x.Id == osModel.Content.Id).FirstOrDefault();
            if (contentFile == null)
            {
                return BadRequest("ERROR: The Content file was not found");
            }

            // Create new OS Model
            OSModel newOSModel = Mapper.Map<OSModel>(osModel);
            newOSModel.Content = contentFile;
            newOSModel.HardwareModels = hardwareModels;
            UnitOfWork.OSModels.MarkForInsert(newOSModel, GetCurrentUser().Id);
            UnitOfWork.SaveChanges();

            // Return OS data
            OSModelViewModel result = Mapper.Map<OSModelViewModel>(newOSModel);
            result.HardwareModels = Mapper.Map<List<HardwareModelViewModel>>(newOSModel.HardwareModels);
            result.Content = Mapper.Map<FileRef>(newOSModel.Content);

            var json = JsonConvert.SerializeObject(result, _serializerSettings);
            return new OkObjectResult(json);
        }

        [HttpPut]
        [Authorize(Policy = Constants.Policies.Admin)]
        public IActionResult UpdateOSModel([FromBody] OSModelViewModel osModel)
        {
            // TODO: update data in DB
            return null;
        }

        [HttpDelete]
        [Authorize(Policy = Constants.Policies.Admin)]
        [Route("{osModelId}")]
        public async Task<IActionResult> DeleteOSModelAsync([FromRoute] string osModelId)
        {
            OSModel toDelete = UnitOfWork.OSModels.Get(osModelId, "Content");
            if (toDelete == null)
            {
                return BadRequest("ERROR: The os model does not exist");
            }

            // Delete content file
            File contentToDelete = toDelete.Content;
            ResourcesRepository resourcesRepository = new ResourcesRepository(_connectionStrings.FileRepository, _appSettings.ResourcesRepositoryFolder);
            await resourcesRepository.DeleteFile(contentToDelete.Name);
            UnitOfWork.Files.MarkForDelete(contentToDelete);

            UnitOfWork.OSModels.MarkForDelete(toDelete, GetCurrentUser().Id);
            UnitOfWork.SaveChanges();
            return new OkResult();
        }
    }
}
