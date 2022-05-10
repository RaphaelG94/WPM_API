using WPM_API.Data.Files;
using WPM_API.Data.Infrastructure;
using WPM_API.Code.Infrastructure;

namespace WPM_API.Code.Scheduler.SchedulerActions
{
    public class SchedulerActionArgs
    {
        public UnitOfWork UnitOfWork { get; set; }
        public IPathResolver PathResolver { get; set; }
        public IAttachmentService AttachmentService { get; set; }
    }
}