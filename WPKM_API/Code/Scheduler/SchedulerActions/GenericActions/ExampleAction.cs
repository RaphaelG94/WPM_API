using WPM_API.Common.Logs;
using WPM_API.Code.Scheduler.SchedulerModels.GenericActionModels;

namespace WPM_API.Code.Scheduler.SchedulerActions.GenericActions
{
    public class ExampleAction : SchedulerActionBase<ExampleActionModel>
    {
        public ExampleAction(SchedulerActionArgs args) : base(args)
        {
        }

        protected override void DoProcess(ExampleActionModel actionModel)
        {
            LogHolder.MainLog.Info("Example Action fired - " + actionModel.Value);
        }
    }
}