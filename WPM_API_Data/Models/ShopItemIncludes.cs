using System;
using System.Collections.Generic;
using System.Text;

namespace  WPM_API.Data.Models
{
    public static class ShopItemIncludes
    {
        public const string Categories = "Categories";
        public const string Category = "Categories.Category";
        public const string Images = "Images";
        public const string DriverShopItems = "DriverShopItems";


        public static string[] GetAllIncludes()
        {
            string[] includes =
            {
                Categories, Images, Category, DriverShopItems
            };
            return includes;
        }
    }
}
