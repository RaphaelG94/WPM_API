using WPM_API.Data.DataContext.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace  WPM_API.Data.DataContext.Entities
{
    public class DriverShopItem : IEntity, IDeletable
    {
        [Key, Column("PK_DriverShopItem")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string DriverId { get; set; }
        [ForeignKey("DriverId")]
        public Driver Driver { get; set; }
        public string ShopItemId { get; set; }
        [ForeignKey("ShopItemId")]
        public ShopItem ShopItem { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedByUserId { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string DeletedByUserId { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}
