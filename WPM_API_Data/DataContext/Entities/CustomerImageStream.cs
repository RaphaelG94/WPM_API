using WPM_API.Data.DataContext.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace  WPM_API.Data.DataContext.Entities
{
    public class CustomerImageStream : IEntity, IDeletable
    {
        [Key, Column("PK_ImageStream")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Vendor { get; set; }
        public string Description { get; set; }
        public string DescriptionShort { get; set; }
        public List<CustomerImage> Images { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? UpdatedByUserId { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string? DeletedByUserId { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string Architecture { get; set; }
        public string Language { get; set; }
        public string Website { get; set; }
        public File Icon { get; set; }
        public string CustomerId { get; set; }
        public string ImageStreamId { get; set; }
        public string SubFolderName { get; set; }
        public string Edition { get; set; }
        public string Type { get; set; }
        public string PrefixUrl { get; set; }
        public string SASKey { get; set; }
        public string ProductKey { get; set; }        
        public string LocalSettingLinux { get; set; }        
    }
}
