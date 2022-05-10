using System;
using System.Collections.Generic;

namespace  WPM_API.TransferModels
{
    public class ClientAddViewModel
    {
        public string Description { get; set; }
        public string CustomerName { get; set; }
        public string Name { get; set; }
        public string uuid { get; set; }
        public List<InstalledSoftwareViewModel> InstalledSoftware { get; set; }
        public List<string> MACAdresses { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public string Type { get; set; }
        public string HyperVisor { get; set; }
        public string SerialNumber { get; set; }
        public string AutoRegisterPassword { get; set; }
    }

    public class ClientAddRegisterAuto : ClientAddViewModel
    {
        public string OSEdition { get; set; }
        public string OSMemorySize { get; set; }
        public string OSOperatingSystemSKU { get; set; }
        public string OSArchitecture { get; set; }
        public string CSPvendor { get; set; }
        public DateTime OSInstallDateUTC { get; set; }
        public string OSType { get; set; }
        public string OSVersion { get; set; }
        public string ModelSeries { get; set; }
        public string CSPversion { get; set; }
        public string CSPname { get; set; }
        public string OSLanguage { get; set; }
        public string OSProductSuite { get; set; }
        public string Processor { get; set; }
        public string MainFrequentUser { get; set; }
        public List<string> MacAddresses { get; set; }
        public string SerialNumber { get; set; }
    }

    public class InstalledSoftwareViewModel
    {
        public string DisplayName { get; set; }
        public string Version { get; set; }
        public string Size { get; set; }
        public string InstalledAt { get; set; }
    }

    public class HelpdeskInfoViewModel
    {
        public string Email { get; set; }
        public string OpeningTimes { get; set; }
        public string Phone { get; set; }
        public string CustomerName { get; set; }
        public string CmdBtn1 { get; set; }
        public string CmdBtn2 { get; set; }
        public string CmdBtn3 { get; set; }
        public string CmdBtn4 { get; set; }
        public string Btn1Label { get; set; }
        public string Btn2Label { get; set; }
        public string Btn3Label { get; set; }
        public string Btn4Label { get; set; }
        public string BannerLink { get; set; }
    }
}
