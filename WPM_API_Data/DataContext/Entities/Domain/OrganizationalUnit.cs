using WPM_API.Data.DataContext.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace  WPM_API.Data.DataContext.Entities
{
    public class OrganizationalUnit : IEntity
    {
        [Key, Column("PK_OrganizationalUnit")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<OrganizationalUnit> Children { get; set; }
        public bool IsLeaf { get; set; }
        
    }
}
