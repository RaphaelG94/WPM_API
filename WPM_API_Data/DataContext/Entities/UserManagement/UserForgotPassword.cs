using WPM_API.Data.DataContext.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace  WPM_API.Data.DataContext.Entities
{
    public class UserForgotPassword : IEntity
    {
        [Key, Column("PK_UserForgotPassword")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string UserId { get; set; }
        public Guid RequestGuid { get; set; }
        public DateTime CreatedDate { get; set; }
        [StringLength(64)]
        public string CreatorIpAddress { get; set; }
        public DateTime? ApprovedDateTime { get; set; }
        [StringLength(64)]
        public string ApproverIpAddress { get; set; }

        [NotMapped]
        public bool IsExpired => CreatedDate.AddDays(1) < DateTime.Now;

        public virtual User User { get; set; }
        
    }
}
