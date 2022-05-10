using System;
using WPM_API.Data.Transaction.Actions;

namespace  WPM_API.Data.Transaction
{
    public interface ITransactionWrapper : IDisposable
    {
        void RegisterAfterCommitAction(ActionBase action);
        void RegisterAfterRollbackAction(ActionBase action);

        void Commit();
    }
}
