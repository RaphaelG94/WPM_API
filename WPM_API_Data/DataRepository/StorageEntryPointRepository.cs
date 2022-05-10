using WPM_API.Data.DataContext.Entities.Storages;
using WPM_API.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace  WPM_API.Data.DataRepository
{
    public class StorageEntryPointRepository : RepositoryEntityDeletableBase<StorageEntryPoint>
    {
        public StorageEntryPointRepository(DataContextProvider context) : base(context)
        {
        }
    }
}
