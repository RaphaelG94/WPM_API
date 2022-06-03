using WPM_API.Data.DataContext.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace  WPM_API.Data.DataContext.Entities
{
    public class CustomerSoftware : IEntity, IDeletable
    {
        [Key, Column("PK_CustomerSoftware")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public Rule RuleDetection { get; set; }
        public Rule RuleApplicability { get; set; }
        public Task TaskInstall { get; set; }
        public Task TaskUninstall { get; set; }
        public Task TaskUpdate { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? UpdatedByUserId { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string? DeletedByUserId { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public string InstallationTime { get; set; }
        public string PackageSize { get; set; }
        public string Prerequisites { get; set; }
        public string VendorReleaseDate { get; set; }
        public string CompliancyRule { get; set; }
        public string Checksum { get; set; }
        public string CustomerSoftwareStreamId { get; set; }
        public string SoftwareId { get; set; }
        [ForeignKey("SoftwareId")]
        public string RunningContext { get; set; }
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
