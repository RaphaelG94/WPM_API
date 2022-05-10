using WPM_API.Azure;
using WPM_API.Common;
using WPM_API.Data.DataContext.Entities;
using WPM_API.Data.DataContext.Entities.Storages;
using WPM_API.Data.DataRepository;
using WPM_API.Data.Models;
using WPM_API.Models;
using WPM_API.Models.Release_Mgmt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using static WPM_API.FileRepository.FileRepository;

namespace WPM_API.Controllers
{
    [Authorize(Policy = Constants.Roles.Customer)]
    [Route("/order")]
    public class OrderController : BasisController
    {

        /// <summary>
        /// Place a new order.
        /// </summary>
        /// <param name="order">New Order</param>
        /// <returns></returns>
        [HttpPost]
        [Route("{customerId}")]
        public async Task<IActionResult> PlaceOrder([FromBody] OrderAddViewModel order, [FromRoute] string customerId)
        {
            try
            {
                using (var unitOfWork = CreateUnitOfWork())
                {
                    Data.DataContext.Entities.Customer customer = unitOfWork.Customers.GetOrNull(customerId, CustomerIncludes.GetAllIncludes());
                    var cep = GetCEP(customerId);
                    StorageEntryPoint csdp = null;

                    if (customer == null)
                    {
                        return BadRequest("ERROR: The customer does not exist");
                    }
                    csdp = customer.StorageEntryPoints.Find(x => x.IsCSDP == true);
                    if (csdp == null)
                    {
                        return BadRequest("ERROR: The csdp storage entry point does not exist or has not been created yet");
                    }
                    if (cep == null && !csdp.Managed)
                    {
                        return BadRequest("ERROR: The csdp Cloud Entry Point is not set");
                    }

                    AzureCommunicationService azureCustomer;
                    if (csdp.Managed)
                    {
                        // TODO: Check system; fix for live sys
                        azureCustomer = new AzureCommunicationService(_appSettings.DevelopmentTenantId, _appSettings.DevelopmentClientId, _appSettings.DevelopmentClientSecret);
                    } else
                    {
                        azureCustomer = new AzureCommunicationService(cep.TenantId, cep.ClientId, cep.ClientSecret);
                    }
                    
                    // string connectionString = azureCustomer.StorageService().GetStorageAccConnectionString(csdp.SubscriptionId, csdp.ResourceGrpName, csdp.StorageAccount);
                    var newOrder = unitOfWork.Order.CreateEmpty();
                    newOrder.CreatedByUserId = GetCurrentUser().Id;
                    newOrder.CreatedDate = DateTime.Now;
                    newOrder.ShopItems = new List<OrderShopItem>();
                    if (order.ShopItems != null)
                    {
                        foreach (var o in order.ShopItems)
                        {
                            ShopItem si = unitOfWork.Shop.GetOrNull(o, "DriverShopItems");
                            CustomerHardwareModel hwModel = unitOfWork.CustomerHardwareModels.GetAll("Drivers").Where(x => x.Name == si.Name && x.CustomerId == customerId).FirstOrDefault();
                            if (hwModel != null)
                            {
                                hwModel.Counter++;
                            }
                            if (si == null)
                            {
                                return BadRequest("ERROR: The shop item does not exist");
                            }
                            foreach (DriverShopItem dsi in si.DriverShopItems)
                            {
                                Driver driver = unitOfWork.Drivers.GetOrNull(dsi.DriverId);
                                if (driver != null)
                                {
                                    // Copy files to csdp
                                    string connectionString = azureCustomer.StorageService().GetStorageAccConnectionString(csdp.SubscriptionId, csdp.ResourceGrpName, csdp.StorageAccount);

                                    // Setup reference to customer csdp & copy blob
                                    CloudStorageAccount customerStorageAcc = CloudStorageAccount.Parse(connectionString);
                                    CloudBlobClient destClient = customerStorageAcc.CreateCloudBlobClient();
                                    CloudBlobContainer destContainer = destClient.GetContainerReference(csdp.BlobContainerName);
                                    var exists = await destContainer.ExistsAsync();

                                    // TODO: list files and copy them to csdp with SubFolderPath
                                    CloudStorageAccount srcStrgAcc = CloudStorageAccount.Parse(driver.ConnectionString);
                                    CloudBlobClient srcClient = srcStrgAcc.CreateCloudBlobClient();
                                    CloudBlobContainer srcContainer = srcClient.GetContainerReference(driver.ContainerName);
                                    CloudBlobDirectory srcDirectory = srcContainer.GetDirectoryReference(driver.SubFolderPath);
                                    var fileList = await srcDirectory.ListBlobsSegmentedAsync(null);

                                    await CopyDrivers(fileList, srcDirectory, destContainer, srcContainer, driver, srcStrgAcc);
                                }
                            }
                            newOrder.ShopItems.Add(new OrderShopItem() { ShopItemId = o });
                        }
                    }

                    unitOfWork.SaveChanges();
                    newOrder = unitOfWork.Order.Get(newOrder.Id, "ShopItems", "ShopItems.ShopItem");
                    var json = JsonConvert.SerializeObject(Mapper.Map<OrderViewModel>(newOrder), _serializerSettings);
                    // SendOrderEmail(newOrder);
                    return new OkObjectResult(json);
                }
            }
            catch (Exception e) {
                return BadRequest("ERROR: " + e.Message);
            }
        }

