
namespace  WPM_API.TransferModels
{
    /// <summary>
    /// Purchase properties of a Client.
    /// </summary>
    public class PurchaseViewModels
    {
        public string Id { get; set; }
        public string PurchaseDate { get; set; }
        public string CostUnitAssignment { get; set; }
        public string Vendor { get; set; }
        public string AcquisitionCost { get; set; }
        public string DecommissioningDate { get; set; }
    }
}