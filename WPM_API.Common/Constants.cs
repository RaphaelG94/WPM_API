namespace WPM_API.Common
{
    public class Constants
    {
        public const int PageSizeDefault = 14;
        public const int CountryUSA_Id = 223;
        public const int MinPasswordLength = 7;

        public class Roles
        {
            public const string Admin = "admin";
            public const string Systemhouse = "systemhouse";
            public const string Customer = "customer";
        }

        public class BitstreamClaimTypes
        {
            public const string UserId = "id";
            public const string Admin = "admin";
            public const string Name = "name";
            public const string Sub = "sub";
            public const string Customer = "customer";
            public const string Systemhouse = "systemhouse";
            public const string GeneratedDate = "generatedDateTicks";
        }

        public class Policies
        {
            public const string Admin = "admin";
            public const string Systemhouse = "systemhouse";
            public const string Customer = "customer";
        }
    }
}
