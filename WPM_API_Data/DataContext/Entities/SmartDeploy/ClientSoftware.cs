using WPM_API.Data.DataContext.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace  WPM_API.Data.DataContext.Entities
{
    public class ClientSoftware : IEntity, IDeletable
    {
        [Key, Column("PK_ClientSoftware")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string ClientId { get; set; }
        [ForeignKey("ClientId")]
        public Client Client { get; set; }
        [ForeignKey("CustomerSoftwareId")]
        public CustomerSoftware CustomerSoftware { get; set; }
        public string CustomerSoftwareId { get; set; }
        public bool? Install { get; set; }
        public string RunningContext { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedByUserId { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string DeletedByUserId { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string RevisionNumber { get; set; }
        public bool AllWin10Versions { get; set; }
        public bool AllWin11Versions { get; set; }
    }
}