        private async System.Threading.Tasks.Task CopyDrivers(BlobResultSegment fileList, CloudBlobDirectory srcDirectory, CloudBlobContainer destContainer, CloudBlobContainer srcContainer, Driver driver, CloudStorageAccount srcStrgAcc)
        {
            for (int i = 0; i < fileList.Results.Count(); i++)
            {
                var element = fileList.Results.ElementAt(i);
                if (element.GetType().Name == "CloudBlockBlob")
                {
                    CloudBlockBlob blob = (CloudBlockBlob)element;
                    await CopyFilesToCSDP(blob, destContainer, srcContainer, driver, srcStrgAcc);
                }
                else if (element.GetType().Name == "CloudBlobDirectory")
                {
                    CloudBlobDirectory directory = (CloudBlobDirectory)element;
                    var tempFileList = await GetFilesOfBlobDirectory(directory);
                    await CopyDrivers(tempFileList, srcDirectory, destContainer, srcContainer, driver, srcStrgAcc);
                }
            }
        }

        private async System.Threading.Tasks.Task CopyFilesToCSDP(CloudBlockBlob blob, CloudBlobContainer destContainer, CloudBlobContainer srcContainer, Driver driver, CloudStorageAccount srcStrgAcc)
        {
            
            CloudBlockBlob destBlob = destContainer.GetBlockBlobReference(blob.Name);
            // Create SAS key
            string sas = srcContainer.GetSharedAccessSignature(new SharedAccessBlobPolicy()
            {
                SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(15),
                Permissions = SharedAccessBlobPermissions.Read | SharedAccessBlobPermissions.List
            });
            string blobPathUri = blob.Name.Replace(" ", "%20");
            string uriString = srcStrgAcc.BlobStorageUri.PrimaryUri.AbsoluteUri + driver.ContainerName + "/" + blobPathUri + sas;
            var uri = new Uri(uriString);
            var result = await destBlob.StartCopyAsync(uri);
        }

        private async Task<BlobResultSegment> GetFilesOfBlobDirectory(CloudBlobDirectory directory)
        {
            var fileList = await directory.ListBlobsSegmentedAsync(null);
            return fileList;
        }

