using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WPM_API.Options
{
    public class SendMailCreds
    {
        public string Email { get; set; }
        public string Host { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
        public string EnableSsl { get; set; }
        public string DisplayName { get; set; }
    }
}
