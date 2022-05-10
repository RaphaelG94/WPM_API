using System;
using WPM_API.Common;

namespace WPM_API.Code.Scheduler.Attributes
{
    public class SchedulerActionTypeAttribute : Attribute
    {
        public Enums.SchedulerActionTypes SchedulerActionsType { get; private set; }

        public SchedulerActionTypeAttribute(Enums.SchedulerActionTypes schedulerActionsType)
        {
            SchedulerActionsType = schedulerActionsType;
        }
    }
}