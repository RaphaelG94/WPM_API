using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WPM_API.Models
{
    public class OptionAssignRefViewModel
    {
        public string Id { get; set; }
        public int Order { get; set; }
        public string Name { get; set; }
        public List<ParameterViewModel> Parameters { get; set; }
        public string ScriptVersionId { get; set; }
    }

    public class OptionAssignViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        //public string Description { get; set; }
        public string Version { get; set; }
    }

    public class DeviceOptionVersionRefViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string OSType { get; set; }
    }

    public class OptionAssignVersionViewModel
    {
        public string Id { get; set; }
        public List<ParameterViewModel> Parameter { get; set; }
    }
}
