using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace  WPM_API.Azure.Models
{
    public class StorageAccountViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public StorageType Type { get; set; }
        public string ResourceGroup { get; set; }
    }

    public class StorageAccountAddViewModel
    {
        public string Name { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public StorageType Type { get; set; }
    }

    public class StorageAccountEditViewModel
    {
        public string Name { get; set; }
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