        [HttpPost]
        [Route("software/{customerId}")]
        public async Task<IActionResult> OrderSoftware([FromBody] ShopItemViewModel shopItem, [FromRoute] string customerId)
        {
            try
            {
                using (var unitOfWork = CreateUnitOfWork()) {
                    WPM_API.Data.DataContext.Entities.Customer customer = unitOfWork.Customers.GetOrNull(customerId, CustomerIncludes.GetAllIncludes());
                    var cep = GetCEP(customerId);
                    Software software = unitOfWork.Software.GetOrNull(shopItem.Id, SoftwareIncludes.GetAllIncludes());
                    StorageEntryPoint csdp = null;
                    if (customer == null)
                    {
                        return BadRequest("ERROR: The customer does not exist");
                    }
                    csdp = customer.StorageEntryPoints.Find(x => x.IsCSDP == true);
                    if (cep == null && csdp!= null && !csdp.Managed)
                    {
                        return BadRequest("ERROR: The csdp Cloud Entry Point is not set");
                    }
                    if (software == null)
                    {
                        return BadRequest("ERROR: The software does not exist");
                    }
                    if (csdp == null)
                    {
                        return BadRequest("ERROR: The csdp storage entry point does not exist or has not been created yet");
                    }

                    List<FileAndSAS> sasKeys = new List<FileAndSAS>();

                    // Create needed Azrue connections
                    if (_appSettings == null || _connectionStrings == null)
                    {
                        return BadRequest("ERROR: Cannot fetch Bitstream Azure connection from config files");
                    }
                    FileRepository.FileRepository csdpBitstreamRepo = new FileRepository.FileRepository(_connectionStrings.FileRepository, _appSettings.FileRepositoryFolder);
                    AzureCommunicationService azureCustomer;
                    if (!csdp.Managed) {
                        azureCustomer = new AzureCommunicationService(cep.TenantId, cep.ClientId, cep.ClientSecret);
                    } else
                    {
                        // TODO: Check for system; fix for live system
                        azureCustomer = new AzureCommunicationService(_appSettings.DevelopmentTenantId, _appSettings.DevelopmentClientId, _appSettings.DevelopmentClientSecret);
                    }

                    // Get files from BitStream storage account
                    foreach (Data.DataContext.Entities.File file in software.TaskInstall.Files)
                    {
                        if (file.Guid == null)
                        {
                            return BadRequest("ERROR: The file has no Guid for Azure");
                        }
                        FileAndSAS sasKey = await csdpBitstreamRepo.GetSASFile(file.Name, file.Guid);
                        if (sasKey == null)
                        {
                            return BadRequest("ERROR: The sas key for " + file.Name + " is null");
                        }
                        sasKeys.Add(sasKey);
                    }

                    List<Microsoft.Azure.Management.Storage.Models.StorageAccount> storageAccounts = await azureCustomer.StorageService().GetStorageAccounts(csdp.SubscriptionId, csdp.ResourceGrpName);
                    Microsoft.Azure.Management.Storage.Models.StorageAccount storageAccountCustomer = storageAccounts.Find(x => x.Name == csdp.StorageAccount);
                    if (storageAccountCustomer == null)
                    {
                        return BadRequest("ERROR: Your csdp in Azrue has been deleted. Please restore the storage account");
                    }

                    string connectionString = azureCustomer.StorageService().GetStorageAccConnectionString(csdp.SubscriptionId, csdp.ResourceGrpName, csdp.StorageAccount);
                    FileRepository.FileRepository csdpCustomer = new FileRepository.FileRepository(connectionString, "csdp");
                    foreach (FileAndSAS sasKey in sasKeys)
                    {
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(sasKey.SasUri);
                        request.Method = "GET";
                        WebResponse response = request.GetResponse();
                        await csdpCustomer.UploadFile(response.GetResponseStream(), "filerepository/" + sasKey.FileName);
                    }
                    // Check for CustomerSoftwareStream
                    CustomerSoftwareStream customerSoftwareStream = null;
                    if (customer.CustomerSoftwareStreams == null)
                    {
                        customer.CustomerSoftwareStreams = new List<CustomerSoftwareStream>();
                    }
                    SoftwareStream stream = unitOfWork.SoftwareStreams.GetOrNull(software.SoftwareStreamId);
                    customerSoftwareStream = customer.CustomerSoftwareStreams.Find(x => x.SoftwareStreamId == stream.Id && x.DeletedDate == null);
                    CustomerSoftware newSoftware = Mapper.Map<CustomerSoftware>(software);
                    newSoftware.Id = null;
                    newSoftware.SoftwareId = software.Id;
                    newSoftware.CustomerStatus = "Test";
                    if (customerSoftwareStream != null)
                    {
                        customerSoftwareStream = unitOfWork.CustomerSoftwareStreamss.GetOrNull(customerSoftwareStream.Id, "StreamMembers");
                        // Add CustomerSoftware
                        if (customerSoftwareStream.StreamMembers == null)
                        {
                            customerSoftwareStream.StreamMembers = new List<CustomerSoftware>();
                        }
                        newSoftware.CustomerSoftwareStreamId = customerSoftwareStream.Id;
                        customerSoftwareStream.StreamMembers.Add(newSoftware);
                        unitOfWork.CustomerSoftwareStreamss.MarkForUpdate(customerSoftwareStream, GetCurrentUser().Id);
                        unitOfWork.CustomerSoftwares.MarkForInsert(newSoftware, GetCurrentUser().Id);
                    } else
                    {
                        // Create new CustomerSoftwareStream and add CustomerSoftware
                        SoftwareStream softwareStream = unitOfWork.SoftwareStreams.GetOrNull(software.SoftwareStreamId);
                        if (softwareStream == null)
                        {
                            return BadRequest("ERROR: The software stream does not exist");
                        }
                        customerSoftwareStream = Mapper.Map<CustomerSoftwareStream>(softwareStream);
                        customerSoftwareStream.SoftwareStreamId = softwareStream.Id;
                        customerSoftwareStream.Id = null;
                        customerSoftwareStream.CustomerId = customerId;
                        customerSoftwareStream.StreamMembers = new List<CustomerSoftware>();
                        customerSoftwareStream.StreamMembers.Add(newSoftware);
                        unitOfWork.CustomerSoftwareStreamss.MarkForInsert(customerSoftwareStream, GetCurrentUser().Id);
                        unitOfWork.CustomerSoftwares.MarkForInsert(newSoftware, GetCurrentUser().Id);
                    }
                    if (customer.CustomerSoftwareStreams == null)
                    {
                        customer.CustomerSoftwareStreams = new List<CustomerSoftwareStream>();
                    }
                    unitOfWork.Customers.MarkForUpdate(customer, GetCurrentUser().Id);
                    unitOfWork.SaveChanges();
                    newSoftware.CustomerSoftwareStreamId = customerSoftwareStream.Id;
                    unitOfWork.SaveChanges();

                    newSoftware.TaskInstall = null;
                    newSoftware.TaskUpdate = null;
                    newSoftware.TaskUninstall = null;
                    var json = JsonConvert.SerializeObject(newSoftware, _serializerSettings);

                    return Ok(json);
                }
            } catch (Exception e)
            {
                string innerExceptionMessage = "";
                if (e.InnerException != null)
                {
                    innerExceptionMessage = e.InnerException.Message;
                }
                return BadRequest("ERROR: " + e.Message + "\nInnerException: " + innerExceptionMessage);
            }
        }

