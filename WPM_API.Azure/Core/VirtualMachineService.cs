using System.Collections.Generic;
using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;
using AZURE = Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;
using WPM_API.Azure.Models;
using System.Threading.Tasks;
using System;
using System.IO;
using System.Linq;
using Microsoft.Azure.Management.Compute.Fluent;
using Microsoft.Azure.Management.Compute.Fluent.Models;
using System.Reflection;
using WPM_API.FileRepository;

namespace  WPM_API.Azure.Core
{
    public class VirtualMachineService
    {
        private AzureCredentials _credentials;

        public VirtualMachineService(AzureCredentials _credentials)
        {
            this._credentials = _credentials;
        }

        public async Task<VirtualMachineModel> GetVirtualMachineAsync(string subscriptionId, string resourceGroupName, string id)
        {

            var azure = AZURE.Azure.Configure()
                .WithLogLevel(HttpLoggingDelegatingHandler.Level.Basic)
                .Authenticate(_credentials)
                .WithSubscription(subscriptionId);

            var virtualMachine = azure.VirtualMachines.ListByResourceGroup(resourceGroupName).Where(x => x.Id == id).FirstOrDefault();

            var networkInterface = azure.NetworkInterfaces.GetById(virtualMachine.PrimaryNetworkInterfaceId);
            var network = azure.Networks.GetByIdAsync(networkInterface.PrimaryIPConfiguration.NetworkId);

            VirtualMachineModel vm = new VirtualMachineModel();
            if (virtualMachine.StorageProfile.DataDisks.Count > 0)
            {
                vm.AdditionalDisk = new Disk(virtualMachine.StorageProfile.DataDisks[0].Name, virtualMachine.StorageProfile.DataDisks[0].DiskSizeGB);
            }
            vm.Admin = new AdminCredentials(virtualMachine.OSProfile.AdminUsername, virtualMachine.OSProfile.AdminPassword);
            vm.Id = virtualMachine.Id;
            vm.Name = virtualMachine.Name;
            vm.Network = new VirtualMachineNetwork((await network).Name, networkInterface.PrimaryIPConfiguration.SubnetName);
            // vm.OperatingSystem = (OperatingSystem)Enum.Parse(typeof(OperatingSystem), v.StorageProfile.ImageReference.Offer + v.StorageProfile.ImageReference.Sku.Replace("-", ""));
            vm.OperatingSystem = virtualMachine.StorageProfile.ImageReference.Sku;
            vm.ResourceGroupName = virtualMachine.ResourceGroupName;
            vm.SubscriptionId = subscriptionId;
            vm.Type = virtualMachine.Size.Value;
            if (virtualMachine.OSDiskId != null)
            {
                var osDisk = azure.Disks.GetById(virtualMachine.OSDiskId);
                vm.SystemDisk = new Disk(osDisk.Name, osDisk.SizeInGB);
            }
            return vm;
        }

        public async Task<List<VirtualMachineModel>> GetVirtualMachinesAsync(string subscriptionId)
        {
            var azure = AZURE.Azure.Configure()
                .WithLogLevel(HttpLoggingDelegatingHandler.Level.Basic)
                .Authenticate(_credentials)
                .WithSubscription(subscriptionId);

            var virtualMachines = azure.VirtualMachines.List();

            List<VirtualMachineModel> result = new List<VirtualMachineModel>();
            foreach (var v in virtualMachines)
            {
                var networkInterface = azure.NetworkInterfaces.GetById(v.PrimaryNetworkInterfaceId);
                var network = azure.Networks.GetByIdAsync(networkInterface.PrimaryIPConfiguration.NetworkId);

                VirtualMachineModel vm = new VirtualMachineModel();
                if (v.StorageProfile.DataDisks.Count > 0)
                {
                    vm.AdditionalDisk = new Disk(v.StorageProfile.DataDisks[0].Name, v.StorageProfile.DataDisks[0].DiskSizeGB);
                }
                vm.Admin = new AdminCredentials(v.OSProfile.AdminUsername, v.OSProfile.AdminPassword);
                vm.Id = v.Id;
                vm.Name = v.Name;
                vm.Network = new VirtualMachineNetwork((await network).Name, networkInterface.PrimaryIPConfiguration.SubnetName);
                // vm.OperatingSystem = (OperatingSystem)Enum.Parse(typeof(OperatingSystem), v.StorageProfile.ImageReference.Offer + v.StorageProfile.ImageReference.Sku.Replace("-", ""));
                vm.OperatingSystem = v.StorageProfile.ImageReference.Sku;
                vm.ResourceGroupName = v.ResourceGroupName;
                vm.SubscriptionId = subscriptionId;
                vm.Type = v.Size.Value;
                if (v.OSDiskId != null)
                {
                    var osDisk = azure.Disks.GetById(v.OSDiskId);
                    vm.SystemDisk = new Disk(osDisk.Name, osDisk.SizeInGB);
                }
                result.Add(vm);
            }

            return result;
        }

