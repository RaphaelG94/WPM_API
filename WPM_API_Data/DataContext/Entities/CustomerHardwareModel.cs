using WPM_API.Data.DataContext.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace  WPM_API.Data.DataContext.Entities
{
    public class CustomerHardwareModel : IEntity, IDeletable
    {
        [Key, Column("PK_CustomerHardwareModel")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public int Counter { get; set; }
        public string Name { get; set; }
        public List<File> Drivers { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? UpdatedByUserId { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string? DeletedByUserId { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string CustomerId { get; set; }
    }
}
