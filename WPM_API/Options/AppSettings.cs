namespace WPM_API.Options
{
    public class AppSettings
    {
        /* Azure Zugangsdaten */
        public string TenantId { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }

        /* Development Zugangsdaten */
        public string DevelopmentTenantId { get; set; }
        public string DevelopmentClientId { get; set; }
        public string DevelopmentClientSecret { get; set; }

        /* Storage Zugangsdaten */
        public string AzureStoragePath { get; set; }
        public string StorageAccountName { get; set; }
        public string StorageAccountKey { get; set; }

        /* SmartDeploy Source Folder */
        public string SmartDeploySources { get; set; }
        /* Folder für weitere Ressourcen */
        public string ResourcesRepositoryFolder { get; set; }
        /* Folder der Repositories für Fileupload*/
        public string FileRepositoryFolder { get; set; }
        /* Folder für temporäre Aktionen (File Upload in Schwebe, CSE execution)*/
        public string TempFolder { get; set; }
        public string IconsAndBanners { get; set; }
        /* Destination Connection String for BitStream files */
        public string FileDestConnectionString { get; set; }
        public string LiveSystemConnectionString { get; set; }
    }
}
