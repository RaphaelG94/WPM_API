﻿using WPM_API.Common;
using WPM_API.Data.DataContext.Entities;
using WPM_API.Data.Models;
using WPM_API.TransferModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WPM_API.Controllers
{
    [Route("vendor-mgmt")]
    public class VendorModelController : BasisController
    {
        [HttpGet]
        [Authorize(Policy = Constants.Policies.Customer)]
        public IActionResult GetVendorModels()
        {
            List<VendorModel> vendorModels = UnitOfWork.VendorModels.GetAll("Files").ToList();

            var json = JsonConvert.SerializeObject(Mapper.Map<List<VendorModelViewModel>>(vendorModels), _serializerSettings);

            return Ok(json);
        }

        [HttpPost]
        [Authorize(Policy = Constants.Policies.Admin)]
        public IActionResult AddVendorModel([FromBody] VendorModelViewModel data)
        {
            VendorModel newVendorModel = Mapper.Map<VendorModel>(data);

            UnitOfWork.VendorModels.MarkForInsert(newVendorModel, GetCurrentUser().Id);
            UnitOfWork.SaveChanges();

            var json = JsonConvert.SerializeObject(Mapper.Map<VendorModelViewModel>(newVendorModel), _serializerSettings);

            return Ok(json);
        }

        [HttpPut]
        [Authorize(Policy = Constants.Policies.Admin)]
        public IActionResult UpdateVendorModel([FromBody] VendorModelViewModel data)
        {
            VendorModel toUpdate = UnitOfWork.VendorModels.GetOrNull(data.Id);

            if (toUpdate == null)
            {
                return BadRequest("ERROR: The asset model does not exist");
            }

            // Get files
            List<Data.DataContext.Entities.File> files = new List<Data.DataContext.Entities.File>();
            foreach(FileRef tempFile in data.Files)
            {
                Data.DataContext.Entities.File file = UnitOfWork.Files.Get(tempFile.Id);
                files.Add(file);
            }

            toUpdate.Name = data.Name;
            toUpdate.ModelFamily = data.ModelFamily;
            toUpdate.ModelType = data.ModelType;
            toUpdate.Files = files;
            UnitOfWork.VendorModels.MarkForUpdate(toUpdate, GetCurrentUser().Id);
            UnitOfWork.SaveChanges();

            var json = JsonConvert.SerializeObject(Mapper.Map<VendorModelViewModel>(toUpdate), _serializerSettings);
            return Ok(json);
        }

        [HttpGet]
        [Authorize(Policy = Constants.Policies.Customer)]
        [Route("{vendorModelId}")]
        public IActionResult GetVendorModel([FromRoute] string vendorModelId)
        {
            VendorModel vendorModel = UnitOfWork.VendorModels.GetOrNull(vendorModelId);

            if (vendorModel == null)
            {
                return BadRequest("ERROR: The vendor model does not exist");
            }

            var json = JsonConvert.SerializeObject(Mapper.Map<VendorModelViewModel>(vendorModel), _serializerSettings);

            return Ok(json);
        }
    }
}
