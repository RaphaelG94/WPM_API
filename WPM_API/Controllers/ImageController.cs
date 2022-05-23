using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using WPM_API.Azure;
using WPM_API.Code.Infrastructure;
using WPM_API.Code.Infrastructure.LogOn;
using WPM_API.Common;
using WPM_API.Data.DataContext.Entities;
using WPM_API.Models;
using WPM_API.Options;
using File = WPM_API.Data.DataContext.Entities.File;

namespace WPM_API.Controllers
{
    [Route("swImages")]
    [Authorize(Policy = Constants.Policies.Systemhouse)]
    public class ImageController : BasisController
    {
        public ImageController(AppSettings appSettings, ConnectionStrings connectionStrings, OrderEmailOptions orderEmailOptions, AgentEmailOptions agentEmailOptions, SendMailCreds sendMailCreds, SiteOptions siteOptions, ILogonManager logonManager) : base(appSettings, connectionStrings, orderEmailOptions, agentEmailOptions, sendMailCreds, siteOptions, logonManager)
        {
        }

        [HttpPost]
        public IActionResult AddImage([FromBody] ImageViewModel data)
        {
            using (var unitOfWork = CreateUnitOfWork())
            {
                Image newImage = Mapper.Map<Image>(data);
                ImageStream stream = UnitOfWork.ImageStreams.Get(data.ImageStreamId, "Images");
                newImage.Systemhouses = new List<ImagesSystemhouse>();
                newImage.Customers = new List<ImagesCustomer>();
                if (newImage.OEMPartition.Guid == "")
                {
                    newImage.OEMPartition = null;
                    newImage.OEMPartitionId = null;
                }
                if (newImage.Unattend.Guid == "")
                {
                    newImage.Unattend = null;
                    newImage.UnattendId = null;
                }
                newImage.ImageStreamId = data.ImageStreamId;
                if (data.Systemhouses.Count > 0)
                {
                    newImage.Systemhouses = new List<ImagesSystemhouse>();
                    foreach (string id in data.Systemhouses)
                    {
                        var systemhouse = unitOfWork.Systemhouses.Get(id);
                        ImagesSystemhouse temp = new ImagesSystemhouse();
                        temp.SystemhouseId = systemhouse.Id;
                        temp.ImageId = newImage.Id;
                        newImage.Systemhouses.Add(temp);
                    }

                    if (data.Customers.Count > 0)
                    {
                        foreach (string id in data.Customers)
                        {
                            var customer = unitOfWork.Customers.Get(id);
                            ImagesCustomer temp = new ImagesCustomer();
                            temp.ImageId = newImage.Id;
                            temp.CustomerId = customer.Id;
                            newImage.Customers.Add(temp);
                        }

                        /*
                        if (data.Clients.Count > 0)
                        {
                            foreach (string id in software.Clients)
                            {
                                var client = unitOfWork.Clients.Get(id);
                                SoftwaresClient temp = new SoftwaresClient();
                                temp.SoftwareId = newSoftware.Id;
                                temp.ClientId = client.Id;
                                newSoftware.Clients.Add(temp);
                            }
                        }
                        */
                    }
                }
                stream.Images.Add(newImage);
                UnitOfWork.Images.MarkForInsert(newImage, GetCurrentUser().Id);
                UnitOfWork.ImageStreams.MarkForUpdate(stream, GetCurrentUser().Id);
                UnitOfWork.SaveChanges();
                var json = JsonConvert.SerializeObject(Mapper.Map<ImageViewModel>(newImage), serializerSettings);
                return Ok(json);
            }
        }

        [HttpGet]
        [Route("imageStreamImages/{streamId}")]
        public IActionResult GetStreamsImages([FromRoute] string streamId)
        {
            List<Image> images = UnitOfWork.Images.GetAll("OEMPartition", "Unattend", "Systemhouses", "Customers")
                .Where(x => x.ImageStreamId == streamId).ToList();
            List<ImageViewModel> result = Mapper.Map<List<ImageViewModel>>(images);
            return Ok(JsonConvert.SerializeObject(result, serializerSettings));
        }

