using System;
using WPM_API.Data.DataContext;

namespace  WPM_API.Data.Infrastructure
{
    public interface IUnitOfWorkFactory : IDisposable
    {
        UnitOfWork UnitOfWork { get; }
    }

    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        public UnitOfWorkFactory(DBData context)
        {
            UnitOfWork = new UnitOfWork(context);
        }
        
        public UnitOfWork UnitOfWork { get; }

        public void Dispose()
        {
            UnitOfWork.Dispose();
        }
    }
}
