using WPM_API.Data.DataContext.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace  WPM_API.Data.DataContext.Entities
{
    public class Location : IEntity, IDeletable
    {
        [Key, Column("PK_Location")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public string Name { get; set; }

        public string NameAbbreviation { get; set; }

        public string City { get; set; }

        public string CityAbbreviation { get; set; }

        public string Country { get; set; }

        public string CountryAbbreviation { get; set; }

        public  string TimeZone { get; set; }

        public string Street { get; set; }

        public string Number { get; set; }

        public string Postcode { get; set; }

        public string PublicIP { get; set; }

        public string UploadSpeed { get; set; }

        public string DownloadSpeed { get; set; }

        public string Type { get; set; }

        public string AzureLocation { get; set; }

        public string CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }

        public string CompanyId { get; set; }

        [ForeignKey("CompanyId")]
        public Company Company { get; set; }

        public string CreatedByUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedByUserId { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string DeletedByUserId { get; set; }
        public DateTime? DeletedDate { get; set; }
        
    }
}
