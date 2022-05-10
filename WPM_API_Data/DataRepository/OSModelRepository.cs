using WPM_API.Data.DataContext.Entities;
using WPM_API.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace  WPM_API.Data.DataRepository
{
    public class OSModelRepository : RepositoryEntityDeletableBase<OSModel>
    {
        public OSModelRepository(DataContextProvider context) : base(context)
        {
        }
    }
}
