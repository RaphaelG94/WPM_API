using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace  WPM_API.TransferModels.SmartDeploy
{
    public class SoftwareAssignRefViewModel
    {
        public string Id { get; set; }
        public bool Required { get; set; }
    }

    public class SoftwareAssignViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public bool Required { get; set; }
    }

    public class SoftwareClientViewModel : SoftwareViewModel
    {
        public bool? Required { get; set; }
    }

    public class SoftwareViewModel : SoftwareEditViewModel
    {
        public string Id { get; set; }
    }

    public class SoftwareAddViewModel
    {
        public string Name { get; set; }
        public FileRef Icon { get; set; }
        public string Version { get; set; }
        public RuleAddViewModel RuleApplicability { get; set; }
        public RuleAddViewModel RuleDetection { get; set; }
        public TaskAddViewModel TaskInstall { get; set; }
        public TaskAddViewModel TaskUninstall { get; set; }
        public TaskAddViewModel TaskUpdate { get; set; }
        public string RuleApplicabilityId { get; set; }
        public string RuleDetectionId { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public string InstallationTime { get; set; }
        public string PackageSize { get; set; }
        public string Prerequisites { get; set; }
        public string VendorReleaseDate { get; set; }
        public string CompliancyRule { get; set; }
        public string Checksum { get; set; }
        public List<string> Systemhouses { get; set; }
        public List<string> Customers { get; set; }
        public List<string> Clients { get; set; }
        public string RunningContext { get; set; }
        public string StreamId { get; set; }
        public string SoftwareStreamId { get; set; }
        public bool PublishInShop { get; set; }
        public string NextSoftwareId { get; set; }
        public string PrevSoftwareId { get; set; }
        public string MinimalSoftwareId { get; set; }
        public string DedicatedDownloadLink { get; set; }
        public string RevisionNumber { get; set; }
        public bool AllWin10Versions { get; set; }
        public bool AllWin11Versions { get; set; }
    }

    public class SoftwareEditViewModel
    {
        public string Name { get; set; }
        public FileRef Icon { get; set; }
        public string Version { get; set; }
        public string RuleApplicabilityId { get; set; }
        public RuleViewModel RuleApplicability { get; set; }
        public string RuleDetectionId { get; set; }
        public RuleViewModel RuleDetection { get; set; }
        public TaskViewModel TaskInstall { get; set; }
        public TaskViewModel TaskUninstall { get; set; }
        public TaskViewModel TaskUpdate { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public string InstallationTime { get; set; }
        public string PackageSize { get; set; }
        public string Prerequisites { get; set; }
        public string VendorReleaseDate { get; set; }
        public string CompliancyRule { get; set; }
        public string Checksum { get; set; }
        public List<string> Systemhouses { get; set; }
        public List<string> Customers { get; set; }
        public List<string> Clients { get; set; }
        public string RunningContext { get; set; }
        public string StreamId { get; set; }
        public string SoftwareStreamId { get; set; }
        public string PublishInShop { get; set; }
        public string NextSoftwareId { get; set; }
        public string PrevSoftwareId { get; set; }
        public string MinimalSoftwareId { get; set; }
        public string DedicatedDownloadLink { get; set; }
        public string CustomerStatus { get; set; }
        public string RevisionNumber { get; set; }
        public int DisplayRevisionNumber { get; set; }
        public bool AllWin10Versions { get; set; }
        public bool AllWin11Versions { get; set; }
    }
}
