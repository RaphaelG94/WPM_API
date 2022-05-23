using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WPM_API.Code.Infrastructure;
using WPM_API.Code.Infrastructure.LogOn;
using WPM_API.Common;
using WPM_API.Data.DataContext.Entities;
using WPM_API.Data.Models;
using WPM_API.Models;
using WPM_API.Models.Release_Mgmt;
using WPM_API.Options;
using DATA = WPM_API.Data.DataContext.Entities;

namespace WPM_API.Controllers
{
    [Authorize(Policy = Constants.Roles.Customer)]
    [Route("shop")]
    public class ShopItemController : BasisController
    {
        public ShopItemController(AppSettings appSettings, ConnectionStrings connectionStrings, OrderEmailOptions orderEmailOptions, AgentEmailOptions agentEmailOptions, SendMailCreds sendMailCreds, SiteOptions siteOptions, ILogonManager logonManager) : base(appSettings, connectionStrings, orderEmailOptions, agentEmailOptions, sendMailCreds, siteOptions, logonManager)
        {
        }

        [HttpGet]
        public IActionResult GetShopItems()
        {
            var ShopItemList = new List<DATA.ShopItem>();
            var ShopItemListView = new List<ShopItemViewModel>();
            using (var unitOfWork = CreateUnitOfWork())
            {
                ShopItemList = unitOfWork.Shop.GetAll(ShopItemIncludes.GetAllIncludes()).ToList();
                foreach (var shopItem in ShopItemList)
                {
                    ShopItemViewModel itemView = null;
                    itemView = Mapper.Map<ShopItemViewModel>(shopItem);
                    itemView.Categories = new List<CategoryViewModel>();
                    foreach (var category in shopItem.Categories)
                    {
                        itemView.Categories.Add(Mapper.Map<DATA.Category, CategoryViewModel>(unitOfWork.Categories.Get(category.CategoryId)));
                    }
                    itemView.bruttoPrice = ConvertToBruttoPrice(Convert.ToDouble(shopItem.Price)).ToString("N2");
                    itemView.bruttoManagedServicePrice = ConvertToBruttoPrice(Convert.ToDouble(shopItem.ManagedServicePrice)).ToString("N2");
                    itemView.bruttoManagedServiceLifecyclePrice = ConvertToBruttoPrice(Convert.ToDouble(shopItem.ManagedServiceLifecyclePrice)).ToString("N2");
                    itemView.Drivers = new List<string>();

                    foreach (DriverShopItem ds in shopItem.DriverShopItems)
                    {
                        itemView.Drivers.Add(ds.DriverId);
                    }
                    itemView.DriverShopItems = null;
                    ShopItemListView.Add(itemView);
                }
            }
            var json = JsonConvert.SerializeObject(ShopItemListView, serializerSettings);
            return new OkObjectResult(json);
        }

        [HttpGet]
        [Route("getSoftware/{customerId}")]
        public IActionResult GetSoftwareForShop([FromRoute] string customerId)
        {
            using (var unitOfWork = CreateUnitOfWork())
            {
                List<Software> softwares = UnitOfWork.Software.GetAll("Customers").Where(x => x.SoftwareStreamId != null && x.PublishInShop).ToList();
                List<SoftwareShopViewModel> result = new List<SoftwareShopViewModel>();
                foreach (Software sw in softwares)
                {
                    SoftwareStream stream = unitOfWork.SoftwareStreams.Get(sw.SoftwareStreamId, "Icon");
                    bool skip = false;
                    if (sw.Customers != null && sw.Customers.Count != 0)
                    {
                        if (sw.Customers.Find(x => x.CustomerId == customerId) == null)
                        {
                            skip = true;
                        }
                    }
                    if (!skip)
                    {
                        SoftwareShopViewModel model = new SoftwareShopViewModel();
                        model.Id = sw.Id;
                        model.Name = sw.Name;
                        model.Architecture = stream.Architecture;
                        model.Language = stream.Language;
                        model.Icon = Mapper.Map<FileRefModel>(stream.Icon);
                        model.Vendor = stream.Vendor;
                        model.Version = sw.Version;
                        result.Add(model);
                    }
                }
                var json = JsonConvert.SerializeObject(result, serializerSettings);
                return Ok(json);
            }
        }

