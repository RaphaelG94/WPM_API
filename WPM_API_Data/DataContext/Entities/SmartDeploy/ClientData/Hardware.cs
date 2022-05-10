using WPM_API.Data.DataContext.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace  WPM_API.Data.DataContext.Entities
{
    public class Hardware : IEntity
    {
        [Key, Column("PK_HardwareId")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string Manufacturer { get; set; }
        public string Type { get; set; }
        public string ModelNumber { get; set; }
        public string SerialNumber { get; set; }
        public string ServiceTag { get; set; }
        public string Processor { get; set; }
        public string RAM { get; set; }
        public string HDD { get; set; }
        public string ChipSet { get; set; }
        public string DisplayResolution { get; set; }
        public string TPMChipData { get; set; }
        
    }
}