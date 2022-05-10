using WPM_API.Data.DataContext.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace  WPM_API.Data.DataContext.Entities.AssetMgmt
{
    public class AssetModel : IEntity, IDeletable 
    {
        [Key, Column("PK_AssetModel")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string AssetID { get; set; }
        [ForeignKey("AssetTypeId")]
        public AssetType AssetType { get; set; }
        public string CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedByUserId { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string DeletedByUserId { get; set; }
        public DateTime? DeletedDate { get; set; }
        [ForeignKey("ClientId")]
        public Client Client { get; set; }
        [ForeignKey("AssetClassId")]
        public AssetClass AssetClass { get; set; }
        public string AssetStatus { get; set; }
        public string Description { get; set; }
        public string Building { get; set; }
        public string Floor { get; set; }
        public string Room { get; set; }
        public string Coordinates { get; set; }
        public string AssetClassId { get; set; }
        public string AssetTypeId { get; set; }
        [ForeignKey("InvoiceId")]
        public File Invoice { get; set; }
        public string InvoiceId { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string PurchaseValue { get; set; }
        public string CINumber { get; set; }
        public string PersonId { get; set; }
        [ForeignKey("PersonId")]
        public Person Person { get; set; }
        public string LocationId { get; set; }
        [ForeignKey("LocationId")]
        public Location Location { get; set; }
        public string ClientId { get; set; }
        public string DepreciationMonths { get; set; }
        public string Currency { get; set; }
        public string VendorModelId { get; set; }
        public VendorModel VendorModel { get; set; }
    }

    public class AssetModelCSV
    {
        public AssetModelCSV (
            string Id, 
            string AssetID, 
            string AssetType, 
            string ClientName, 
            string QRValue,
            string AssetClass,
            string AssetStatus,
            string Description,
            string Building,
            string Floor,
            string Room,
            string Coordinates
            )
        {
            this.Id = Id;
            this.AssetID = AssetID;
            this.AssetType = AssetType;
            this.ClientName = ClientName;
            this.QRValue = QRValue;
            this.AssetClass = AssetClass;
            this.AssetStatus = AssetStatus;
            this.Description = Description;
            this.Building = Building;
            this.Floor = Floor;
            this.Room = Room;
            this.Coordinates = Coordinates;
        }

        public string Id { get; set; }
        public string AssetID { get; set; }
        public string AssetType { get; set; }
        public string  ClientName { get; set; }
        public string QRValue { get; set; }
        public string AssetClass { get; set; }
        public string AssetStatus { get; set; }
        public string Description { get; set; }
        public string Building { get; set; }
        public string Floor { get; set; }
        public string Room { get; set; }
        public string Coordinates { get; set; }
    }
}
