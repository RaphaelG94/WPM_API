using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace WPM_API.Models
{
    public class AddCompanyViewModel
    {
        public AddCompanyViewModel() {}

        public string Id { get; set; }

        public string CorporateName { get; set; }

        public string Description { get; set; }

        public string FormOfOrganization { get; set; }

        public string LinkWebsite { get; set; }

        public string Type { get; set; }

        public string CustomerId { get; set; }

        public string ExpertId { get; set; }

        public bool IsMainCompany { get; set; }

        public string HeadquarterId { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public string Name { get; set; }

        public string PhoneNr { get; set; }

        public string Postcode { get; set; }

        public string PublicIp { get; set; }

        public string Street { get; set; }

        public string StreetNr { get; set; }

        public PersonViewModel Expert { get; set; }

        public LocationViewModel Headquarter { get; set; }
    }

    public class CompanyViewModel
    {
        public CompanyViewModel()
        {}

        public string Id { get; set; }

        public string CorporateName { get; set; }

        public string Description { get; set; }

        public string FormOfOrganization { get; set; }

        public string LinkWebsite { get; set; }

        public string Type { get; set; }

        public PersonViewModel Expert { get; set; }

        public AddLocationViewModel Headquarter { get; set; }
    }

    public class UpdateMainCompanyViewModel
    {
        public UpdateMainCompanyViewModel() { }
        public string Id { get; set; }
        public string CorporateName { get; set; }
        public string FormOfOrganization { get; set; }
        public string LinkWebsite { get; set; }
        public string ExpertId { get; set; }
        public string HeadquarterId { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string CustomerId { get; set; }
    }


    public class CompanyOverviewViewModel
    {
        public CompanyOverviewViewModel() { }

        public string Id { get; set; }

        public string CorporateName { get; set; }

        public string Description { get; set; }

        public string FormOfOrganization { get; set; }

        public string LinkWebsite { get; set; }

        public string Type { get; set; }

        public PersonNameView Expert { get; set; }

        public AddLocationViewModel Headquarter { get; set; }
    }
}
