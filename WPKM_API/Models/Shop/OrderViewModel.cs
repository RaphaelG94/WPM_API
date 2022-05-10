using WPM_API.Data.DataContext.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WPM_API.Models
{
    public class OrderViewModel
    {
        public string Id { get; set; }
        public List<ShopItemViewModel> ShopItems { get; set; }
    }
    public class OrderAddViewModel
    {
        public List<string> ShopItems { get; set; }
    }
}
