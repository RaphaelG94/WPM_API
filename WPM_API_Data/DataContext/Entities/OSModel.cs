using WPM_API.Data.DataContext.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace  WPM_API.Data.DataContext.Entities
{
    public class OSModel : IEntity, IDeletable
    {
        [Key, Column("PK_OSModel")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string Vendor { get; set; }
        public string OSName { get; set; }
        public string Architecture { get; set; }
        public string OSType { get; set; }
        public string Version { get; set; }
        public DateTime ReleaseDate { get; set; }
        public DateTime SupportEnd { get; set; }
        public File Content { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedByUserId { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string DeletedByUserId { get; set; }
        public DateTime? DeletedDate { get; set; }
        public List<HardwareModel> HardwareModels { get; set; }
    }
}
