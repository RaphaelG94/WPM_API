using System;
using System.Collections.Generic;

namespace  WPM_API.TransferModels
{
    public class DatasheetViewModel
    {
        public List<DatasheetEntryViewModel> Categories { get; set; }
        public List<PreinstalledSoftwareViewModel> PreinstalledSoftwares {get; set;}
    }

    public class DatasheetEntryViewModel
    {
        public string Category { get; set; }
        public List<PropertyResultViewModel> Values { get; set; }
    }

    public class PropertyResultViewModel
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public class PreinstalledSoftwareViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public string Size { get; set; }
        public string InstalledAt { get; set; }
    }
}
