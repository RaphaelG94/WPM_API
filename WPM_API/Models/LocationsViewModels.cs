namespace WPM_API.Models
{
    public class AddLocationViewModel
    {
        public AddLocationViewModel() { }

        public string Id { get; set; }
        public string Name { get; set; }
        public string NameAbbreviation { get; set; }
        public string Country { get; set; }
        public string CountryAbbreviation { get; set; }
        public string City { get; set; } 
        public string CityAbbreviation { get; set; }
        public string TimeZone { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public string Postcode { get; set; }
        public string PublicIP { get; set; }
        public string DownloadSpeed { get; set; }
        public string UploadSpeed { get; set; }
        public string Type { get; set; }
        public string AzureLocation { get; set; }
        public string CustomerId { get; set; }
        public string CompanyId { get; set; }
    }


    public class LocationViewModel
    {
        public LocationViewModel()
        {
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string NameAbbreviation { get; set; }
        public string Country { get; set; }
        public string CountryAbbreviation { get; set; }
        public string City { get; set; }
        public string CityAbbreviation { get; set; }
        public string TimeZone { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public string Postcode { get; set; }
        public string PublicIP { get; set; }
        public string DownloadSpeed { get; set; }
        public string UploadSpeed { get; set; }
        public string Type { get; set; }
        public string CustomerId { get; set; }
        public CustomerViewModel Customer { get; set; }
        public string CompanyId { get; set; }
        public CompanyViewModel Company { get; set; }
    }
}
