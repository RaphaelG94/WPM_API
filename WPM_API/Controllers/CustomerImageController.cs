using WPM_API.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DATA = WPM_API.Data.DataContext.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WPM_API.Azure;
using WPM_API.Data.DataContext.Entities.Storages;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using WPM_API.TransferModels;
using WPM_API.Models;
using WPM_API.Data.DataContext.Entities;
using WPM_API.Data.Models;
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace WPM_API.Controllers
{
    [Route("customerImages")]
    [Authorize(Policy = Constants.Policies.Customer)]
    public class CustomerImageController : BasisController
    {
        [HttpGet]
        [Route("{customerId}")]
        public IActionResult GetCustomersOSImages([FromRoute] string customerId)
        {
            DATA.Customer customer = UnitOfWork.Customers.GetOrNull(customerId);
            if (customer == null)
            {
                return BadRequest("ERROR: The cusotmer does not exist");
            }

            List<DATA.CustomerImage> images = UnitOfWork.CustomerImages.GetAll("Unattend", "OEMPartition").Where(x => x.CustomerId == customerId).ToList();
            List<CustomerImageViewModel> result = Mapper.Map<List<CustomerImageViewModel>>(images);

            var json = JsonConvert.SerializeObject(result, _serializerSettings);
            return Ok(json);
        }

        [HttpDelete]
        [Route("{customerId}/{imageId}")]
        public async Task<IActionResult> DeleteCustomerOSImage([FromRoute] string customerId, [FromRoute] string imageId)
        {
            try
            {
                using (var unitOfWork = CreateUnitOfWork())
                {
                    // Initialize variables
                    DATA.Customer customer;
                    DATA.CustomerImage toDelete;
                    DATA.CustomerImageStream customerImageStream;
                    DATA.CloudEntryPoint cep;
                    AzureCommunicationService azureCustomer;
                    StorageEntryPoint csdp;
                    string connectionString;


                    // Declare variables & validate them
                    toDelete = unitOfWork.CustomerImages.GetAll("OEMPartition", "Unattend").Where(x => x.CustomerId == customerId && x.Id == imageId && x.DeletedDate == null).FirstOrDefault();
                    if (toDelete == null)
                    {
                        return BadRequest("ERROR: The shoped image does not exist");
                    }
                    customerImageStream = unitOfWork.CustomerImageStreams.Get(toDelete.CustomerImageStreamId);

                    customer = unitOfWork.Customers.GetOrNull(customerId, "StorageEntryPoints");
                    if (customer == null)
                    {
                        return BadRequest("ERROR: The customer does not exist");
                    }

                    csdp = customer.StorageEntryPoints.Find(x => x.IsCSDP == true);
                    if (csdp == null)
                    {
                        return BadRequest("ERROR: The csdp does not exist");
                    }

                    if (!csdp.Managed)
                    {
                        cep = GetCEP(customerId);
                        if (cep == null)
                        {
                            return BadRequest("ERROR: The Cloud Entry Points are not set");
                        }

                        azureCustomer = new AzureCommunicationService(cep.TenantId, cep.ClientId, cep.ClientSecret);
                    } else
                    {
                        azureCustomer = new AzureCommunicationService(_appSettings.DevelopmentTenantId, _appSettings.DevelopmentClientId, _appSettings.DevelopmentClientSecret);
                    }

                    connectionString = azureCustomer.StorageService().GetStorageAccConnectionString(csdp.SubscriptionId, csdp.ResourceGrpName, csdp.StorageAccount);

                    // Delete file in csdp
                    CloudStorageAccount customerStorageAcc = CloudStorageAccount.Parse(connectionString);
                    CloudBlobClient destClient = customerStorageAcc.CreateCloudBlobClient();
                    CloudBlobContainer destContainer = destClient.GetContainerReference("csdp");
                    CloudBlockBlob destBlob = destContainer.GetBlockBlobReference("Image_Repository/" + customerImageStream.SubFolderName + "/" + toDelete.FileName);
                    await destBlob.DeleteIfExistsAsync();

                    if (toDelete.OEMPartition != null)
                    {
                        destBlob = destContainer.GetBlockBlobReference("Image_Repository/" + customerImageStream.SubFolderName + "/" + toDelete.OEMPartition.Name);
                        await destBlob.DeleteIfExistsAsync();
                    }
                    if (toDelete.Unattend != null)
                    {
                        destBlob = destContainer.GetBlockBlobReference("Image_Repository/" + customerImageStream.SubFolderName + "/" + toDelete.Unattend.Name);
                        await destBlob.DeleteIfExistsAsync();
                    }

                    // Delete CustomerImage
                    toDelete.UnattendId = null;
                    toDelete.OEMPartitionId = null;
                    toDelete.CustomerImageStreamId = null;
                    unitOfWork.CustomerImages.MarkForUpdate(toDelete, GetCurrentUser().Id);
                    unitOfWork.SaveChanges();

                    unitOfWork.CustomerImages.MarkForDelete(toDelete, GetCurrentUser().Id);
                    unitOfWork.SaveChanges();

                    var json = JsonConvert.SerializeObject(Mapper.Map<CustomerImageViewModel>(toDelete), _serializerSettings);
                    return Ok(json);
                }
            } catch (Exception e)
            {
                return BadRequest("ERROR: " + e.Message);
            }
        }

        [HttpPost]
        [Route("oemPartition/upload/{customerId}/{customerImageId}")]
        public async Task<IActionResult> UploadOEMPartition([FromRoute] string customerId, [FromRoute] string customerImageId, Microsoft.AspNetCore.Http.IFormFile file)
        {
            try
            {
                using (var unitOfWork = CreateUnitOfWork())
                {
                    // Get DB data
                    DATA.CustomerImage customerImage = unitOfWork.CustomerImages.GetOrNull(customerImageId, "OEMPartition", "Unattend");
                    DATA.CustomerImageStream customerImageStream = unitOfWork.CustomerImageStreams.Get(customerImage.CustomerImageStreamId);
                    DATA.Customer customer = unitOfWork.Customers.GetOrNull(customerId, "StorageEntryPoints");
                    DATA.CloudEntryPoint cep = GetCEP(customerId);
                    StorageEntryPoint csdp = customer.StorageEntryPoints.Find(x => x.IsCSDP == true);
                    if (customer == null)
                    {
                        return BadRequest("ERROR: The customer does not exist");
                    }
                    if (customerImage == null)
                    {
                        return BadRequest("ERROR: The CustomerImage does not exist");
                    }
                    if (csdp == null)
                    {
                        return BadRequest("ERROR: There is not Storage Entry Point set yet");
                    }

                    // Connect to Azure
                    AzureCommunicationService azureCustomer;
                    if (csdp.Managed)
                    {
                        azureCustomer = new AzureCommunicationService(_appSettings.DevelopmentTenantId, _appSettings.DevelopmentClientId, _appSettings.DevelopmentClientSecret);
                    }
                    else
                    {
                        if (cep == null)
                        {
                            return BadRequest("ERROR: The Cloud Entry Point is not set or does not exist anymore");
                        }

                        azureCustomer = new AzureCommunicationService(cep.TenantId, cep.ClientId, cep.ClientSecret);

                    }
                    string connectionString = azureCustomer.StorageService().GetStorageAccConnectionString(csdp.SubscriptionId, csdp.ResourceGrpName, csdp.StorageAccount);

                    CloudStorageAccount customerStorageAccount = CloudStorageAccount.Parse(connectionString);
                    CloudBlobClient customerClient = customerStorageAccount.CreateCloudBlobClient();
                    CloudBlobContainer csdpContainer = customerClient.GetContainerReference(csdp.BlobContainerName);

                    var sas = csdpContainer.GetSharedAccessSignature(new SharedAccessBlobPolicy()
                    {
                        SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(15),
                        Permissions = SharedAccessBlobPermissions.Write | SharedAccessBlobPermissions.Read | SharedAccessBlobPermissions.List
                    });

                    CloudBlockBlob blob;
                    // Delete old file and upload the new one
                    if (customerImage.OEMPartition != null)
                    {
                        blob = csdpContainer.GetBlockBlobReference("Image_Repository/" + customerImageStream.SubFolderName + "/" + customerImage.OEMPartition.Name);
                        await blob.DeleteAsync();
                    }
                    blob = csdpContainer.GetBlockBlobReference("Image_Repository/" + customerImageStream.SubFolderName + "/" + file.FileName);
                    await blob.UploadFromStreamAsync(file.OpenReadStream());
                    /*
                    blob = csdpContainer.GetBlockBlobReference("Image_Repository/" + customerImage.SubFolderName + "/" + customerImage.OEMLogo.Name);
                    await blob.DeleteAsync();
                    blob = csdpContainer.GetBlockBlobReference("Image_Repository/" + customerImage.SubFolderName + "/" + customerImage.OEMPartition.Name);
                    await blob.DeleteAsync();
                    blob = csdpContainer.GetBlockBlobReference("Image_Repository/" + customerImage.SubFolderName + "/" + customerImage.Unattend.Name);
                    await blob.DeleteAsync();
                    */
                    // Update DB Data
                    if (customerImage.OEMPartition != null)
                    {
                        customerImage.OEMPartition.Guid = file.FileName;
                        customerImage.OEMPartition.Name = file.FileName;
                        unitOfWork.CustomerImages.MarkForUpdate(customerImage, GetCurrentUser().Id);
                        unitOfWork.SaveChanges();
                    } else
                    {
                        customerImage.OEMPartition = new DATA.File();
                        customerImage.OEMPartition.Name = file.FileName;
                        unitOfWork.Files.MarkForInsert(customerImage.OEMPartition);
                        unitOfWork.CustomerImages.MarkForUpdate(customerImage);
                        unitOfWork.SaveChanges();
                    }
                    return Ok();
                }
            }
            catch (Exception e)
            {
                return BadRequest("ERROR: " + e.Message);
            }
        }

        [HttpGet]
        [Route("getImageStreams/{customerId}")]
        public IActionResult GetCustomerImageStreams([FromRoute] string customerId)
        {
            List<CustomerImageStream> streams = UnitOfWork.CustomerImageStreams.GetAll("Images", "Icon", "Images.OEMPartition", "Images.Unattend")
                .Where(x => x.CustomerId == customerId).ToList();

            return Ok(JsonConvert.SerializeObject(streams, _serializerSettings));
        }

        [HttpDelete]
        [Route("customerImageStream/{streamId}")]
        public async Task<IActionResult> DeleteCustomerImageStream([FromRoute] string streamId)
        {
            using (var unitOfWork = CreateUnitOfWork())
            {
                CustomerImageStream stream = unitOfWork.CustomerImageStreams.Get(streamId, "Images", "Icon");
                Customer customer = unitOfWork.Customers.GetOrNull(stream.CustomerId, CustomerIncludes.GetAllIncludes());
                var cep = GetCEP(stream.CustomerId);
                StorageEntryPoint csdp = null;
                csdp = customer.StorageEntryPoints.Find(x => x.IsCSDP == true);
                AzureCommunicationService azureCustomer;

                csdp = customer.StorageEntryPoints.Find(x => x.IsCSDP == true);
                if (csdp == null)
                {
                    return BadRequest("ERROR: The csdp does not exist");
                }

                if (!csdp.Managed)
                {
                    cep = GetCEP(stream.CustomerId);
                    if (cep == null)
                    {
                        return BadRequest("ERROR: The Cloud Entry Points are not set");
                    }

                    azureCustomer = new AzureCommunicationService(cep.TenantId, cep.ClientId, cep.ClientSecret);
                }
                else
                {
                    azureCustomer = new AzureCommunicationService(_appSettings.DevelopmentTenantId, _appSettings.DevelopmentClientId, _appSettings.DevelopmentClientSecret);
                }

                string connectionString = azureCustomer.StorageService().GetStorageAccConnectionString(csdp.SubscriptionId, csdp.ResourceGrpName, csdp.StorageAccount);

                foreach (CustomerImage toDelete in stream.Images)
                {
                    // Delete file in csdp
                    CloudStorageAccount customerStorageAcc = CloudStorageAccount.Parse(connectionString);
                    CloudBlobClient destClient = customerStorageAcc.CreateCloudBlobClient();
                    CloudBlobContainer destContainer = destClient.GetContainerReference("csdp");
                    CloudBlockBlob destBlob = destContainer.GetBlockBlobReference("Image_Repository/" + stream.SubFolderName + "/" + toDelete.FileName);
                    await destBlob.DeleteIfExistsAsync();

                    if (toDelete.OEMPartition != null)
                    {
                        destBlob = destContainer.GetBlockBlobReference("Image_Repository/" + stream.SubFolderName + "/" + toDelete.OEMPartition.Name);
                        await destBlob.DeleteIfExistsAsync();
                    }
                    if (toDelete.Unattend != null)
                    {
                        destBlob = destContainer.GetBlockBlobReference("Image_Repository/" + stream.SubFolderName + "/" + toDelete.Unattend.Name);
                        await destBlob.DeleteIfExistsAsync();
                    }

                    // Delete CustomerImage
                    toDelete.UnattendId = null;
                    toDelete.OEMPartitionId = null;
                    unitOfWork.CustomerImages.MarkForUpdate(toDelete, GetCurrentUser().Id);
                    unitOfWork.SaveChanges();

                    unitOfWork.CustomerImages.MarkForDelete(toDelete, GetCurrentUser().Id);
                    unitOfWork.SaveChanges();
                }

                // Delete CustomerImageStream
                unitOfWork.CustomerImageStreams.MarkForDelete(stream, GetCurrentUser().Id);
                unitOfWork.SaveChanges();

                var json = JsonConvert.SerializeObject(stream, _serializerSettings);
                return Ok(json);
            }
        }

        [HttpGet]
        [Route("customerImageStream/{streamId}")]
        public IActionResult GetCustomerImageStreamsImages([FromRoute] string streamId)
        {
            CustomerImageStream stream = UnitOfWork.CustomerImageStreams.Get(streamId, "Images");

            var json = JsonConvert.SerializeObject(stream.Images, _serializerSettings);
            return Ok(json);
        }

        [HttpPost]
        [Route("unattend/upload/{customerId}/{customerImageId}")]
        public async Task<IActionResult> UploadUnattendFile([FromRoute] string customerId, [FromRoute] string customerImageId, Microsoft.AspNetCore.Http.IFormFile file)
        {
            try
            {
                using (var unitOfWork = CreateUnitOfWork())
                {
                    // Get DB data
                    DATA.CustomerImage customerImage = unitOfWork.CustomerImages.GetOrNull(customerImageId, "OEMPartition", "Unattend");
                    DATA.CustomerImageStream customerImageStream = unitOfWork.CustomerImageStreams.Get(customerImage.CustomerImageStreamId);
                    DATA.Customer customer = unitOfWork.Customers.GetOrNull(customerId, "StorageEntryPoints");
                    DATA.CloudEntryPoint cep = GetCEP(customerId);
                    StorageEntryPoint csdp = customer.StorageEntryPoints.Find(x => x.IsCSDP == true);
                    if (customer == null)
                    {
                        return BadRequest("ERROR: The customer does not exist");
                    }
                    if (customerImage == null)
                    {
                        return BadRequest("ERROR: The CustomerImage does not exist");
                    }
                    if (csdp == null)
                    {
                        return BadRequest("ERROR: There is not Storage Entry Point set yet");
                    }

                    // Connect to Azure
                    AzureCommunicationService azureCustomer;
                    if (csdp.Managed)
                    {
                        azureCustomer = new AzureCommunicationService(_appSettings.DevelopmentTenantId, _appSettings.DevelopmentClientId, _appSettings.DevelopmentClientSecret);
                    }
                    else
                    {
                        if (cep == null)
                        {
                            return BadRequest("ERROR: The Cloud Entry Point is not set or does not exist anymore");
                        }

                        azureCustomer = new AzureCommunicationService(cep.TenantId, cep.ClientId, cep.ClientSecret);

                    }
                    string connectionString = azureCustomer.StorageService().GetStorageAccConnectionString(csdp.SubscriptionId, csdp.ResourceGrpName, csdp.StorageAccount);

                    CloudStorageAccount customerStorageAccount = CloudStorageAccount.Parse(connectionString);
                    CloudBlobClient customerClient = customerStorageAccount.CreateCloudBlobClient();
                    CloudBlobContainer csdpContainer = customerClient.GetContainerReference(csdp.BlobContainerName);

                    var sas = csdpContainer.GetSharedAccessSignature(new SharedAccessBlobPolicy()
                    {
                        SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(15),
                        Permissions = SharedAccessBlobPermissions.Write | SharedAccessBlobPermissions.Read | SharedAccessBlobPermissions.List
                    });

                    // Delete old file and upload the new one
                    CloudBlockBlob blob;
                    if (customerImage.Unattend != null) {
                        blob = csdpContainer.GetBlockBlobReference("Image_Repository/" + customerImageStream.SubFolderName + "/" + customerImage.Unattend.Name);
                        if (await blob.ExistsAsync())
                        {
                            await blob.DeleteAsync();
                        }
                    }
                    blob = csdpContainer.GetBlockBlobReference("Image_Repository/" + customerImageStream.SubFolderName + "/" + file.FileName);
                    await blob.UploadFromStreamAsync(file.OpenReadStream());
                    /*
                    blob = csdpContainer.GetBlockBlobReference("Image_Repository/" + customerImage.SubFolderName + "/" + customerImage.OEMLogo.Name);
                    await blob.DeleteAsync();
                    blob = csdpContainer.GetBlockBlobReference("Image_Repository/" + customerImage.SubFolderName + "/" + customerImage.OEMPartition.Name);
                    await blob.DeleteAsync();
                    blob = csdpContainer.GetBlockBlobReference("Image_Repository/" + customerImage.SubFolderName + "/" + customerImage.Unattend.Name);
                    await blob.DeleteAsync();
                    */
                    // Update DB Data
                    if (customerImage.Unattend != null)
                    {
                        customerImage.Unattend.Guid = file.FileName;
                        customerImage.Unattend.Name = file.FileName;
                        unitOfWork.CustomerImages.MarkForUpdate(customerImage, GetCurrentUser().Id);
                        unitOfWork.SaveChanges();
                    } else
                    {
                        customerImage.Unattend = new DATA.File();
                        customerImage.Unattend.Name = file.FileName;
                        unitOfWork.Files.MarkForInsert(customerImage.Unattend);
                        unitOfWork.CustomerImages.MarkForUpdate(customerImage);
                        unitOfWork.SaveChanges();
                    }
                    return Ok();
                }
            }
            catch (Exception e)
            {
                return BadRequest("ERROR: " + e.Message);
            }
        }

        [HttpDelete]
        [Route("deleteOEMPartition/{customerId}/{imageId}")]
        public async Task<IActionResult> DeleteOEMPartitionFile([FromRoute] string imageId, [FromRoute] string customerId)
        {
            try
            {
                using (var unitOfWork = CreateUnitOfWork())
                {
                    DATA.CustomerImage image = unitOfWork.CustomerImages.GetOrNull(imageId, "OEMPartition");
                    if (image == null)
                    {
                        return BadRequest("ERROR: The customer image does not exist");
                    }
                    DATA.CustomerImageStream imageStream = unitOfWork.CustomerImageStreams.Get(image.CustomerImageStreamId);

                    // Connect to Azure
                    DATA.Customer customer = unitOfWork.Customers.GetOrNull(customerId, "StorageEntryPoints");
                    DATA.CloudEntryPoint cep = GetCEP(customerId);
                    StorageEntryPoint csdp = customer.StorageEntryPoints.Find(x => x.IsCSDP == true);
                    if (customer == null)
                    {
                        return BadRequest("ERROR: The customer does not exist");
                    }
                    if (csdp == null)
                    {
                        return BadRequest("ERROR: There is not Storage Entry Point set yet");
                    }

                    // Connect to Azure
                    AzureCommunicationService azureCustomer;
                    if (csdp.Managed)
                    {
                        azureCustomer = new AzureCommunicationService(_appSettings.DevelopmentTenantId, _appSettings.DevelopmentClientId, _appSettings.DevelopmentClientSecret);
                    }
                    else
                    {
                        if (cep == null)
                        {
                            return BadRequest("ERROR: The Cloud Entry Point is not set or does not exist anymore");
                        }

                        azureCustomer = new AzureCommunicationService(cep.TenantId, cep.ClientId, cep.ClientSecret);

                    }
                    string connectionString = azureCustomer.StorageService().GetStorageAccConnectionString(csdp.SubscriptionId, csdp.ResourceGrpName, csdp.StorageAccount);

                    CloudStorageAccount customerStorageAccount = CloudStorageAccount.Parse(connectionString);
                    CloudBlobClient customerClient = customerStorageAccount.CreateCloudBlobClient();
                    CloudBlobContainer csdpContainer = customerClient.GetContainerReference(csdp.BlobContainerName);
                    CloudBlockBlob blob;
                    if (image.OEMPartition != null)
                    {
                        blob = csdpContainer.GetBlockBlobReference("Image_Repository/" + imageStream.SubFolderName + "/" + image.OEMPartition.Name);
                        if (await blob.ExistsAsync())
                        {
                            await blob.DeleteAsync();
                        }
                    }

                    // Delete File from Database
                    string fileIdToDelete = image.OEMPartitionId;
                    image.OEMPartition = null;
                    image.OEMPartitionId = null;
                    unitOfWork.CustomerImages.MarkForUpdate(image, GetCurrentUser().Id);
                    unitOfWork.SaveChanges();
                    DATA.File toDelete = unitOfWork.Files.Get(fileIdToDelete);
                    unitOfWork.Files.MarkForDelete(toDelete);
                    unitOfWork.SaveChanges();

                    return Ok();
                }
            }
            catch (Exception e)
            {
                return BadRequest("ERROR: " + e.Message);
            }
        }

        [HttpDelete]
        [Route("deleteUnattend/{customerId}/{imageId}")]
        public async Task<IActionResult> DeleteUnattendFile([FromRoute] string imageId, [FromRoute] string customerId)
        {
            try
            {
                using (var unitOfWork = CreateUnitOfWork())
                {
                    DATA.CustomerImage image = unitOfWork.CustomerImages.GetOrNull(imageId, "Unattend");
                    if (image == null)
                    {
                        return BadRequest("ERROR: The customer image does not exist");
                    }
                    CustomerImageStream imageStream = unitOfWork.CustomerImageStreams.Get(image.CustomerImageStreamId);

                    // Connect to Azure
                    DATA.Customer customer = unitOfWork.Customers.GetOrNull(customerId, "StorageEntryPoints");
                    DATA.CloudEntryPoint cep = GetCEP(customerId);
                    StorageEntryPoint csdp = customer.StorageEntryPoints.Find(x => x.IsCSDP == true);
                    if (customer == null)
                    {
                        return BadRequest("ERROR: The customer does not exist");
                    }
                    if (csdp == null)
                    {
                        return BadRequest("ERROR: There is not Storage Entry Point set yet");
                    }

                    // Connect to Azure
                    AzureCommunicationService azureCustomer;
                    if (csdp.Managed)
                    {
                        azureCustomer = new AzureCommunicationService(_appSettings.DevelopmentTenantId, _appSettings.DevelopmentClientId, _appSettings.DevelopmentClientSecret);
                    }
                    else
                    {
                        if (cep == null)
                        {
                            return BadRequest("ERROR: The Cloud Entry Point is not set or does not exist anymore");
                        }

                        azureCustomer = new AzureCommunicationService(cep.TenantId, cep.ClientId, cep.ClientSecret);

                    }
                    string connectionString = azureCustomer.StorageService().GetStorageAccConnectionString(csdp.SubscriptionId, csdp.ResourceGrpName, csdp.StorageAccount);

                    CloudStorageAccount customerStorageAccount = CloudStorageAccount.Parse(connectionString);
                    CloudBlobClient customerClient = customerStorageAccount.CreateCloudBlobClient();
                    CloudBlobContainer csdpContainer = customerClient.GetContainerReference(csdp.BlobContainerName);
                    CloudBlockBlob blob;
                    if (image.Unattend != null)
                    {
                        blob = csdpContainer.GetBlockBlobReference("Image_Repository/" + imageStream.SubFolderName + "/" + image.Unattend.Name);
                        if (await blob.ExistsAsync())
                        {
                            await blob.DeleteAsync();
                        }
                    }

                    // Delete File from Database
                    string fileIdToDelete = image.UnattendId;
                    image.UnattendId = null;
                    image.Unattend = null;
                    unitOfWork.CustomerImages.MarkForUpdate(image, GetCurrentUser().Id);
                    unitOfWork.SaveChanges();
                    DATA.File toDelete = unitOfWork.Files.Get(fileIdToDelete);
                    unitOfWork.Files.MarkForDelete(toDelete);
                    unitOfWork.SaveChanges();

                    return Ok();
                }
            }
            catch (Exception e)
            {
                return BadRequest("ERROR: " + e.Message);
            }
        }

        [HttpPost]
        [Route("setOSSettings/{streamId}")]
        public IActionResult SetStreamSettings([FromBody] OSStreamSettings data, [FromRoute] string streamId)
        {
            using (var unitOfWork = CreateUnitOfWork())
            {
                CustomerImageStream imageStream = unitOfWork.CustomerImageStreams.Get(streamId);
                imageStream.ProductKey = data.ProductKey;
                imageStream.LocalSettingLinux = data.LocalSettingLinux;

                unitOfWork.CustomerImageStreams.MarkForUpdate(imageStream, GetCurrentUser().Id);
                unitOfWork.SaveChanges();

                var json = JsonConvert.SerializeObject(imageStream, _serializerSettings);

                return Ok(json);
            }
        }

        [HttpGet]
        [Route("{customerId}/checkRevisionNumbers")]
        public IActionResult GetRevisionImages ([FromRoute] string customerId)
        {
            List<Image> result = new List<Image>();
            // Get all software streams 
            using (var unitOfWork = CreateUnitOfWork())
            {
                Customer customer = unitOfWork.Customers.GetOrNull(customerId);
                if (customer == null)
                {
                    return BadRequest("ERROR: The customer does not exist");
                }
                List<CustomerImageStream> customerStreams = unitOfWork.CustomerImageStreams.GetAll("Images")
                        .Where(x => x.CustomerId == customerId).ToList();
                foreach (CustomerImageStream stream in customerStreams)
                {
                    foreach (CustomerImage cimg in stream.Images)
                    {
                        Image image = unitOfWork.Images.GetOrNull(cimg.ImageId);
                        if (image != null)
                        {
                            string guid = Guid.NewGuid().ToString();
                            if (image.RevisionNumber == null)
                            {
                                image.RevisionNumber = guid;
                                cimg.RevisionNumber = guid;
                                unitOfWork.Images.MarkForUpdate(image, GetCurrentUser().Id);
                                unitOfWork.CustomerImages.MarkForUpdate(cimg, GetCurrentUser().Id);
                                unitOfWork.SaveChanges();
                            }
                            else
                            {
                                if (cimg.RevisionNumber != image.RevisionNumber)
                                {
                                    result.Add(image);
                                }
                            }
                        }
                    }
                }
            }
            var json = JsonConvert.SerializeObject(result, _serializerSettings);
            return Ok(json);
        }

        /*
        [HttpGet]
        [Route("newSoftware")]
        public IActionResult GetNewSoftware([FromRoute] string customerId)
        {
            try
            {
                List<Image> result = new List<Image>();
                // Get all software streams 
                using (var unitOfWork = CreateUnitOfWork())
                {
                    Customer customer = unitOfWork.Customers.GetOrNull(customerId);
                    if (customer == null)
                    {
                        return BadRequest("ERROR: The customer does not exist");
                    }
                    List<CustomerImageStream> customerStreams = unitOfWork.CustomerImageStreams.GetAll("Images")
                            .Where(x => x.CustomerId == customerId).ToList();

                    // Get latest version of customer used software package
                    string latestCustomerVersion;
                    foreach (CustomerImageStream stream in customerStreams)
                    {
                        latestCustomerVersion = String.Empty;
                        foreach (CustomerImage image in stream.Images)
                        {
                            if (latestCustomerVersion == String.Empty)
                            {
                                latestCustomerVersion = image.BuildNr;
                            }
                            else
                            {
                                if (IsLaterVersion(latestCustomerVersion, swpackage.Version))
                                {
                                    latestCustomerVersion = swpackage.Version;
                                }
                            }
                        }
                        // Compare to admin stream latest version
                        SoftwareStream adminStream = unitOfWork.SoftwareStreams.GetOrNull(stream.SoftwareStreamId, "StreamMembers", "StreamMembers.Customers");
                        if (adminStream != null)
                        {
                            string latestAdminSWVersion = String.Empty;
                            foreach (Software software in adminStream.StreamMembers)
                            {
                                bool skip = false;
                                if (software.Customers != null && software.Customers.Count != 0)
                                {
                                    if (software.Customers.Find(x => x.CustomerId == customerId) == null)
                                    {
                                        skip = true;
                                    }
                                }
                                if (!skip)
                                {
                                    if (software.PublishInShop)
                                    {
                                        if (software.DeletedDate == null)
                                        {
                                            if (latestAdminSWVersion == String.Empty)
                                            {
                                                latestAdminSWVersion = software.Version;
                                            }
                                            else
                                            {
                                                if (IsLaterVersion(latestAdminSWVersion, software.Version))
                                                {
                                                    latestAdminSWVersion = software.Version;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            if (IsLaterVersion(latestCustomerVersion, latestAdminSWVersion))
                            {
                                result.Add(adminStream.StreamMembers.Find(x => x.Version == latestAdminSWVersion));
                            }
                        }
                    }
                    // Return result
                    foreach (Software sw in result)
                    {
                        sw.Customers = null;
                    }
                    var json = JsonConvert.SerializeObject(result, _serializerSettings);
                    return Ok(json);
                }
            }
            catch (Exception e)
            {
                return BadRequest("ERROR: " + e.Message);
            }
        }
        */

        [HttpPost]
        [Route("repairImage/{customerId}/{imageId}")]
        public async Task<IActionResult> RepairCustomerImage ([FromRoute] string imageId, [FromRoute] string customerId) 
        {
            try
            {
                using (var unitOfWork = CreateUnitOfWork())
                {
                    Customer customer = unitOfWork.Customers.GetOrNull(customerId, CustomerIncludes.GetAllIncludes());
                    var cep = GetCEP(customerId);
                    Image image = unitOfWork.Images.GetOrNull(imageId);
                    CustomerImageStream customerImageStream = unitOfWork.CustomerImageStreams.GetAll("Images")
                        .Where(x => x.ImageStreamId == image.ImageStreamId && x.CustomerId == customer.Id).FirstOrDefault();
                    if (customerImageStream == null)
                    {
                        return BadRequest("ERROR: The customer stream does not exist!");
                    }
                    CustomerImage customerImage = unitOfWork.CustomerImages.GetAll()
                        .Where(x => x.ImageId == imageId && x.CustomerImageStreamId == customerImageStream.Id)
                        .FirstOrDefault();
                    StorageEntryPoint csdp = null;
                    if (customer == null)
                    {
                        return BadRequest("ERROR: The customer does not exist");
                    }
                    csdp = customer.StorageEntryPoints.Find(x => x.IsCSDP == true);
                    if (cep == null && !csdp.Managed)
                    {
                        return BadRequest("ERROR: The csdp Cloud Entry Point is not set");
                    }
                    if (image == null)
                    {
                        return BadRequest("ERROR: The image does not exist");
                    }
                    if (customerImage == null)
                    {
                        return BadRequest("ERROD: The customer image does not exist");
                    }
                    if (csdp == null)
                    {
                        return BadRequest("ERROR: The csdp storage entry point does not exist or has not been created yet");
                    }

                    // Copy files to csdp
                    CloudStorageAccount srcStrgAcc = CloudStorageAccount.Parse(_appSettings.FileDestConnectionString);
                    CloudBlobClient csdpClient = srcStrgAcc.CreateCloudBlobClient();
                    CloudBlobContainer csdpBitstream = csdpClient.GetContainerReference("bsdp-v202011");

                    AzureCommunicationService azureCustomer;
                    if (!csdp.Managed)
                    {
                        azureCustomer = new AzureCommunicationService(cep.TenantId, cep.ClientId, cep.ClientSecret);
                    }
                    else
                    {
                        // TODO: Check system; fix for live system
                        azureCustomer = new AzureCommunicationService(_appSettings.DevelopmentTenantId, _appSettings.DevelopmentClientId, _appSettings.DevelopmentClientSecret);
                    }
                    string connectionString = azureCustomer.StorageService().GetStorageAccConnectionString(csdp.SubscriptionId, csdp.ResourceGrpName, csdp.StorageAccount);
                    CloudStorageAccount customerStorageAcc = CloudStorageAccount.Parse(connectionString);
                    CloudBlobClient customerClient = customerStorageAcc.CreateCloudBlobClient();
                    CloudBlobContainer customerContainer = customerClient.GetContainerReference("csdp");
                    CloudBlockBlob customerDestBlob;
                    ImageStream imageStream = unitOfWork.ImageStreams.Get(image.ImageStreamId);
                    if (imageStream.Type == "Windows")
                    {
                        //  HELPER.Helper helper = new HELPER.Helper();
                        // helper.TransferUrlToAzureBlob(imageStream.PrefixUrl + "Image_Repository/" + imageStream.SubFolderName + "/" + image.FileName + imageStream.SASKey, connectionString, "csdp", "Image_Repository/" + imageStream.SubFolderName + "/" + image.FileName);
                        // Setup reference to customer csdp & copy blob                        
                        customerDestBlob = customerContainer.GetBlockBlobReference("Image_Repository/" + imageStream.SubFolderName + "/" + image.FileName);
                        var resultTemp = await customerDestBlob.StartCopyAsync(new Uri(imageStream.PrefixUrl + "Image_Repository/" + imageStream.SubFolderName + "/" + image.FileName + imageStream.SASKey));
                    }
                    else if (customerImageStream.Type == "Linux")
                    {
                        // Copy all files of imageStream subfolder path

                        // string subfolder = imageStream.PrefixUrl +  + imageStream.SASKey;
                        CloudBlobDirectory linuxImageDirectory = csdpBitstream.GetDirectoryReference("Image_Repository/" + customerImageStream.SubFolderName);
                        var linuxImgFiles = await linuxImageDirectory.ListBlobsSegmentedAsync(null);
                        await CopyFiles(linuxImgFiles, linuxImageDirectory, customerContainer, csdpBitstream, srcStrgAcc, "bsdp-v202011");
                    }

                    // Copy standard files from CSDP of BitStream

                    CloudBlobDirectory customerRepository = csdpBitstream.GetDirectoryReference("Customer_Repository");
                    CloudBlobDirectory softwareRepository = csdpBitstream.GetDirectoryReference("Software_Repository");
                    var customerRepFileList = await customerRepository.ListBlobsSegmentedAsync(null);
                    var softwareRepFileList = await softwareRepository.ListBlobsSegmentedAsync(null);
                    await CopyFiles(customerRepFileList, customerRepository, customerContainer, csdpBitstream, srcStrgAcc, "bsdp-v202011");
                    await CopyFiles(softwareRepFileList, customerRepository, customerContainer, csdpBitstream, srcStrgAcc, "bsdp-v202011");

                    var sasKey = csdpBitstream.GetSharedAccessSignature(new SharedAccessBlobPolicy()
                    {
                        SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(15),
                        Permissions = SharedAccessBlobPermissions.Read | SharedAccessBlobPermissions.List
                    });

                    // Copy all settings files                                        
                    if (image.OEMPartition != null)
                    {
                        // Reference to csdp blob
                        customerDestBlob = customerContainer.GetBlockBlobReference("Image_Repository/" + customerImageStream.SubFolderName + "/" + image.OEMPartition.Name);

                        // Get blob from bsdp & copy
                        CloudBlockBlob toCopy = csdpBitstream.GetBlockBlobReference("Image_Repository/" + customerImageStream.SubFolderName + "/" + image.OEMPartition.Name);
                        string blobPathUri = toCopy.Name.Replace(" ", "%20");
                        string uriString = srcStrgAcc.BlobStorageUri.PrimaryUri.AbsoluteUri + "bsdp-v202011" + "/" + blobPathUri + sasKey;
                        var uri = new Uri(uriString);
                        await customerDestBlob.StartCopyAsync(uri);
                    }

                    if (image.Unattend != null)
                    {
                        // Reference to csdp blob
                        customerDestBlob = customerContainer.GetBlockBlobReference("Image_Repository/" + customerImageStream.SubFolderName + "/" + image.Unattend.Name);

                        // Get blob from bsdp & copy
                        CloudBlockBlob toCopy = csdpBitstream.GetBlockBlobReference("Image_Repository/" + customerImageStream.SubFolderName + "/" + image.Unattend.Name);
                        string blobPathUri = toCopy.Name.Replace(" ", "%20");
                        string uriString = srcStrgAcc.BlobStorageUri.PrimaryUri.AbsoluteUri + "bsdp-v202011" + "/" + blobPathUri + sasKey;
                        var uri = new Uri(uriString);
                        await customerDestBlob.StartCopyAsync(uri);
                    }
                    customerImage.RevisionNumber = image.RevisionNumber;
                    customerImage.DisplayRevisionNumber = image.DisplayRevisionNumber;
                    customerImage.FileName = image.FileName;
                    unitOfWork.CustomerImages.MarkForUpdate(customerImage, GetCurrentUser().Id);
                    unitOfWork.SaveChanges();

                    ImageViewModel result = Mapper.Map<ImageViewModel>(image);
                    var json = JsonConvert.SerializeObject(result, _serializerSettings);
                    return Ok(json);
                }                
            } catch (Exception e)
            {
                return BadRequest("ERROR: " + e.Message);
            }
        }

        [HttpGet]
        [Route("{customerId}/newImages")]
        public IActionResult GetNewImages ([FromRoute] string customerId)
        {
            try
            {
                List<Image> result = new List<Image>();
                using (var unitOfWork = CreateUnitOfWork())
                {
                    Customer customer = unitOfWork.Customers.GetOrNull(customerId);
                    if (customer == null)
                    {
                        return BadRequest("ERROR: The customer does not exist");
                    }

                    List<CustomerImageStream> customerImageStreams = unitOfWork.CustomerImageStreams.GetAll("Images")
                        .Where(x => x.CustomerId == customerId).ToList();

                    // Get latest BuildNr of customer image package for each stream
                    string latestCustomerVersion;
                    foreach (CustomerImageStream stream in customerImageStreams)
                    {
                        latestCustomerVersion = String.Empty;
                        foreach (CustomerImage image in stream.Images)
                        {
                            if (latestCustomerVersion == String.Empty)
                            {
                                latestCustomerVersion = image.BuildNr;
                            }
                            else
                            {
                                if (IsLaterVersion(latestCustomerVersion, image.BuildNr))
                                {
                                    latestCustomerVersion = image.BuildNr;
                                }
                            }
                        }

                        // Check if related admin stream has later version
                        ImageStream adminStream = unitOfWork.ImageStreams.GetOrNull(stream.ImageStreamId, "Images");
                        if (adminStream != null)
                        {
                            string latestAdminImageVersion = String.Empty;
                            foreach (Image image in adminStream.Images)
                            {
                                if (unitOfWork.CustomerImages.GetAll().Where(x => x.CustomerId == customerId && x.ImageId == image.Id).FirstOrDefault() == null)
                                {
                                    if (image.PublishInShop && image.DeletedDate == null)
                                    {
                                        if (latestAdminImageVersion == String.Empty)
                                        {
                                            latestAdminImageVersion = image.BuildNr;
                                        }
                                        else
                                        {
                                            if (IsLaterVersion(latestAdminImageVersion, image.BuildNr))
                                            {
                                                latestAdminImageVersion = image.BuildNr;
                                            }
                                        }
                                    }
                                }
                            }
                            if (latestAdminImageVersion != String.Empty && latestCustomerVersion != String.Empty)
                            {
                                if (IsLaterVersion(latestCustomerVersion, latestAdminImageVersion))
                                {
                                    result.Add(adminStream.Images.Find(x => x.BuildNr == latestAdminImageVersion));
                                }
                            }                            
                        }
                    }

                    // Return result
                    var json = JsonConvert.SerializeObject(result, _serializerSettings);
                    return Ok(json);
                }
            } catch (Exception e)
            {
                return BadRequest("ERROR: " + e.Message);
            }
        }
        
        [HttpPost]
        [Route("{customerId}/shopNewImage/{imageId}")]
        public async Task<IActionResult> ShopNewImage ([FromRoute] string customerId, [FromRoute] string imageId)
        {
            try
            {
                using (var unitOfWork = CreateUnitOfWork())
                {
                    Customer customer = unitOfWork.Customers.GetOrNull(customerId, CustomerIncludes.GetAllIncludes());
                    var cep = GetCEP(customerId);

                    Image image = unitOfWork.Images.GetOrNull(imageId, "Unattend", "OEMPartition");
                    StorageEntryPoint csdp = null;
                    if (customer == null)
                    {
                        return BadRequest("ERROR: The customer does not exist");
                    }
                    csdp = customer.StorageEntryPoints.Find(x => x.IsCSDP == true);
                    if (cep == null && csdp != null && !csdp.Managed)
                    {
                        return BadRequest("ERROR: The csdp storage entry point does not exist or has not been created yet");
                    }
                    if (csdp == null)
                    {
                        return BadRequest("ERROR: The csdp storage entry point does not exist or has not been created yet");
                    }

                    if (image == null)
                    {
                        return BadRequest("ERROR: The os image does not exist");
                    }
                    ImageStream imageStream = unitOfWork.ImageStreams.Get(image.ImageStreamId);

                    CloudStorageAccount srcStrgAcc = CloudStorageAccount.Parse(_appSettings.FileDestConnectionString);
                    CloudBlobClient csdpClient = srcStrgAcc.CreateCloudBlobClient();
                    CloudBlobContainer csdpBitstream = csdpClient.GetContainerReference("bsdp-v202011");

                    AzureCommunicationService azureCustomer;
                    if (!csdp.Managed)
                    {
                        azureCustomer = new AzureCommunicationService(cep.TenantId, cep.ClientId, cep.ClientSecret);
                    }
                    else
                    {
                        // TODO: Check system; fix for live system
                        azureCustomer = new AzureCommunicationService(_appSettings.DevelopmentTenantId, _appSettings.DevelopmentClientId, _appSettings.DevelopmentClientSecret);
                    }
                    string connectionString = azureCustomer.StorageService().GetStorageAccConnectionString(csdp.SubscriptionId, csdp.ResourceGrpName, csdp.StorageAccount);
                    CloudStorageAccount customerStorageAcc = CloudStorageAccount.Parse(connectionString);
                    CloudBlobClient customerClient = customerStorageAcc.CreateCloudBlobClient();
                    CloudBlobContainer customerContainer = customerClient.GetContainerReference("csdp");
                    CloudBlockBlob customerDestBlob;
                    if (imageStream.Type == "Windows")
                    {
                        //  HELPER.Helper helper = new HELPER.Helper();
                        // helper.TransferUrlToAzureBlob(imageStream.PrefixUrl + "Image_Repository/" + imageStream.SubFolderName + "/" + image.FileName + imageStream.SASKey, connectionString, "csdp", "Image_Repository/" + imageStream.SubFolderName + "/" + image.FileName);
                        // Setup reference to customer csdp & copy blob                        
                        customerDestBlob = customerContainer.GetBlockBlobReference("Image_Repository/" + imageStream.SubFolderName + "/" + image.FileName);
                        var result = await customerDestBlob.StartCopyAsync(new Uri(imageStream.PrefixUrl + "Image_Repository/" + imageStream.SubFolderName + "/" + image.FileName + imageStream.SASKey));
                    }
                    else if (imageStream.Type == "Linux")
                    {
                        // Copy all files of imageStream subfolder path

                        // string subfolder = imageStream.PrefixUrl +  + imageStream.SASKey;
                        CloudBlobDirectory linuxImageDirectory = csdpBitstream.GetDirectoryReference("Image_Repository/" + imageStream.SubFolderName);
                        var linuxImgFiles = await linuxImageDirectory.ListBlobsSegmentedAsync(null);
                        await CopyFiles(linuxImgFiles, linuxImageDirectory, customerContainer, csdpBitstream, srcStrgAcc, "bsdp-v202011");
                    }

                    // Copy standard files from CSDP of BitStream

                    CloudBlobDirectory customerRepository = csdpBitstream.GetDirectoryReference("Customer_Repository");
                    CloudBlobDirectory softwareRepository = csdpBitstream.GetDirectoryReference("Software_Repository");
                    var customerRepFileList = await customerRepository.ListBlobsSegmentedAsync(null);
                    var softwareRepFileList = await softwareRepository.ListBlobsSegmentedAsync(null);
                    await CopyFiles(customerRepFileList, customerRepository, customerContainer, csdpBitstream, srcStrgAcc, "bsdp-v202011");
                    await CopyFiles(softwareRepFileList, customerRepository, customerContainer, csdpBitstream, srcStrgAcc, "bsdp-v202011");

                    var sasKey = csdpBitstream.GetSharedAccessSignature(new SharedAccessBlobPolicy()
                    {
                        SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(15),
                        Permissions = SharedAccessBlobPermissions.Read | SharedAccessBlobPermissions.List
                    });

                    // Copy all settings files                                        
                    if (image.OEMPartition != null)
                    {
                        // Reference to csdp blob
                        customerDestBlob = customerContainer.GetBlockBlobReference("Image_Repository/" + imageStream.SubFolderName + "/" + image.OEMPartition.Name);

                        // Get blob from bsdp & copy
                        CloudBlockBlob toCopy = csdpBitstream.GetBlockBlobReference("Image_Repository/" + imageStream.SubFolderName + "/" + image.OEMPartition.Name);
                        string blobPathUri = toCopy.Name.Replace(" ", "%20");
                        string uriString = srcStrgAcc.BlobStorageUri.PrimaryUri.AbsoluteUri + "bsdp-v202011" + "/" + blobPathUri + sasKey;
                        var uri = new Uri(uriString);
                        await customerDestBlob.StartCopyAsync(uri);
                    }

                    if (image.Unattend != null)
                    {
                        // Reference to csdp blob
                        customerDestBlob = customerContainer.GetBlockBlobReference("Image_Repository/" + imageStream.SubFolderName + "/" + image.Unattend.Name);

                        // Get blob from bsdp & copy
                        CloudBlockBlob toCopy = csdpBitstream.GetBlockBlobReference("Image_Repository/" + imageStream.SubFolderName + "/" + image.Unattend.Name);
                        string blobPathUri = toCopy.Name.Replace(" ", "%20");
                        string uriString = srcStrgAcc.BlobStorageUri.PrimaryUri.AbsoluteUri + "bsdp-v202011" + "/" + blobPathUri + sasKey;
                        var uri = new Uri(uriString);
                        await customerDestBlob.StartCopyAsync(uri);
                    }

                    // Check Customer Image Stream
                    CustomerImageStream customerImageStream = null;
                    if (customer.CustomerImageStreams == null)
                    {
                        customer.CustomerImageStreams = new List<CustomerImageStream>();
                    }
                    ImageStream stream = unitOfWork.ImageStreams.GetOrNull(image.ImageStreamId, "Icon");
                    customerImageStream = customer.CustomerImageStreams.Find(x => x.ImageStreamId == stream.Id && x.DeletedDate == null);
                    // Create new customer image
                    CustomerImage newImage = Mapper.Map<CustomerImage>(image);
                    newImage.Id = null;
                    newImage.CustomerId = customerId;
                    newImage.ImageId = image.Id;
                    if (customerImageStream != null)
                    {
                        customerImageStream = unitOfWork.CustomerImageStreams.GetOrNull(customerImageStream.Id, "Images");
                        // Add CustomerSoftware
                        if (customerImageStream.Images == null)
                        {
                            customerImageStream.Images = new List<CustomerImage>();
                        }
                        newImage.CustomerImageStreamId = customerImageStream.Id;
                        customerImageStream.Images.Add(newImage);
                        unitOfWork.CustomerImageStreams.MarkForUpdate(customerImageStream, GetCurrentUser().Id);
                        unitOfWork.CustomerImages.MarkForInsert(newImage, GetCurrentUser().Id);
                    }
                    else
                    {
                        // Create new CustomerImageStream and add CustomerImage
                        imageStream = unitOfWork.ImageStreams.GetOrNull(image.ImageStreamId);
                        if (imageStream == null)
                        {
                            return BadRequest("ERROR: The image stream does not exist");
                        }
                        customerImageStream = Mapper.Map<CustomerImageStream>(imageStream);
                        customerImageStream.ImageStreamId = imageStream.Id;
                        customerImageStream.Id = null;
                        customerImageStream.CustomerId = customerId;
                        customerImageStream.Images = new List<CustomerImage>();
                        customerImageStream.Type = imageStream.Type;
                        customerImageStream.Images.Add(newImage);
                        unitOfWork.CustomerImageStreams.MarkForInsert(customerImageStream, GetCurrentUser().Id);
                        unitOfWork.CustomerImages.MarkForInsert(newImage, GetCurrentUser().Id);
                    }

                    unitOfWork.Customers.MarkForUpdate(customer, GetCurrentUser().Id);
                    unitOfWork.SaveChanges();
                    newImage.CustomerImageStreamId = customerImageStream.Id;
                    unitOfWork.SaveChanges();

                    // Add customer paramter if not existing ($RegisteredOwnerNewName & $RegisteredOrganizationNewName)
                    if (customer.Parameters.Find(x => x.Key == "$RegisteredOwnerNewName") == null)
                    {
                        customer.Parameters.Add(new Parameter { Key = "$RegisteredOwnerNewName", Value = customer.Name, IsEditable = true });
                    }
                    if (customer.Parameters.Find(x => x.Key == "$RegisteredOrganizationNewName") == null)
                    {
                        customer.Parameters.Add(new Parameter { Key = "$RegisteredOrganizationNewName", Value = customer.Name, IsEditable = true });
                    }
                    unitOfWork.Customers.MarkForUpdate(customer, GetCurrentUser().Id);
                    unitOfWork.SaveChanges();

                    var json = JsonConvert.SerializeObject(image, _serializerSettings);
                    return Ok(json);
                }
            }
            catch (Exception e)
            {
                string innerExceptionMessage = "";
                if (e.InnerException != null)
                {
                    innerExceptionMessage = e.InnerException.Message;
                }
                return BadRequest("ERROR: " + e.Message + "\nInnerException: " + innerExceptionMessage);
            }            
        }           

        private bool IsLaterVersion(string currentVersion, string toCompare)
        {
            System.Version currentPackageVersion = System.Version.Parse(currentVersion);
            System.Version toCompareVersion = System.Version.Parse(toCompare);
            int compareValue = toCompareVersion.CompareTo(currentPackageVersion);
            if (compareValue == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

            private async System.Threading.Tasks.Task CopyFiles(BlobResultSegment fileList, CloudBlobDirectory srcDirectory, CloudBlobContainer destContainer, CloudBlobContainer srcContainer, CloudStorageAccount srcStrgAcc, string containerName)
        {
            for (int i = 0; i < fileList.Results.Count(); i++)
            {
                var element = fileList.Results.ElementAt(i);
                if (element.GetType().Name == "CloudBlockBlob")
                {
                    CloudBlockBlob blob = (CloudBlockBlob)element;
                    await CopyStandardFilesToCSDP(blob, destContainer, srcContainer, srcStrgAcc, containerName);
                }
                else if (element.GetType().Name == "CloudBlobDirectory")
                {
                    CloudBlobDirectory directory = (CloudBlobDirectory)element;
                    var tempFileList = await GetFilesOfBlobDirectory(directory);
                    await CopyFiles(tempFileList, srcDirectory, destContainer, srcContainer, srcStrgAcc, containerName);
                }
            }
        }

        private async Task<BlobResultSegment> GetFilesOfBlobDirectory(CloudBlobDirectory directory)
        {
            var fileList = await directory.ListBlobsSegmentedAsync(null);
            return fileList;
        }

        private async System.Threading.Tasks.Task CopyStandardFilesToCSDP(CloudBlockBlob blob, CloudBlobContainer destContainer, CloudBlobContainer srcContainer, CloudStorageAccount srcStrgAcc, string containerName)
        {

            CloudBlockBlob destBlob = destContainer.GetBlockBlobReference(blob.Name);
            // Create SAS key
            string sas = srcContainer.GetSharedAccessSignature(new SharedAccessBlobPolicy()
            {
                SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(15),
                Permissions = SharedAccessBlobPermissions.Read | SharedAccessBlobPermissions.List
            });
            string blobPathUri = blob.Name.Replace(" ", "%20");
            string uriString = srcStrgAcc.BlobStorageUri.PrimaryUri.AbsoluteUri + containerName + "/" + blobPathUri + sas;
            var uri = new Uri(uriString);
            var result = await destBlob.StartCopyAsync(uri);
        }

        public class OSStreamSettings
        {
            public string ProductKey { get; set; }            
            public string LocalSettingLinux { get; set; }
        }

        public class CustomerImageViewModel
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Update { get; set; }
            public string BuildNr { get; set; }
            public bool PublishInShop { get; set; }
            public string CustomerId { get; set; }
            public string FileName { get; set; }
            public string ImageId { get; set; }
            public string UnattendId { get; set; }
            public FileRefModel Unattend { get; set; }
            public string OEMPartitionId { get; set; }
            public FileRefModel OEMPartition { get; set; }
        }
    }
}