        [HttpGet]
        [Route("shopCategories")]
        public IActionResult GetShopCategories()
        {
            List<ShopItemCategory> categories = UnitOfWork.ShopItemCategories.GetAll("Category").ToList();
            List<Category> alreadyPushed = new List<Category>();
            foreach (ShopItemCategory sic in categories)
            {
                var foundCategory = alreadyPushed.Find(x => x.Id == sic.CategoryId);
                if (foundCategory == null)
                {
                    alreadyPushed.Add(sic.Category);
                }
            }

            var json = JsonConvert.SerializeObject(Mapper.Map<List<CategoryViewModel>>(alreadyPushed), serializerSettings);
            return Ok(json);
        }

        [HttpGet]
        [Route("{shopItemId}")]
        public IActionResult GetShopItem([FromRoute] string shopItemId)
        {
            DATA.ShopItem shopItem = new DATA.ShopItem();
            ShopItemViewModel shopItemView = new ShopItemViewModel();
            using (var unitOfWork = CreateUnitOfWork())
            {
                shopItem = unitOfWork.Shop.Get(shopItemId, ShopItemIncludes.GetAllIncludes());
                shopItemView = Mapper.Map<ShopItemViewModel>(shopItem);
                shopItemView.Categories = new List<CategoryViewModel>();
                foreach (var category in shopItem.Categories)
                {
                    DATA.Category dbCategory = unitOfWork.Categories.Get(category.Id);
                    shopItemView.Categories.Add(Mapper.Map<DATA.Category, CategoryViewModel>(dbCategory));
                }
            }
            // Calculate brutto price
            shopItemView.bruttoPrice = ConvertToBruttoPrice(Convert.ToDouble(shopItem.Price)).ToString("N2");
            shopItemView.bruttoManagedServicePrice = ConvertToBruttoPrice(Convert.ToDouble(shopItem.ManagedServicePrice)).ToString("N2");
            shopItemView.bruttoManagedServiceLifecyclePrice = ConvertToBruttoPrice(Convert.ToDouble(shopItem.ManagedServiceLifecyclePrice)).ToString("N2");
            var json = JsonConvert.SerializeObject(shopItemView, serializerSettings);
            return new OkObjectResult(json);
        }

