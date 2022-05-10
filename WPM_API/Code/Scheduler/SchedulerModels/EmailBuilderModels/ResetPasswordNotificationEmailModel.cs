using WPM_API.Common;
using WPM_API.Code.Scheduler.Attributes;
using WPM_API.Code.Scheduler.DataModels;

namespace WPM_API.Code.Scheduler.SchedulerModels.EmailBuilderModels
{
    [SchedulerActionType(Enums.SchedulerActionTypes.ResetPasswordEmail)]
    public class ResetPasswordNotificationEmailModel : SchedulerModelBase
    {
        public string UserForgotPasswordId { get; set; }

        public ResetPasswordNotificationEmailModel(string createdByUserId) : base(createdByUserId)
        {
        }

        protected override void DoFillFromSchedulerData(SchedulerData schedulerData)
        {
            UserForgotPasswordId = schedulerData.EntityId1;
        }

        protected override void DoFillSchedulerData(SchedulerData schedulerData)
        {
            schedulerData.EntityId1 = UserForgotPasswordId;
        }
    }
}