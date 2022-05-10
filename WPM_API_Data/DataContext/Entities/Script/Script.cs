using WPM_API.Data.DataContext.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace  WPM_API.Data.DataContext.Entities
{
    public class Script : IEntity, IDeletable
    {
        [Key, Column("PK_Script")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<ScriptVersion> Versions { get; set; }
        public ScriptType Type { get; set; }
        public AuthorType AuthorType { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedByUserId { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string DeletedByUserId { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool showToCustomer { get; set; }
        public bool PEOnly { get; set; }
        public string OSType { get; set; }
    }

    public enum AuthorType { BitStream, Customer }
}
