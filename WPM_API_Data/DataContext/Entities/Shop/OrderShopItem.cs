using WPM_API.Data.DataContext.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace  WPM_API.Data.DataContext.Entities
{
    public class OrderShopItem : IEntity, IEquatable<OrderShopItem>
    {
        [Key, Column("PK_OrderShopItem")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string OrderId { get; set; }
        [ForeignKey("OrderId")]
        public Order Order { get; set; }
        public string ShopItemId { get; set; }
        [ForeignKey("ShopItemId")]
        public ShopItem ShopItem { get; set; }

        public bool Equals(OrderShopItem other)
        {
            //Check whether the compared object is null. 
            if (Object.ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data. 
            if (Object.ReferenceEquals(this, other)) return true;

            //Check whether the products' properties are equal. 
            return ShopItemId.Equals(other.ShopItemId) && OrderId.Equals(other.OrderId);
        }

        // If Equals() returns true for a pair of objects  
        // then GetHashCode() must return the same value for these objects. 
        public override int GetHashCode()
        {
            //Get hash code for the Order id if it is not null. 
            int hashOrderId = OrderId.GetHashCode();

            //Get hash code for the Code field. 
            int hashShopItemId = ShopItemId.GetHashCode();

            //Calculate the hash code for the ordershopitem. 
            return hashOrderId ^ hashShopItemId;
        }

        
    }
}
