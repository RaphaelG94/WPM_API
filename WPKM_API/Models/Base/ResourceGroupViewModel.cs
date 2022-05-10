namespace WPM_API.Models
{
    public class ResourceGroupViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string AzureSubscriptionId { get; set; }
    }

    public class AddResourceGroupViewModel
    {
        public string Name { get; set; }
        public string SubscriptionId { get; set; }
        public string AzureLocation { get; set; }
        public string CustomerId { get; set; }
        public string Managed { get; set; }
    }
}