        public async Task<VirtualMachineModel> AddVirtualMachinesAsync(VirtualMachineAddModel vm)
        {
            var azure = AZURE.Azure.Configure()
                .WithLogLevel(HttpLoggingDelegatingHandler.Level.Basic)
                .Authenticate(_credentials)
                .WithSubscription(vm.SubscriptionId);

            var network = await azure.Networks.GetByIdAsync(vm.Network.VirtualNetworkId);
            var storageAccount = await azure.StorageAccounts.GetByIdAsync(vm.StorageAccountId);
            var resGrp = await azure.ResourceGroups.GetByNameAsync(vm.ResourceGroupName);

            ImageReference image = new ImageReference();
            image.SetInner(new ImageReferenceInner(null, "MicrosoftWindowsServer", "WindowsServer", vm.OperatingSystem.ToString(), "latest"));

            // Create VM
            IVirtualMachine machine = null;
            if (vm.AdditionalDisk != null)
            {
                machine = await azure.VirtualMachines.Define(vm.Name)
                        .WithRegion(vm.Location)
                        .WithExistingResourceGroup(resGrp)
                        .WithExistingPrimaryNetwork(network)
                        .WithSubnet(vm.Network.SubnetName)
                        .WithPrimaryPrivateIPAddressDynamic()
                        .WithoutPrimaryPublicIPAddress()
                        .WithSpecificWindowsImageVersion(image)
                        .WithAdminUsername(vm.Admin.Username)
                        .WithAdminPassword(vm.Admin.Password)
                        .WithNewDataDisk(vm.AdditionalDisk.SizeInGb)
                        .WithSize(vm.Type)
                        .WithOSDiskName(vm.SystemDisk.Name)
                        .WithOSDiskSizeInGB(vm.SystemDisk.SizeInGb)
                        .WithExistingStorageAccount(storageAccount)
                        .CreateAsync();
                /*
                ;
                ; 
                // TODO: Add Name
                //.WithUnmanagedDisks().DefineUnmanagedDataDisk(vm.AdditionalDisk.Name).WithNewVhd(vm.AdditionalDisk.SizeInGb).WithCaching(CachingTypes.ReadWrite).WithLun(1).StoreAt(storageAccount.Name, vm.Name, vm.AdditionalDisk.Name).Attach()
                //.DefineUnmanagedDataDisk(vm.AdditionalDisk.Name).WithNewVhd(vm.SystemDisk.SizeInGb).WithCaching(CachingTypes.ReadWrite).WithLun(0).StoreAt(storageAccount.Name, vm.Name, vm.AdditionalDisk.Name).Attach()
                
                .
                .Create();
                */
            }
            else
            {
                machine = await azure.VirtualMachines.Define(vm.Name)
                .WithRegion(vm.Location)
                .WithExistingResourceGroup(vm.ResourceGroupName)
                .WithExistingPrimaryNetwork(network)
                .WithSubnet(vm.Network.SubnetName)
                .WithPrimaryPrivateIPAddressDynamic()
                .WithoutPrimaryPublicIPAddress()
                .WithSpecificWindowsImageVersion(image)
                .WithAdminUsername(vm.Admin.Username)
                .WithAdminPassword(vm.Admin.Password)
                .WithSize(vm.Type)
                .WithOSDiskName(vm.SystemDisk.Name)
                .WithOSDiskSizeInGB(vm.SystemDisk.SizeInGb)
                .WithExistingStorageAccount(storageAccount).CreateAsync();
            }

            // Rückgabewerte
            var v = azure.VirtualMachines.GetById(machine.Id);

            var ni = azure.NetworkInterfaces.GetById(v.PrimaryNetworkInterfaceId);
            var n = azure.Networks.GetByIdAsync(ni.PrimaryIPConfiguration.NetworkId);

            VirtualMachineModel result = new VirtualMachineModel();
            if (v.DataDisks.Count > 0)
            {
                result.SystemDisk = new Disk(v.DataDisks[0].Name, v.DataDisks[0].Size);
            }
            if (v.DataDisks.Count > 1)
            {
                result.AdditionalDisk = new Disk(v.DataDisks[1].Name, v.DataDisks[1].Size);
            }
            result.Admin = new AdminCredentials(v.OSProfile.AdminUsername, v.OSProfile.AdminPassword);
            result.Id = v.Id;
            result.Name = v.Name;
            result.Network = new VirtualMachineNetwork((await n).Name, ni.PrimaryIPConfiguration.SubnetName);
            // vm.OperatingSystem = (OperatingSystem)Enum.Parse(typeof(OperatingSystem), v.StorageProfile.ImageReference.Offer + v.StorageProfile.ImageReference.Sku.Replace("-", ""));
            result.OperatingSystem = v.StorageProfile.ImageReference.Sku;
            result.ResourceGroupName = v.ResourceGroupName;
            result.SubscriptionId = vm.SubscriptionId;
            result.Type = v.Size.Value;
            result.LocalIp = v.GetPrimaryNetworkInterface().PrimaryPrivateIP;

            while (machine.PowerState != PowerState.Running)
            {
                System.Threading.Thread.Sleep(50);
            };

            return result;
        }

