using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WPM_API.Data.DataContext.Interfaces;

namespace  WPM_API.Data.DataContext.Entities
{
    public class UserCustomer : IEntity
    {
        [Key, Column("PK_UserCustomer")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public string CustomerId { get; set; }
        public Customer Customer { get; set; }

        
    }

    public class UserSubscription : IEntity
    {
        public UserSubscription() { }
        public UserSubscription(User user, Subscription subscription)
        {
            this.User = user;
            this.Subscription = subscription;
        }

        public string Id { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public string SubscriptionId { get; set; }
        public Subscription Subscription { get; set; }
        
    }

    public class UserSystemhouse : IEntity
    {
        public string Id { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public string SystemhouseId { get; set; }
        public Systemhouse Systemhouse { get; set; }
        
    }
}
