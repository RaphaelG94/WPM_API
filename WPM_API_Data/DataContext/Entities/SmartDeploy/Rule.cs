using WPM_API.Data.DataContext.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace  WPM_API.Data.DataContext.Entities
{
    public class Rule : IEntity, IDeletable
    {
        [Key, Column("PK_Rule")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string Name { get; set; }
        public RuleType Type { get; set; }
        public File Data { get; set; }
        public List<Architecture> Architecture { get; set; }
        public bool Successon { get; set; }
        public string Path { get; set; }

        public string CreatedByUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? UpdatedByUserId { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string? DeletedByUserId { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string VersionNr { get; set; }
        public bool CheckVersionNr { get; set; }
        public string OsType  { get; set; }
        public List<OsVersionName> OsVersionNames { get; set; }
        public List<Win10Version> Win10Versions { get; set; }
        public List<Win11Version> Win11Versions { get; set; }
    }    

    public class OsVersionName : IEntity, IDeletable
    {
        [Key, Column("PK_OSVersion")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string Version { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? UpdatedByUserId { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string? DeletedByUserId { get; set; }
        public DateTime? DeletedDate { get; set; }
    }

    public class Win10Version : IEntity, IDeletable
    {
        [Key, Column("PK_Win10Version")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string Version { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? UpdatedByUserId { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string? DeletedByUserId { get; set; }
        public DateTime? DeletedDate { get; set; }
    }

    public class Win11Version :IEntity, IDeletable
    {
        [Key, Column("PK_Win11Version")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string Version { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? UpdatedByUserId { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string? DeletedByUserId { get; set; }
        public DateTime? DeletedDate { get; set; }
    }

    public class Architecture : IEntity, IDeletable
    {
        [Key, Column("PK_Architecture")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string Version { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? UpdatedByUserId { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string? DeletedByUserId { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}
