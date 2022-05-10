using System;
using WPM_API.Code.Infrastructure;
using WPM_API.Code.Scheduler.DataModels;
using WPM_API.Code.Scheduler.Queue;
using WPM_API.Code.Scheduler.SchedulerModels;

namespace WPM_API.Code.Scheduler
{
    public class SchedulerService: ISchedulerService
    {
        private readonly WorkersQueue _workersQueue;

        public SchedulerService(WorkersQueue workersQueue)
        {
            _workersQueue = workersQueue;
        }

        protected SchedulerService()
        {
        }

        public void ScheduleAction<T>(T schedulerModel) where T : SchedulerModelBase
        {
            SaveSchedulerData(schedulerModel);
        }

        public void EmailSync<T>(T schedulerModel) where T : SchedulerModelBase
        {
            var schedulerData = SaveSchedulerData(schedulerModel, true);
            _workersQueue.EmailSync(schedulerData);
        }

        private SchedulerData SaveSchedulerData<T>(T schedulerModel, bool isSync = false)
            where T : SchedulerModelBase
        {
            var schedulerData = schedulerModel.BuildSchedulerData();

            Data.DataContext.Entities.Scheduler scheduler;
            using (var unitOfWork = AppDependencyResolver.Current.CreateUoWinCurrentThread())
            {
                scheduler = unitOfWork.Schedulers.CreateEmpty();
                MapScheduler(schedulerData, scheduler);
                scheduler.IsSynchronous = isSync;
                unitOfWork.SaveChanges();
            }

            schedulerData.Id = scheduler.Id;

            return schedulerData;
        }

        private void MapScheduler(SchedulerData schedulerData, Data.DataContext.Entities.Scheduler destination)
        {
            destination.CreatedDate = DateTime.Now;
            destination.SchedulerActionType = schedulerData.SchedulerActionType;
            destination.CreatedByUserId = schedulerData.CreatedByUserId;
            destination.OnDate = schedulerData.OnDate;
            destination.EntityId1 = schedulerData.EntityId1;
            destination.EntityId2 = schedulerData.EntityId2;
            destination.EntityId3 = schedulerData.EntityId3;
            destination.EntityId4 = schedulerData.EntityId4;
            destination.EntityData1 = schedulerData.EntityData1;
            destination.EntityData2 = schedulerData.EntityData2;
            destination.EntityData3 = schedulerData.EntityData3;
            destination.EntityData4 = schedulerData.EntityData4;

        }
    }
}