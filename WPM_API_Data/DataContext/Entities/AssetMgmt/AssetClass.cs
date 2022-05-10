using WPM_API.Data.DataContext.Entities;
using WPM_API.Data.DataContext.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace  WPM_API.Data.DataContext
{
    public class AssetClass : IEntity
    {
        [Key, Column("PK_AssetClass")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string AssetTypeId { get; set; }
        [ForeignKey("AssetTypeId")]
        public AssetType AssetType { get; set; }
        public string CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }
        public bool fromAdmin { get; set; }
    }
}
