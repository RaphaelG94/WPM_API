using System;
using WPM_API.Common;

namespace WPM_API.Code.Scheduler.DataModels
{
    public class SchedulerData
    {
        public string Id { get; set; }
        public string CreatedByUserId { get; set; }
        public Enums.SchedulerActionTypes SchedulerActionType { get; set; }
        public DateTime OnDate { get; set; }
        public string EntityId1 { get; set; }
        public string EntityId2 { get; set; }
        public string EntityId3 { get; set; }
        public string EntityId4 { get; set; }
        public string EntityData1 { get; set; }
        public string EntityData2 { get; set; }
        public string EntityData3 { get; set; }
        public string EntityData4 { get; set; }
    }
}