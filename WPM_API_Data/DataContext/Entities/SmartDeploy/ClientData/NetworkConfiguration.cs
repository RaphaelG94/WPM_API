using WPM_API.Data.DataContext.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace  WPM_API.Data.DataContext.Entities
{
    public class NetworkConfiguration : IEntity
    {
        [Key, Column("PK_NetworkConfigurationId")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string IPv4 { get; set; }
        public string IPv6 { get; set; }
        public string DNS { get; set; }
        public string Gateway { get; set; }
        public string DHCP { get; set; }
        
    }
}