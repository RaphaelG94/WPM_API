using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WPM_API.Data.DataContext.Interfaces;

namespace WPM_API.Data.DataContext
{
    public class HardwareModel : IEntity, IDeletable
    {
        [Key, Column("PK_HardwareModel")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Vendor { get; set; }
        public string ModelFamily { get; set; }
        public string ModelType { get; set; }
        public DateTime ProductionStart { get; set; }
        public DateTime ProductionEnd { get; set; }
        public string? CreatedByUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? UpdatedByUserId { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string? DeletedByUserId { get; set; }
        public DateTime? DeletedDate { get; set; }

    }
}
