using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WPM_API.Common;
using System.ComponentModel.DataAnnotations.Schema;
using WPM_API.Data.DataContext.Interfaces;

namespace  WPM_API.Data.DataContext.Entities
{
    public class Scheduler : IEntity
    {
        public Scheduler()
        {
            ChildSchedulers = new HashSet<Scheduler>();
            NotificationEmails = new HashSet<NotificationEmail>();
        }

        [Key, Column("PK_Scheduler")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public Enums.SchedulerActionTypes SchedulerActionType { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedByUserId { get; set; }
        public string ParentSchedulerId { get; set; }
        public DateTime OnDate { get; set; }
        public bool IsSynchronous { get; set; }
        public DateTime? StartProcessDate { get; set; }
        public DateTime? EndProcessDate { get; set; }
        
        public string ErrorMessage { get; set; }
        public string EntityId1 { get; set; }
        public string EntityId2 { get; set; }
        public string EntityId3 { get; set; }
        public string EntityId4 { get; set; }

        
        public string EntityData1 { get; set; }
        public string EntityData2 { get; set; }
        public string EntityData3 { get; set; }
        public string EntityData4 { get; set; }
        

        public virtual User CreatedByUser { get; set; }
        public virtual Scheduler ParentScheduler { get; set; }
        public virtual ICollection<Scheduler> ChildSchedulers { get; set; }

        public virtual ICollection<NotificationEmail> NotificationEmails { get; set; }
        
    }
}
