namespace  WPM_API.Data.Models
{
    public static class BaseIncludes
    {
        public const string AdvancedProperties = "Properties";
        public const string PropertyCategories = "Properties.Category";
        public const string Clients = "Clients";
        public const string VirtualMachines = "VirtualMachines";
        public const string VirtualMachineDisks = "VirtualMachines.Disks";
        public const string ResourceGroup = "ResourceGroup";
        public const string Subscription = "Subscription";
        public const string VirtualNetwork = "VirtualNetwork";
        public const string Subnets = "VirtualNetwork.Subnets";
        public const string StorageAccount = "StorageAccount";
        public const string VPN = "Vpn";
        public const string BaseStatus = "BaseStatus";


        public static string[] GetAllIncludes()
        {
            string[] includes =
            {
                BaseIncludes.AdvancedProperties, BaseIncludes.Clients, BaseIncludes.VirtualMachines,
                BaseIncludes.ResourceGroup, BaseIncludes.Subscription, BaseIncludes.VirtualNetwork,
                BaseIncludes.StorageAccount, BaseIncludes.Subnets, BaseIncludes.VirtualMachineDisks,
                BaseIncludes.VPN, BaseIncludes.PropertyCategories, BaseIncludes.BaseStatus
            };
            return includes;
        }
    }
}
