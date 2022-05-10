using WPM_API.Data.DataContext.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace  WPM_API.Data.DataContext.Entities
{
    public class Task : IEntity, IDeletable
    {
        [Key, Column("PK_Task")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string DescriptionShort { get; set; }

        public string CommandLine { get; set; }
        public string SuccessValue { get; set; }
        public int EstimatedExecutionTime { get; set; }
        public bool UseShellExecute { get; set; }
        public string BlockFilePath { get; set; }

        public string ExecutionFileId { get; set; }
        [ForeignKey("ExecutionFileId")]
        public virtual File ExecutionFile { get; set; }

        [InverseProperty("Task")]
        public virtual List<File> Files { get; set; }

        public string CreatedByUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedByUserId { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string DeletedByUserId { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string VersionNr { get; set; }
        public bool CheckVersionNr { get; set; }
        public string ExePath { get; set; }
        public string ExecutionContext { get; set; }
        public string Visibility { get; set; }
        public bool RestartRequired { get; set; }
        public string RunningContext { get; set; }
        public string InstallationType { get; set; }
    }
}
