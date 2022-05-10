namespace WPM_API.Models
{
    public class VpnViewModel
    {
        public VpnViewModel() {
            this.Local = new VpnLocalProperties();
            this.Virtual = new VpnVirtualProperties();
        }
        public string Name { get; set; }
        public VpnLocalProperties Local { get; set; }
        public VpnVirtualProperties Virtual { get; set; }
    }

    public class VpnAddViewModel
    {
        public string Name { get; set; }
        public VpnLocalProperties Local { get; set; }
    }

    public class VpnLocalProperties
    {
        public string AddressRange { get; set; }
        public string PublicIp { get; set; }
    }

    public class VpnVirtualProperties
    {
        public string VirtualNetwork { get; set; }
        public string PublicIp { get; set; }
    }
}