        [HttpPost]
        [Route("publish/{imageId}")]
        public IActionResult PublishInShop([FromRoute] string imageId)
        {
            using (var unitOfWork = CreateUnitOfWork())
            {
                Image image = unitOfWork.Images.Get(imageId, "Systemhouses");
                image.PublishInShop = true;
                if (image.Systemhouses == null)
                {
                    image.Systemhouses = new List<ImagesSystemhouse>();
                }
                unitOfWork.Images.MarkForUpdate(image, GetCurrentUser().Id);
                unitOfWork.SaveChanges();
                return Ok(JsonConvert.SerializeObject(image, serializerSettings));
            }
        }

        [HttpPost]
        [Route("retreat/{imageId}")]
        public IActionResult RetreatFromShop([FromRoute] string imageId)
        {
            using (var unitOfWork = CreateUnitOfWork())
            {
                Image image = unitOfWork.Images.Get(imageId, "Systemhouses");
                image.PublishInShop = false;
                if (image.Systemhouses == null)
                {
                    image.Systemhouses = new List<ImagesSystemhouse>();
                }

                unitOfWork.Images.MarkForUpdate(image, GetCurrentUser().Id);
                unitOfWork.SaveChanges();
                return Ok(JsonConvert.SerializeObject(image, serializerSettings));
            }
        }

        [HttpPost]
        [Route("addToStream/{streamId}")]
        public IActionResult AddImagesToStream([FromBody] List<ImageViewModel> images, [FromRoute] string streamId)
        {
            try
            {
                foreach (ImageViewModel image in images)
                {
                    Image temp = UnitOfWork.Images.Get(image.Id);
                    temp.ImageStreamId = streamId;
                    UnitOfWork.Images.MarkForUpdate(temp, GetCurrentUser().Id);
                    UnitOfWork.SaveChanges();
                }
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest("ERROR: " + e.Message);
            }
        }

        [HttpGet]
        public IActionResult GetImages()
        {
            List<Image> images = UnitOfWork.Images.GetAll("OEMPartition", "Unattend").ToList();
            var json = JsonConvert.SerializeObject(Mapper.Map<List<ImageViewModel>>(images), serializerSettings);
            return Ok(json);
        }

        [HttpGet]
        [Route("streamless")]
        public IActionResult GetStreamlessImagess()
        {
            List<Image> result = UnitOfWork.Images.GetAll().Where(x => x.ImageStreamId == null).ToList();

            var json = JsonConvert.SerializeObject(result, serializerSettings);
            return Ok(json);
        }

