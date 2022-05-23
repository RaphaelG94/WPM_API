using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WPM_API.Data.DataContext.Interfaces;

namespace WPM_API.Data.DataContext.Entities
{
    public class Attachment : IEntity
    {
        [Key, Column("PK_Attachment")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        [Required, StringLength(256)]
        public string FileName { get; set; }
        [Required, StringLength(512)]
        public string GenFileName { get; set; }
        public long FileSize { get; set; }
        [Required, StringLength(256)]
        public string ContentType { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedByUserId { get; set; }

        public virtual User CreatedByUser { get; set; }
    }
}