        [HttpPost]
        [Route("image/{customerId}")]
        public async Task<IActionResult> OrderOSImage([FromRoute] string customerId, [FromBody] ShopItemViewModel shopItem)
        {
            try
            {                
                using (var unitOfWork = CreateUnitOfWork()) 
                {
                    WPM_API.Data.DataContext.Entities.Customer customer = unitOfWork.Customers.GetOrNull(customerId, CustomerIncludes.GetAllIncludes());
                    var cep = GetCEP(customerId);
                    
                    Image image = unitOfWork.Images.GetOrNull(shopItem.Id, "Unattend", "OEMPartition");
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
                    if (!csdp.Managed) {
                        azureCustomer = new AzureCommunicationService(cep.TenantId, cep.ClientId, cep.ClientSecret);
                    } else
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
                    } else if (imageStream.Type == "Linux")
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

                    var json = JsonConvert.SerializeObject(newImage, _serializerSettings);
                    return Ok(json);
                }
            } catch (Exception e)
            {
                string innerExceptionMessage = "";
                if (e.InnerException != null)
                {
                    innerExceptionMessage = e.InnerException.Message;
                }
                return BadRequest("ERROR: " + e.Message + "\nInnerException: " + innerExceptionMessage);
            }
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
            string uriString = srcStrgAcc.BlobStorageUri.PrimaryUri.AbsoluteUri + containerName +"/" + blobPathUri + sas;
            var uri = new Uri(uriString);
            var result = await destBlob.StartCopyAsync(uri);
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

        [HttpGet]
        [Route("{orderId}")]
        public IActionResult GetOrder([FromRoute] string orderId)
        {
            Order dbOrder = null;
            using (var unitOfWork = CreateUnitOfWork())
            {
                dbOrder = unitOfWork.Order.Get(orderId, "ShopItems");
                if (dbOrder == null)
                {
                    return new NotFoundResult();
                }
            }

            var json = JsonConvert.SerializeObject(Mapper.Map<OrderViewModel>(dbOrder), _serializerSettings);
            return new OkObjectResult(json);
        }