        public async Task<VirtualMachineModel> EditVirtualMachinesAsync(VirtualMachineEditModel vm)
        {
            var azure = AZURE.Azure.Configure()
                .WithLogLevel(HttpLoggingDelegatingHandler.Level.Basic)
                .Authenticate(_credentials)
                .WithSubscription(vm.SubscriptionId);

            var virtualMachine = azure.VirtualMachines.GetById(vm.Id);
            var additionalDisk = azure.Disks
                .Define(vm.AdditionalDisk.Name)
                .WithRegion(virtualMachine.RegionName)
                .WithExistingResourceGroup(vm.ResourceGroupName)
                .WithData()
                .WithSizeInGB(vm.AdditionalDisk.SizeInGb);

            var machine = virtualMachine.Update()
                .WithNewDataDisk(additionalDisk)
                .WithSize(vm.Type)
                .Apply();

            // Rückgabewerte
            var v = azure.VirtualMachines.GetById(machine.Id);

            var ni = azure.NetworkInterfaces.GetById(v.PrimaryNetworkInterfaceId);
            var n = azure.Networks.GetByIdAsync(ni.PrimaryIPConfiguration.NetworkId);

            VirtualMachineModel result = new VirtualMachineModel();
            if (v.DataDisks.Count > 0)
            {
                result.SystemDisk = new Disk(v.DataDisks[0].Name, v.DataDisks[0].Size);
            }
            if (v.DataDisks.Count > 1)
            {
                result.AdditionalDisk = new Disk(v.DataDisks[1].Name, v.DataDisks[1].Size);
            }
            result.Admin = new AdminCredentials(v.OSProfile.AdminUsername, v.OSProfile.AdminPassword);
            result.Id = v.Id;
            result.Name = v.Name;
            result.Network = new VirtualMachineNetwork((await n).Name, ni.PrimaryIPConfiguration.SubnetName);
            // vm.OperatingSystem = (OperatingSystem)Enum.Parse(typeof(OperatingSystem), v.StorageProfile.ImageReference.Offer + v.StorageProfile.ImageReference.Sku.Replace("-", ""));
            result.OperatingSystem = v.StorageProfile.ImageReference.Sku;
            result.ResourceGroupName = v.ResourceGroupName;
            result.SubscriptionId = vm.SubscriptionId;
            result.Type = v.Size.Value;
            return result;
        }



