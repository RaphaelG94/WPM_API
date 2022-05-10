using WPM_API.Data.DataContext.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace  WPM_API.Data.DataContext.Entities
{
    public class Systemhouse : IEntity, IDeletable
    {
        [Key, Column("PK_Systemhouse")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        /// <summary>
        /// Liste aller Kunden, die von diesem Systemhouse adminsitriert werden
        /// </summary>
        public List<Customer> Customer { get; set; }

        /// <summary>
        /// Systemhousename
        /// </summary>
        public string Name { get; set; }
        public Boolean Deletable { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedByUserId { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string DeletedByUserId { get; set; }
        public DateTime? DeletedDate { get; set; }
        
    }
}
