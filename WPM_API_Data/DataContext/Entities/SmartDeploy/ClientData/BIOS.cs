using WPM_API.Data.DataContext.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace  WPM_API.Data.DataContext.Entities
{
    public class Bios : IEntity
    {
        [Key, Column("PK_BiosId")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string Manufacturer { get; set; }
        public string InstalledVersion { get; set; }
        public string ManufacturerVersion { get; set; }
        public string InternalVersion { get; set; }
        public BiosSettings BiosSettings { get; set; }
    }
}