        [HttpPost]
        public IActionResult AddShopItem([FromBody] ShopItemAddViewModel shopItem)
        {
            DATA.ShopItem newShopItem = Mapper.Map<DATA.ShopItem>(shopItem);
            using (var unitOfWork = CreateUnitOfWork())
            {
                newShopItem = unitOfWork.Shop.CreateEmpty(GetCurrentUser().Id);
                newShopItem.Images = Mapper.Map<List<DATA.File>>(shopItem.Images);
                newShopItem.Name = shopItem.Name;
                newShopItem.Price = shopItem.Price;
                newShopItem.ManagedServiceLifecyclePrice = shopItem.ManagedServiceLifecyclePrice;
                newShopItem.ManagedServicePrice = shopItem.ManagedServicePrice;
                newShopItem.Description = shopItem.Description;
                newShopItem.DescriptionShort = shopItem.DescriptionShort;
                //unitOfWork.Shop.MarkForInsert(newShopItem, GetCurrentUser().Id);
                newShopItem.Categories = new List<DATA.ShopItemCategory>();
                newShopItem.DriverShopItems = new List<DriverShopItem>();
                if (shopItem.Categories != null)
                {
                    foreach (var category in shopItem.Categories)
                    {
                        DATA.Category dbCategory = null;
                        if (category.Id != null)
                        {
                            dbCategory = unitOfWork.Categories.Get(category.Id);
                        }
                        else
                        {
                            var cat = unitOfWork.Categories.GetByName(category.Name.ToLower(), DATA.CategoryType.ShopItem);
                            if (cat == null)
                            {
                                dbCategory = Mapper.Map<DATA.Category>(category);
                                unitOfWork.Categories.MarkForInsert(dbCategory);
                                //unitOfWork.SaveChanges();
                            }
                        }
                        newShopItem.Categories.Add(new DATA.ShopItemCategory() { ShopItemId = newShopItem.Id, Category = dbCategory, ShopItem = newShopItem });
                    }
                }
                if (shopItem.Drivers != null)
                {
                    foreach (string driverId in shopItem.Drivers)
                    {
                        Driver driver = unitOfWork.Drivers.GetOrNull(driverId, "DriverShopItems");
                        if (driver == null)
                        {
                            return BadRequest("ERROR: A selected Driver does not exist");
                        }
                        DriverShopItem driverShopItem = unitOfWork.DriverShopItems.CreateEmpty();
                        driverShopItem.DriverId = driverId;
                        driverShopItem.ShopItemId = newShopItem.Id;
                        newShopItem.DriverShopItems.Add(driverShopItem);
                        driver.DriverShopItems.Add(driverShopItem);
                        unitOfWork.Drivers.MarkForUpdate(driver, GetCurrentUser().Id);
                    }
                }
                unitOfWork.SaveChanges();
            }
            using (var unitOfWork = CreateUnitOfWork())
            {
                ShopItemAddViewModel itemView = Mapper.Map<ShopItemViewModel>(unitOfWork.Shop.Get(newShopItem.Id, ShopItemIncludes.GetAllIncludes()));
                itemView.bruttoPrice = ConvertToBruttoPrice(Convert.ToDouble(shopItem.Price)).ToString("N2");
                itemView.bruttoManagedServicePrice = ConvertToBruttoPrice(Convert.ToDouble(shopItem.ManagedServicePrice)).ToString("N2");
                itemView.bruttoManagedServiceLifecyclePrice = ConvertToBruttoPrice(Convert.ToDouble(shopItem.ManagedServiceLifecyclePrice)).ToString("N2");
                itemView.DriverShopItems = null;
                itemView.Drivers = shopItem.Drivers;
                var json = JsonConvert.SerializeObject(itemView, serializerSettings);
                return new OkObjectResult(json);
            }
        }

        [HttpPost]
        [Route("upload")]
        public async Task<IActionResult> UploadFileAsync([FromForm] IFormFile file)
        {
            FileRepository.FileRepository shop = new FileRepository.FileRepository(connectionStrings.FileRepository, appSettings.FileRepositoryFolder);
            string id = await shop.UploadFile(file.OpenReadStream());
            var json = JsonConvert.SerializeObject(new { Id = id, Name = file.FileName }, serializerSettings);
            return new OkObjectResult(json);
        }

