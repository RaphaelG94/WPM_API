using WPM_API.Data.DataContext.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace  WPM_API.Data.DataContext.Entities
{
    /**
     * This class represents the credentials used for getting access to different Clouds 
     **/
    public class CloudEntryPoint : IEntity, IDeletable
    {
        [Key, Column("PK_CloudEntryPoint")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string TenantId { get; set; }
        public string ClientSecret { get; set; }
        public string ClientId { get; set; }
        public string Type { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? UpdatedByUserId { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string? DeletedByUserId { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }
        public bool IsStandard { get; set; }
        public string Name { get; set; }
    }
}
