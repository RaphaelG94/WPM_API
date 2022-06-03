using WPM_API.Data.DataContext.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace  WPM_API.Data.DataContext.Entities
{
    public class VirtualMachine : IEntity, IDeletable
    {
        [Key, Column("PK_VirtualMachine")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string AzureId { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Type { get; set; }
        public string Subnet { get; set; }
        public string OperatingSystem { get; set; }
        public string AdminUserName { get; set; }
        public string AdminUserPassword { get; set; }
        public List<Disk> Disks { get; set; }
        public string LocalIp { get; set; }
        [ForeignKey("BaseId")]
        public Base Base { get; set; }
        public string BaseId { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? UpdatedByUserId { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string? DeletedByUserId { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string Status { get; set; }        
        public string CurrentCustomerId { get; set; }
        public string SubscriptionName { get; set; }
        public string SubscriptionId { get; set; }
    }
}