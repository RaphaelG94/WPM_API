using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WPM_API.Data.DataContext.Interfaces;

namespace WPM_API.Data.DataContext.Entities
{
    public class ActivityLog : IEntity, IDeletable
    {
        [Key, Column("PK_ActivityLog")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string Message { get; set; }
        public DateTime TimeStamp { get; set; }
        public string UserName { get; set; }
        public string ClientId { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? UpdatedByUserId { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string? DeletedByUserId { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}
