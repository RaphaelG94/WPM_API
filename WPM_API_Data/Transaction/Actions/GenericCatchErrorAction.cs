using System;

namespace  WPM_API.Data.Transaction.Actions
{
    public class GenericCatchErrorAction : ActionBase
    {
        private Action Action { get; set; }
        private string CatchErrorMessage { get; set; }

        public GenericCatchErrorAction(Action action, string catchErrorMessage)
        {
            if (string.IsNullOrWhiteSpace(catchErrorMessage))
                throw new ArgumentNullException(nameof(catchErrorMessage));

            Action = action ?? throw new ArgumentNullException(nameof(action));
            CatchErrorMessage = catchErrorMessage;
        }

        public override void Execute()
        {
            try
            {
                Action();
            }
            catch (Exception)
            {
                //TODO or remove
                //LogHolder.MainLog.ErrorException(CatchErrorMessage, ex);
            }
        }
    }
}
