using WPM_API.Data.DataContext.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace  WPM_API.Data.DataContext.Entities
{
    public class ClientTask : IEntity
    {
        [Key, Column("PK_ClientTask")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string ClientId { get; set; }
        [ForeignKey("ClientId")]
        public Client Client { get; set; }
        public string TaskId { get; set; }
        [ForeignKey("TaskId")]
        public Task Task { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
    }
}
