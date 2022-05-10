using WPM_API.Data.DataContext.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace  WPM_API.Data.DataContext.Entities
{
    public class OS : IEntity
    {
        [Key, Column("PK_OSId")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public bool Uefi { get; set; }
        public string OSVersion { get; set; }
        public string LanguagePackage { get; set; }
        public string KeyboardLayout { get; set; }
        
    }
}