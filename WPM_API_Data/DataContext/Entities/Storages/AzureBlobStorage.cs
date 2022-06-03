using WPM_API.Data.DataContext.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace  WPM_API.Data.DataContext.Entities
{
    public class AzureBlobStorage : IEntity, IDeletable
    {
        [Key, Column("PK_AzureBlob")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string StorageAccountId { get; set; }
        [ForeignKey("StorageAccountId")]
        public StorageAccount StorageAccount { get; set; }
        public string Type { get; set; }
        public string RessourceGroupId { get; set; }    
        [ForeignKey("RessourceGroupId")]
        public ResourceGroup RessourceGroup { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? UpdatedByUserId { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string? DeletedByUserId { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string CustomerId { get; set; }
        public Customer Customer { get; set; }
        public string SubscriptionId { get; set; }
        [ForeignKey("SubscriptionId")]
        public Subscription Subscription { get; set; }
    }
}
