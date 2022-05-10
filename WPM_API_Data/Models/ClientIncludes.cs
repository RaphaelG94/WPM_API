namespace  WPM_API.Data.Models
{
    public static class ClientIncludes
    {
        public const string Os = "Os";
        public const string Bios = "Bios";
        public const string Hardware = "Hardware";
        public const string Network = "Network";
        public const string Purchase = "Purchase";
        public const string Partition = "Partition";
        public const string MacAddress = "MacAddresses";
        public const string ClientOptions = "AssignedOptions";
        public const string ClientOptionsDeviceOption = "AssignedOptions.DeviceOption";
        public const string ClientOptionsParameters = "AssignedOptions.Parameters";
        public const string Properties = "Properties";
        public const string Property = "Properties.ClientProperty";
        public const string PropertyCategory = "Properties.ClientProperty.Category";
        public const string Base = "Base";
        // public const string PreinstalledSoftware = "PreinstalledSoftwares";
        public const string Customer = "Customer";
        public const string CustomerSEPs = "Customer.StorageEntryPoints";
        public const string AssignedSoftware = "AssignedSoftware";
        public const string CustomerIconRight = "Customer.IconRight";
        public const string CustomerIconLeft = "Customer.IconLeft";
        public const string CustomerBanner = "Customer.Banner";

        public static string[] GetAllIncludes()
        {
            string[] includes =
            {
                ClientIncludes.Os, ClientIncludes.Bios, ClientIncludes.Hardware,
                ClientIncludes.Network, ClientIncludes.Purchase, ClientIncludes.Partition,
                ClientIncludes.MacAddress,ClientIncludes.ClientOptions, Properties, Property, PropertyCategory,
                ClientIncludes.Base, ClientIncludes.Customer,
                ClientIncludes.CustomerSEPs, ClientIncludes.CustomerIconLeft, ClientIncludes.CustomerBanner,
                ClientIncludes.CustomerIconRight
            };
            return includes;
        }
    }
}
