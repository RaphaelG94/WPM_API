using WPM_API.Data.DataContext.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace  WPM_API.Data.DataContext.Entities
{
    public class CustomerSoftwareStream : IEntity, IDeletable
    {
        [Key, Column("PK_CustomerSoftwareStream")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string UpdateSettings { get; set; }
        public List<CustomerSoftware> StreamMembers { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? UpdatedByUserId { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string? DeletedByUserId { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string CustomerId { get; set; }
        public string SoftwareStreamId { get; set; }
        public string Vendor { get; set; }
        public string Description { get; set; }
        public string DescriptionShort { get; set; }
        public bool GnuLicence { get; set; }
        public string Architecture { get; set; }
        public string Language { get; set; }
        public string Website { get; set; }
        public string DownloadLink { get; set; }
        [ForeignKey("ApplicationOwnerId")]
        public Person ApplicationOwner { get; set; }
        public string ApplicationOwnerId { get; set; }
        [ForeignKey("ReleasePlanId")]
        public ReleasePlan ReleasePlan { get; set; }
        public string ReleasePlanId { get; set; }
        public File Icon { get; set; }
        public string Type { get; set; }
        public bool IsEnterpriseStream { get; set; }
        public int Priority { get; set; }
        public string ClientOrServer { get; set; }
    }
}
