using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Azure.Storage.Sas;
using System.IO.Compression;

namespace WPM_API.FileRepository
{
    public class FileRepository
    {
        protected string ConnectionString = String.Empty;
        protected string Folder = String.Empty;

        public FileRepository(string connectionString, string folder)
        {
            ConnectionString = connectionString;
            Folder = folder;
        }

        protected Stream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        public async Task<string> UploadFile(Stream fileStream)
        {
            // New Guid as Filename
            Guid g = Guid.NewGuid();

            // Get reference of Blob container from connection string.
            BlobContainerClient container = new BlobContainerClient(ConnectionString, Folder);
            container.CreateIfNotExists();

            // Get a reference to a blob
            BlobClient blob = container.GetBlobClient(g.ToString());

            while (await blob.ExistsAsync())
            {
                // New Guid as Filename
                g = Guid.NewGuid();
                // Check Guid is not used.
                blob = container.GetBlobClient(g.ToString());
            }

            await blob.UploadAsync(fileStream);

            // OLD
            //CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConnectionString);

            //// Create the blob client.
            //CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            //// Retrieve reference to a previously created container.
            //CloudBlobContainer container = blobClient.GetContainerReference(Folder);

            //// Check Guid is not used.
            //CloudBlockBlob blockBlob = container.GetBlockBlobReference(g.ToString());
            //while (await blockBlob.ExistsAsync())
            //{
            //    // New Guid as Filename
            //    g = Guid.NewGuid();
            //    // Check Guid is not used.
            //    blockBlob = container.GetBlockBlobReference(g.ToString());
            //}

            //// Create or overwrite the "myblob" blob with contents from a local file.
            //await blockBlob.UploadFromStreamAsync(fileStream);
            return g.ToString();
        }

        public async Task<string> UploadFile(Stream fileStream, string fileName)
        {
            // Get reference of Blob container from connection string.
            BlobContainerClient container = new BlobContainerClient(ConnectionString, Folder);
            container.CreateIfNotExists();

            // Get a reference to a blob
            BlobClient blob = container.GetBlobClient(fileName);
            await blob.DeleteIfExistsAsync();
            await blob.UploadAsync(fileStream);

            // OLD
            //// Retrieve storage account from connection string.
            //CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConnectionString);

            //// Create the blob client.
            //CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            //// Retrieve reference to a previously created container.
            //CloudBlobContainer container = blobClient.GetContainerReference(Folder);

            //// File reference
            //CloudBlockBlob blockBlob = container.GetBlockBlobReference(fileName); 

            //// Create or overwrite the "myblob" blob with contents from a local file.
            //await blockBlob.UploadFromStreamAsync(fileStream);
            return fileName;
        }

        public async Task<string> UploadFile(string fileName, Stream fileStream)
        {
            // Get reference of Blob container from connection string.
            BlobContainerClient container = new BlobContainerClient(ConnectionString, Folder);
            container.CreateIfNotExists();

            // Get a reference to a blob
            BlobClient blob = container.GetBlobClient(fileName);

            await blob.UploadAsync(fileStream);

            // OLD
            //// Retrieve storage account from connection string.
            //CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConnectionString);

            //// Create the blob client.
            //CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            //// Retrieve reference to a previously created container.
            //CloudBlobContainer container = blobClient.GetContainerReference(Folder);
            //CloudBlockBlob blockBlob = container.GetBlockBlobReference(fileName);

            //// Create or overwrite the "myblob" blob with contents from a local file.
            //await blockBlob.UploadFromStreamAsync(fileStream);
            return fileName;
        }

        public async Task<string> UploadFile(string fileName, string content)
        {
            // Remove Spaces
            fileName = fileName.Replace(" ", "");

            // Get reference of Blob container from connection string.
            BlobContainerClient container = new BlobContainerClient(ConnectionString, Folder);
            container.CreateIfNotExists();

            // Get a reference to a blob
            BlobClient blob = container.GetBlobClient(fileName);



            // Create or overwrite the "myblob" blob with contents from a local file.
            using (var fileStream = GenerateStreamFromString(content))
            {
                await blob.UploadAsync(fileStream);
            }

            return fileName;

            // OLD
            //// Retrieve storage account from connection string.
            //CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConnectionString);

            //// Create the blob client.
            //CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            //// Retrieve reference to a previously created container.
            //CloudBlobContainer container = blobClient.GetContainerReference(Folder);

            //// New Guid as Filename
            //Guid g = Guid.NewGuid();
            //// Check Guid is not used.
            //CloudBlockBlob blockBlob = container.GetBlockBlobReference(g.ToString());
            //while (await blockBlob.ExistsAsync())
            //{
            //    // New Guid as Filename
            //    g = Guid.NewGuid();
            //    // Check Guid is not used.
            //    blockBlob = container.GetBlockBlobReference(g.ToString());
            //}                        
        }

