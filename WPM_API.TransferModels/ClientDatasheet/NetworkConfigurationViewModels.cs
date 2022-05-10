namespace  WPM_API.TransferModels
{
    public class NetworkConfigurationViewModels
    {
        public string Id { get; set; }
        public string IPv4 { get; set; }
        public string IPv6 { get; set; }
        public string DNS { get; set; }
        public string Gateway { get; set; }
        public string DHCP { get; set; }
    }
}