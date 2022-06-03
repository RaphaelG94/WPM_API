using System.Collections.Generic;
using WPM_API.Data.DataContext.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace  WPM_API.Data.DataContext.Entities
{
    public class StorageAccount : IEntity, IDeletable
    {
        [Key, Column("PK_StorageAccount")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public string Name { get; set; }
        public string Type { get; set; }
        public string AzureId { get; set; }
        public string CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }
        public string ResourceGroupId { get; set; }
        [ForeignKey("ResourceGroupId")]
        public ResourceGroup ResourceGroup { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? UpdatedByUserId { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string? DeletedByUserId { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}