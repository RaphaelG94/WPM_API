using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WPM_API.Models
{
    public class InventarViewModel
    {
        public List<InvClientViewModel> Clients { get; set; }
        public List<ServerViewModel> Server { get; set; }
    }

    public class InvClientViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Ip { get; set; }
        public string Location { get; set; }
    }

    public class ServerViewModel
    {
        public string Id { get; set; }
        public string AzureId { get; set; }
        public string Name { get; set; }
        public string Ip { get; set; }
    }
}
