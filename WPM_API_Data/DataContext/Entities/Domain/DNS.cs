using WPM_API.Data.DataContext.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections;

namespace  WPM_API.Data.DataContext.Entities
{
    public class DNS : IEntity, IDeletable
    {
        [Key, Column("PK_DNS")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string Forwarder { get; set; }
        public string CreatedByUserId { get ; set ; }
        public DateTime CreatedDate { get ; set ; }
        public string UpdatedByUserId { get ; set ; }
        public DateTime UpdatedDate { get ; set ; }
        public string DeletedByUserId { get ; set ; }
        public DateTime? DeletedDate { get ; set ; }
        
    }
}