        [HttpPost]
        [Route("edit")]
        public IActionResult EditImage([FromBody] ImageViewModel data)
        {
            using (var unitOfWork = CreateUnitOfWork())
            {
                Image toEdit = UnitOfWork.Images.GetOrNull(data.Id, "Systemhouses", "Customers", "OEMPartition", "Unattend");

                if (toEdit == null)
                {
                    return BadRequest("ERROR: The OS image does not exist");
                }

                toEdit.BuildNr = data.BuildNr;
                toEdit.Name = data.Name;
                toEdit.Update = data.Update;
                toEdit.FileName = data.FileName;
                toEdit.RevisionNumber = new Guid().ToString();
                toEdit.DisplayRevisionNumber++;

                if (data.OEMPartition != null && data.OEMPartition.Guid == "")
                {
                    data.OEMPartition = null;
                    toEdit.OEMPartitionId = null;
                    toEdit.OEMPartition = null;
                }
                if (data.Unattend != null && data.Unattend.Guid == "")
                {
                    data.Unattend = null;
                    toEdit.Unattend = null;
                    toEdit.UnattendId = null;
                }
                if (data.OEMPartition != null)
                {
                    if (toEdit.OEMPartition != null)
                    {
                        toEdit.OEMPartition.Guid = data.OEMPartition.Guid;
                        toEdit.OEMPartition.Name = data.OEMPartition.Name;
                    }
                    else
                    {
                        toEdit.OEMPartition = new File() { Guid = data.OEMPartition.Guid, Name = data.OEMPartition.Name };
                    }
                }
                if (data.Unattend != null)
                {
                    if (toEdit.Unattend != null)
                    {
                        toEdit.Unattend.Guid = data.Unattend.Guid;
                        toEdit.Unattend.Name = data.Unattend.Name;
                    }
                    else
                    {
                        toEdit.Unattend = new File() { Guid = data.Unattend.Guid, Name = data.Unattend.Name };
                    }
                }
                string userId = GetCurrentUser().Id;
                foreach (ImagesSystemhouse sys in toEdit.Systemhouses)
                {
                    unitOfWork.ImagesSystemhouses.MarkForDelete(sys, userId);
                }
                foreach (ImagesCustomer cus in toEdit.Customers)
                {
                    unitOfWork.ImagesCustomers.MarkForDelete(cus, userId);
                }

                unitOfWork.Images.MarkForUpdate(toEdit, GetCurrentUser().Id);
                unitOfWork.SaveChanges();
                toEdit.Systemhouses = new List<ImagesSystemhouse>();
                toEdit.Customers = new List<ImagesCustomer>();
                if (data.Systemhouses.Count > 0)
                {
                    foreach (string id in data.Systemhouses)
                    {
                        var systemhouse = unitOfWork.Systemhouses.Get(id);
                        ImagesSystemhouse temp = new ImagesSystemhouse();
                        temp.SystemhouseId = systemhouse.Id;
                        temp.ImageId = toEdit.Id;
                        toEdit.Systemhouses.Add(temp);
                    }

                    if (data.Customers.Count > 0)
                    {
                        foreach (string id in data.Customers)
                        {
                            var customer = unitOfWork.Customers.Get(id);
                            ImagesCustomer temp = new ImagesCustomer();
                            temp.ImageId = toEdit.Id;
                            temp.CustomerId = customer.Id;
                            toEdit.Customers.Add(temp);
                        }
                    }
                }
                unitOfWork.SaveChanges();
                toEdit = UnitOfWork.Images.GetOrNull(data.Id, "Systemhouses", "Customers", "OEMPartition", "Unattend");

                var json = JsonConvert.SerializeObject(Mapper.Map<ImageViewModel>(toEdit), serializerSettings);
                return Ok(json);
            }
        }

        [HttpDelete]
        [Route("{imageId}")]
        public async Task<IActionResult> DeleteImage([FromRoute] string imageId)
        {
            try
            {
                using (var unitOfWork = CreateUnitOfWork())
                {
                    Image toDelete = UnitOfWork.Images.GetOrNull(imageId, "OEMPartition", "Unattend");
                    if (toDelete == null)
                    {
                        return BadRequest("ERROR: The image does not exist");
                    }
                    ImageStream imageStream = unitOfWork.ImageStreams.Get(toDelete.ImageStreamId);

                    // Delete each file in DB                     
                    unitOfWork.Files.MarkForDelete(toDelete.OEMPartition);
                    unitOfWork.Files.MarkForDelete(toDelete.Unattend);

                    // Connect to Azure
                    CloudStorageAccount storage = CloudStorageAccount.Parse(appSettings.LiveSystemConnectionString);
                    CloudBlobClient customerClient = storage.CreateCloudBlobClient();
                    CloudBlobContainer csdpContainer = customerClient.GetContainerReference("bsdp-v202011");

                    // Get SAS key for blob container
                    var sas = csdpContainer.GetSharedAccessSignature(new SharedAccessBlobPolicy()
                    {
                        SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(15),
                        Permissions = SharedAccessBlobPermissions.Delete | SharedAccessBlobPermissions.Write | SharedAccessBlobPermissions.Read | SharedAccessBlobPermissions.List
                    });

                    // Delete files in Azure
                    CloudBlockBlob blob;
                    // Image
                    /*
                     * blob = csdpContainer.GetBlockBlobReference("Image_Repository/" + toDelete.SubFolderName + "/" + toDelete.FileName);
                    if (await blob.ExistsAsync())
                    {
                        await blob.DeleteAsync();
                    }
                    */
                    // Settings file                    
                    blob = csdpContainer.GetBlockBlobReference("Image_Repository/" + imageStream.SubFolderName + "/" + toDelete.OEMPartition.Name);
                    if (await blob.ExistsAsync())
                    {
                        await blob.DeleteAsync();
                    }
                    blob = csdpContainer.GetBlockBlobReference("Image_Repository/" + imageStream.SubFolderName + "/" + toDelete.Unattend.Name);
                    if (await blob.ExistsAsync())
                    {
                        await blob.DeleteAsync();
                    }

                    toDelete.OEMPartitionId = null;
                    toDelete.UnattendId = null;
                    unitOfWork.Images.MarkForUpdate(toDelete, GetCurrentUser().Id);
                    unitOfWork.SaveChanges();

                    // Delete image in DB
                    unitOfWork.Images.MarkForDelete(toDelete);
                    unitOfWork.SaveChanges();

                    var json = JsonConvert.SerializeObject(Mapper.Map<ImageViewModel>(toDelete), serializerSettings);
                    return Ok(json);
                }
            }
            catch (Exception e)
            {
                return BadRequest("ERROR: " + e.Message);
            }
        }