        [HttpPost]
        [Route("driver/{customerId}")]
        public async Task<IActionResult> OrderDriver([FromRoute] string customerId, [FromBody] DriverViewModel payload)
        {
            using (var unitOfWork = CreateUnitOfWork())
            {                
                Driver toShop = unitOfWork.Drivers.Get(payload.Id);
                List<CustomerDriver> customerDrivers = unitOfWork.CustomerDrivers.GetAll().ToList();
                if (customerDrivers.Find(x => x.DriverId == toShop.Id) != null)
                {
                    return BadRequest("ERROR: The driver already has been shopped");
                }
                else
                {
                    Customer customer = unitOfWork.Customers.Get(customerId, CustomerIncludes.GetAllIncludes());
                    var cep = GetCEP(customerId);
                    StorageEntryPoint csdp = null;
                    csdp = customer.StorageEntryPoints.Find(x => x.IsCSDP == true);
                    AzureCommunicationService azureCustomer;

                    if (csdp == null)
                    {
                        return BadRequest("ERROR: The csdp storage entry point does not exist or has not been created yet");
                    }
                    if (cep == null && !csdp.Managed)
                    {
                        return BadRequest("ERROR: The csdp Cloud Entry Point is not set");
                    }
                    if (csdp.Managed)
                    {
                        // TODO: Check system; fix for live sys
                        azureCustomer = new AzureCommunicationService(_appSettings.DevelopmentTenantId, _appSettings.DevelopmentClientId, _appSettings.DevelopmentClientSecret);
                    }
                    else
                    {
                        azureCustomer = new AzureCommunicationService(cep.TenantId, cep.ClientId, cep.ClientSecret);
                    }
                    string connectionString = azureCustomer.StorageService().GetStorageAccConnectionString(csdp.SubscriptionId, csdp.ResourceGrpName, csdp.StorageAccount);
                    CloudStorageAccount customerStorageAcc = CloudStorageAccount.Parse(connectionString);
                    CloudBlobClient destClient = customerStorageAcc.CreateCloudBlobClient();
                    CloudBlobContainer destContainer = destClient.GetContainerReference(csdp.BlobContainerName);
                    var exists = await destContainer.ExistsAsync();

                    // TODO: list files and copy them to csdp with SubFolderPath
                    try
                    {
                        CloudStorageAccount srcStrgAcc = CloudStorageAccount.Parse(toShop.ConnectionString);
                        CloudBlobClient srcClient = srcStrgAcc.CreateCloudBlobClient();
                        CloudBlobContainer srcContainer = srcClient.GetContainerReference(toShop.ContainerName);
                        CloudBlobDirectory srcDirectory = srcContainer.GetDirectoryReference(toShop.SubFolderPath);
                        var fileList = await srcDirectory.ListBlobsSegmentedAsync(null);
                        await CopyDrivers(fileList, srcDirectory, destContainer, srcContainer, toShop, srcStrgAcc);

                        // Add Driver to CustomerDriver
                        CustomerDriver newCustomerDriver = Mapper.Map<CustomerDriver>(toShop);
                        newCustomerDriver.Id = null;
                        newCustomerDriver.CustomerId = customerId;
                        newCustomerDriver.DriverId = toShop.Id;
                        unitOfWork.CustomerDrivers.MarkForInsert(newCustomerDriver, GetCurrentUser().Id);
                        unitOfWork.SaveChanges();
                        return Ok();
                    } catch (Exception ex1)
                    {
                        return BadRequest("ERROR: " + ex1.Message);
                    }                                        
                }
            }            
        }

