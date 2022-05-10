using WPM_API.Data.Infrastructure;
using WPM_API.Code.Infrastructure;

namespace WPM_API.Code.Scheduler.Queue.Workers.Impl
{
    public abstract class WorkerServiceBase
    {
        public abstract void LoadAndProcess();

        protected UnitOfWork CreateUnitOfWork()
        {
            return AppDependencyResolver.Current.CreateUoWinCurrentThread();
        }
    }
}