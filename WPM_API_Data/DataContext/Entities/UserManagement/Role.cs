using WPM_API.Data.DataContext.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace  WPM_API.Data.DataContext.Entities
{
    public class Role : IEntity
    {
        public Role()
        {
            UserRoles = new List<UserRole>();
        }

        [Key, Column("PK_Role")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        [Required, StringLength(64)]
        public string Name { get; set; }

        public virtual List<UserRole> UserRoles { get; set; }
        
    }
}
