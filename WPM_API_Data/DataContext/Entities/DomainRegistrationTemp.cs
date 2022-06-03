using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WPM_API.Data.DataContext.Interfaces;

namespace WPM_API.Data.DataContext.Entities
{
    public class DomainRegistrationTemp : IEntity
    {
        [Key, Column("PK_DomainRegistrationTemp")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}