        public BlobClient GetBlobFile(string fileName)
        {
            // Remove Spaces
            fileName = fileName.Replace(" ", "");

            // Get reference of Blob container from connection string.
            BlobContainerClient container = new BlobContainerClient(ConnectionString, Folder);

            // Get a reference to a blob
            BlobClient blob = container.GetBlobClient(fileName);

            // OLD
            //CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConnectionString);

            //// Create the blob client.
            //CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            //// Retrieve reference to a previously created container.
            //CloudBlobContainer container = blobClient.GetContainerReference(Folder);

            // Retrieve reference to a blob named "myblob".
            return blob;
        }

        public async Task<bool> DeleteFile(string fileName)
        {
            fileName = fileName.Replace(" ", "");

            // Get reference of Blob container from connection string.
            BlobContainerClient container = new BlobContainerClient(ConnectionString, Folder);

            // Get a reference to a blob
            BlobClient blob = container.GetBlobClient(fileName);

            return await blob.DeleteIfExistsAsync();

            // OLD
            //CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConnectionString);

            //// Create the blob client.
            //CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            //// Retrieve reference to a previously created container.
            //CloudBlobContainer container = blobClient.GetContainerReference(Folder);

            //CloudBlockBlob file = container.GetBlockBlobReference(fileName);

            //// Delete reference to a file
            // await file.DeleteIfExistsAsync();
        }

        //public async Task<string> MoveFileAsync(string fileName, string destContainerName, bool delete = false)
        //{
        //    return await MoveFileAsync(fileName, fileName, destContainerName, delete);
        //}

        //public async Task<string> MoveFileAsync(string srcFileName, string destFileName, string destContainerName,
        //    bool delete = false)
        //{
        //    // Retrieve storage account from connection string.
        //    CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConnectionString);

        //    // Create the blob client.
        //    CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

        //    // Retrieve reference to a previously created container.
        //    CloudBlobContainer sourceContainer = blobClient.GetContainerReference(Folder);
        //    CloudBlobContainer destContainer = blobClient.GetContainerReference(destContainerName);

        //    // Retrieve reference to a blob.
        //    CloudBlockBlob srcBlob = sourceContainer.GetBlockBlobReference(srcFileName);
        //    CloudBlockBlob destBlob = destContainer.GetBlockBlobReference(destFileName);

        //    // Copy blob to other folder
        //    await destBlob.StartCopyAsync(srcBlob);

        //    //remove source blob after copy is done.
        //    if (delete)
        //    {
        //        await srcBlob.DeleteIfExistsAsync();
        //    }

        //    return destFileName;
        //}

        /// <summary>
        /// Downloads specific file cached from Server.
        /// </summary>
        /// <param name="fileName">Specific filename</param>
        /// <param name="path">Cachepath</param>
        /// <returns>The actual file</returns>
        public async Task<FileStream> DownloadWithLocalStorageAsync(string fileName, string path)
        {
            // Remove Spaces
            fileName = fileName.Replace(" ", "");

            string pathCombined = Path.Combine(path, fileName);

            BlobContainerClient blobContainer = new BlobContainerClient(ConnectionString, Folder);
            BlobClient cloudFile = blobContainer.GetBlobClient(fileName);
            BlobProperties properties = cloudFile.GetProperties();

            // OLD
            //CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConnectionString);

            //// Create the blob client.
            //CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            //// Retrieve reference to a previously created container.
            //CloudBlobContainer container = blobClient.GetContainerReference(Folder);
            //CloudBlockBlob cloudFile = container.GetBlockBlobReference(fileName);
            //cloudFile.FetchAttributesAsync().Wait();

            // Check if File is latest
            string localLastModified = string.Empty;
            string modifiedPath = pathCombined + ".mod";
            if (File.Exists(modifiedPath))
            {
                localLastModified = File.ReadAllText(modifiedPath);
            }

            // Create Path
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            // Save changed File
            if (!properties.LastModified.ToString().Equals(localLastModified))
            {
                await cloudFile.DownloadToAsync(pathCombined);
                await File.WriteAllTextAsync(modifiedPath, properties.LastModified.ToString());
            }

            // Download
            return new FileStream(pathCombined, FileMode.Open);
        }

        /// <summary>
        /// Pack files from blob streams into a zip file.
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        public async Task<byte[]> CreateZipFileFromBlobs(Dictionary<string, string> files)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                // Build the archive
                using (System.IO.Compression.ZipArchive zipArchive = new ZipArchive(stream, ZipArchiveMode.Create, true))
                {
                    foreach (var file in files)
                    {
                        // add the item name to the zip
                        System.IO.Compression.ZipArchiveEntry archiveEntry = zipArchive.CreateEntry(file.Value);
                        using (var entry = archiveEntry.Open())
                        {

                            // add the item bytes to the zip entry by opening the original file and copying the bytes 
                            await GetBlobFile(file.Key).DownloadToAsync(entry);
                        }
                    }
                }

