using WPM_API.Data.DataContext.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace  WPM_API.Data.DataContext.Entities
{
    public class Person : IEntity, IDeletable
    {
        // Primitive data
        [Key, Column("PK_Person")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public string Title { get; set; }

        public string GivenName { get; set; }

        public string MiddleName { get; set; }

        public string Surname { get; set; }

        public string AcademicDegree { get; set; }

        public string EmployeeType { get; set; }

        public string CostCenter { get; set; }

        public string PhoneNr { get; set; }

        public string FaxNr { get; set; }

        public string MobileNr { get; set; }

        public string EmailPrimary { get; set; }

        public string State { get; set; }

        public string EmailOptional { get; set; }

        public string Domain { get; set; }
        
        public string DepartementName { get; set; }
        
        public string DepartementShort { get; set; }

        public string RoomNr { get; set; }

        public string EmployeeNr { get; set; }

        // Relationships to other entities
        public string CompanyId { get; set; }

        [ForeignKey("CompanyId")]
        public virtual Company Company { get; set; }

        public string CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }

        public string CreatedByUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedByUserId { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string DeletedByUserId { get; set; }
        public DateTime? DeletedDate { get; set; }

        
    }
}
