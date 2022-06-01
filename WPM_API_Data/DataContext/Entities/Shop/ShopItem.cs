using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WPM_API.Data.DataContext.Interfaces;

namespace WPM_API.Data.DataContext.Entities
{
    public class ShopItem : IEntity, IDeletable
    {
        [Key, Column("PK_ShopItem")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? DescriptionShort { get; set; }
        public List<File>? Images { get; set; }
        public string? Price { get; set; }
        public string? ManagedServicePrice { get; set; }
        public string? ManagedServiceLifecyclePrice { get; set; }
        public List<ShopItemCategory> Categories { get; set; }

        public string CreatedByUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? UpdatedByUserId { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string? DeletedByUserId { get; set; }
        public DateTime? DeletedDate { get; set; }
        public List<DriverShopItem> DriverShopItems { get; set; }
    }
}
