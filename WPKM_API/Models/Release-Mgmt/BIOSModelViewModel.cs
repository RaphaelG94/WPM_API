using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WPM_API.Models.Release_Mgmt
{
    // TODO: Add Hardware models & File content/readme
    public class BIOSModelViewModel
    {
        public string Id { get; set; }
        public string Vendor { get; set; }
        public string Version { get; set; }
        public string ValidOS { get; set; }
        public DateTime ReleaseDate { get; set; }
    }

    public class BIOSModelsViewModel
    {
        public List<BIOSModelViewModel> BiosModels { get; set; }
    }
}
