using WPM_API.Data.DataContext.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace  WPM_API.Data.DataContext.Entities
{
    public class ErrorLog : IEntity
    {
        [Key, Column("PK_ErrorLog")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Error { get; set; }
        public string Source { get; set; }
        public DateTime Time { get; set; }
        public string Data { get; set; }
    }
}
