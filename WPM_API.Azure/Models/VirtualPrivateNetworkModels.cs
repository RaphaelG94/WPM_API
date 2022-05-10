namespace WPM_API.Azure.Models
{
    public class VPN
    {
        public VPN()
        {
            Local = new VPNLocalProperties();
            Virtual = new VPNVirtualProperties();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string ResourceGroup { get; set; }
        public string SharedKey { get; set; }

        public VPNLocalProperties Local { get; set; }
        public VPNVirtualProperties Virtual { get; set; }
        //public string localAddressPrefix;
        //public string gatewayIpAddress;
        //public string publicIpAddress;

        //public string virtualNetwork;
        //public string virtualNetworkSubnet;
        //public string vpnType;
    }

    public class VPNLocalProperties
    {
        public string AddressRange { get; set; }
        public string PublicIp { get; set; }
    }

    public class VPNVirtualProperties
    {
        public string VirtualNetwork { get; set; }
        public string PublicIp { get; set; }
    }

    public class VpnAdd
    {
        public string CustomerId { get; set; }
        public string SubscriptionId { get; set; }
        public string ResourceGroupName { get; set; }
        public string Name { get; set; }
        public string VirtualNetworkId { get; set; }
        public VPNLocalProperties Local { get; set; }
    }

    public class VpnEdit
    {
        public string CustomerId { get; set; }
        public string SubscriptionId { get; set; }
        public string ResourceGroupName { get; set; }
        public string Name { get; set; }
        public VPNLocalProperties Local { get; set; }
    }
}
