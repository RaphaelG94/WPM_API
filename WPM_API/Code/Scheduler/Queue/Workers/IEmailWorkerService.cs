namespace WPM_API.Code.Scheduler.Queue.Workers
{
    public interface IEmailWorkerService: IWorkerServiceBase
    {
        void ProcessSchedulerSync(string schedulerId);
    }
}
