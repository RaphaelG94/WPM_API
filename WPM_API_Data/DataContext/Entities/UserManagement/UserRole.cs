using System.Collections.Generic;
using WPM_API.Data.DataContext.Interfaces;
using WPM_API.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace  WPM_API.Data.DataContext.Entities
{
    public class UserRole : IEntity
    {
        [Key, Column("PK_UserRole")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string UserId { get; set; }
        public string RoleId { get; set; }

        public virtual User User { get; set; }
        public virtual Role Role { get; set; }
        
    }
}