        [HttpPost]
        [Route("selectedSoftware/{customerId}")]
        public async Task<IActionResult> OrderSelectedSoftware([FromBody] List<ShopItemViewModel> shopItems, [FromRoute] string customerId)
        {
            try
            {
                List<CustomerSoftware> result = new List<CustomerSoftware>();
                using (var unitOfWork = CreateUnitOfWork())
                {
                    WPM_API.Data.DataContext.Entities.Customer customer = unitOfWork.Customers.GetOrNull(customerId, CustomerIncludes.GetAllIncludes());
                    var cep = GetCEP(customerId);
                    StorageEntryPoint csdp = null;
                    if (customer == null)
                    {
                        return BadRequest("ERROR: The customer does not exist");
                    }
                    csdp = customer.StorageEntryPoints.Find(x => x.IsCSDP == true);
                    if (cep == null && csdp != null && !csdp.Managed)
                    {
                        return BadRequest("ERROR: The csdp Cloud Entry Point is not set");
                    }
                    if (csdp == null)
                    {
                        return BadRequest("ERROR: The csdp storage entry point does not exist or has not been created yet");
                    }

                    // Create needed Azrue connections
                    if (_appSettings == null || _connectionStrings == null)
                    {
                        return BadRequest("ERROR: Cannot fetch Bitstream Azure connection from config files");
                    }
                    FileRepository.FileRepository csdpBitstreamRepo = new FileRepository.FileRepository(_connectionStrings.FileRepository, _appSettings.FileRepositoryFolder);
                    AzureCommunicationService azureCustomer;
                    if (!csdp.Managed)
                    {
                        azureCustomer = new AzureCommunicationService(cep.TenantId, cep.ClientId, cep.ClientSecret);
                    }
                    else
                    {
                        // TODO: Check for system; fix for live system
                        azureCustomer = new AzureCommunicationService(_appSettings.DevelopmentTenantId, _appSettings.DevelopmentClientId, _appSettings.DevelopmentClientSecret);
                    }

                    // Connect to customer Azure storage
                    List<Microsoft.Azure.Management.Storage.Models.StorageAccount> storageAccounts = await azureCustomer.StorageService().GetStorageAccounts(csdp.SubscriptionId, csdp.ResourceGrpName);
                    Microsoft.Azure.Management.Storage.Models.StorageAccount storageAccountCustomer = storageAccounts.Find(x => x.Name == csdp.StorageAccount);
                    if (storageAccountCustomer == null)
                    {
                        return BadRequest("ERROR: Your csdp in Azrue has been deleted. Please restore the storage account");
                    }

                    string connectionString = azureCustomer.StorageService().GetStorageAccConnectionString(csdp.SubscriptionId, csdp.ResourceGrpName, csdp.StorageAccount);
                    FileRepository.FileRepository csdpCustomer = new FileRepository.FileRepository(connectionString, "csdp");

                    // Shop software via sas keys for files
                    foreach (ShopItemViewModel shopItem in shopItems)
                    {
                        List<FileAndSAS> sasKeys = new List<FileAndSAS>();
                        Software software = unitOfWork.Software.GetOrNull(shopItem.Id, SoftwareIncludes.GetAllIncludes());

                        foreach (Data.DataContext.Entities.File file in software.TaskInstall.Files)
                        {
                            if (file.Guid == null)
                            {
                                return BadRequest("ERROR: The file has no Guid for Azure");
                            }
                            FileAndSAS sasKey = await csdpBitstreamRepo.GetSASFile(file.Name, file.Guid);
                            if (sasKey == null)
                            {
                                return BadRequest("ERROR: The sas key for " + file.Name + " is null");
                            }
                            sasKeys.Add(sasKey);

                            foreach (FileAndSAS sasItem in sasKeys)
                            {
                                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(sasItem.SasUri);
                                request.Method = "GET";
                                WebResponse response = request.GetResponse();
                                await csdpCustomer.UploadFile(response.GetResponseStream(), "filerepository/" + sasItem.FileName);
                            }                            
                        }
                        // Check for CustomerSoftwareStream
                        CustomerSoftwareStream customerSoftwareStream = null;
                        if (customer.CustomerSoftwareStreams == null)
                        {
                            customer.CustomerSoftwareStreams = new List<CustomerSoftwareStream>();
                        }
                        SoftwareStream stream = unitOfWork.SoftwareStreams.GetOrNull(software.SoftwareStreamId);
                        customerSoftwareStream = customer.CustomerSoftwareStreams.Find(x => x.SoftwareStreamId == stream.Id && x.DeletedDate == null);
                        CustomerSoftware newSoftware = Mapper.Map<CustomerSoftware>(software);
                        newSoftware.Id = null;
                        newSoftware.SoftwareId = software.Id;
                        newSoftware.TaskInstall = software.TaskInstall;
                        newSoftware.CustomerStatus = "Test";
                        if (customerSoftwareStream != null)
                        {
                            customerSoftwareStream = unitOfWork.CustomerSoftwareStreamss.GetOrNull(customerSoftwareStream.Id, "StreamMembers");
                            // Add CustomerSoftware
                            if (customerSoftwareStream.StreamMembers == null)
                            {
                                customerSoftwareStream.StreamMembers = new List<CustomerSoftware>();
                            }
                            newSoftware.CustomerSoftwareStreamId = customerSoftwareStream.Id;
                            customerSoftwareStream.StreamMembers.Add(newSoftware);
                            unitOfWork.CustomerSoftwareStreamss.MarkForUpdate(customerSoftwareStream, GetCurrentUser().Id);
                            unitOfWork.CustomerSoftwares.MarkForInsert(newSoftware, GetCurrentUser().Id);
                        }
                        else
                        {
                            // Create new CustomerSoftwareStream and add CustomerSoftware
                            SoftwareStream softwareStream = unitOfWork.SoftwareStreams.GetOrNull(software.SoftwareStreamId);
                            if (softwareStream == null)
                            {
                                return BadRequest("ERROR: The software stream does not exist");
                            }
                            customerSoftwareStream = Mapper.Map<CustomerSoftwareStream>(softwareStream);
                            customerSoftwareStream.SoftwareStreamId = softwareStream.Id;
                            customerSoftwareStream.Id = null;
                            customerSoftwareStream.CustomerId = customerId;
                            customerSoftwareStream.StreamMembers = new List<CustomerSoftware>();
                            customerSoftwareStream.StreamMembers.Add(newSoftware);
                            unitOfWork.CustomerSoftwareStreamss.MarkForInsert(customerSoftwareStream, GetCurrentUser().Id);
                            unitOfWork.CustomerSoftwares.MarkForInsert(newSoftware, GetCurrentUser().Id);
                        }
                        if (customer.CustomerSoftwareStreams == null)
                        {
                            customer.CustomerSoftwareStreams = new List<CustomerSoftwareStream>();
                        }
                        unitOfWork.Customers.MarkForUpdate(customer, GetCurrentUser().Id);
                        unitOfWork.SaveChanges();
                        newSoftware.CustomerSoftwareStreamId = customerSoftwareStream.Id;
                        unitOfWork.SaveChanges();

                        result.Add(newSoftware);
                    }
                }
                foreach (CustomerSoftware cs in result)
                {
                    cs.TaskInstall = null;
                    cs.TaskUninstall = null;
                    cs.TaskUpdate = null;
                }
                var json = JsonConvert.SerializeObject(result, _serializerSettings);
                return Ok(json);
            } catch (Exception e)
            {
                return BadRequest("ERROR: " + e.Message);
            }            
        }

