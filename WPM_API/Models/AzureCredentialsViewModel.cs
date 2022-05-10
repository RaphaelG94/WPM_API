namespace WPM_API.Models
{
    public class AzureCredentialViewModel
    {
        public AzureCredentialViewModel() { }
        public string Id { get; set; }
        public string TenantId { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public bool IsStandard { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
    }

    public class AzureCredsNoSecret
    {
        public AzureCredsNoSecret() { }
        public string Id { get; set; }
        public string TenantId { get; set; }
        public string ClientId { get; set; }
        public bool IsStandard { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
    }
}
