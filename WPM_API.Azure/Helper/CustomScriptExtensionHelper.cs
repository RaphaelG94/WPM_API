using System.Collections.Generic;
using WPM_API.Azure.Models;
using Microsoft.Azure.Management.Compute.Fluent.VirtualMachine.Definition;
using Microsoft.Azure.Management.Compute.Fluent.VirtualMachine.Update;

namespace  WPM_API.Azure.Core
{
    public class CustomScriptExtensionHelper
    {
        readonly static string windowsCustomScriptExtensionPublisherName = "Microsoft.Compute";
        readonly static string windowsCustomScriptExtensionTypeName = "CustomScriptExtension";
        readonly static string windowsCustomScriptExtensionVersionName = "1.8";

        private string AzureStoragePath;
        private string StorageAccountName;
        private string StorageAccountKey;

        public CustomScriptExtensionHelper(string storageAccountName, string storageAccountKey, string azureStoragePath)
        {
            AzureStoragePath = azureStoragePath;
            StorageAccountName = storageAccountName;
            StorageAccountKey = storageAccountKey;
        }

        public IUpdate AddCSE(IUpdate vm, string scriptName, string scriptArguments, params string[] files)
        {
            return AddCSE(vm, scriptName, scriptArguments, scriptName, files);
        }

        public IUpdate AddCSE(IUpdate vm, string scriptName, string scriptArguments, string cseName, params string[] files)
        {
            // Kein Script übergeben
            if (string.IsNullOrEmpty(scriptName))
            {
                return vm;
            }

            // Fix spaces in scriptname
            scriptName = scriptName.Replace(" ", "");

            List<string> fileUris = new List<string>()
            {
                AzureStoragePath + scriptName
            };
            if(files!=null)
           {
                foreach (string f in files)
                {
                    fileUris.Add(AzureStoragePath + f);
                }
            }


            return vm.DefineNewExtension(cseName)
                .WithPublisher(windowsCustomScriptExtensionPublisherName)
                .WithType(windowsCustomScriptExtensionTypeName)
                .WithVersion(windowsCustomScriptExtensionVersionName)
                .WithMinorVersionAutoUpgrade()
                .WithPublicSetting("storageAccountName", StorageAccountName)
                .WithPublicSetting("storageAccountKey", StorageAccountKey)
                .WithPublicSetting("fileUris", fileUris)
                .WithPublicSetting("commandToExecute", "powershell.exe -ExecutionPolicy Unrestricted -File \"" + scriptName + "\" " + scriptArguments)
                .Attach();
        }
    }
}