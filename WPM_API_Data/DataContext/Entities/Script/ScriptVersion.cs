using WPM_API.Data.DataContext.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace  WPM_API.Data.DataContext.Entities
{
    public class ScriptVersion : IEntity
    {
        [Key, Column("PK_ScriptVersion")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public int Number { get; set; }
        public string ContentUrl { get; set; }
        public string Name { get; set; }
        public List<File> Attachments { get; set; }                
    }
}
