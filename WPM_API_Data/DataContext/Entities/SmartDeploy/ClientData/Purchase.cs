using WPM_API.Data.DataContext.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace  WPM_API.Data.DataContext.Entities
{
    /// <summary>
    /// Purchase properties of a Client.
    /// </summary>
    public class Purchase : IEntity
    {
        [Key, Column("PK_PurchaseId")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string PurchaseDate { get; set; }
        public string CostUnitAssignment { get; set; }
        public string Vendor { get; set; }
        public string AcquisitionCost { get; set; }
        public string DecommissioningDate { get; set; }
        
    }
}