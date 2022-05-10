using WPM_API.TransferModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace  WPM_API.Data.Models
{
    public class VendorModelViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ModelFamily { get; set; }
        public string ModelType { get; set; }
        public List<FileRef> Files { get; set; }
    }
}
