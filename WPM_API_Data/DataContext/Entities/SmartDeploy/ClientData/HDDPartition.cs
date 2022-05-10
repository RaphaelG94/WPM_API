using WPM_API.Data.DataContext.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace  WPM_API.Data.DataContext.Entities
{
    public class HDDPartition : IEntity
    {
        [Key, Column("PK_HDDPartitionId")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string PartitionNumber { get; set; }
        public string isGpt { get; set; }
        public string DriveLetter { get; set; }
        public string SizeInBytes { get; set; }
        public string Type { get; set; }
        public string Overprovisioning { get; set; }
        
    }
}