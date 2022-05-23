using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;
using Microsoft.Azure.Management.Storage;
using Microsoft.Azure.Management.Storage.Fluent;
using Microsoft.Azure.Management.Storage.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using AZURE = Microsoft.Azure.Management.Fluent;

namespace WPM_API.Azure.Core
{
    public class StorageService
    {
        private readonly AzureCredentials _credentials;

        public StorageService(AzureCredentials credentials)
        {
            this._credentials = credentials;
        }

        public string GetStorageAccConnectionString(string subscriptionId, string resGrpName, string storageAccName)
        {
            var azure = AZURE.Azure.Configure()
                .WithLogLevel(HttpLoggingDelegatingHandler.Level.Basic)
                .Authenticate(_credentials)
                .WithSubscription(subscriptionId);

            var storageAcc = azure.StorageAccounts.GetByResourceGroup(resGrpName, storageAccName);
            var storageKeys = storageAcc.GetKeys();

            // Build the connection string
            string storageConnectionString = "DefaultEndpointsProtocol=https;"
                + "AccountName=" + storageAcc.Name
                + ";AccountKey=" + storageKeys[0].Value
                + ";EndpointSuffix=core.windows.net";

            return storageConnectionString;
        }

        public string CreateSASKey(string connectionString, string folder, string permission, DateTime expireDate)
        {
            // string connectionString = "DefaultEndpointsProtocol=https;AccountName=stagingsysteme;AccountKey=fh5j6d5Y4eEvY/i43CgGgvT5pxXnxv/gN28Pfu8yLVeVEe6MsmdM0XLFoEED3SuuVErDdNo8gRP69goKROnrCg==;EndpointSuffix=core.windows.net";
            string sasKey;

            // Create SAS key for one year
            SharedAccessBlobPolicy accessBlobPolicy = null;
            if (permission == "read")
            {
                accessBlobPolicy = new SharedAccessBlobPolicy()
                {
                    SharedAccessStartTime = DateTime.UtcNow.AddDays(-1),
                    SharedAccessExpiryTime = expireDate,
                    Permissions = SharedAccessBlobPermissions.Read | SharedAccessBlobPermissions.List
                };
            }
            else if (permission == "write")
            {
                accessBlobPolicy = new SharedAccessBlobPolicy()
                {
                    SharedAccessStartTime = DateTime.UtcNow.AddDays(-1),
                    SharedAccessExpiryTime = expireDate,
                    Permissions = SharedAccessBlobPermissions.Write | SharedAccessBlobPermissions.Read | SharedAccessBlobPermissions.List
                };
            }

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);

            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(folder);

            sasKey = container.GetSharedAccessSignature(accessBlobPolicy, null);

            return sasKey;
        }

        public async Task<List<StorageAccount>> GetStorageAccounts(string subscriptionId, string ressourceGroupName)
        {
            using (var storageClient = new Microsoft.Azure.Management.Storage.StorageManagementClient(_credentials))
            {
                storageClient.SubscriptionId = subscriptionId;
                return (await storageClient.StorageAccounts.ListByResourceGroupAsync(ressourceGroupName)).ToList();
            }
        }

        public async Task<CloudBlobContainer> CreateBlobContainer(string connectionString, string containerName)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);

            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            CloudBlobContainer container = blobClient.GetContainerReference(containerName);

            bool result = await container.CreateIfNotExistsAsync();

            return container;
        }

        public async Task<IStorageAccount> AddStorageAccountAsync(string subscriptionId, string ressourceGroupName, string storageAccountName, string storageAccountType, string location, string kind)
        {
            var azure = AZURE.Azure.Configure()
                .WithLogLevel(HttpLoggingDelegatingHandler.Level.Basic)
                .Authenticate(_credentials)
                .WithSubscription(subscriptionId);

            var definition = azure.StorageAccounts.Define(storageAccountName)
                .WithRegion(location)
                .WithNewResourceGroup(ressourceGroupName)
                .WithSku(StorageAccountSkuType.FromSkuName(Microsoft.Azure.Management.Storage.Fluent.Models.SkuName.Parse(storageAccountType)))
                .WithOnlyHttpsTraffic();

            if (kind == "Storage")
            {
                return await definition.WithGeneralPurposeAccountKind().CreateAsync();
            }
            else if (kind == "Storage V2")
            {
                return await definition.WithGeneralPurposeAccountKindV2().CreateAsync();
            }
            else
            {
                return await definition.WithGeneralPurposeAccountKindV2().CreateAsync();
            }
        }

        public Task<IStorageAccount> EditStorageAccountAsync(string subscriptionId, string storageAccountId, string storageAccountType)
        {
            var azure = AZURE.Azure.Configure()
                .WithLogLevel(HttpLoggingDelegatingHandler.Level.Basic)
                .Authenticate(_credentials)
                .WithSubscription(subscriptionId);

            var storageAccount = azure.StorageAccounts.GetById(storageAccountId);
            return storageAccount
                .Update()
                .WithSku((Microsoft.Azure.Management.Storage.Fluent.Models.SkuName)Enum.Parse(typeof(Microsoft.Azure.Management.Storage.Fluent.Models.SkuName), storageAccountType, true))
                .ApplyAsync();
        }

        public async void DeleteStorageAccountAsync(string storageAccountId)
        {
            var azure = AZURE.Azure.Configure()
                .WithLogLevel(HttpLoggingDelegatingHandler.Level.Basic)
                .Authenticate(_credentials)
                .WithDefaultSubscription();

            await azure.StorageAccounts.DeleteByIdAsync(storageAccountId);
        }

        public Task<Microsoft.Azure.Management.Storage.Fluent.CheckNameAvailabilityResult> CheckNameAvailabilityAsync(string name)
        {
            var azure = AZURE.Azure.Configure()
                .WithLogLevel(HttpLoggingDelegatingHandler.Level.Basic)
                .Authenticate(_credentials)
                .WithDefaultSubscription();
            return azure.StorageAccounts.CheckNameAvailabilityAsync(name);
        }

        public async Task<IStorageAccount> GetStorageAccountAsync(string storageAccountId)
        {
            var azure = AZURE.Azure.Configure()
                .WithLogLevel(HttpLoggingDelegatingHandler.Level.Basic)
                .Authenticate(_credentials)
                .WithDefaultSubscription();
            return await azure.StorageAccounts.GetByIdAsync(storageAccountId);
        }
    }
}
