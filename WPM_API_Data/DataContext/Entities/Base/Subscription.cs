using WPM_API.Data.DataContext.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace  WPM_API.Data.DataContext.Entities
{
    public class Subscription : IEntity, IDeletable
    {
        [Key, Column("PK_Subscription")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public string AzureId { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// Alle User denen diese Subscription zugeordnet ist
        /// </summary>
        public ICollection<UserSubscription> UserSubscription { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? UpdatedByUserId { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string? DeletedByUserId { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}