        [HttpPost]
        [Route("oemPartition/{subfolderPath}")]
        public async Task<IActionResult> UploadOEMPartition(Microsoft.AspNetCore.Http.IFormFile file, [FromRoute] string subfolderPath)
        {
            // Connect to Bitstream Azure
            AzureCommunicationService azure = new AzureCommunicationService(appSettings.DevelopmentTenantId, appSettings.DevelopmentClientId, appSettings.DevelopmentClientSecret);
            CloudStorageAccount storage = CloudStorageAccount.Parse(appSettings.LiveSystemConnectionString);
            CloudBlobClient customerClient = storage.CreateCloudBlobClient();
            CloudBlobContainer csdpContainer = customerClient.GetContainerReference("bsdp-v202011");

            // Get SAS key for blob container
            var sas = csdpContainer.GetSharedAccessSignature(new SharedAccessBlobPolicy()
            {
                SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(15),
                Permissions = SharedAccessBlobPermissions.Write | SharedAccessBlobPermissions.Read | SharedAccessBlobPermissions.List
            });

            // Upload file
            CloudBlockBlob blob = csdpContainer.GetBlockBlobReference("Image_Repository/" + subfolderPath + "/" + file.FileName);
            blob.Properties.ContentType = file.ContentType;
            await blob.UploadFromStreamAsync(file.OpenReadStream());

            // Create response
            FileRefModel result = new FileRefModel();
            result.Guid = file.FileName;
            result.Name = file.FileName;
            var json = JsonConvert.SerializeObject(result, serializerSettings);

            return Ok(json);
        }

        [HttpPost]
        [Route("unattend/{subfolderPath}")]
        public async Task<IActionResult> UploadUnattend(Microsoft.AspNetCore.Http.IFormFile file, [FromRoute] string subfolderPath)
        {
            // Connect to Bitstream Azure
            AzureCommunicationService azure = new AzureCommunicationService(appSettings.DevelopmentTenantId, appSettings.DevelopmentClientId, appSettings.DevelopmentClientSecret);
            CloudStorageAccount storage = CloudStorageAccount.Parse(appSettings.LiveSystemConnectionString);
            CloudBlobClient customerClient = storage.CreateCloudBlobClient();
            CloudBlobContainer csdpContainer = customerClient.GetContainerReference("bsdp-v202011");

            // Get SAS key for blob container
            var sas = csdpContainer.GetSharedAccessSignature(new SharedAccessBlobPolicy()
            {
                SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(15),
                Permissions = SharedAccessBlobPermissions.Write | SharedAccessBlobPermissions.Read | SharedAccessBlobPermissions.List
            });

            // Upload file
            CloudBlockBlob blob = csdpContainer.GetBlockBlobReference("Image_Repository/" + subfolderPath + "/" + file.FileName);
            blob.Properties.ContentType = file.ContentType;
            await blob.UploadFromStreamAsync(file.OpenReadStream());

            // Create response
            FileRefModel result = new FileRefModel();
            result.Guid = file.FileName;
            result.Name = file.FileName;
            var json = JsonConvert.SerializeObject(result, serializerSettings);

            return Ok(json);
        }

