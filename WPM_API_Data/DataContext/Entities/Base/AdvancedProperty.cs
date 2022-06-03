using WPM_API.Data.DataContext.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace  WPM_API.Data.DataContext.Entities
{
    public class AdvancedProperty : IEntity, IDeletable
    {
        [Key, Column("PK_AdvancedProperty")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string BaseId { get; set; }
        /// <summary>
        /// Der Customer an dem die Domain hängt
        /// </summary>
        [ForeignKey("BaseId")]
        public virtual Base Base { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public bool isEditable { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
        public string CategoryId { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? UpdatedByUserId { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string? DeletedByUserId { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}
