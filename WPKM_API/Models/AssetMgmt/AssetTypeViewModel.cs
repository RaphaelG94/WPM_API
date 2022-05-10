using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WPM_API.Models
{
    public class AssetTypeViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string CustomerId { get; set; }
        public CustomerViewModel Customer { get; set; }
        public bool fromAdmin { get; set; }
    }

    public class AssetClassViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string AssetTypeId { get; set; }
        public AssetTypeViewModel AssetType { get; set; }
        public string CustomerId { get; set; }
        public CustomerViewModel Customer { get; set; }
        public bool fromAdmin { get; set; }
    }
}
