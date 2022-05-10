using WPM_API.Data.DataContext.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace  WPM_API.Data.DataContext.Entities
{
    public class CustomerImage : IEntity, IDeletable
    {
        [Key, Column("PK_CustomerImage")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedByUserId { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string DeletedByUserId { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string Update { get; set; }
        public string BuildNr { get; set; }
        public bool PublishInShop { get; set; }
        public string CustomerId { get; set; }
        public string FileName { get; set; }
        public string ImageId { get; set; }
        public string UnattendId { get; set; }
        [ForeignKey("UnattendId")]
        public File Unattend { get; set; }
        public string OEMPartitionId { get; set; }
        [ForeignKey("OEMPartitionId")]
        public File OEMPartition { get; set; }        
        public string CustomerImageStreamId { get; set; }
        public string RevisionNumber { get; set; }
        public int DisplayRevisionNumber { get; set; }
    }
}
