using System.Reflection;
using WPM_API.Code.Infrastructure;
using WPM_API.Code.Scheduler.Attributes;
using WPM_API.Code.Scheduler.DataModels;
using WPM_API.Code.Scheduler.SchedulerActions;
using WPM_API.Code.Scheduler.SchedulerModels;
using WPM_API.Common;
using WPM_API.Data.Files;

namespace WPM_API.Code.Scheduler.Queue.Workers.Impl
{
    public class SchedulerWorkerService : WorkerServiceBase, ISchedulerWorkerService
    {
        private readonly IPathResolver _pathResolver;
        private IAttachmentService _attachmentService { get; }

        public SchedulerWorkerService(IPathResolver pathResolver, IAttachmentService attachmentService)
        {
            _pathResolver = pathResolver;
            _attachmentService = attachmentService;
        }

        public override void LoadAndProcess()
        {
            List<SchedulerData> schedulers;
            using (var unitOfWork = CreateUnitOfWork())
            {
                var data = unitOfWork.Schedulers.GetSchedulersToProcess();
                schedulers = MapSchedulers(data);
            }

            foreach (var schedulerData in schedulers)
            {
                ProcessItem(schedulerData);
            }
        }

        public void ProcessSchedulerSync(SchedulerData schedulerData)
        {
            ProcessItem(schedulerData, true);
        }

        private void ProcessItem(SchedulerData schedulerData, bool isSync = false)
        {
            DateTime startTime = DateTime.Now;
            try
            {
                using (var unitOfWork = CreateUnitOfWork())
                {
                    var scheduler = unitOfWork.Schedulers.GetScheduler(schedulerData.Id);
                    scheduler.StartProcessDate = startTime;
                    unitOfWork.SaveChanges();

                    var manager = GetSchedulerManager(schedulerData.SchedulerActionType, new SchedulerActionArgs
                    {
                        UnitOfWork = unitOfWork,
                        PathResolver = _pathResolver,
                        AttachmentService = _attachmentService
                    });
                    manager.Process(schedulerData);

                    scheduler.EndProcessDate = DateTime.Now;
                    unitOfWork.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                // LogHolder.MainLog.Error(ex, "Error processing scheduler - " + schedulerData.Id);

                try
                {
                    using (var unitOfWork = CreateUnitOfWork())
                    {
                        var scheduler = unitOfWork.Schedulers.GetScheduler(schedulerData.Id);
                        scheduler.StartProcessDate = startTime;
                        scheduler.EndProcessDate = DateTime.Now;
                        scheduler.ErrorMessage = ex.GetBaseException().Message;

                        unitOfWork.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    // LogHolder.MainLog.Error(e, "Error occured while saving scheduler data in failed state - " + schedulerData.Id);
                }

                if (isSync)
                    throw;
            }
        }

        private ISchedulerAction GetSchedulerManager(Enums.SchedulerActionTypes schedulerActionType, SchedulerActionArgs args)
        {
            var modelType = Assembly.GetExecutingAssembly().GetTypes()
                .Single(t => typeof(SchedulerModelBase).IsAssignableFrom(t)
                           && t.GetCustomAttributes<SchedulerActionTypeAttribute>().Any(m => m.SchedulerActionsType == schedulerActionType));

            var builderType = typeof(SchedulerActionBase<>).MakeGenericType(modelType);

            var schedulerManagerType = Assembly.GetExecutingAssembly().GetTypes()
                .Where(builderType.IsAssignableFrom)
                .SingleOrDefault();

            if (schedulerManagerType == null)
                throw new Exception("Scheduler manager not found for - " + modelType);

            var manager = (ISchedulerAction)Activator.CreateInstance(schedulerManagerType, args);
            return manager;
        }

        private List<SchedulerData> MapSchedulers(IEnumerable<Data.DataContext.Entities.Scheduler> schedulers)
        {
            return schedulers.Select(m => new SchedulerData
            {
                Id = m.Id,
                SchedulerActionType = m.SchedulerActionType,
                CreatedByUserId = m.CreatedByUserId,
                OnDate = m.OnDate,
                EntityId1 = m.EntityId1,
                EntityId2 = m.EntityId2,
                EntityId3 = m.EntityId3,
                EntityId4 = m.EntityId4,
                EntityData1 = m.EntityData1,
                EntityData2 = m.EntityData2,
                EntityData3 = m.EntityData3,
                EntityData4 = m.EntityData4
            }).ToList();
        }
    }
}