        public void DeleteVirtualMachine(string subscriptionId, string id)
        {
            var azure = AZURE.Azure.Configure()
                .WithLogLevel(HttpLoggingDelegatingHandler.Level.Basic)
                .Authenticate(_credentials)
                .WithSubscription(subscriptionId);
            azure.VirtualMachines.DeleteById(id);
        }

        /*public async Task<IVirtualMachine> ExecuteVirtualMachineScriptAsync(VirtualMachineRefModel virtualMachineRefModel, StorageCredentialModel credentials, string connectionString, string repositoryName, string scriptName, string arguments)
        {
            string fileName = await CopyEmbeddedScriptToFolderAsync(scriptName, connectionString, repositoryName);
            // Execute Script with Params
            //var credentialModel = new Azure.Models.StorageCredentialModel()
            //{
            //    // TODO: Read these from appsettings.json or similar without moving Startup/AppSettings to TransferModels.
            //    ScriptAzureStoragePath = "https://devbitstream.blob.core.windows.net/" + repositoryName + "/",
            //    ScriptStorageAccountKey = "Vvxsro+D5hMv8E23pe6/gsM5cb2L12dw9ZYV9COL/FZWmuWhdbDRga5Rrz5CKvcgOrRSQRRaNfliJrjYJpISeQ==",
            //    ScriptStorageAccountName = "devbitstream"
            //};

            return await ExecuteVirtualMachineScriptAsync(
                virtualMachineRefModel,
                credentials,
                fileName,
                arguments
            );
        }*/
        /*
        public async Task<IVirtualMachine> ExecuteVirtualMachineScriptAsync(VirtualMachineRefModel virtualMachineRefModel, StorageCredentialModel credentials, string connectionString, string repositoryName, string scriptName, string arguments, params string[] files)
        {
            string fileName = await CopyEmbeddedScriptToFolderAsync(scriptName, connectionString, repositoryName);
            // Execute Script with Params
            //var credentialModel = new Azure.Models.StorageCredentialModel()
            //{
            //    ScriptAzureStoragePath = "https://devbitstream.blob.core.windows.net/" + repositoryName + "/",
            //    ScriptStorageAccountKey = "Vvxsro+D5hMv8E23pe6/gsM5cb2L12dw9ZYV9COL/FZWmuWhdbDRga5Rrz5CKvcgOrRSQRRaNfliJrjYJpISeQ==",
            //    ScriptStorageAccountName = "devbitstream"
            //};
            List<StorageFileModel> storageFiles = new List<StorageFileModel>();
            foreach(string f in files) {
                storageFiles.Add(new StorageFileModel() { Name = f, Path = credentials.ScriptAzureStoragePath  });
            }
            if (scriptName.Equals("ConfigureGPO.ps1"))
            {
                await CopyGPOScriptsToFolderAsync(connectionString, repositoryName);
            }
            return await ExecuteVirtualMachineScriptAsync(
                virtualMachineRefModel,
                credentials,
                fileName,
                arguments,
                storageFiles.ToArray()
            );
        }
        */
        /*
        public async Task<IVirtualMachine> ExecuteVirtualMachineScriptAsync(VirtualMachineRefModel virtualMachineRefModel, string scriptName, string arguments, params string[] files)
        {
            string ScriptAzureStoragePath = "https://bitstream.blob.core.windows.net/scripts/";
            List<StorageFileModel> storageFiles = new List<StorageFileModel>();
            foreach(string f in files) {
                storageFiles.Add(new StorageFileModel() { Name = f, Path = ScriptAzureStoragePath  });
            }
            return await ExecuteVirtualMachineScriptAsync(virtualMachineRefModel, scriptName, arguments, storageFiles.ToArray());
        }
        *//*
        public async Task<IVirtualMachine> ExecuteVirtualMachineScriptAsync(VirtualMachineRefModel virtualMachineRefModel, string scriptName, string arguments, params StorageFileModel[] files)
        {
            // Execute Script with Params
            var credentialModel = new Azure.Models.StorageCredentialModel()
            {
                ScriptAzureStoragePath = "https://bitstream.blob.core.windows.net/scripts/",
                ScriptStorageAccountKey = "9ADGFP3TKQic67WsW3p3E/1lp/9X4koprnXNHDZTEtPhY/ZeFmJDWM79PeH8kvbdM9WWQyLk80BNB5xB0Z3bVw==",
                ScriptStorageAccountName = "bitstream"
            };

            return await ExecuteVirtualMachineScriptAsync(
                virtualMachineRefModel,
                credentialModel,
                scriptName, 
                arguments,
                files
            );
        }
        */
        public Task<IVirtualMachine> ExecuteVirtualMachineScriptAsync(StorageCredentialModel credentials, VirtualMachineRefModel virtualMachineRefModel, string scriptName, string arguments, params string[] files)
        {
            CustomScriptExtensionHelper cse = new CustomScriptExtensionHelper(credentials.ScriptStorageAccountName, credentials.ScriptStorageAccountKey, credentials.ScriptAzureStoragePath);
            var azure = AZURE.Azure.Configure()
                .WithLogLevel(HttpLoggingDelegatingHandler.Level.Basic)
                .Authenticate(_credentials)
                .WithSubscription(virtualMachineRefModel.SubscriptionId);
            var machine = azure.VirtualMachines.GetById(virtualMachineRefModel.VmId).Update();

            var vm = cse.AddCSE(machine, scriptName, arguments, files).Apply();
            return vm.Update().WithoutExtension(scriptName).ApplyAsync();
        }

