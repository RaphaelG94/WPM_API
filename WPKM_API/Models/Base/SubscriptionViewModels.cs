namespace WPM_API.Models
{
    public class SubscriptionViewModel
    {
        public SubscriptionViewModel() { }
        public string Id { get; set; }
        public string Name { get; set; }
        public string AzureId { get; set; }
    }

    public class SubscriptionRefViewModel
    {
        public SubscriptionRefViewModel() { }
        public string Id { get; set; }
    }
}
