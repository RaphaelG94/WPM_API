using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WPM_API.Data.DataContext.Entities.AssetMgmt;
using WPM_API.Data.DataContext.Entities.SmartDeploy;
using WPM_API.Data.DataContext.Interfaces;

namespace  WPM_API.Data.DataContext.Entities
{
    public class Client : IEntity, IDeletable
    {
        [Key, Column("PK_Client")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string UUID { get; set; }
        public string AssetId { get; set; }
        /// Computername
        public string Name { get; set; }
        public string Description { get; set; }
        public string WdsIp { get; set; }
        public string Unattend { get; set; }
        public string CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }
        public string OrganizationalUnitId { get; set; }
        [ForeignKey("OrganizationalUnitId")]
        public virtual OrganizationalUnit OrganizationalUnit { get; set; }
        public List<MacAddress> MacAddresses { get; set; }
        public virtual List<ClientSoftware> AssignedSoftware { get; set; }
        public virtual List<ClientTask> Tasks { get; set; }
        public virtual List<ClientOption> AssignedOptions { get; set; }
        public string Vendor { get; set; }
        public string UsageStatus { get; set; }
        public OS Os { get; set; }
        public Bios Bios { get; set; }
        public Hardware Hardware { get; set; }
        public NetworkConfiguration Network { get; set; }
        public Purchase Purchase { get; set; }
        public List<HDDPartition> Partition { get; set; }
        public string JoinedDomain { get; set; }
        public string Proxy { get; set; }
        public string Location { get; set; }
        public string InstallationDate { get; set; }
        public string MainUser { get; set; }
        public string Subnet { get; set; }
        public virtual List<ClientClientProperty> Properties { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedByUserId { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string DeletedByUserId { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string CloudFlag { get; set; }
        public string BaseId { get; set; }
        [ForeignKey("BaseId")]
        public Base Base { get; set; }
        // public List<PreinstalledSoftware> PreinstalledSoftwares { get; set; }
        public DateTime LastInventoryUpdate { get; set; }
        [ForeignKey("InventoryId")]
        public string InventoryId { get; set; }
        public string AssetModelId { get; set; }
        public List<ActivityLog> ActivityLogs { get; set; }
        public string SerialNumber { get; set; }
        public string Type { get; set; }
        public string HyperVisor { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public bool IsOnline { get; set; }
        public DateTime LastOnlineStatusUpdate { get; set; }
        public string OSSettingsImageId { get; set; }
        public string UsernameLinux { get; set; }
        public string UserPasswordLinux { get; set; }
        public string AdminPasswordLinux { get; set; }
        public string PartitionEncryptionPassLinux { get; set; }
        public string KeyboardLayoutWindows { get; set; }
        public string TimeZoneWindows { get; set; }
        public string TimeZoneLinux { get; set; }
        public string KeyboardLayoutLinux { get; set; }
        public string OSEdition { get; set; }
        public string OSMemorySize { get; set; }
        public string OSOperatingSystemSKU { get; set; }
        public string OSArchitecture { get; set; }
        public string CSPvendor { get; set; }
        public DateTime OSInstallDateUTC { get; set; }
        public string OSType { get; set; }
        public string OSVersion { get; set; }
        public string ModelSeries { get; set; }
        public string CSPversion { get; set; }
        public string CSPname { get; set; }
        public string OSLanguage { get; set; }
        public string OSProductSuite { get; set; }
        public string Processor { get; set; }
        public string MainFrequentUser { get; set; }
        public string DownloadSeedURL { get; set; }
        public string LanguagePackLinux { get; set; }
        public string Timezone { get; set; }
        public string InstallScript { get; set; }
        public string BaseLineFile1 { get; set; }
        // Inventory File
        public string BaseLineFile2 { get; set; }
        public string BaseLineFile3 { get; set; }
        public string OSTypeDevice { get; set; }
        public string LocalAdminUsername { get; set; }
        public string LocalAdminPassword { get; set; }
    }
}
