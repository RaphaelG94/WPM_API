using System.Collections.Generic;
using WPM_API.Data.DataContext.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace  WPM_API.Data.DataContext.Entities
{
    public class ClientProperty : IEntity
    {
        [Key, Column("PK_ClientProperty")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string PropertyName { get; set; }
        public string CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
        public string Command { get; set; }
        public string ParameterName { get; set; }
        
    }
}
