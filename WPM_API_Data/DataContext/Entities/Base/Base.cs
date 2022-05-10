using WPM_API.Data.DataContext.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace  WPM_API.Data.DataContext.Entities
{
    public class Base : IEntity, IDeletable
    {
        [Key, Column("PK_Base")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        // TODO: Add CSDP Credentials
        public string Name { get; set; }
        public string CredentialsId { get; set; }
        public Subscription Subscription { get; set; }
        public ResourceGroup ResourceGroup { get; set; }
        public StorageAccount StorageAccount { get; set; }
        public VirtualNetwork VirtualNetwork { get; set; }
        public Vpn Vpn { get; set; }
        public List<VirtualMachine> VirtualMachines { get; set; }
        public string Status { get; set; }
        public virtual List<AdvancedProperty> Properties { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedByUserId { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string DeletedByUserId { get; set; }
        [InverseProperty("Base")]
        public virtual List<Client> Clients { get; set; }
        public string CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }
        [ForeignKey("BaseStatusId")]
        public BaseStatus BaseStatus { get; set; }
        public string BaseStatusId { get; set; }
    }
}
