using WPM_API.Data.DataContext.Entities;
using WPM_API.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace  WPM_API.Data.DataRepository
{
    public class AdminDeviceOptionRepository : RepositoryEntityDeletableBase<AdminDeviceOption>
    {
        public AdminDeviceOptionRepository(DataContextProvider context) : base(context)
        {
        }
    }
}
