using System.Collections.Generic;
using WPM_API.Data.DataContext.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace  WPM_API.Data.DataContext.Entities
{
    public class Subnet : IEntity
    {
        [Key, Column("PK_Subnet")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string AddressRange { get; set; }
        public int Number { get; set; }
        [ForeignKey("VirtualNetwork")]
        public string VirtualNetworkId { get; set; }
        public VirtualNetwork VirtualNetwork { get; set; }
        
    }
}