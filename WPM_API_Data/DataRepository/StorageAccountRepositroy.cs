using WPM_API.Data.DataContext.Entities;
using WPM_API.Data.Infrastructure;

namespace  WPM_API.Data.DataRepository
{
    public class StorageAccountRepository : RepositoryEntityDeletableBase<StorageAccount>
    {
        public StorageAccountRepository(DataContextProvider context) : base(context)
        {
        }
    }
}