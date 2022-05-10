using WPM_API.Data.DataContext.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace  WPM_API.Data.DataContext.Entities
{
    public class ShopItemCategory : IEntity
    {
        [Key, Column("PK_ShopItemCategory")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
        public string ShopItemId { get; set; }
        [ForeignKey("ShopItemId")]
        public ShopItem ShopItem { get; set; }

        
    }
}
