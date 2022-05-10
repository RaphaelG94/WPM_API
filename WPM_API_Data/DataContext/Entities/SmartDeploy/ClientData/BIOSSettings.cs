using WPM_API.Data.DataContext.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace  WPM_API.Data.DataContext.Entities
{
    /// <summary>
    /// BIOS Settings. Empty boilerplate class.
    /// TODO: Specify BIOS Settings attributes.
    /// </summary>
    public class BiosSettings : IEntity
    {
        [Key, Column("PK_BiosSettingsId")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
    }
}