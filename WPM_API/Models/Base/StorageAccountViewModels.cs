using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace WPM_API.Models
{
    public class StorageAccountViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public StorageType Type { get; set; }
        public string ResourceGroupId { get; set; }
    }

    public class StorageAccountNameViewModel
    {
        public string Name { get; set; }
        public string Managed { get; set; }
    }

    public class StorageAccountAddViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string CustomerId { get; set; }
        public string ResourceGroupId { get; set; }
        public string SubscriptionId { get; set; }
        public string Type { get; set; }
    }

    public enum StorageType
    {
        PremiumLRS,
        StandardGRS,
        StandardLRS,
        StandardRAGRS,
        StandardZRS
    }
}
