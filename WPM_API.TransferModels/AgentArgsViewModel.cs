using System;
using System.Collections.Generic;
using System.Text;

namespace  WPM_API.TransferModels
{
    public enum CloudFlag
    {
        AzureVM,
        OnPremise
    }
    public class AgentArgsViewModel
    {
        public string CustomerId { get; set; }
        public CloudFlag CloudFlag { get; set; }
        public string SecurityToken { get; set; }
        public string uuid { get; set; }
        public string ComputerName { get; set; }
    }
}
