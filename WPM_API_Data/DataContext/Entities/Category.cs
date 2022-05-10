using WPM_API.Data.DataContext.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace  WPM_API.Data.DataContext.Entities
{
    public class Category : IEntity
    {
        [Key, Column("PK_Category")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string Name { get; set; }
        public CategoryType Type { get; set; }
        //public List<ShopItemCategory> AssignedShopItems { get; set; }
    }

    public enum CategoryType
    {
        ShopItem = 0,
        DeviceProperty = 1,
        AdvancedProperty = 2
    }
}
