using WPM_API.Data.DataContext.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace  WPM_API.Data.DataContext.Entities
{
    public class ClientOption : IEntity, IDeletable
    {
        [Key, Column("PK_ClientOption")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string ClientId { get; set; }
        [ForeignKey("ClientId")]
        public Client Client { get; set; }
        public string DeviceOptionId { get; set; }
        [ForeignKey("DeviceOptionId")]
        public ScriptVersion DeviceOption { get; set; }
        public int Order { get; set; }
        public List<Parameter> Parameters { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? UpdatedByUserId { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string? DeletedByUserId { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool PEOnly { get; set; }
        public string OSType { get; set; }
    }
}
