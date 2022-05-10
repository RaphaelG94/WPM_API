using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WPM_API.Models
{
    public class WMIInventoryCmdViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Command { get; set; }
        public ClientViewModel Client { get; set; }
        public string ClientId { get; set; }
    }
}
