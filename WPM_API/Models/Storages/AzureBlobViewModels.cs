using WPM_API.Data.DataContext.Entities;

namespace WPM_API.Models
{
    public class AzureBlobViewModel
    {
        public AzureBlobViewModel() { }
        public string Id { get; set; }
        public string Name { get; set; }
        public string StorageAccountId { get; set; }
        public string StorageType { get; set; }
        public string ResourceGroupId { get; set; }
        public string AzureLocation { get; set; }
        public string CustomerId { get; set; }
        public string Type { get; set; }
        public string SubscriptionId { get; set; }
        public CustomerNameViewModel Customer { get; set; }
    }

    public class RequiredData
    {
        public RequiredData() { }
        public string SubscriptionId { get; set; }
        public string ResourceGroupName { get; set; }
        public string StorageAccountName { get; set; }
        public string CustomerId { get; set; }
    }

    public class Exists
    {
        public Exists(bool exists) { Existing = exists; }
        public bool Existing { get; set; }
    }

    public class CSDPResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string AzureLocation { get; set; }
    }
}
