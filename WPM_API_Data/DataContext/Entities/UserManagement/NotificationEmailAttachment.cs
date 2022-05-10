using System.Collections.Generic;
using WPM_API.Data.DataContext.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace  WPM_API.Data.DataContext.Entities
{
    public class NotificationEmailAttachment : IEntity
    {
        [Key, Column("PK_NotificationEmailAttachment")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string NotificationEmailId { get; set; }
        public string AttachmentId { get; set; }

        public virtual NotificationEmail NotificationEmail { get; set; }
        public virtual Attachment Attachment { get; set; }
        
    }
}
