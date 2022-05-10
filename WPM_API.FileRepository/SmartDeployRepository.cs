//using Microsoft.Extensions.Configuration;
using Azure.Storage.Blobs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

//using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace WPM_API.FileRepository.SmartDeploy
{

    public class SmartDeployRepository
    {
        // private string _connectionString = "DefaultEndpointsProtocol=https;AccountName=workplacemanager;AccountKey=2JHzgIhOnFJ7gbnJ1p1E6AEps5e5IoXQhkBh0KodPvAw2f4n3f+ScntuGnKylm9jl6T54Lme0JPac+3oY//K2g==";
        // private string _shareReference = "swrepository";

        private readonly string _connectionString = string.Empty;
        private readonly string _shareReference = string.Empty;

        public SmartDeployRepository(string connectionString, string shareReference)
        {
            _connectionString = connectionString;
            _shareReference = shareReference;
        }

        public BlobClient DownloadFile(string filePath, string fileName)
        {
            BlobContainerClient blobContainerClient = new BlobContainerClient(_connectionString, filePath);
            BlobClient blob = blobContainerClient.GetBlobClient(fileName);
            return blob;
            
            // OLD
            //CloudStorageAccount sA = CloudStorageAccount.Parse(_connectionString);
            ////CloudBlobClient bCl = sA.CreateCloudBlobClient();
            ////CloudBlobContainer bCo = bCl.GetContainerReference(_cloudContainer);
            //CloudFileClient cfc = sA.CreateCloudFileClient();
            //CloudFileShare cfs = cfc.GetShareReference(_shareReference);
            //CloudFileDirectory rootDir = cfs.GetRootDirectoryReference();
            //CloudFileDirectory fileDir = rootDir.GetDirectoryReference(filePath);
            //CloudFile cf = fileDir.GetFileReference(fileName);
            //return cf;
            /*var memStream = new MemoryStream();

            cf.DownloadToStream(memStream);
            return memStream;*/
        }                
    }
}