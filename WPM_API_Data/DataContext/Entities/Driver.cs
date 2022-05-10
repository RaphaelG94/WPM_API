using WPM_API.Data.DataContext.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace  WPM_API.Data.DataContext.Entities
{
    public class Driver : IEntity, IDeletable
    {
        [Key, Column("PK_Driver")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string SubFolderPath { get; set; }
        public string ConnectionString { get; set; }
        public string Version { get; set; }
        public string Vendor { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedByUserId { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string DeletedByUserId { get; set; }
        public DateTime? DeletedDate { get; set; }
        public List<DriverShopItem> DriverShopItems { get; set; }
        public string ContainerName { get; set; }
        public bool PublishInShop { get; set; }
    }
}
