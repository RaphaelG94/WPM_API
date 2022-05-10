using WPM_API.Data.DataContext.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace  WPM_API.Data.DataContext.Entities
{
    public class NotificationEmail : IEntity
    {
        public NotificationEmail()
        {
            NotificationEmailAttachments = new HashSet<NotificationEmailAttachment>();
        }
        [Key, Column("PK_NotificationEmail")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string SchedulerId { get; set; }
        public DateTime? ProcessedDate { get; set; }

        [Required, StringLength(1024)]
        public string Subject { get; set; }

        [Required, MaxLength]
        public string Body { get; set; }

        [Required, MaxLength]
        public string ToEmailAddresses { get; set; }

        [MaxLength]
        public string ToCcEmailAddresses { get; set; }

        [MaxLength]
        public string ToBccEmailAddresses { get; set; }

        public int AttemptsCount { get; set; }
        public DateTime? LastAttemptDate { get; set; }

        [MaxLength]
        public string LastAttemptError { get; set; }

        public virtual Scheduler Scheduler { get; set; }
        public virtual ICollection<NotificationEmailAttachment> NotificationEmailAttachments { get; set; }
        
    }
}
