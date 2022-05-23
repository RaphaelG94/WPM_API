using WPM_API.Code.Infrastructure;
using WPM_API.Code.Infrastructure.LogOn;
using WPM_API.Options;

namespace WPM_API.Controllers
{
    public class DomainUserController : BasisController
    {

        //public async Task<Domain> CreateDomainUsers(string customerId, string baseId, string domainId, FileRef fileRef, TempRepository tempRepository)
        //{
        //    Domain dbDomain = null;
        //    File file = null;
        //    using (var unitOfWork = CreateUnitOfWork())
        //    {
        //        file = unitOfWork.Files.GetByGuid(fileRef.Id);
        //        dbDomain = unitOfWork.Domains.Get(domainId);
        //        dbDomain.Status = "Creating Domain Users.";
        //        dbDomain.DomainUserCSV = file;
        //        unitOfWork.Domains.MarkForUpdate(dbDomain);
        //        unitOfWork.SaveChanges();
        //    }

        //    //DomainFileRepository domainFileRepository = new DomainFileRepository(connectionStrings.FileRepository, appSettings.DomainFileRepositoryFolder);
        //    CloudBlockBlob blob = tempRepository.GetBlobFile(file.Guid);
        //    await GenerateUsersFromCSV(blob, domainId, true);
        //    CreateUsersInAd(customerId, domainId, baseId, file.Guid);

        //    // Return something so that this method's execution can be awaited.
        //    return dbDomain;
        //}


        ///// <summary>
        ///// Creates the User in the AD
        ///// </summary>
        ///// <param name="fileRepository"></param>
        ///// <param name="domainUserRepositoryFolder"></param>
        ///// <param name="id"></param>
        //private async void CreateUsersInAd(string customerId, string domainId, string baseId, string fileId)
        //{
        //    AzureCredentials credentials = GetCurrentAzureCredentials(customerId);
        //    AzureCommunicationService azure = new AzureCommunicationService(credentials.TenantId, credentials.ClientId, credentials.ClientSecret);

        //    using (var unitOfWork = CreateUnitOfWork())
        //    {
        //        string subscriptionId = unitOfWork.Bases.Get(baseId, "Subscription").Subscription.AzureId;
        //        string vmId = unitOfWork.Domains.Get(domainId, "Servers", "Servers.VirtualMachine").Servers.First(x => x.Type.Equals(ServerType.ADController)).VirtualMachine.AzureId;
        //        VirtualMachineRefModel virtualMachineRefModel = new VirtualMachineRefModel() { SubscriptionId = subscriptionId, VmId = vmId };

        //        // Execute Script with Params
        //        //string AzureStoragePath = "https://bitstream.blob.core.windows.net/scripts/";
        //        var credentialModel = new Azure.Models.StorageCredentialModel()
        //        {
        //            ScriptAzureStoragePath = appSettings.AzureStoragePath,
        //            ScriptStorageAccountKey = appSettings.StorageAccountKey,
        //            ScriptStorageAccountName = appSettings.StorageAccountName
        //        };
        //        string connectionString = connectionStrings.FileRepository;
        //        await azure.VirtualMachineService().ExecuteVirtualMachineScriptAsync(virtualMachineRefModel, credentialModel, "ImportUser" + ".ps1", "-Path " + fileId, new StorageFileModel() { Name = fileId, Path = credentialModel.ScriptAzureStoragePath });
        //        //azure.VirtualMachineService().ExecuteVirtualMachineScriptAsync(virtualMachineRefModel, "ImportUser.ps1", "-Path " + fileId, new StorageFileModel() { Name = fileId, Path = AzureStoragePath });
        //    }
        //}


