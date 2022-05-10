using System;
using System.Collections.Generic;
using System.Text;

namespace  WPM_API.TransferModels
{
  public  class InventoryViewModel
    {
       public List<InventoryDataViewModel> InventoryData { get; set; }
    }

    public class InventoryDataViewModel
    {
        public string Value { get; set; }
        public string Method { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Class { get; set; }
        public string KeyName { get; set; }
        public bool IsMultiValue { get; set; }
        public string Category { get; set; }
    }
}
