using WPM_API.Code.Scheduler.DataModels;

namespace WPM_API.Code.Scheduler.Queue.Workers
{
    public interface ISchedulerWorkerService : IWorkerServiceBase
    {
        void ProcessSchedulerSync(SchedulerData schedulerData);
    }
}
