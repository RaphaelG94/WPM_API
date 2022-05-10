using System;
using System.Collections.Generic;
using System.Text;

namespace WPM_API.Azure.Models
{
    public class StorageCredentialModel
    {
        public string ScriptAzureStoragePath { get; set; }
        public string ScriptStorageAccountName { get; set; }
        public string ScriptStorageAccountKey { get; set; }
        public string ScriptStorageConnectionString { get; set; }
    }
}
