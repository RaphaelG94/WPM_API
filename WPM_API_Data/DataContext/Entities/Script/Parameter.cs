using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WPM_API.Data.DataContext.Interfaces;

namespace WPM_API.Data.DataContext.Entities
{
    public class Parameter : IEntity, IDeletable
    {
        [Key, Column("PK_Parameter")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string Key { get; set; }
        public string? Value { get; set; }
        public string? Description { get; set; }
        public bool IsEditable { get; set; }
        public string? CreatedByUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? UpdatedByUserId { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string? DeletedByUserId { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string CustomerId { get; set; }
    }
}