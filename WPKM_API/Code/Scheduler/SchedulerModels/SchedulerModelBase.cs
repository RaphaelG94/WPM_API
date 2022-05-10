using System;
using System.Reflection;
using WPM_API.Common;
using WPM_API.Code.Scheduler.Attributes;
using WPM_API.Code.Scheduler.DataModels;

namespace WPM_API.Code.Scheduler.SchedulerModels
{
    public abstract class SchedulerModelBase
    {
        public string SchedulerId { get; set; }

        /// <summary>
        /// DateTime when scheduler will fire. Leaving empty equals using DateTime.Now
        /// </summary>
        public DateTime? OnDate { get; set; }

        public string CreatedByUserId { get; private set; }
        public Enums.SchedulerActionTypes SchedulerActionsType
        {
            get
            {
                var attr = GetType().GetCustomAttribute<SchedulerActionTypeAttribute>();
                if(attr == null)
                    throw new Exception("NotificationModelAttribute required for type - " + GetType());

                return attr.SchedulerActionsType;
            }
        }

        protected SchedulerModelBase(string createdByUserId)
        {
            CreatedByUserId = createdByUserId;
        }

        public void FillFromSchedulerData(SchedulerData schedulerData)
        {
            if (schedulerData.SchedulerActionType != SchedulerActionsType)
                throw new Exception(String.Format("EmailNotificationsType not match. {0} - {1}", schedulerData.SchedulerActionType, SchedulerActionsType));

            SchedulerId = schedulerData.Id;
            OnDate = schedulerData.OnDate;

            DoFillFromSchedulerData(schedulerData);
        }

        public SchedulerData BuildSchedulerData()
        {
            var res = new SchedulerData();
            res.Id = SchedulerId;
            res.SchedulerActionType = SchedulerActionsType;
            res.OnDate = OnDate ?? DateTime.Now;
            res.CreatedByUserId = CreatedByUserId;

            DoFillSchedulerData(res);

            return res;
        }

        protected abstract void DoFillFromSchedulerData(SchedulerData schedulerData);
        protected abstract void DoFillSchedulerData(SchedulerData schedulerData);
    }
}