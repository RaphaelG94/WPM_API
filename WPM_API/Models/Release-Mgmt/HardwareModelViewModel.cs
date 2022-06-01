namespace WPM_API.Models.Release_Mgmt
{
    public class HardwareModelViewModel
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Vendor { get; set; }
        public string? ModelFamily { get; set; }
        public string? ModelType { get; set; }
        public DateTime ProductionStart { get; set; }
        public DateTime ProductionEnd { get; set; }
    }

    public class HardwareModelAddViewModel
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Vendor { get; set; }
        public string? ModelFamily { get; set; }
        public string? ModelType { get; set; }
        public string? ProductionStart { get; set; }
        public string? ProductionEnd { get; set; }
    }

    public class HardwareModelsViewModel
    {
        public List<HardwareModelViewModel> HardwareModels { get; set; }
    }
}
