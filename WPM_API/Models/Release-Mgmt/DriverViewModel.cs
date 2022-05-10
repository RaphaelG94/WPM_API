using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WPM_API.Models.Release_Mgmt
{
    public class DriverViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string SubFolderPath { get; set; }
        public string ConnectionString { get; set; }
        public string Version { get; set; }
        public string Vendor { get; set; }
        public string ContainerName { get; set; }
        public bool PublishInShop { get; set; }
    }

    public class DriversViewModel
    {
        public List<DriverViewModel> Drivers { get; set; }
    }

}