        /*
        /// <summary>
        /// Executes a script from the storagepath specified in the credentials on the virtualmachine VmId.
        /// </summary>
        /// <param name="virtualMachineRefModel"></param>
        /// <param name="credentials"></param>
        /// <param name="scriptName"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public Task<IVirtualMachine> ExecuteVirtualMachineScriptAsync(StorageCredentialModel credentials, VirtualMachineRefModel virtualMachineRefModel, string scriptName, string arguments)
        {
            CustomScriptExtensionHelper cse = new CustomScriptExtensionHelper(credentials.ScriptStorageAccountName, credentials.ScriptStorageAccountKey, credentials.ScriptAzureStoragePath);
            var azure = AZURE.Azure.Configure()
                .WithLogLevel(HttpLoggingDelegatingHandler.Level.Basic)
                .Authenticate(_credentials)
                .WithSubscription(virtualMachineRefModel.SubscriptionId);
            var machine = azure.VirtualMachines.GetById(virtualMachineRefModel.VmId).Update();

            var vm = cse.AddCSE(machine, scriptName, arguments, "executeScript").Apply();
            return vm.Update().WithoutExtension("executeScript").ApplyAsync();
        }*/

        public List<string> GetImages()
        {
            var azure = AZURE.Azure.Configure()
                .WithLogLevel(HttpLoggingDelegatingHandler.Level.Basic)
                .Authenticate(_credentials)
                .WithDefaultSubscription();

            var publishers = azure
                    .VirtualMachineImages
                    .Publishers
                    .ListByRegion(Region.EuropeWest);

            List<string> result = new List<string>();

            foreach (var publisher in publishers)
            {
                if (StringComparer.OrdinalIgnoreCase.Equals(publisher.Name, "MicrosoftWindowsServer"))
                {
                    foreach (var offer in publisher.Offers.List())
                    {
                        if (StringComparer.OrdinalIgnoreCase.Equals(offer.Name, "WindowsServer"))
                        {
                            foreach (var sku in offer.Skus.List())
                            {
                                result.Add(sku.Name);
                            }
                        }
                    }
                }
            }
            return result;
        }
                
        private static async Task CopyGPOScriptsToFolderAsync(string connectionString,
            string folderName)
        {
            var assembly = Assembly.Load("WPM_API.Web");
            var allResourceNames = assembly.GetManifestResourceNames();
            var CompanyIndex = "BitStream_v1709._1806._14.";
            var resourcePath = "WPM_API.Web.Resources.ScriptFolder." + CompanyIndex + ".zip";
            Stream stream = assembly.GetManifestResourceStream(resourcePath);
            FileRepository.FileRepository tempRepository =
                new FileRepository.FileRepository(connectionString, folderName);
            string id = await tempRepository.UploadFile(CompanyIndex + ".zip", stream);
        }

    }
}
