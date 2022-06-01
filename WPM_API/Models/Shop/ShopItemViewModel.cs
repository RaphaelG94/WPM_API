using WPM_API.Data.DataContext.Entities;

namespace WPM_API.Models
{
    public class ShopItemViewModel : ShopItemAddViewModel
    {
        public string Id { get; set; }
    }

    public class ShopItemAddViewModel
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? DescriptionShort { get; set; }
        public string? Price { get; set; }
        public string? ManagedServicePrice { get; set; }
        public string? ManagedServiceLifecyclePrice { get; set; }
        public string? bruttoPrice { get; set; }
        public string? bruttoManagedServicePrice { get; set; }
        public string? bruttoManagedServiceLifecyclePrice { get; set; }
        public List<FileRefModel>? Images { get; set; }
        public List<CategoryViewModel>? Categories { get; set; }
        public List<string>? Drivers { get; set; }
        public List<DriverShopItem>? DriverShopItems { get; set; }

    }


    public class ShopItemEditViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string DescriptionShort { get; set; }
        public string Price { get; set; }
        public string ManagedServicePrice { get; set; }
        public string ManagedServiceLifecyclePrice { get; set; }
        public List<FileRefModel> Images { get; set; }
        public List<CategoryViewModel> Categories { get; set; }
        public List<string> Drivers { get; set; }
    }
}
