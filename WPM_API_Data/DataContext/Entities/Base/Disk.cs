using WPM_API.Data.DataContext.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace  WPM_API.Data.DataContext.Entities
{
    public class Disk : IEntity
    {
        [Key, Column("PK_Disk")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string Name { get; set; }
        public int SizeInGb { get; set; }
        public DiskType DiskType { get; set; }
        
    }

    public enum DiskType
    {
        SystemDisk = 0,
        AdditionalDisk = 1
    }
}