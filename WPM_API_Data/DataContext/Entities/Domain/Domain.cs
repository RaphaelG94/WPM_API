using WPM_API.Data.DataContext.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace  WPM_API.Data.DataContext.Entities
{
    public class Domain : IEntity, IDeletable
    {
        [Key, Column("PK_Domain")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public string CustomerId { get; set; }
        /// <summary>
        /// Der Customer an dem die Domain hängt
        /// </summary>
        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }
        public string BaseId { get; set; }
        [ForeignKey("BaseId")]
        public virtual Base Base { get; set; }
        public string Name { get; set; }
        public string Tld { get; set; }
        public string Status { get; set; }
        public GroupPolicyObject Gpo { get; set; }
        public List<DomainUser> DomainUsers { get; set; }
        public List<OrganizationalUnit> OrganizationalUnits { get; set; }
        public DateTime? DeletedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedByUserId { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string DeletedByUserId { get; set; }
        public string CreatedByUserId { get; set; }
        public string ExecutionVMId { get; set; }
        public List<Server> Servers { get; set; }
        public List<DNS> DNS { get; set; }
        public File Office365ConfigurationXML { get; set; }
        public File DomainUserCSV { get; set; }
        
    }
}
