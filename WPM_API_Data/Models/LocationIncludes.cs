namespace  WPM_API.Data.Models
{
    public static class LocationIncludes
    {
        public const string Customer = "Customer";
        public const string Company = "Company";

        public static string[] GetAllIncludes()
        {
            string[] includes =
            {
                LocationIncludes.Customer, LocationIncludes.Company
            };
            return includes;
        }
    }
}
