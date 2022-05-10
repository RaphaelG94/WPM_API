using WPM_API.Code.Scheduler.DataModels;

namespace WPM_API.Code.Scheduler.SchedulerActions
{
    public interface ISchedulerAction
    {
        void Process(SchedulerData schedulerData);
    }
}