        [HttpPut]
        [Route("{shopItemId}")]
        public IActionResult UpdateShopItem([FromRoute] string shopItemId, [FromBody] ShopItemEditViewModel shopItemEdit)
        {
            DATA.ShopItem dbShopItem = null;
            ShopItemViewModel itemView = null;
            using (var unitOfWork = CreateUnitOfWork())
            {
                dbShopItem = unitOfWork.Shop.Get(shopItemId, ShopItemIncludes.GetAllIncludes());

                foreach (FileRefModel image in shopItemEdit.Images)
                {
                    DATA.File tempFile = unitOfWork.Files.Get(image.Id);
                    if (tempFile != null)
                    {
                        dbShopItem.Images.Add(tempFile);
                    }
                }
                //dbShopItem.Images = Mapper.Map<List<Data.DataContext.Entities.File>>(shopItemEdit.Images);
                if (dbShopItem == null)
                {
                    return new NotFoundResult();
                }
                foreach (string driverId in shopItemEdit.Drivers)
                {
                    Driver driver = unitOfWork.Drivers.GetOrNull(driverId, "DriverShopItems");
                    if (driver == null)
                    {
                        return BadRequest("ERROR: A selected Driver does not exist");
                    }
                    DriverShopItem driverShopItem = dbShopItem.DriverShopItems.Find(x => x.DriverId == driverId);
                    if (driverShopItem == null)
                    {
                        DriverShopItem newDriverShopItem = unitOfWork.DriverShopItems.CreateEmpty();
                        newDriverShopItem.DriverId = driverId;
                        newDriverShopItem.ShopItemId = dbShopItem.Id;
                        dbShopItem.DriverShopItems.Add(newDriverShopItem);
                        driver.DriverShopItems.Add(newDriverShopItem);
                    }
                    unitOfWork.Drivers.MarkForUpdate(driver, GetCurrentUser().Id);
                }
                dbShopItem.Name = shopItemEdit.Name;
                dbShopItem.Description = shopItemEdit.Description;
                dbShopItem.DescriptionShort = shopItemEdit.DescriptionShort;
                dbShopItem.Price = shopItemEdit.Price;
                dbShopItem.ManagedServicePrice = shopItemEdit.ManagedServicePrice;
                dbShopItem.ManagedServiceLifecyclePrice = shopItemEdit.ManagedServiceLifecyclePrice;
                // delete all old categories, then add all assigned categories new
                itemView = Mapper.Map<ShopItemViewModel>(dbShopItem);
                itemView.Categories = new List<CategoryViewModel>();
                foreach (var category in shopItemEdit.Categories)
                {
                    ShopItemCategory existingCategory = null;
                    if (category.Id != null)
                    {
                        existingCategory = dbShopItem.Categories.Find(x => x.CategoryId == category.Id);
                        if (existingCategory == null)
                        {
                            DATA.Category dbCategory = new Category();
                            dbCategory = unitOfWork.Categories.GetByName(category.Name.ToLower(), DATA.CategoryType.ShopItem);
                            if (dbCategory == null)
                            {
                                dbCategory = Mapper.Map<DATA.Category>(category);
                                unitOfWork.Categories.MarkForInsert(dbCategory);
                                unitOfWork.SaveChanges();
                            }
                            dbShopItem.Categories.Add(new DATA.ShopItemCategory() { CategoryId = dbCategory.Id, ShopItemId = shopItemId, Category = dbCategory, ShopItem = dbShopItem });
                            itemView.Categories.Add(Mapper.Map<DATA.Category, CategoryViewModel>(dbCategory));
                        }
                        else
                        {
                            Category c = unitOfWork.Categories.Get(existingCategory.CategoryId);
                            itemView.Categories.Add(Mapper.Map<DATA.Category, CategoryViewModel>(c));
                        }
                    }
                    else
                    {
                        DATA.Category dbCategory = new Category();
                        dbCategory = unitOfWork.Categories.GetByName(category.Name.ToLower(), DATA.CategoryType.ShopItem);
                        if (dbCategory == null)
                        {
                            dbCategory = Mapper.Map<DATA.Category>(category);
                            unitOfWork.Categories.MarkForInsert(dbCategory);
                            unitOfWork.SaveChanges();
                        }
                        dbShopItem.Categories.Add(new DATA.ShopItemCategory() { CategoryId = dbCategory.Id, ShopItemId = shopItemId, Category = dbCategory, ShopItem = dbShopItem });
                        itemView.Categories.Add(Mapper.Map<DATA.Category, CategoryViewModel>(dbCategory));
                    }
                }
                unitOfWork.Shop.MarkForUpdate(dbShopItem);
                unitOfWork.SaveChanges();
            }

            itemView.Drivers = shopItemEdit.Drivers;
            itemView.bruttoPrice = ConvertToBruttoPrice(Convert.ToDouble(dbShopItem.Price)).ToString("N2");
            itemView.bruttoManagedServicePrice = ConvertToBruttoPrice(Convert.ToDouble(dbShopItem.ManagedServicePrice)).ToString("N2");
            itemView.bruttoManagedServiceLifecyclePrice = ConvertToBruttoPrice(Convert.ToDouble(dbShopItem.ManagedServiceLifecyclePrice)).ToString();
            itemView.DriverShopItems = null;
            // ShopItem was changed and is returned.
            var json = JsonConvert.SerializeObject(itemView, serializerSettings);
            return new OkObjectResult(json);
        }

        [HttpDelete]
        [Route("{shopItemId}")]
        public IActionResult DeleteShopItem([FromRoute] string shopItemId)
        {
            DATA.ShopItem dbShopItem = null;
            using (var unitOfWork = CreateUnitOfWork())
            {
                dbShopItem = unitOfWork.Shop.Get(shopItemId);
                if (dbShopItem == null)
                {
                    return new NotFoundResult();
                }
                FileRepository.FileRepository shop = new FileRepository.FileRepository(connectionStrings.FileRepository, appSettings.FileRepositoryFolder);
                if (dbShopItem.Images != null)
                {
                    foreach (Data.DataContext.Entities.File image in dbShopItem.Images)
                    {
                        shop.DeleteFile(image.Guid);
                    }
                }
                unitOfWork.Shop.MarkForDelete(dbShopItem, GetCurrentUser().Id);
                unitOfWork.SaveChanges();
            }

            // ShopItem was successfully deleted.
            return new StatusCodeResult(204);
        }

