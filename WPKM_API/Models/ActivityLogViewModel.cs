using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WPM_API.Models
{
    public class ActivityLogViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Message { get; set; }
        public string TimeStamp { get; set; }
    }
}
