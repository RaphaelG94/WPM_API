using WPM_API.Data.DataContext.Interfaces;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace  WPM_API.Data.DataContext.Entities
{
    public class Inventory : IEntity, IDeletable
    {
        [Key, Column("PK_Inventory")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string ClientId { get; set; }
        [ForeignKey("ClientId")]
        public Client Client { get; set; }
        public string Type { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedByUserId { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string DeletedByUserId { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string Value { get; set; }
        public string OperationType { get; set; }
    }

    public class InventoryCSV
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string OperationType { get; set; }
        public string Value { get; set; }
        public string ClientId { get; set; }
        public string Type { get; set; }
    }
}


