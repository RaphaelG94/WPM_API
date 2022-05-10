using WPM_API.Data.DataContext.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace  WPM_API.Data.DataContext.Entities
{
    public class File : IEntity
    {
        [Key, Column("PK_File")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string Guid { get; set; }
        public string Name { get; set; }
        public string TaskId { get; set; }
        [ForeignKey("TaskId")]
        public virtual Task Task { get; set; }

        
    }
}
