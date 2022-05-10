using WPM_API.Data.DataContext.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace  WPM_API.Data.DataContext.Entities
{
    public class Server : IEntity
    {
        [Key, Column("PK_Server")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public virtual Domain Domain { get; set; }

        public ServerType Type { get; set; }

        public string VirtualMachineId { get; set; }

        [ForeignKey("VirtualMachineId")]
        public virtual VirtualMachine VirtualMachine { get; set; }

        public string OrganizationalUnitId { get; set; }

        [ForeignKey("OrganizationalUnitId")]
        public virtual OrganizationalUnit OrganizationalUnit { get; set; }

        
    }

    public enum ServerType
    {
        ADController
    };

}

