using TM = WPM_API.TransferModels;

namespace WPM_API.Models.Release_Mgmt
{
    public class OSModelViewModel
    {
        public string Id { get; set; }
        public string Vendor { get; set; }
        public string OSName { get; set; }
        public string Architecture { get; set; }
        public string OSType { get; set; }
        public string Version { get; set; }
        public DateTime ReleaseDate { get; set; }
        public DateTime SupportEnd { get; set; }
        public List<HardwareModelViewModel>? HardwareModels { get; set; }
        public List<string> ValidModels { get; set; }
        public TM.FileRef Content { get; set; }
    }

    public class OSModelAddViewModel
    {
        public string? Id { get; set; }
        public string Vendor { get; set; }
        public string OSName { get; set; }
        public string Architecture { get; set; }
        public string OSType { get; set; }
        public string Version { get; set; }
        public string ReleaseDate { get; set; }
        public string SupportEnd { get; set; }
        public List<HardwareModelViewModel>? HardwareModels { get; set; }
        public List<string> ValidModels { get; set; }
        public TM.FileRef Content { get; set; }
    }

    public class OSModelsViewModel
    {
        public List<OSModelViewModel> OsModels { get; set; }
    }
}