        [HttpDelete]
        [Route("unattend/{subfolderPath}/{imageId}")]
        public async Task<IActionResult> DeleteUnattend([FromRoute] string subfolderPath, [FromRoute] string imageId)
        {
            using (var unitOfWork = CreateUnitOfWork())
            {
                // Connect to Bitstream Azure
                AzureCommunicationService azure = new AzureCommunicationService(appSettings.DevelopmentTenantId, appSettings.DevelopmentClientId, appSettings.DevelopmentClientSecret);
                CloudStorageAccount storage = CloudStorageAccount.Parse(appSettings.LiveSystemConnectionString);
                CloudBlobClient customerClient = storage.CreateCloudBlobClient();
                CloudBlobContainer csdpContainer = customerClient.GetContainerReference("bsdp-v202011");

                // Get SAS key for blob container
                var sas = csdpContainer.GetSharedAccessSignature(new SharedAccessBlobPolicy()
                {
                    SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(15),
                    Permissions = SharedAccessBlobPermissions.Write | SharedAccessBlobPermissions.Read | SharedAccessBlobPermissions.List
                });

                // Get File
                Image image = unitOfWork.Images.Get(imageId, "Unattend");

                // Delete file
                CloudBlockBlob blob = csdpContainer.GetBlockBlobReference("Image_Repository/" + subfolderPath + "/" + image.Unattend.Name);

                // Remove file from image
                if (await blob.ExistsAsync())
                {
                    await blob.DeleteAsync();
                }
                return Ok();
            }
        }

        [HttpDelete]
        [Route("oemPartition/{subfolderPath}/{imageId}")]
        public async Task<IActionResult> DeleteOEMPartition([FromRoute] string subfolderPath, [FromRoute] string imageId)
        {
            using (var unitOfWork = CreateUnitOfWork())
            {
                // Connect to Bitstream Azure
                AzureCommunicationService azure = new AzureCommunicationService(appSettings.DevelopmentTenantId, appSettings.DevelopmentClientId, appSettings.DevelopmentClientSecret);
                CloudStorageAccount storage = CloudStorageAccount.Parse(appSettings.LiveSystemConnectionString);
                CloudBlobClient customerClient = storage.CreateCloudBlobClient();
                CloudBlobContainer csdpContainer = customerClient.GetContainerReference("bsdp-v202011");

                // Get SAS key for blob container
                var sas = csdpContainer.GetSharedAccessSignature(new SharedAccessBlobPolicy()
                {
                    SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(15),
                    Permissions = SharedAccessBlobPermissions.Write | SharedAccessBlobPermissions.Read | SharedAccessBlobPermissions.List
                });

                // Get File
                Image image = unitOfWork.Images.Get(imageId, "OEMPartition");

                // Delete file
                CloudBlockBlob blob = csdpContainer.GetBlockBlobReference("Image_Repository/" + subfolderPath + "/" + image.OEMPartition.Name);

                // Remove file from image
                if (await blob.ExistsAsync())
                {
                    await blob.DeleteAsync();
                }
                return Ok();
            }
        }
    }


    public class ImageViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Update { get; set; }
        public string BuildNr { get; set; }
        public string FileName { get; set; }
        public FileRefModel Unattend { get; set; }
        public FileRefModel OEMPartition { get; set; }
        public string ImageStreamId { get; set; }
        public List<string> Systemhouses { get; set; }
        public List<string> Customers { get; set; }
        public bool PublishInShop { get; set; }
    }
}

