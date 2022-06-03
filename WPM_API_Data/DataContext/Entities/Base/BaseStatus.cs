using WPM_API.Data.DataContext.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace  WPM_API.Data.DataContext.Entities
{
    public class BaseStatus : IEntity, IDeletable
    {
        [Key, Column("PK_BaseStatus")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string Status { get; set; }
        public string ResourceGroupStatus { get; set; }
        public string VirtualNetworkStatus { get; set; }
        public string StorageAccountStatus { get; set; }
        public string VPNStatus { get; set; }
        public string ErrorMessage { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? UpdatedByUserId { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string? DeletedByUserId { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}
