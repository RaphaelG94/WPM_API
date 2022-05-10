using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WPM_API.Common;
using WPM_API.Code.Scheduler.Attributes;
using WPM_API.Code.Scheduler.DataModels;

namespace WPM_API.Code.Scheduler.SchedulerModels.GenericActionModels
{
    [SchedulerActionType(Enums.SchedulerActionTypes.ExampleAction)]
    public class ExampleActionModel : SchedulerModelBase
    {
        public string Value { get; set; }

        public ExampleActionModel(string createdByUserId) : base(createdByUserId)
        {
        }

        protected override void DoFillFromSchedulerData(SchedulerData schedulerData)
        {
            Value = schedulerData.EntityData1;
        }

        protected override void DoFillSchedulerData(SchedulerData schedulerData)
        {
            schedulerData.EntityData1 = Value;
        }
    }
}