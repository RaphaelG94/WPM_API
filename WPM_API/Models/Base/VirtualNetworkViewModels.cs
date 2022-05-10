using System.Collections.Generic;

namespace WPM_API.Models
{
    public class VirtualNetworkViewModel
    {
        public string Name { get; set; }
        public string AddressRange { get; set; }
        public List<SubnetViewModel> Subnets { get; set; }
        public string Dns { get; set; }
    }

    public class VirtualNetworkAddViewModel
    {
        public string Name { get; set; }
        public string AddressRange { get; set; }
        public List<SubnetViewModel> Subnets { get; set; }
    }

    public class SubnetViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string AddressRange { get; set; }
    }

}
