using WPM_API.Data.DataContext.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace  WPM_API.Data.DataContext.Entities
{
    public class Default : IEntity
    {
        [Key, Column("PK_Default")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string CustomerId { get; set; }
        /// <summary>
        /// Der Customer an dem die Domain hängt
        /// </summary>
        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string Category { get; set; }
        
    }
}
