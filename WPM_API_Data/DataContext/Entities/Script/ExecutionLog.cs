using WPM_API.Data.DataContext.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace  WPM_API.Data.DataContext.Entities
{
    public class ExecutionLog : IEntity, IDeletable
    {
        [Key, Column("PK_ExecutionLog")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        public string VirtualMachineId { get; set; }
        [ForeignKey("VirtualMachineId")]
        public virtual VirtualMachine VirtualMachine { get; set; }
        public string ClientId { get; set; }
        [ForeignKey("ClientId")]
        public virtual Client Client { get; set; }
        public DateTime ExecutionDate { get; set; }
        public List<Parameter> ScriptArguments { get; set; }
        public string Script { get; set; }
        public string ScriptVersionId { get; set; }
        [ForeignKey("ScriptVersionId")]
        public virtual ScriptVersion ScriptVersion { get; set; }
        public string CreatedByUserId {get; set; }
        public DateTime CreatedDate {get; set; }
        public string UpdatedByUserId {get; set; }
        public DateTime UpdatedDate {get; set; }
        public string DeletedByUserId {get; set; }
        public DateTime? DeletedDate {get; set; }
    }
}
