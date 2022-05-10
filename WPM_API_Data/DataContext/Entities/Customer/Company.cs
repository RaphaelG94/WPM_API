using WPM_API.Data.DataContext.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace  WPM_API.Data.DataContext.Entities
{
    public class Company : IEntity, IDeletable
    {
        // Primitive data
        [Key, Column("PK_Company")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public string CorporateName { get; set; }

        public string Description { get; set; }

        public string FormOfOrganization { get; set; }

        public string LinkWebsite { get; set; }

        public string Type { get; set; }

        public string ExpertId { get; set; }

        // Relationships to other entities

        [ForeignKey("ExpertId")]
        public virtual Person Expert { get; set; }

        [InverseProperty("Company")]
        public List<Person> Employees { get; set; }

        public string CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }

        public string HeadquarterId { get; set; }

        [ForeignKey("HeadquarterId")]
        public Location Headquarter { get; set; }

        [InverseProperty("Company")]
        public List<Location> Locations { get; set; }

        public string CreatedByUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedByUserId { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string DeletedByUserId { get; set; }
        public DateTime? DeletedDate { get; set; }
        
    }
}
