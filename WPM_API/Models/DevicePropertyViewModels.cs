using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WPM_API.Models
{
    public class DevicePropertyViewModel
    {
        public string Id { get; set; }
        public string PropertyName { get; set; }
        public string Command { get; set; }
        public CategoryViewModel Category { get; set; }
        public string ParameterName { get; set; }
    }
    public class DevicePropertyAddViewModel
    {
        public string PropertyName { get; set; }
        public string Command { get; set; }
        public CategoryAddViewModel Category { get; set; }
        public string ParameterName { get; set; }
    }
    public class DevicePropertyEditViewModel
    {
        public string PropertyName { get; set; }
        public string Command { get; set; }
        public CategoryAddViewModel Category { get; set; }
    }
}