        private double ConvertToBruttoPrice(double nettoprice)
        {
            const double c = 1.19; // MwSt 19%(1.19)
            return nettoprice * c;
        }

        [HttpGet]
        [Route("download/{fileId}")]
        public async Task<IActionResult> DownloadFileAsync([FromRoute] string fileId)
        {
            FileRepository.FileRepository shop = new FileRepository.FileRepository(connectionStrings.FileRepository, appSettings.FileRepositoryFolder);
            var file = UnitOfWork.Files.Get(fileId);
            var blob = shop.GetBlobFile(file.Guid);
            var ms = new MemoryStream();
            await blob.DownloadToAsync(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, System.Net.Mime.MediaTypeNames.Application.Octet, file.Name);
        }

        [HttpGet]
        [Route("getOsImages")]
        public IActionResult GetOSImages()
        {
            try
            {
                using (var unitOfWork = CreateUnitOfWork())
                {
                    List<ImageShopViewModel> result = new List<ImageShopViewModel>();
                    List<Image> images = unitOfWork.Images.GetAll().ToList();
                    foreach (Image image in images)
                    {
                        ImageStream stream = unitOfWork.ImageStreams.GetOrNull(image.ImageStreamId, "Icon");
                        if (image.PublishInShop)
                        {
                            ImageShopViewModel si = new ImageShopViewModel();
                            si.Id = image.Id;
                            si.Architecture = stream.Architecture;
                            if (image.BuildNr != null)
                            {
                                si.BuildNr = image.BuildNr;
                            }
                            else
                            {
                                si.BuildNr = "//";
                            }
                            if (stream.Edition != null)
                            {
                                si.Edition = stream.Edition;
                            }
                            else
                            {
                                si.Edition = "//";
                            }
                            si.Update = image.Update;
                            si.Language = stream.Language;
                            si.Name = image.Name;
                            si.Icon = Mapper.Map<FileRefModel>(stream.Icon);
                            result.Add(si);
                        }
                    }

                    var json = JsonConvert.SerializeObject(result, serializerSettings);
                    return Ok(json);
                }
            }
            catch (Exception e)
            {
                return BadRequest("ERROR: " + e.Message);
            }
        }

        [HttpGet]
        [Route("getDrivers/{customerId}")]
        public IActionResult GetDriversForShop([FromRoute] string customerId)
        {
            using (var unitOfWork = CreateUnitOfWork())
            {
                List<Driver> drivers = unitOfWork.Drivers.GetAll().Where(x => x.PublishInShop).ToList();
                List<CustomerDriver> customersDrivers = unitOfWork.CustomerDrivers.GetAll().Where(x => x.CustomerId == customerId).ToList();
                List<Driver> toRemove = new List<Driver>();
                foreach (Driver driver in drivers)
                {
                    if (customersDrivers.Find(x => x.DriverId == driver.Id) != null)
                    {
                        toRemove.Add(driver);
                    }
                }
                drivers = drivers.Except(toRemove).ToList();
                List<DriverViewModel> result = Mapper.Map<List<DriverViewModel>>(drivers);

                string json = JsonConvert.SerializeObject(result, serializerSettings);
                return Ok(json);
            }

        }
    }

    public class SoftwareShopViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Architecture { get; set; }
        public string Language { get; set; }
        public string Version { get; set; }
        public FileRefModel Icon { get; set; }
        public string Vendor { get; set; }
    }

    public class ImageShopViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Edition { get; set; }
        public string Architecture { get; set; }
        public string Language { get; set; }
        public string BuildNr { get; set; }
        public string Update { get; set; }
        public FileRefModel Icon { get; set; }
    }

    public class ShopItemCategoryViewModel
    {

    }
}