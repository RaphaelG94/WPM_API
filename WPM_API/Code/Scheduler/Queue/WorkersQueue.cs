using WPM_API.Code.Scheduler.DataModels;
using WPM_API.Code.Scheduler.Queue.Workers;

namespace WPM_API.Code.Scheduler.Queue
{
    public class WorkersQueue
    {
        private WorkerThread<IEmailWorkerService> EmailSenderThread { get; set; }
        private WorkerThread<ISchedulerWorkerService> SchedulerThread { get; set; }
        
        private readonly IEmailWorkerService _emailWorkerService;
        private readonly ISchedulerWorkerService _schedulerWorkerService;

        public WorkersQueue(IEmailWorkerService emailWorkerService, ISchedulerWorkerService schedulerWorkerService)
        {
            _emailWorkerService = emailWorkerService;
            _schedulerWorkerService = schedulerWorkerService;
        }

        public void Init()
        {
            EmailSenderThread = new WorkerThread<IEmailWorkerService>("notification-email-sender-thread", _emailWorkerService);
            SchedulerThread = new WorkerThread<ISchedulerWorkerService>("notification-scheduler-thread", _schedulerWorkerService);
        }

        public void EmailSync(SchedulerData schedulerData)
        {
            SchedulerThread.NotificationWorker.ProcessSchedulerSync(schedulerData);
            EmailSenderThread.NotificationWorker.ProcessSchedulerSync(schedulerData.Id);
        }

        public void WakeUpScheduler()
        {
            SchedulerThread.WakeUp();
        }
    }
}