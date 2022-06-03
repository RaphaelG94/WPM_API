using System.Collections.Generic;
using WPM_API.Data.DataContext.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace  WPM_API.Data.DataContext.Entities
{
    public class Vpn : IEntity, IDeletable
    {
        [Key, Column("PK_Vpn")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string LocalAddressRange { get; set; }
        public string LocalPublicIp { get; set; }
        public string VirtualNetwork { get; set; }
        public string VirtualPublicIp { get; set; }
        public string SharedKey { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? UpdatedByUserId { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string? DeletedByUserId { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}