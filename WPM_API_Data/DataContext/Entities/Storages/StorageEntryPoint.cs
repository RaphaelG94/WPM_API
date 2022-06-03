using WPM_API.Data.DataContext.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace  WPM_API.Data.DataContext.Entities.Storages
{
    public class StorageEntryPoint : IEntity, IDeletable
    {
        [Key, Column("PK_StorageEntryPoint")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string ResourceGrpName { get; set; }
        public string Type { get; set; }
        public string SubscriptionId { get; set; }
        public bool IsCSDP { get; set; }
        public string StorageAccount { get; set; }
        public string Location { get; set; }
        public string StorageAccountType { get; set; }

        public string CreatedByUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? UpdatedByUserId { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string? DeletedByUserId { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string Status { get; set; }
        public string BlobContainerName { get; set; }
        public string Url { get; set; }
        public string Kind { get; set; }
        public bool Managed { get; set; }
    }
}