        private IActionResult SendOrderEmail(Order order)
        {
            String fromUserName = _orderEmailOptions.ReceiverEmail;
            MailMessage msg = new MailMessage();
            using (var unitOfWork = CreateUnitOfWork())
            {
                User dbUser = unitOfWork.Users.Get(order.CreatedByUserId);
                if (dbUser == null)
                {
                    return new NotFoundResult();
                }
                msg.To.Add(new MailAddress(fromUserName));
                msg.From = new MailAddress(fromUserName);
                msg.Subject = "Neue Bestellung";
                StringBuilder stringBuilder = new StringBuilder(
                    "Neue Bestellung von Nutzer: " + dbUser.UserName
                    + "</br> E-Mail: " + dbUser.Email
                    + "</br> Customer: " + dbUser.Customer
                    + "</br> Systemhaus: " + dbUser.Systemhouse
                    + "</br> Eingegangen am: " + order.CreatedDate
                    + "</br> Bestellte Produkte: ");
                IEnumerable<OrderShopItem> distinctItems = order.ShopItems.Distinct();
                foreach (OrderShopItem item in distinctItems)
                {
                    int amount = order.ShopItems.FindAll(x => x.ShopItemId == item.ShopItemId).Count();

                    // TODO: At the Moment is only one order possible
                    string totalPrice = item.ShopItem.Price;
                    // int totalPrice = Int32.Parse(item.ShopItem.Price.Remove(item.ShopItem.Price.Length - 1)) * amount;
                    stringBuilder.Append("</br>" + amount + "x "
                        + item.ShopItem.Name 
                        + "</br>Preis: "
                        + totalPrice + "€</br>");
                }
                msg.Body = stringBuilder.ToString();
                msg.IsBodyHtml = true;
            }

            SmtpClient client = ConfigureSMTPClient(fromUserName);
            client.Send(msg);
            return null;
        }

        private SmtpClient ConfigureSMTPClient(string receiverEmail) {
            String password = _orderEmailOptions.Password;
            SmtpClient client = new SmtpClient();
            client.Host = _orderEmailOptions.Host;
            client.Credentials = new System.Net.NetworkCredential(receiverEmail, password);
            client.Port = Int32.Parse(_orderEmailOptions.Port);
            client.EnableSsl = bool.Parse(_orderEmailOptions.EnableSsl);
            return client;
        }

    }
}
