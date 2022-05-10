using WPM_API.Code.Scheduler.SchedulerModels;

namespace WPM_API.Code.Scheduler
{
    public interface ISchedulerService
    {
        void ScheduleAction<T>(T schedulerModel) where T : SchedulerModelBase;
        void EmailSync<T>(T schedulerModel) where T : SchedulerModelBase;
    }
}
