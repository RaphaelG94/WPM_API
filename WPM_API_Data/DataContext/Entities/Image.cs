using WPM_API.Data.DataContext.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace  WPM_API.Data.DataContext.Entities
{
    public class Image : IEntity, IDeletable
    {
        [Key, Column("PK_Image")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? UpdatedByUserId { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string? DeletedByUserId { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string Update { get; set; }
        public string BuildNr { get; set; }
        public bool PublishInShop { get; set; }
        public string FileName { get; set; }
        public string UnattendId { get; set; }
        [ForeignKey("UnattendId")]
        public File Unattend { get; set; }
        public string OEMPartitionId { get; set; }
        [ForeignKey("OEMPartitionId")]
        public File OEMPartition { get; set; }        
        public string ImageStreamId { get; set; }
        public string RevisionNumber { get; set; }
        public int DisplayRevisionNumber { get; set; }
        public virtual List<ImagesSystemhouse> Systemhouses { get; set; }
        public virtual List<ImagesCustomer> Customers { get; set; }
        public virtual List<ImagesClient> Clients { get; set; }
    }

    public class ImagesSystemhouse : IEntity, IDeletable
    {
        [Key, Column("PK_SoftwaresSystemhouse")]
        public string Id { get; set; }
        public string ImageId { get; set; }
        [ForeignKey("ImageId")]
        public Image Image { get; set; }
        public string SystemhouseId { get; set; }
        [ForeignKey("SystemhouseId")]
        public Systemhouse Systemhouse { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? UpdatedByUserId { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string? DeletedByUserId { get; set; }
        public DateTime? DeletedDate { get; set; }
    }

    public class ImagesCustomer : IEntity, IDeletable
    {
        [Key, Column("PK_SoftwaresCustomer")]
        public string Id { get; set; }
        public string ImageId { get; set; }
        [ForeignKey("ImageId")]
        public Image Image { get; set; }
        public string CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? UpdatedByUserId { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string? DeletedByUserId { get; set; }
        public DateTime? DeletedDate { get; set; }
    }

    public class ImagesClient : IEntity, IDeletable
    {
        [Key, Column("PK_SoftwaresClient")]
        public string Id { get; set; }
        public string ImageId { get; set; }
        [ForeignKey("ImageId")]
        public Image Image { get; set; }
        public string ClientId { get; set; }
        [ForeignKey("ClientId")]
        public Client Client { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? UpdatedByUserId { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string? DeletedByUserId { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}
