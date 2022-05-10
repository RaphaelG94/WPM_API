using WPM_API.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WPM_API.Models.AssetMgmt
{
    public class AssetMgmtViewModels
    {
        public List<AssetModelViewModel> Assets { get; set; }
    }

    public class AssetModelViewModel
    {
        public string Id { get; set; }
        public string AssetID { get; set; }
        public string AssetTypeId { get; set; }
        public string CustomerId { get; set; }
        public CustomerViewModel Customer { get; set; }
        public ClientDetailInformation Client { get; set; }
        public string AssetClassId { get; set; }
        public string AssetStatus { get; set; }
        public string Description { get; set; }
        public AssetClassViewModel AssetClass { get; set; }
        public AssetTypeViewModel AssetType { get; set; }
        public string Building { get; set; }
        public string Floor { get; set; }
        public string Room { get; set; }
        public string Coordinates { get; set; }
        public FileRefModel Invoice { get; set; }
        public string InvoiceId { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string PurchaseValue { get; set; }
        public string CINumber { get; set; }
        public string PersonId { get; set; }
        public PersonViewModel Person { get; set; }
        public string LocationId { get; set; }
        public AddLocationViewModel Location { get; set; }
        public string ClientId { get; set; }
        public string DepreciationMonths { get; set; }
        public string Currency { get; set; }
        public VendorModelViewModel VendorModel { get; set; }
    }
}