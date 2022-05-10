using WPM_API.Data.DataContext.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace  WPM_API.Data.DataContext.Entities
{
    public class GroupPolicyObject : IEntity
    {
        [Key, Column("PK_GPO")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public bool BsiCertified { get; set; }
        public File Wallpaper { get; set; }
        public File Lockscreen { get; set; }
        public string Settings { get; set; }
        
    }
}
