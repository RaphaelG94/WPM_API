using WPM_API.Data.DataContext.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace  WPM_API.Data.DataContext.Entities
{
    public class ClientParameter : IEntity
    {
        [Key, Column("PK_ClientParameter")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string PropertyName { get; set; }
        public string ParameterName { get; set; }
        public string Value { get; set; }
        public string ClientId { get; set; }
        [ForeignKey("ClientId")]
        public Client Client { get; set; }
        public bool IsEditable { get; set; }
        
    }
}
