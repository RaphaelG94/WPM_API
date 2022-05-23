namespace WPM_API.Options
{
    public class AppSettings
    {
        /* Azure Zugangsdaten */
        public string TenantId { get; set; } = String.Empty;
        public string ClientId { get; set; } = String.Empty;
        public string ClientSecret { get; set; } = String.Empty;

        /* Development Zugangsdaten */
        public string DevelopmentTenantId { get; set; } = String.Empty;
        public string DevelopmentClientId { get; set; } = String.Empty;
        public string DevelopmentClientSecret { get; set; } = String.Empty;

        /* Storage Zugangsdaten */
        public string AzureStoragePath { get; set; } = String.Empty;
        public string StorageAccountName { get; set; } = String.Empty;
        public string StorageAccountKey { get; set; } = String.Empty;

        /* SmartDeploy Source Folder */
        public string SmartDeploySources { get; set; } = String.Empty;
        /* Folder für weitere Ressourcen */
        public string ResourcesRepositoryFolder { get; set; } = String.Empty;
        /* Folder der Repositories für Fileupload*/
        public string FileRepositoryFolder { get; set; } = String.Empty;
        /* Folder für temporäre Aktionen (File Upload in Schwebe, CSE execution)*/
        public string TempFolder { get; set; } = String.Empty;
        public string IconsAndBanners { get; set; } = String.Empty;
        /* Destination Connection String for BitStream files */
        public string FileDestConnectionString { get; set; } = String.Empty;
        public string LiveSystemConnectionString { get; set; } = String.Empty;
    }
}