        /// <summary>
        /// Generate users from a csv file.
        /// The first line (header) is always skipped.
        /// Columns userpw and otherattributes are ignored.
        /// CSV headers have to match Model, i.e. PascalCase.
        /// </summary>
        /// <returns>[CSVUploadUser]</returns>    
        //    private async Task<List<DomainUser>> GenerateUsersFromCSV(CloudBlob file, string domainId, bool hasHeader)
        //    {
        //        Stream stream = await file.OpenReadAsync();
        //        List<DomainUser> users = new List<DomainUser>();
        //        using (var csvStream = new StreamReader(stream))
        //        {
        //            var csvReader = ConfigureCsvReader(csvStream, hasHeader);
        //            List<DomainUser> domainUsers = null;
        //            using (var unitOfWork = CreateUnitOfWork())
        //            {
        //                domainUsers = unitOfWork.DomainUser.GetAll().Where(x => x.DomainId == domainId).ToList();

        //                while (csvReader.Read())
        //                {
        //                    DomainUserViewModel domainUserRecord = csvReader.GetRecord<DomainUserViewModel>();
        //                    var oldUser = domainUsers.Find(x => x.SamAccountName == domainUserRecord.SamAccountName);
        //                    if (oldUser != null)
        //                    {
        //                        Mapper.Map(domainUserRecord, oldUser);
        //                        unitOfWork.DomainUser.MarkForUpdate(oldUser);
        //                        users.Add(oldUser);
        //                    }
        //                    else
        //                    {
        //                        DomainUser newUser = new DomainUser();
        //                        //Mapper.Map(domainUserRecord, newUser);
        //                        newUser.Name = domainUserRecord.Name;
        //                        newUser.UserGivenName = domainUserRecord.UserGivenName;
        //                        newUser.UserLastName = domainUserRecord.UserLastName;
        //                        newUser.SamAccountName = domainUserRecord.SamAccountName;
        //                        newUser.UserPrincipalName = domainUserRecord.UserPrincipalName;
        //                        newUser.MemberOf = domainUserRecord.MemberOf;
        //                        newUser.Description = domainUserRecord.Description;
        //                        newUser.Displayname = domainUserRecord.Displayname;
        //                        newUser.Workphone = domainUserRecord.Workphone;
        //                        newUser.Email = domainUserRecord.Email;

        //                        newUser.DomainId = domainId;
        //                        users.Add(newUser);
        //                        unitOfWork.DomainUser.MarkForInsert(newUser);
        //                    }
        //                    unitOfWork.SaveChanges();
        //                }
        //            }
        //        }

        //        return users;
        //        //var json = JsonConvert.SerializeObject(Mapper.Map<List<DomainUser>>(users), serializerSettings);
        //        //return new OkObjectResult(json);
        //    }

        //    private CsvReader ConfigureCsvReader(StreamReader csvStream, bool hasHeader)
        //    {
        //        CsvReader csvReader = new CsvReader(csvStream);
        //        csvReader.Configuration.MissingFieldFound = null;
        //        csvReader.Configuration.HasHeaderRecord = hasHeader;
        //        csvReader.Configuration.IgnoreQuotes = true;
        //        // Trim
        //        csvReader.Configuration.PrepareHeaderForMatch = header => header?.Trim();

        //        // Remove whitespace
        //        csvReader.Configuration.PrepareHeaderForMatch = header => header.Replace(" ", string.Empty);

        //        // Remove underscores
        //        csvReader.Configuration.PrepareHeaderForMatch = header => header.Replace("_", string.Empty);

        //        // Ignore case
        //        csvReader.Configuration.PrepareHeaderForMatch = header => header.ToLower();
        //        csvReader.Read();
        //        if (hasHeader)
        //        {
        //            csvReader.ReadHeader();
        //        }
        //        return csvReader;
        //    }
        public DomainUserController(AppSettings appSettings, ConnectionStrings connectionStrings, OrderEmailOptions orderEmailOptions, AgentEmailOptions agentEmailOptions, SendMailCreds sendMailCreds, SiteOptions siteOptions, ILogonManager logonManager) : base(appSettings, connectionStrings, orderEmailOptions, agentEmailOptions, sendMailCreds, siteOptions, logonManager)
        {
        }
    }
}