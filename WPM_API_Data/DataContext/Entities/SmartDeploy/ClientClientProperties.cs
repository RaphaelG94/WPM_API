using System.Collections.Generic;
using WPM_API.Data.DataContext.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace  WPM_API.Data.DataContext.Entities
{
    public class ClientClientProperty : IEntity, IDeletable
    {
        [Key, Column("PK_ClientClientProperty")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string ClientId { get; set; }
        [ForeignKey("ClientId")]
        public Client Client { get; set; }
        public string ClientPropertyId { get; set; }
        [ForeignKey("ClientPropertyId")]
        public ClientProperty ClientProperty { get; set; }
        public string Value { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? UpdatedByUserId { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string? DeletedByUserId { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}