                //Rewind the stream for reading to output.
                stream.Seek(0, SeekOrigin.Begin);
                return stream.ToArray();
            }
        }

        public string GetSASFile(string fileName, bool isCustomerCsdp)
        {
            string policyName = "winpePolicy";
            // Connect to the Azure storeage account
            BlobContainerClient blobContainerClient = new BlobContainerClient(ConnectionString, Folder);
            BlobClient file = null;

            // OLD
            //CloudStorageAccount sa = CloudStorageAccount.Parse(ConnectionString);
            //var blobClient = sa.CreateCloudBlobClient();

            //// Get reference to the container
            //var blobContainer = blobClient.GetContainerReference(Folder);
            //CloudBlockBlob file = null;

            if (isCustomerCsdp)
            {
                file = blobContainerClient.GetBlobClient("filerepository/" + fileName);
            }
            else
            {
                file = blobContainerClient.GetBlobClient(fileName);
            }

            if (file.CanGenerateSasUri)
            {
                BlobSasBuilder sasBuilder = new BlobSasBuilder()
                {
                    BlobContainerName = file.GetParentBlobContainerClient().Name,
                    BlobName = file.Name,
                    Resource = "b"
                };
                sasBuilder.ExpiresOn = DateTime.UtcNow.AddMinutes(15);
                sasBuilder.SetPermissions(BlobSasPermissions.Read);
                return file.GenerateSasUri(sasBuilder).ToString();
            }
            else
            {
                return null;
            }
        }

        public async Task<FileAndSAS> GetSASFile(string fileName, string guid)
        {
            try
            {
                // Connect to the Azure storeage account
                BlobContainerClient blobContainerClient = new BlobContainerClient(ConnectionString, Folder);
                BlobClient blob = blobContainerClient.GetBlobClient(fileName);

                // OLD
                //CloudStorageAccount sa = CloudStorageAccount.Parse(ConnectionString);
                //var blobClient = sa.CreateCloudBlobClient();

                //// Get reference to the container
                //var blobContainer = blobClient.GetContainerReference(Folder);
                //CloudBlockBlob file = blobContainer.GetBlockBlobReference(fileName);
                bool exists = await blob.ExistsAsync();
                string sas = String.Empty;
                if (!exists)
                {
                    blob = blobContainerClient.GetBlobClient(guid);
                }

                if (blob.CanGenerateSasUri)
                {
                    BlobSasBuilder sasBuilder = new BlobSasBuilder()
                    {
                        BlobContainerName = blob.GetParentBlobContainerClient().Name,
                        BlobName = blob.Name,
                        Resource = "b"
                    };
                    sasBuilder.ExpiresOn = DateTime.UtcNow.AddMinutes(15);
                    sasBuilder.SetPermissions(BlobSasPermissions.Read);
                    sas = blob.GenerateSasUri(sasBuilder).ToString();
                }

                // OLD
                //var sas = file.GetSharedAccessSignature(new SharedAccessBlobPolicy()
                //{
                //    SharedAccessExpiryTime = DateTime.UtcNow.AddHours(5),
                //    Permissions = SharedAccessBlobPermissions.Read
                //});
                FileAndSAS result = new FileAndSAS();
                result.FileName = blob.Name;
                result.SasUri = sas;
                return result;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public class FileAndSAS
        {
            public string SasUri { get; set; }
            public string FileName { get; set; }
        }

        public string GetSASBlobContainer()
        {
            // Connect to the Azure storage account
            BlobContainerClient blobContainerClient = new BlobContainerClient(ConnectionString, Folder);
            if (blobContainerClient.CanGenerateSasUri)
            {
                BlobSasBuilder sasBuilder = new BlobSasBuilder()
                {
                    BlobContainerName = blobContainerClient.Name,
                    Resource = "c"
                };
                sasBuilder.ExpiresOn = DateTime.UtcNow.AddHours(2);
                sasBuilder.SetPermissions(BlobSasPermissions.Read);
                return blobContainerClient.GenerateSasUri(sasBuilder).ToString();
            }
            else
            {
                return null;
            }

            // OLD
            //CloudStorageAccount sa = CloudStorageAccount.Parse(ConnectionString);
            //var blobClient = sa.CreateCloudBlobClient();


            //// Get reference to Blob container
            //var blobContainer = blobClient.GetContainerReference(Folder);

            //// Get SAS token to write a new Blob into Blob container
            //var sas = blobContainer.GetSharedAccessSignature(new SharedAccessBlobPolicy()
            //{
            //    SharedAccessExpiryTime = DateTime.UtcNow.AddHours(2),
            //    Permissions = SharedAccessBlobPermissions.Write
            //});

            //return blobContainer.Uri + sas;
        }

        public async Task<bool> FindFileAsync(string fileName)
        {
            fileName = fileName.Replace(" ", "");

            // Get reference of Blob container from connection string.
            BlobContainerClient container = new BlobContainerClient(ConnectionString, Folder);

            // Get a reference to a blob
            BlobClient blob = container.GetBlobClient(fileName);
            // OLD
            //CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConnectionString);
            //CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            //CloudBlobContainer container = blobClient.GetContainerReference(Folder);
            //var blob = container.GetBlockBlobReference(fileName);
            return await blob.ExistsAsync();
        }
    }
}