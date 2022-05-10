using System.Collections.Generic;

namespace WPM_API.Azure.Models
{

    public class VirtualNetworkViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ResourceGroup { get; set; }
        public string AddressRange { get; set; }
        public List<SubnetViewModel> Subnets { get; set; }
        public string Dns { get; set; }
    }

    public class VirtualNetworkAddOrEditViewModel
    {
        public string Name { get; set; }
        public string AddressRange { get; set; }
        public List<SubnetViewModel> Subnets { get; set; }
    }

    public class SubnetViewModel
    {
        public string Name { get; set; }
        public string AddressRange { get; set; }
    }

}
