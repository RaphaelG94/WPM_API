using WPM_API.Data.DataContext.Entities;
using WPM_API.Data.DataContext.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using System.Text;

namespace  WPM_API.Data.DataContext.Entities
{
    public class Software : IEntity, IDeletable
    {
        [Key, Column("PK_Software")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public Rule RuleDetection { get; set; }
        public Rule RuleApplicability { get; set; }
        public Task TaskInstall { get; set; }
        public Task TaskUninstall { get; set; }
        public Task TaskUpdate { get; set; }
        // public File Icon { get; set; }
        public virtual List<ClientSoftware> AssignedClients { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedByUserId { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string DeletedByUserId { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string Status { get; set; }
        public string Type {get; set;}
        public string InstallationTime { get; set; }
        public string PackageSize { get; set; }
        public string Prerequisites { get; set; }
        public string VendorReleaseDate { get; set; }
        public string CompliancyRule { get; set; }
        public string Checksum { get; set; }
        public virtual List<SoftwaresSystemhouse> Systemhouses { get; set; }
        public virtual List<SoftwaresCustomer> Customers { get; set; }
        public virtual List<SoftwaresClient> Clients { get; set; }
        public string RunningContext { get; set; }
        public string SoftwareStreamId { get; set; }
        public bool PublishInShop { get; set; }
        public string NextSoftwareId { get; set; }
        public string PrevSoftwareId { get; set; }
        public string MinimalSoftwareId { get; set; }
        public string DedicatedDownloadLink { get; set; }
        public string RevisionNumber { get; set; }
        public int DisplayRevisionNumber { get; set; }
        public bool AllWin10Versions { get; set; }
        public bool AllWin11Versions { get; set; }
    }

    public class SoftwaresSystemhouse : IEntity, IDeletable
    {
        [Key, Column("PK_SoftwaresSystemhouse")]
        public string Id { get; set; }
        public string SoftwareId { get; set; }
        [ForeignKey("SoftwareId")]
        public Software Software { get; set; }
        public string SystemhouseId { get; set; }
        [ForeignKey("SystemhouseId")]
        public Systemhouse Systemhouse { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedByUserId { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string DeletedByUserId { get; set; }
        public DateTime? DeletedDate { get; set; }
    }

    public class SoftwaresCustomer : IEntity, IDeletable
    {
        [Key, Column("PK_SoftwaresCustomer")]
        public string Id { get; set; }
        public string SoftwareId { get; set; }
        [ForeignKey("SoftwareId")]
        public Software Software { get; set; }
        public string CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedByUserId { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string DeletedByUserId { get; set; }
        public DateTime? DeletedDate { get; set; }
    }

    public class SoftwaresClient : IEntity, IDeletable
    {
        [Key, Column("PK_SoftwaresClient")]
        public string Id { get; set; }
        public string SoftwareId { get; set; }
        [ForeignKey("SoftwareId")]
        public Software Software { get; set; }
        public string ClientId { get; set; }
        [ForeignKey("ClientId")]
        public Client Client { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedByUserId { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string DeletedByUserId { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}
