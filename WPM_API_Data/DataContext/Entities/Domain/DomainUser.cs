using WPM_API.Data.DataContext.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections;

namespace  WPM_API.Data.DataContext.Entities
{
    public class DomainUser : IEntity, IDeletable
    {
        [Key, Column("PK_DomainUser")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public string Name { get; set; }
        public string UserGivenName { get; set; }
        public string UserLastName { get; set; }
        public string SamAccountName { get; set; }
        public string UserPrincipalName { get; set; }
        public string MemberOf{ get; set; }
        public string Description { get; set; }
        public string Displayname { get; set; }
        public string Workphone { get; set; }
        public string Email { get; set; }
        public string DomainId { get; set; }
        /// <summary>
        /// The domain the user belongs to.
        /// </summary>
        [ForeignKey("DomainId")]
        public virtual Domain Domain { get; set; }
        public string CreatedByUserId { get ; set ; }
        public DateTime CreatedDate { get ; set ; }
        public string UpdatedByUserId { get ; set ; }
        public DateTime UpdatedDate { get ; set ; }
        public string DeletedByUserId { get ; set ; }
        public DateTime? DeletedDate { get ; set ; }
        
    }
}
