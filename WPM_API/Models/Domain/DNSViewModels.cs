using System.Collections.Generic;

namespace WPM_API.Models
{
    public class DNSViewModel
    {
        public string Id { get; set; }
    }

    public class DNSAddViewModel 
    {
        public List<string> Forwarders { get; set; }
    }
}
