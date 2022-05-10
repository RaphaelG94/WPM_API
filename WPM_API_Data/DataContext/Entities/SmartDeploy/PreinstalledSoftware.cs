using WPM_API.Data.DataContext.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace  WPM_API.Data.DataContext.Entities.SmartDeploy
{
    public class PreinstalledSoftware : IEntity
    {
        [Key, Column("PK_PreinstalledSoftware")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Size { get; set; }
        public string Version { get; set; }
        public string InstalledAt { get; set; }
        public Client Client { get; set; }        
    }
}
